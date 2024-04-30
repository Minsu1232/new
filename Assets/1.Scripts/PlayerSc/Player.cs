
using System.Collections;
using System.Diagnostics;
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
    


    // ������Ʈ
    Rigidbody rb;
    Animator animator;
    CharacterController controller;
    public Boss boss;

    // ĳ������Ʈ�ѷ� ������ ����
    private Vector3 playerVelocity; //ĳ������ y�� üũ��
    private bool isGrounded;
    float gravity = -8f; //���� �߷°�

    // ���Ӽ� ����
    int shotCount;


    // �ִϸ��̼� �� ���� üũ��
    bool isWalk;
    bool isDie;
    public bool isLockOn;
    bool isAim;
    public bool isRoll;
    public bool isRolling;
    public int roll; // ������ ��Ÿ�ӿ� ����
    public bool isInvincible = false;
    public bool isSand;



    // ��ų ��Ÿ�� üũ��
    bool isSkill;

    AudioSource audioSource;
    bool isSound;


    public float detectionRadius = 5f; // �˻� �ݰ�
    private void OnEnable()
    {

        roll = 1;
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
        isSound = false;
        controller = gameObject.GetComponent<CharacterController>(); // ������ �´� �������� �ڿ������� �ϱ����� ���
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

            if (Input.GetMouseButtonDown(1))  // ���콺 ��Ŭ���� ����
            {
                DetectObjects();

                if (target != null) 
                {                           
                    isLockOn = !isLockOn;  // ��� ���
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
                    LockOnTarget();  // LockOn ������ �� Ÿ���� �ٶ󺸴� �Լ�
                    LockOnCamera();  // LockOn ������ �� �����̴� �Լ�
                    ArrowShot();

                }
            }
           

            Skill(); // ��ų ���� �ż���
            Recovery(); // �ڿ� ȸ�� �ż���
            Stamina();
        }


    }

    void DetectObjects()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, 100f);
        RaycastHit closestHit = new RaycastHit();
        float closestDistance = Mathf.Infinity;

        // ���� ����� ��ü ã��
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

        // ���� ����� ���� �������� �ֺ� ��ü ����
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
                // �Է°��� vector2�� ����
                Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                Vector3 moveDir;

                // �Է°��� ���ϰ�쿣 ������ ��� ��� �Ұ�
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

            audioSource.PlayOneShot(buffSound);
            StartCoroutine(SkillCool(20, 0, 4));
            skillArrow[4].gameObject.SetActive(true);
            skillComand = 0;
            StartCoroutine(BuffTime());

        }
        else if (mp >= 8 && isLockOn == false && isSkill == false && cool[1] == false && Input.GetKeyDown(KeyCode.Alpha2))
        {
            //��ȭ��

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
        else if (mp >= 3 && isLockOn == false && cool[4] == false && Input.GetKeyDown(KeyCode.F))
        {
            // ī�������ϻ�붧 ��ų ���� �Ͻ����� ����
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

                // ��Ÿ���� 3��
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
                bowAnimation.ArrowShot();
                isSkill = false; // ��ųȭ�� �����ÿ� �ٸ� ��ų ��� �Ұ���

                isLockOn = false; // ȭ�� �߻� �� ���ӱ���� �ٽ� �ϱ� ���� üũ
                UnityEngine.Debug.Log("��");

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
        if (!isSound)
        {
          
        }
       
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
    /// �÷��̾��� �ǰ� �Լ�, NormalMounster���� ȣ���ؼ� �ǰ�
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        if (isInvincible) return; // ���� ������ ���� ���ظ� ���� ����
        remainHealth -= damage; // ���ط��� ���� ����
        remainHealth = Mathf.Clamp(remainHealth, 0, initialHealth); // ü���� ������ ���� �ʵ��� ����

        // Hpbar ����ui

        hp.text = $"{remainHealth}/{initialHealth}";
        hpBar.fillAmount = (float)remainHealth / initialHealth;

        if (!isRolling)
        {
            if (remainHealth > 0)
            {
                animator.SetBool("Hit", true); // �ǰ� �ִϸ��̼� ���
            }

        }
        if (remainHealth <= 0)
        {
            Die(); // ü���� 0 ���ϸ� ��� ó��

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
    //void HandleInput()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        if (mp > 5 && roll == 1)
    //        {
    //            // ��Ÿ���� 3��
    //            StartCoroutine(RollingCool());
    //            mp -= 5;
    //            Roll();
    //        }

    //    }
    //}
    void Roll()
    {
        // ������ �ִϸ��̼� ���
        animator.SetBool("Roll", true);
        isRolling = true;
        // ������ ���� �̵��� �Ÿ� ����
        float rollDistance = 5f;

        // ������ ���� (���� ĳ���Ͱ� �ٶ󺸰� �ִ� ����)
        Vector3 rollDirection = character.forward;

        // ������ �̵� ����
        StartCoroutine(PerformRoll(rollDirection, rollDistance));
    }


    IEnumerator PerformRoll(Vector3 direction, float distance)
    {
        float remainingDistance = distance;
        while (remainingDistance > 0)
        {
            // �� ������ �� �̵��� �Ÿ� ���
            float moveDistance = 6f * Time.deltaTime;

            // �̵��� �Ÿ��� ���� �Ÿ����� ���ٸ� ����
            moveDistance = Mathf.Min(moveDistance, remainingDistance);

            // ĳ���� ��Ʈ�ѷ��� ����Ͽ� �̵�
            controller.Move(direction * moveDistance);

            // ���� �Ÿ� ����
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
    //�ִϸ��̼� �̺�Ʈ
    public void RollFalse()
    {
        animator.SetBool("Roll", false);
    }
    // �ִϸ��̼� �̺�Ʈ
    public void RollInvincibility(int status)
    {
        isInvincible = (status != 0); // ������� 12�����Ӱ� ����
    }
    // �ִϸ��̼� �̺�Ʈ
    public void IsRolling()
    {
        isRolling = false;
    }
  
    // �ִϸ��̼� �̺�Ʈ
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

