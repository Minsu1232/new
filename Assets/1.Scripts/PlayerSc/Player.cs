
using System.Collections;
using System.Diagnostics;
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
    public Transform target;
    public Image hpBar;
    public Image mpBar;
    public Text hp;
    public Text mana;
    public ParticleSystem counterSkill;
    public AudioClip buffSound;
    public AudioClip rollingSound;
    public AudioClip walkSound;
    public AudioClip runSound;
    public AudioClip counterSkillSound;


    [Header("Arrow Attributes")]
    [SerializeField]
    GameObject[] skillArrow;
    [SerializeField]
    Transform ShotPointer;
    [SerializeField]
    BowAnimation bowAnimation;

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
    public Boss boss;

    // 캐릭터컨트롤러 관리용 변수
    private Vector3 playerVelocity; //캐릭터의 y값 체크용
    private bool isGrounded;
    float gravity = -8f; //받을 중력값

    // 연속샷 방지
    int shotCount;


    // 애니메이션 및 상태 체크용
    bool isWalk;
    bool isDie;
    public bool isLockOn;
    bool isAim;
    public bool isRoll;
    public bool isRolling;
    public int roll; // 구르기 쿨타임용 변수
    public bool isInvincible = false;
    public bool isSand;



    // 스킬 쿨타임 체크용
    bool isSkill;

    AudioSource audioSource;
    bool isSound;


    public float detectionRadius = 5f; // 검색 반경
    private void OnEnable()
    {

        roll = 1;
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
        isSound = false;
        controller = gameObject.GetComponent<CharacterController>(); // 지형에 맞는 움직임을 자연스럽게 하기위해 사용
        shotCount = 1;
        hp.text = $"{remainHealth}/{initialHealth}";
        hpBar.fillAmount = remainHealth / initialHealth;
        audioSource = GetComponent<AudioSource>();

    }

    void Update()
    {

        if (!isDie)
        {
            if (isRoll)
            {
                //HandleInput();
            }

            if (Input.GetMouseButtonDown(1))  // 마우스 우클릭을 감지
            {
                DetectObjects();

                if (target != null) 
                {                           
                    isLockOn = !isLockOn;  // 토글 방식
                    bowAnimation.HandleLockOn(isLockOn);
                }
                else
                {                    
                    isLockOn = false;
                }
            
               


            }
            if (!GameManager.Instance.isShop)
            {
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
            }
           

            Skill(); // 스킬 관련 매서드
            Recovery(); // 자연 회복 매서드
            Stamina();
        }


    }

    void DetectObjects()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, 100f);
        RaycastHit closestHit = new RaycastHit();
        float closestDistance = Mathf.Infinity;

        // 가장 가까운 객체 찾기
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("HitTransform") || hit.collider.gameObject.layer == LayerMask.NameToLayer("PowderKeg"))
            {
                float distanceToHit = Vector3.Distance(ray.origin, hit.point);
                if (distanceToHit < closestDistance)
                {
                    closestHit = hit;
                    closestDistance = distanceToHit;
                }
            }
        }

        // 가장 가까운 지점 기준으로 주변 객체 감지
        if (closestHit.collider != null)
        {
            UnityEngine.Debug.Log("Closest target found based on mouse pointer: " + closestHit.collider.gameObject.name);
            target = closestHit.collider.transform;

            Collider[] nearbyObjects = Physics.OverlapSphere(closestHit.point, detectionRadius);
            foreach (Collider nearbyObject in nearbyObjects)
            {
                UnityEngine.Debug.Log("Nearby object detected: " + nearbyObject.gameObject.name);
            }
        }
    }
    void MoveAndCamera()
    {
        isSound = false;
        {
            if (isWalk == true)
            {
                animator.SetBool("IsWalking", false);
                // 입력값을 vector2로 저장
                Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                Vector3 moveDir;

                // 입력값이 뒤일경우엔 구르기 기능 사용 불가
                if (moveInput.y <= 0)
                {
                    isRoll = false;
                }
                else
                {
                    isRoll = true;
                }
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

            audioSource.PlayOneShot(buffSound);
            StartCoroutine(SkillCool(20, 0, 4));
            skillArrow[4].gameObject.SetActive(true);
            skillComand = 0;
            StartCoroutine(BuffTime());

        }
        else if (mp >= 8 && isLockOn == false && isSkill == false && cool[1] == false && Input.GetKeyDown(KeyCode.Alpha2))
        {
            //불화살

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
        else if (mp >= 3 && isLockOn == false && cool[4] == false && Input.GetKeyDown(KeyCode.F))
        {
            // 카운터패턴사용때 스킬 사용시 일시적인 경직
            mp -= 3;
            StartCoroutine(SkillCool(2, 4, 0));
            counterSkill.Play();
            if (boss.gameObject.activeSelf&& boss.animator.GetInteger("AttackInt") == 2)
            {                    
                boss.animator.SetTrigger("Groggy");
            }
           
        }
        else if (mp >= 5 && isLockOn == false && cool[5] == false && Input.GetKeyDown(KeyCode.Space))
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {

                // 쿨타임은 3초
                StartCoroutine(SkillCool(3, 5, 0));
                audioSource.PlayOneShot(rollingSound);
                mp -= 5;
                Roll();


            }
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
                bowAnimation.ArrowShot();
                isSkill = false; // 스킬화살 장전시엔 다른 스킬 사용 불가용

                isLockOn = false; // 화살 발사 후 에임기능을 다시 하기 위한 체크
                UnityEngine.Debug.Log("쏨");

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
        if (!isSound)
        {
          
        }
       
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

        yield return new WaitForSeconds(10f);
        str -= 3;

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
        if (isInvincible) return; // 무적 상태일 때는 피해를 받지 않음
        remainHealth -= damage; // 피해량을 먼저 적용
        remainHealth = Mathf.Clamp(remainHealth, 0, initialHealth); // 체력이 음수가 되지 않도록 제어

        // Hpbar 감소ui

        hp.text = $"{remainHealth}/{initialHealth}";
        hpBar.fillAmount = (float)remainHealth / initialHealth;

        if (!isRolling)
        {
            if (remainHealth > 0)
            {
                animator.SetBool("Hit", true); // 피격 애니메이션 재생
            }

        }
        if (remainHealth <= 0)
        {
            Die(); // 체력이 0 이하면 사망 처리

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
    //void HandleInput()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        if (mp > 5 && roll == 1)
    //        {
    //            // 쿨타임은 3초
    //            StartCoroutine(RollingCool());
    //            mp -= 5;
    //            Roll();
    //        }

    //    }
    //}
    void Roll()
    {
        // 구르기 애니메이션 재생
        animator.SetBool("Roll", true);
        isRolling = true;
        // 구르기 동안 이동할 거리 설정
        float rollDistance = 5f;

        // 구르기 방향 (현재 캐릭터가 바라보고 있는 방향)
        Vector3 rollDirection = character.forward;

        // 구르기 이동 실행
        StartCoroutine(PerformRoll(rollDirection, rollDistance));
    }


    IEnumerator PerformRoll(Vector3 direction, float distance)
    {
        float remainingDistance = distance;
        while (remainingDistance > 0)
        {
            // 한 프레임 당 이동할 거리 계산
            float moveDistance = 6f * Time.deltaTime;

            // 이동할 거리가 남은 거리보다 많다면 조정
            moveDistance = Mathf.Min(moveDistance, remainingDistance);

            // 캐릭터 컨트롤러를 사용하여 이동
            controller.Move(direction * moveDistance);

            // 남은 거리 갱신
            remainingDistance -= moveDistance;


            yield return null;
        }
    }
    IEnumerator RollingCool()
    {
        roll = 0;
        yield return new WaitForSeconds(3f);
        roll = 1;
    }
    //애니메이션 이벤트
    public void RollFalse()
    {
        animator.SetBool("Roll", false);
    }
    // 애니메이션 이벤트
    public void RollInvincibility(int status)
    {
        isInvincible = (status != 0); // 구르기시 12프레임간 무적
    }
    // 애니메이션 이벤트
    public void IsRolling()
    {
        isRolling = false;
    }
  
    // 애니메이션 이벤트
    public void WalkSound()
    {
        if (isSand)
        {
          
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "BossRoom")
        {
            isSand = true;
            
        }
    }
    
}

