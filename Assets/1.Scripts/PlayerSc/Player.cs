
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    [Header("Player Info")]
    public PlayerState playerState;
    // 캐릭터의 정보
    public int initialHealth = 100;
    public int remainHealth;
    public int str = 2;
    public int dex = 5;
    public float walkSpeed = 5f;
    public float runSpeed = 7f;
    public int maxMp = 70;
    public int mp;    
    public float recovery; // 캐릭터 마나 회복 시간

    [Header("Player Attributes")]
    [SerializeField]
    Transform character;
    [SerializeField]
    Transform cameraArm;
    [SerializeField]
    Transform target;
    public Image hpBar;
    public Image mpBar;
    public Text hp;
    public Text mana;

    [Header("Arrow Attributes")]
    [SerializeField]
    GameObject[] skillArrow;
    [SerializeField]
    Transform ShotPointer;

    [Header("Skill Attributes")]
    [SerializeField]
    Image[] skillCool;
    public bool[] cool;
    [SerializeField]
    Text[] coolText;
    public int skillComand;


    // 컴포넌트
    Rigidbody rb;
    Animator animator;
    CharacterController controller;

    // 캐릭터컨트롤러 관리용 변수
    private Vector3 playerVelocity;
    private bool isGrounded;
    float gravity = -4f;

    // 연속샷 방지
    int shotCount;

    bool isWalk;
    bool isDie;

    bool isLockOn;
    bool isAim;

    // 스킬 쿨타임 체크용

    bool isSkill;


    private void OnEnable()
    {

        StopAllCoroutines();
        // 스크립터블 초기화
        initialHealth = playerState.health;
        str = playerState.str;
        dex = playerState.dex;
        walkSpeed = playerState.walkSpeed;
        runSpeed = playerState.runSpeed;
        mp = playerState.mp;
        maxMp = playerState.mp;
        remainHealth = initialHealth;
        mp = maxMp;
        skillComand = 0;
        isDie = false;
        isWalk = true;

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        controller = gameObject.GetComponent<CharacterController>(); // 지형에 맞는 움직임을 자연스럽게 하기위해 사용
        shotCount = 1;
        hp.text = $"{remainHealth}/{initialHealth}";
        hpBar.fillAmount = remainHealth / initialHealth;

    }

    void Update()
    {
        if (!isDie)
        {
            if (Input.GetMouseButtonDown(1))  // 마우스 우클릭을 감지
            {
                if (target != null)
                {
                    isLockOn = !isLockOn;  // 토글 방식
                }

            }

            if (!isLockOn)
            {
                LookAround();
                MoveAndCamera();
            }
            else
            {
                LockOnTarget();  // LockOn 상태일 때 타겟을 바라보는 함수
                LockOnCamera();  // LockOn 상태일 때 움직이는 함수
                ArrowShot();

            }

            Skill(); // 스킬 관련 매서드
            Recovery(); // 자연 회복 매서드
            Stamina();
        }
      

    }


    void MoveAndCamera()
    {

        {
            if (isWalk == true)
            {
                animator.SetBool("IsWalking", false);
                // 입력값을 vector2로 저장
                Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                Vector3 moveDir;


                Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
                Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
                moveDir = lookForward * moveInput.y + lookRight * moveInput.x;
                character.forward = lookForward;
                //중력 적용 if문
                if (controller.isGrounded && playerVelocity.y < 0)
                {
                    playerVelocity.y = 0f;
                }
                playerVelocity.y += gravity * Time.deltaTime;



                controller.Move(playerVelocity * Time.deltaTime);  // // 중력에 의한 수직 이동 적용



                // 전진 속도

                {
                    //달리기
                    if (Input.GetKey(KeyCode.LeftShift) && isLockOn == false)
                    {
                        if (Input.GetAxisRaw("Vertical") > 0.5)
                            animator.SetBool("IsWalking", true);
                        controller.Move(moveDir * runSpeed * Time.deltaTime);

                    }
                    //걷기
                    else
                    {
                        animator.SetBool("IsWalking", false);
                        controller.Move(moveDir * walkSpeed * Time.deltaTime);

                    }

                }
                //// 후진 속도
                //transform.position += moveDir * Time.deltaTime * 1f;


                // 블렌트 트리 애니메이션 움직임의 자연스러움을 위해
                float smoothedValueX = Mathf.Lerp(animator.GetFloat("X"), moveInput.x, 5f * Time.deltaTime);
                float smoothedValueY = Mathf.Lerp(animator.GetFloat("Y"), moveInput.y, 5f * Time.deltaTime);

                // 블렌드 트리 애니메이션
                animator.SetFloat("X", smoothedValueX);
                animator.SetFloat("Y", smoothedValueY);
            }
        }




    }

    void LookAround()
    {
        if (!isLockOn)
        {
            Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            Vector3 camAngle = cameraArm.rotation.eulerAngles;
            float x = camAngle.x - mouseDelta.y;

            // 상하 카메라 이동의 제한
            if (x < 180f)
            {
                x = Mathf.Clamp(x, -1f, 70f);

            }
            else
            {
                x = Mathf.Clamp(x, 335f, 361f);
            }


            cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
        }

    }

    void LockOnCamera()
    {
        if (target != null)
        {
            Vector3 directionToTarget = (target.position - cameraArm.position).normalized;  // 타겟 방향 계산
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);  // 방향을 기반으로 회전 생성
            cameraArm.rotation = Quaternion.Slerp(cameraArm.rotation, targetRotation, Time.deltaTime * 10);  // 부드럽게 회전

            animator.SetBool("IsWalking", false);
            // 입력값을 vector2로 저장
            Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            Vector3 moveDir;


            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
            moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            //중력 적용 if문
            if (controller.isGrounded && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }
            playerVelocity.y += gravity * Time.deltaTime;

            controller.Move(playerVelocity * Time.deltaTime);  // 중력에 의한 수직 이동 적용, 자연스러운 중력을 위해 캐릭터 컨트롤러 사용
            character.forward = lookForward;

            // 카메라 회전에 따른 

            // 전진 속도

            animator.SetBool("IsWalking", false);
            controller.Move(moveDir * walkSpeed * Time.deltaTime);


            //// 후진 속도
            //transform.position += moveDir * Time.deltaTime * 4f;


            // 블렌트 트리 애니메이션 움직임의 자연스러움을 위해
            float smoothedValueX = Mathf.Lerp(animator.GetFloat("X"), moveInput.x, 5f * Time.deltaTime);
            float smoothedValueY = Mathf.Lerp(animator.GetFloat("Y"), moveInput.y, 5f * Time.deltaTime);

            // 블렌드 트리 애니메이션
            animator.SetFloat("X", smoothedValueX);
            animator.SetFloat("Y", smoothedValueY);
        }

    }
    //캐릭터 죽음 관련 메서드

    public void Skill()
    {

        if (mp >= 15 && isLockOn == false && isSkill == false && cool[0] == false && Input.GetKeyDown(KeyCode.Alpha1))
        {
            //버프 (10초간 힘 3 증가)
            mp -= 15;
            isSkill = true;

            StartCoroutine(SkillCool(20, 0, 4));
            skillArrow[4].gameObject.SetActive(true);
            skillComand = 0;
            StartCoroutine(BuffTime());

        }
        else if (mp >= 8 && isLockOn == false && isSkill == false && cool[1] == false && Input.GetKeyDown(KeyCode.Alpha2))
        {
            //불화살
            Debug.Log("fire");
            mp -= 8;
            isSkill = true;
            StartCoroutine(SkillCool(6, 1, 1));
            skillArrow[1].gameObject.SetActive(true);
            skillComand = 1;

        }
        else if (mp >= 6 && isLockOn == false && isSkill == false && cool[2] == false && Input.GetKeyDown(KeyCode.Alpha3))
        {
            //독화살
            mp -= 6;
            isSkill = true;
            StartCoroutine(SkillCool(5, 2, 2));
            skillArrow[2].gameObject.SetActive(true);

            skillComand = 2;
        }
        else if (mp >= 12 && isLockOn == false && isSkill == false && cool[3] == false && Input.GetKeyDown(KeyCode.Alpha4))
        {
            //금화살
            mp -= 12;
            isSkill = true;
            StartCoroutine(SkillCool(10, 3, 3));
            skillArrow[3].gameObject.SetActive(true);
            skillComand = 3;
        }
        else if (isLockOn == false && isSkill == false && Input.GetKeyDown(KeyCode.Alpha5))
        {
            //기본화살            
            skillComand = 0;

        }

    }
    IEnumerator SkillCool(int coolTime, int skillCoolIndex, int skillArrowIndex)
    {
        cool[skillCoolIndex] = true;
        skillCool[skillCoolIndex].fillAmount = 0;
        float time = 0;
        while (time < coolTime)
        {

            time += Time.deltaTime;
            skillCool[skillCoolIndex].fillAmount = time / coolTime;
            int remainingTime = Mathf.CeilToInt(coolTime - time);  // 남은 시간을 정수로 반올림
            coolText[skillCoolIndex].text = remainingTime + "s";   // 남은 시간 업데이트

            yield return null;
        }
        coolText[skillCoolIndex].text = "";
        skillCool[skillCoolIndex].fillAmount = 1;
        cool[skillCoolIndex] = false;

    }

    void ArrowShot()
    {
        if (shotCount > 0)
        {
            if (Input.GetMouseButtonDown(0) && isLockOn == true)
            {
                isSkill = false; // 스킬화살 장전시엔 다른 스킬 사용 불가용

                isLockOn = false; // 화살 발사 후 에임기능을 다시 하기 위한 체크

                Vector3 directionToTarget = (target.position - ShotPointer.transform.position).normalized;

                // 타겟 방향을 바라보는 기본 회전
                Quaternion baseRotation = Quaternion.LookRotation(directionToTarget);

                // 추가적인 X축 90도 회전
                Quaternion additionalRotation = Quaternion.Euler(90, 0, 0);

                // 최종 회전 계산
                Quaternion finalRotation = baseRotation * additionalRotation;

                // 화살 인스턴스 생성 시 최종 회전 적용
                GameObject arrowInstance = Instantiate(skillArrow[skillComand], ShotPointer.transform.position, finalRotation);

                Destroy(arrowInstance, 6f); // 6초뒤 화살 사라짐
                Rigidbody arrowRb = arrowInstance.AddComponent<Rigidbody>(); // Rigidbody 컴포넌트 동적 추가
                arrowRb.useGravity = false;
                ArrowPhysics(arrowRb, directionToTarget);
                while (shotCount > 0)
                {
                    shotCount--;
                }
                skillComand = 0;
            }
        }
        else
        {
            StartCoroutine(ShotDelay());
        }


    }
    // 화살의 물리력과 방향
    public void ArrowPhysics(Rigidbody arrowRb, Vector3 directionToTarget)
    {
        arrowRb.AddForce(directionToTarget * 30f, ForceMode.Impulse);
    }
    void LockOnTarget()
    {
        isAim = false;
        if (target != null)
        {
            Vector3 directionToTarget = (target.position - character.position + ShotPointer.position).normalized;  // 타겟 방향 계산
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);  // 방향을 기반으로 회전 생성
            character.rotation = Quaternion.Slerp(character.rotation, targetRotation, Time.deltaTime * 10);  // 부드럽게 회전
            character.transform.rotation = Quaternion.identity;
        }


    }


    // 샷의 딜레이를 추가해 연속샷 방지
    IEnumerator ShotDelay()
    {
        yield return new WaitForSeconds(0.7f);
        if (shotCount == 0)
        {
            shotCount += 1;
        }

    }
    //버프시간동안 힘 3 증가
    IEnumerator BuffTime()
    {
        str += 3;
        Debug.Log("버프시작" + str);
        yield return new WaitForSeconds(10f);
        str -= 3;
        Debug.Log("버프끝" + str);
    }
    void Recovery()
    {
        recovery += Time.deltaTime;
        if (recovery > 1 && mp < 50)
        {
            mp += 1;
            recovery = 0;
        }
    }

    /// <summary>
    /// 플레이어의 피격 함수, NormalMounster에서 호출해서 피격
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        remainHealth -= damage; // 피해량을 먼저 적용
        remainHealth = Mathf.Clamp(remainHealth, 0, initialHealth); // 체력이 음수가 되지 않도록 제어

        // Hpbar 감소ui

        hp.text = $"{remainHealth}/{initialHealth}";
        hpBar.fillAmount = (float)remainHealth / initialHealth;

        if (remainHealth > 0)
        {
            animator.SetBool("Hit", true); // 피격 애니메이션 재생
        }
        else
        {
            Die(); // 체력이 0 이하면 사망 처리
            Debug.Log("DIE");
        }

    }
    void Die()
    {
        if (!isDie)
        {
            animator.SetTrigger("Die");
            isDie = true;
            Destroy(gameObject, 3f);
        }

    }
    // mpBar 감소 UI
    void Stamina()
    {
        mana.text = $"{mp}/{maxMp}";
        if (maxMp > 0)  // 최대 MP가 0보다 클 때만 계산
        {
            mpBar.fillAmount = (float)mp / maxMp;
        }
        else
        {
            mpBar.fillAmount = 0;  // 최대 MP가 0이면 바를 0으로 설정
        }
    }
    

}

