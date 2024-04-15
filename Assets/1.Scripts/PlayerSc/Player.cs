using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

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

    Rigidbody rb;
    Animator animator;





    bool isWalk;
    bool isDie;

    public bool isLockOn;
    public bool isAim;

    // 스킬 쿨타임 체크용
    bool cool1;
    bool cool2;

    public PlayerState playerState;

    public int skillComand;
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
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))  // 마우스 우클릭을 감지
        {
            isLockOn = !isLockOn;  // 토글 방식
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

        }

        Skill();
        ArrowShot();

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

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            skillArrow[0].gameObject.SetActive(true);
            skillComand = 0;

        }
        if (cool1 == false && Input.GetKeyDown(KeyCode.Alpha2))
        {
            StartCoroutine(SkillCool1());
            skillArrow[1].gameObject.SetActive(true);
            skillComand = 1;
        }
        if (cool2 == false && Input.GetKeyDown(KeyCode.Alpha3))
        {
            StartCoroutine(SkillCool2());
            skillArrow[2].gameObject.SetActive(true);
            skillComand = 2;
        }

    }
    IEnumerator SkillCool1()
    {
        cool1 = true;
        yield return new WaitForSeconds(5f);
        cool1 = false;
    }
    IEnumerator SkillCool2()
    {
        cool2 = true;
        yield return new WaitForSeconds(3f);
        cool2 = false;
    }
    void ArrowShot()
    {
        if (Input.GetMouseButtonDown(0) && isLockOn == true)
        {
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

            Rigidbody arrowRb = arrowInstance.AddComponent<Rigidbody>(); // Rigidbody 컴포넌트 동적 추가
            arrowRb.useGravity = false;
            ArrowPhysics(arrowRb, directionToTarget);

        }

    }
    public void ArrowPhysics(Rigidbody arrowRb, Vector3 directionToTarget) // 화살의 물리력과 방향
    {
        arrowRb.AddForce(directionToTarget * 10f, ForceMode.Impulse);
    }
    void LockOnTarget()
    {
        Vector3 directionToTarget = (target.position - character.position + ShotPointer.position).normalized;  // 타겟 방향 계산
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);  // 방향을 기반으로 회전 생성
        character.rotation = Quaternion.Slerp(character.rotation, targetRotation, Time.deltaTime * 10);  // 부드럽게 회전
        character.transform.rotation = Quaternion.identity;
    }

    void LockOnCamera()
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

