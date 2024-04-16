using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    Transform character;
    [SerializeField]
    Transform cameraArm;
    [SerializeField]
    GameObject[] skillArrow;
    [SerializeField]
    Transform ShotPointer;
    [SerializeField]
    Transform target;
    [SerializeField]
    Image[] skillCool;
    [SerializeField]
    Text[] coolText;

    Rigidbody rb;
    Animator animator;

    // 연속 화살 발살을 막기위한 카운트
    public int shotCount;
    // 손에든 화살과 날라가는 화살의 동기화용 숫자;
    public int skillComand;
    // 버프지속시간
    public float buffDuration;


    bool isWalk;
    bool isDie;

    public bool isLockOn;
    public bool isAim;

    // 스킬 쿨타임 체크용
   public bool[] cool;
    bool isSkill;

    public PlayerState playerState;

    
    private void OnEnable()
    {
        skillComand = 0;
        isDie = false;
        isWalk = true;

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        shotCount = 1;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))  // 마우스 우클릭을 감지
        {if(target != null)
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

        Skill();
        

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

                // 카메라 회전에 따른 

                // 전진 속도

                {
                    //달리기
                    if (Input.GetKey(KeyCode.LeftShift) && isLockOn == false)
                    {
                        if (Input.GetAxisRaw("Vertical") > 0.5)
                            animator.SetBool("IsWalking", true);
                        transform.position += moveDir * Time.deltaTime * playerState.runSpeed;

                    }
                    //걷기
                    else
                    {
                        animator.SetBool("IsWalking", false);
                        transform.position += moveDir * Time.deltaTime * playerState.walkSpeed;

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

    //캐릭터 죽음 관련 메서드
    void Die()
    {
        if (isDie == true)
        {
            animator.SetBool("IsDie", true);
            Destroy(gameObject, 2f);
        }

    }
    public void Skill()
    {

        if (isLockOn == false && isSkill == false && cool[0] == false && Input.GetKeyDown(KeyCode.Alpha1))
        {
            
            buffDuration += Time.deltaTime;
            //버프
            isSkill = true;
            StartCoroutine(SkillCool(20,0,4));
            skillArrow[4].gameObject.SetActive(true);
            skillComand = 0;
            while (buffDuration < 8)
            {
                playerState.str += 3;
            }
            
            buffDuration = 0;
            
            

        }
       else if (isLockOn == false && isSkill == false && cool[1] == false && Input.GetKeyDown(KeyCode.Alpha2))
        {
            //불화살
            isSkill = true;
            StartCoroutine(SkillCool(6,1,1));
            skillArrow[1].gameObject.SetActive(true);
            skillComand = 1;
        }
       else if (isLockOn == false && isSkill == false &&  cool[2] == false && Input.GetKeyDown(KeyCode.Alpha3))
        {
            //독화살
            isSkill = true;
            StartCoroutine(SkillCool(5,2,2));
            skillArrow[2].gameObject.SetActive(true);

            skillComand = 2;
        }
       else if (isLockOn == false && isSkill == false && cool[3] == false && Input.GetKeyDown(KeyCode.Alpha4))
        {
            //금화살
            isSkill = true;
            StartCoroutine(SkillCool(10,3,3));
            skillArrow[3].gameObject.SetActive(true);
            skillComand = 3;
        }
       else if (isLockOn == false && isSkill == false && Input.GetKeyDown(KeyCode.Alpha5))
        {
            //기본화살            
            skillComand = 0;

        }

    }
    IEnumerator SkillCool(int coolTime,int skillCoolIndex,int skillArrowIndex)
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
        if(target != null)
        {
            Vector3 directionToTarget = (target.position - character.position + ShotPointer.position).normalized;  // 타겟 방향 계산
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);  // 방향을 기반으로 회전 생성
            character.rotation = Quaternion.Slerp(character.rotation, targetRotation, Time.deltaTime * 10);  // 부드럽게 회전
            character.transform.rotation = Quaternion.identity;
        }
       
     
    }

    void LockOnCamera()
    {
        if(target != null)
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

            character.forward = lookForward;

            // 카메라 회전에 따른 

            // 전진 속도

            {
                animator.SetBool("IsWalking", false);
                transform.position += moveDir * Time.deltaTime * playerState.walkSpeed;



            }
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
    IEnumerator ShotDelay()
    {
        yield return new WaitForSeconds(0.7f);
        if(shotCount == 0 )
        {
            shotCount += 1;
        }
        
    }

}

