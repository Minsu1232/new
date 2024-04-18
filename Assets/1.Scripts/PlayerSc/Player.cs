
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    [Header("Player Info")]
    public PlayerState playerState;
    // ĳ������ ����
    public int initialHealth = 100;
    public int remainHealth;
    public int str = 2;
    public int dex = 5;
    public float walkSpeed = 5f;
    public float runSpeed = 7f;
    public int maxMp = 70;
    public int mp;    
    public float recovery; // ĳ���� ���� ȸ�� �ð�

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


    // ������Ʈ
    Rigidbody rb;
    Animator animator;
    CharacterController controller;

    // ĳ������Ʈ�ѷ� ������ ����
    private Vector3 playerVelocity;
    private bool isGrounded;
    float gravity = -4f;

    // ���Ӽ� ����
    int shotCount;

    bool isWalk;
    bool isDie;

    bool isLockOn;
    bool isAim;

    // ��ų ��Ÿ�� üũ��

    bool isSkill;


    private void OnEnable()
    {

        StopAllCoroutines();
        // ��ũ���ͺ� �ʱ�ȭ
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
        controller = gameObject.GetComponent<CharacterController>(); // ������ �´� �������� �ڿ������� �ϱ����� ���
        shotCount = 1;
        hp.text = $"{remainHealth}/{initialHealth}";
        hpBar.fillAmount = remainHealth / initialHealth;

    }

    void Update()
    {
        if (!isDie)
        {
            if (Input.GetMouseButtonDown(1))  // ���콺 ��Ŭ���� ����
            {
                if (target != null)
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

            Skill(); // ��ų ���� �ż���
            Recovery(); // �ڿ� ȸ�� �ż���
            Stamina();
        }
      

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
                //�߷� ���� if��
                if (controller.isGrounded && playerVelocity.y < 0)
                {
                    playerVelocity.y = 0f;
                }
                playerVelocity.y += gravity * Time.deltaTime;



                controller.Move(playerVelocity * Time.deltaTime);  // // �߷¿� ���� ���� �̵� ����



                // ���� �ӵ�

                {
                    //�޸���
                    if (Input.GetKey(KeyCode.LeftShift) && isLockOn == false)
                    {
                        if (Input.GetAxisRaw("Vertical") > 0.5)
                            animator.SetBool("IsWalking", true);
                        controller.Move(moveDir * runSpeed * Time.deltaTime);

                    }
                    //�ȱ�
                    else
                    {
                        animator.SetBool("IsWalking", false);
                        controller.Move(moveDir * walkSpeed * Time.deltaTime);

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

    void LockOnCamera()
    {
        if (target != null)
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

            //�߷� ���� if��
            if (controller.isGrounded && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }
            playerVelocity.y += gravity * Time.deltaTime;

            controller.Move(playerVelocity * Time.deltaTime);  // �߷¿� ���� ���� �̵� ����, �ڿ������� �߷��� ���� ĳ���� ��Ʈ�ѷ� ���
            character.forward = lookForward;

            // ī�޶� ȸ���� ���� 

            // ���� �ӵ�

            animator.SetBool("IsWalking", false);
            controller.Move(moveDir * walkSpeed * Time.deltaTime);


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
    //ĳ���� ���� ���� �޼���

    public void Skill()
    {

        if (mp >= 15 && isLockOn == false && isSkill == false && cool[0] == false && Input.GetKeyDown(KeyCode.Alpha1))
        {
            //���� (10�ʰ� �� 3 ����)
            mp -= 15;
            isSkill = true;

            StartCoroutine(SkillCool(20, 0, 4));
            skillArrow[4].gameObject.SetActive(true);
            skillComand = 0;
            StartCoroutine(BuffTime());

        }
        else if (mp >= 8 && isLockOn == false && isSkill == false && cool[1] == false && Input.GetKeyDown(KeyCode.Alpha2))
        {
            //��ȭ��
            Debug.Log("fire");
            mp -= 8;
            isSkill = true;
            StartCoroutine(SkillCool(6, 1, 1));
            skillArrow[1].gameObject.SetActive(true);
            skillComand = 1;

        }
        else if (mp >= 6 && isLockOn == false && isSkill == false && cool[2] == false && Input.GetKeyDown(KeyCode.Alpha3))
        {
            //��ȭ��
            mp -= 6;
            isSkill = true;
            StartCoroutine(SkillCool(5, 2, 2));
            skillArrow[2].gameObject.SetActive(true);

            skillComand = 2;
        }
        else if (mp >= 12 && isLockOn == false && isSkill == false && cool[3] == false && Input.GetKeyDown(KeyCode.Alpha4))
        {
            //��ȭ��
            mp -= 12;
            isSkill = true;
            StartCoroutine(SkillCool(10, 3, 3));
            skillArrow[3].gameObject.SetActive(true);
            skillComand = 3;
        }
        else if (isLockOn == false && isSkill == false && Input.GetKeyDown(KeyCode.Alpha5))
        {
            //�⺻ȭ��            
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
                skillComand = 0;
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
        if (target != null)
        {
            Vector3 directionToTarget = (target.position - character.position + ShotPointer.position).normalized;  // Ÿ�� ���� ���
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);  // ������ ������� ȸ�� ����
            character.rotation = Quaternion.Slerp(character.rotation, targetRotation, Time.deltaTime * 10);  // �ε巴�� ȸ��
            character.transform.rotation = Quaternion.identity;
        }


    }


    // ���� �����̸� �߰��� ���Ӽ� ����
    IEnumerator ShotDelay()
    {
        yield return new WaitForSeconds(0.7f);
        if (shotCount == 0)
        {
            shotCount += 1;
        }

    }
    //�����ð����� �� 3 ����
    IEnumerator BuffTime()
    {
        str += 3;
        Debug.Log("��������" + str);
        yield return new WaitForSeconds(10f);
        str -= 3;
        Debug.Log("������" + str);
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
    /// �÷��̾��� �ǰ� �Լ�, NormalMounster���� ȣ���ؼ� �ǰ�
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        remainHealth -= damage; // ���ط��� ���� ����
        remainHealth = Mathf.Clamp(remainHealth, 0, initialHealth); // ü���� ������ ���� �ʵ��� ����

        // Hpbar ����ui

        hp.text = $"{remainHealth}/{initialHealth}";
        hpBar.fillAmount = (float)remainHealth / initialHealth;

        if (remainHealth > 0)
        {
            animator.SetBool("Hit", true); // �ǰ� �ִϸ��̼� ���
        }
        else
        {
            Die(); // ü���� 0 ���ϸ� ��� ó��
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
    // mpBar ���� UI
    void Stamina()
    {
        mana.text = $"{mp}/{maxMp}";
        if (maxMp > 0)  // �ִ� MP�� 0���� Ŭ ���� ���
        {
            mpBar.fillAmount = (float)mp / maxMp;
        }
        else
        {
            mpBar.fillAmount = 0;  // �ִ� MP�� 0�̸� �ٸ� 0���� ����
        }
    }
    

}

