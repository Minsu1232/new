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

    // ���� ȭ�� �߻��� �������� ī��Ʈ
    public int shotCount;
    // �տ��� ȭ��� ���󰡴� ȭ���� ����ȭ�� ����;
    public int skillComand;
    // �������ӽð�
    public float buffDuration;


    bool isWalk;
    bool isDie;

    public bool isLockOn;
    public bool isAim;

    // ��ų ��Ÿ�� üũ��
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
        if (Input.GetMouseButtonDown(1))  // ���콺 ��Ŭ���� ����
        {if(target != null)
            {
                isLockOn = !isLockOn;  // ��� ���
            }
            
        }

        if (!isLockOn)
        {
            LookAround();
            MoveAndCamera();
        }
        else
        {
            LockOnTarget();  // LockOn ������ �� Ÿ���� �ٶ󺸴� �Լ�
            LockOnCamera();  // LockOn ������ �� �����̴� �Լ�
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
                // �Է°��� vector2�� ����
                Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                Vector3 moveDir;


                Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
                Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
                moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

                character.forward = lookForward;

                // ī�޶� ȸ���� ���� 

                // ���� �ӵ�

                {
                    //�޸���
                    if (Input.GetKey(KeyCode.LeftShift) && isLockOn == false)
                    {
                        if (Input.GetAxisRaw("Vertical") > 0.5)
                            animator.SetBool("IsWalking", true);
                        transform.position += moveDir * Time.deltaTime * playerState.runSpeed;

                    }
                    //�ȱ�
                    else
                    {
                        animator.SetBool("IsWalking", false);
                        transform.position += moveDir * Time.deltaTime * playerState.walkSpeed;

                    }

                }
                //// ���� �ӵ�
                //transform.position += moveDir * Time.deltaTime * 1f;


                // ��Ʈ Ʈ�� �ִϸ��̼� �������� �ڿ��������� ����
                float smoothedValueX = Mathf.Lerp(animator.GetFloat("X"), moveInput.x, 5f * Time.deltaTime);
                float smoothedValueY = Mathf.Lerp(animator.GetFloat("Y"), moveInput.y, 5f * Time.deltaTime);

                // ���� Ʈ�� �ִϸ��̼�
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

            // ���� ī�޶� �̵��� ����
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

    //ĳ���� ���� ���� �޼���
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
            //����
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
            //��ȭ��
            isSkill = true;
            StartCoroutine(SkillCool(6,1,1));
            skillArrow[1].gameObject.SetActive(true);
            skillComand = 1;
        }
       else if (isLockOn == false && isSkill == false &&  cool[2] == false && Input.GetKeyDown(KeyCode.Alpha3))
        {
            //��ȭ��
            isSkill = true;
            StartCoroutine(SkillCool(5,2,2));
            skillArrow[2].gameObject.SetActive(true);

            skillComand = 2;
        }
       else if (isLockOn == false && isSkill == false && cool[3] == false && Input.GetKeyDown(KeyCode.Alpha4))
        {
            //��ȭ��
            isSkill = true;
            StartCoroutine(SkillCool(10,3,3));
            skillArrow[3].gameObject.SetActive(true);
            skillComand = 3;
        }
       else if (isLockOn == false && isSkill == false && Input.GetKeyDown(KeyCode.Alpha5))
        {
            //�⺻ȭ��            
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
            int remainingTime = Mathf.CeilToInt(coolTime - time);  // ���� �ð��� ������ �ݿø�
            coolText[skillCoolIndex].text = remainingTime + "s";   // ���� �ð� ������Ʈ

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
                isSkill = false; // ��ųȭ�� �����ÿ� �ٸ� ��ų ��� �Ұ���

                isLockOn = false; // ȭ�� �߻� �� ���ӱ���� �ٽ� �ϱ� ���� üũ
             
                Vector3 directionToTarget = (target.position - ShotPointer.transform.position).normalized;

                // Ÿ�� ������ �ٶ󺸴� �⺻ ȸ��
                Quaternion baseRotation = Quaternion.LookRotation(directionToTarget);

                // �߰����� X�� 90�� ȸ��
                Quaternion additionalRotation = Quaternion.Euler(90, 0, 0);

                // ���� ȸ�� ���
                Quaternion finalRotation = baseRotation * additionalRotation;

                // ȭ�� �ν��Ͻ� ���� �� ���� ȸ�� ����
                GameObject arrowInstance = Instantiate(skillArrow[skillComand], ShotPointer.transform.position, finalRotation);
                Destroy(arrowInstance, 6f); // 6�ʵ� ȭ�� �����
                Rigidbody arrowRb = arrowInstance.AddComponent<Rigidbody>(); // Rigidbody ������Ʈ ���� �߰�
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
    // ȭ���� �����°� ����
    public void ArrowPhysics(Rigidbody arrowRb, Vector3 directionToTarget) 
    {
        arrowRb.AddForce(directionToTarget * 30f, ForceMode.Impulse);
    }
    void LockOnTarget()
    {
        isAim = false;
        if(target != null)
        {
            Vector3 directionToTarget = (target.position - character.position + ShotPointer.position).normalized;  // Ÿ�� ���� ���
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);  // ������ ������� ȸ�� ����
            character.rotation = Quaternion.Slerp(character.rotation, targetRotation, Time.deltaTime * 10);  // �ε巴�� ȸ��
            character.transform.rotation = Quaternion.identity;
        }
       
     
    }

    void LockOnCamera()
    {
        if(target != null)
        {
            Vector3 directionToTarget = (target.position - cameraArm.position).normalized;  // Ÿ�� ���� ���
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);  // ������ ������� ȸ�� ����
            cameraArm.rotation = Quaternion.Slerp(cameraArm.rotation, targetRotation, Time.deltaTime * 10);  // �ε巴�� ȸ��

            animator.SetBool("IsWalking", false);
            // �Է°��� vector2�� ����
            Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            Vector3 moveDir;


            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
            moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            character.forward = lookForward;

            // ī�޶� ȸ���� ���� 

            // ���� �ӵ�

            {
                animator.SetBool("IsWalking", false);
                transform.position += moveDir * Time.deltaTime * playerState.walkSpeed;



            }
            //// ���� �ӵ�
            //transform.position += moveDir * Time.deltaTime * 4f;


            // ��Ʈ Ʈ�� �ִϸ��̼� �������� �ڿ��������� ����
            float smoothedValueX = Mathf.Lerp(animator.GetFloat("X"), moveInput.x, 5f * Time.deltaTime);
            float smoothedValueY = Mathf.Lerp(animator.GetFloat("Y"), moveInput.y, 5f * Time.deltaTime);

            // ���� Ʈ�� �ִϸ��̼�
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

