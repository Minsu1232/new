
using System.Collections;
using System.Diagnostics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    [Header("Player Info")]
    public PlayerState playerState;
    // ĳ������ ����
    public int initialHealth;
    public int remainHealth;
    public int str;
    public int dex;
    public float walkSpeed;
    public float runSpeed;
    public int maxMp;
    public int mp;
    public int level;
    public float recovery; // ĳ���� ���� ȸ�� �ð�

    [Header("Player Attributes")]
    [SerializeField]
    Transform character;
    [SerializeField]
    Transform cameraArm;
    [SerializeField]
    Money money;
    public GameObject virtualCamera;
    public Camera mainCamera;
    public Transform defaultSpawn;
    public Transform tutorialSpawn;
    public QuestScriptable isTutorial;
    public GameObject healEffect;
    public Transform target;
    public Transform characterObj;
    public ParticleSystem counterSkill;
    public CharacterController controller;  // ������ �´� �������� �ڿ������� �ϱ����� ���
    public AudioClip buffSound;
    public AudioClip rollingSound;
    public AudioClip walkSound;
    public AudioClip runSound;
    public AudioClip counterSkillSound;
    public AudioClip petSummons;
    public GameObject buffEffect;


    [Header("Player Status")]
    public Text maxHP;
    public Text maxMP;
    public Text maxStr;
    public Text maxDex;
    public TextMeshProUGUI nowLevel;
    public Image hpBar;
    public Image mpBar;
    public TextMeshProUGUI hp;
    public TextMeshProUGUI mana;
    public TextMeshProUGUI panelText;
    public GameObject Statuspanel;




    [Header("Arrow Attributes")]
    [SerializeField]
    GameObject[] skillArrow;
    [SerializeField]
    Transform ShotPointer;
    [SerializeField]
    BowAnimation bowAnimation;
    public Image arrowStateImage;
    

    [Header("Skill Attributes")]
    [SerializeField]
    Image[] skillCool;
    public bool[] cool;
    [SerializeField]
    Text[] coolText;
    public int skillComand;
    public PetManager pet;
    public GameObject wings;


    // ������Ʈ
    Rigidbody rb;
    Animator animator;
  
    public Boss boss;

    // ĳ������Ʈ�ѷ� ������ ����
    private Vector3 playerVelocity; //ĳ������ y�� üũ��
    private bool isGrounded;
    float gravity = -8f; //���� �߷°�

    // ���Ӽ� ����
    int shotCount;
    bool isReadyToShoot = true;

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

    
    int a = 0; // tutorial ����Ʈ ��ġ Ȱ��ȭ������ ����
    // ��ų ��Ÿ�� üũ��
    bool isSkill;

    AudioSource audioSource;
    bool isSound;
    public speedboxText guideMan;
    bool isGuide; // ��ų���̵� Ŭ���� Ʈ����
    bool isGuideShot; // ���ذ� �� Ŭ���� Ʈ����
    bool isGuidStatusUp; // /���� ���� Ŭ���� Ʈ����
    [SerializeField] QuestScriptable questScriptable;
    public float detectionRadius = 5f; // �˻� �ݰ�
    private void OnEnable()
    {
        if (isTutorial.isTutorial == true)
        {
            gameObject.transform.position = defaultSpawn.transform.position;
            

        }
        else
        {
            gameObject.transform.position = tutorialSpawn.transform.position;
            gameObject.transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y + 180f, transform.rotation.y);
        }
        roll = 1;
        StopAllCoroutines();
        // ��ũ���ͺ� �ʱ�ȭ
        //initialHealth = playerState.health;
        //str = playerState.str;
        //dex = playerState.dex;
        //walkSpeed = playerState.walkSpeed;
        //runSpeed = playerState.runSpeed;
        //mp = playerState.mp;
        //maxMp = playerState.mp;
        //remainHealth = initialHealth;
        //mp = maxMp;
        //level = playerState.level;
        //skillComand = 0;

        //maxHP.text = initialHealth.ToString();
        //maxMP.text = mp.ToString();
        //maxStr.text = str.ToString();
        //maxDex.text = dex.ToString();
        //nowLevel.text = level.ToString();

        //isDie = false;
        //isWalk = true;

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }
    
    void Start()
    {
        //arrowStateImage.sprite;
        StartCoroutine(InitializePlayer());
        isSound = false;        
        shotCount = 1;
        hp.text = $"{remainHealth}/{playerState.health}";
        hpBar.fillAmount = remainHealth / playerState.health;
        audioSource = GetComponent<AudioSource>();
        isGuide = false;
        

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
            if (!GameManager.Instance.IsAnyUIActive())
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
        if (skillComand == 3 && isLockOn) // ���ȭ��� �����϶��� Ʈ��
        {
            wings.gameObject.SetActive(true);
        }
        else if(!isLockOn&&wings.activeSelf) // ���������� �޽�
        {
            
            wings.gameObject.SetActive(false);
        }

    }
    IEnumerator InitializePlayer()
    {
        while (!DataManager.Instance.isDataLoaded)
        {
            yield return null; // ������ �ε� �Ϸ� ���
        }

        // ������ �ε� �� �ʱ�ȭ ����
        SetupPlayer();
    }
    void SetupPlayer()
    {
        initialHealth = playerState.health;
        str = playerState.str;
        dex = playerState.dex;
        walkSpeed = playerState.walkSpeed;
        runSpeed = playerState.runSpeed;
        mp = playerState.mp;
        maxMp = playerState.mp;
        remainHealth = initialHealth;
        mp = maxMp;
        level = playerState.level;
        skillComand = 0;

        maxHP.text = initialHealth.ToString();
        maxMP.text = mp.ToString();
        maxStr.text = str.ToString();
        maxDex.text = dex.ToString();
        nowLevel.text = level.ToString();

        isDie = false;
        isWalk = true;

        
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
            if (isWalk == true && !animator.GetCurrentAnimatorStateInfo(0).IsName("StandUp")) // ĳ���� ������ ��� ���� �����ϼ� ����
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
        //StartCoroutine(SkillCool(a, b, c)); a = ��ų��Ÿ��,b = ��ų���� �ε���, c = ȭ������ �ε���
        if (mp >= 15 && isLockOn == false && isSkill == false && cool[0] == false && Input.GetKeyDown(KeyCode.Alpha1))
        {
            //���� (10�ʰ� �� 3 ����)
            mp -= 15;
            isSkill = true;
            
            audioSource.PlayOneShot(buffSound);
            StartCoroutine(SkillCool(20, 0, 4));
            skillArrow[4].gameObject.SetActive(true);
            ArrownDamage arrowDamage = skillArrow[0].GetComponent<ArrownDamage>(); // ĳ���� ����â �Ʒ� ȭ���� ���� �̹��� ������Ʈ
            arrowStateImage.sprite = arrowDamage.damageScriptable.arrowStateIcon;
            skillComand = 0;
            StartCoroutine(BuffTime());
            IsGuide();
        }
        else if (mp >= 8 && isLockOn == false && isSkill == false && cool[1] == false && Input.GetKeyDown(KeyCode.Alpha2))
        {
            //��ȭ��

            mp -= 8;
            isSkill = true;
            StartCoroutine(SkillCool(6, 1, 1));
            skillArrow[1].gameObject.SetActive(true);
            ArrownDamage arrowDamage = skillArrow[1].GetComponent<ArrownDamage>();
            arrowStateImage.sprite = arrowDamage.damageScriptable.arrowStateIcon;
            skillComand = 1;
            IsGuide();
        }
        else if (mp >= 6 && isLockOn == false && isSkill == false && cool[2] == false && Input.GetKeyDown(KeyCode.Alpha3))
        {
            //��ȭ��
            mp -= 6;
            isSkill = true;
            StartCoroutine(SkillCool(5, 2, 2));
            skillArrow[2].gameObject.SetActive(true);
            ArrownDamage arrowDamage = skillArrow[2].GetComponent<ArrownDamage>();
            arrowStateImage.sprite = arrowDamage.damageScriptable.arrowStateIcon;
            skillComand = 2;
            IsGuide();
        }
        else if (mp >= 12 && isLockOn == false && isSkill == false && cool[3] == false && Input.GetKeyDown(KeyCode.Alpha4))
        {
            //��ȭ��
            mp -= 12;
            isSkill = true;
            StartCoroutine(SkillCool(10, 3, 3));
            skillArrow[3].gameObject.SetActive(true);
            ArrownDamage arrowDamage = skillArrow[3].GetComponent<ArrownDamage>();
            arrowStateImage.sprite = arrowDamage.damageScriptable.arrowStateIcon;
            skillComand = 3;
            IsGuide();
        }
        else if (mp>= 15 && isLockOn == false && isSkill == false && Input.GetKeyDown(KeyCode.Alpha5))
        {
            //�⺻ȭ��
            mp -= 15;
            isSkill = true;
            StartCoroutine(SkillCool(30, 6, 0));
            audioSource.PlayOneShot(petSummons);
            ArrownDamage arrowDamage = skillArrow[0].GetComponent<ArrownDamage>();
            arrowStateImage.sprite = arrowDamage.damageScriptable.arrowStateIcon;
            StartCoroutine(PetUsing());
            
            skillComand = 0;
            IsGuide();


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
            ArrownDamage arrowDamage = skillArrow[0].GetComponent<ArrownDamage>();
            arrowStateImage.sprite = arrowDamage.damageScriptable.arrowStateIcon;
            IsGuide();

        }
        else if (mp >= 5 && isLockOn == false && cool[5] == false && Input.GetKeyDown(KeyCode.Space))
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {

                // ��Ÿ���� 3��
                StartCoroutine(SkillCool(3, 5, 0));
                audioSource.PlayOneShot(rollingSound);
                ArrownDamage arrowDamage = skillArrow[0].GetComponent<ArrownDamage>();
                arrowStateImage.sprite = arrowDamage.damageScriptable.arrowStateIcon;
                mp -= 5;
                Roll();
                IsGuide();

            }
        }
        

    }

    IEnumerator SkillCool(int coolTime, int skillCoolIndex, int skillArrowIndex) // ��ų ��Ÿ�� �� UI ����
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
    IEnumerator PetUsing()
    {
        pet.gameObject.SetActive(true);
        StartCoroutine(PetAttack()); // ���� �ڷ�ƾ
        yield return new WaitForSeconds(14f);
        if(pet != null)
        {
            pet.animator.SetTrigger("Leave");
        }        
        yield return new WaitForSeconds(1.3f);
        StopCoroutine(PetAttack()); // ��������� �ڷ�ƾ ����
        pet.gameObject.SetActive(false);
    }
    IEnumerator PetAttack()
    {
        while (pet.gameObject.activeSelf) // ��ų�� Ȱ��ȭ �Ǿ� �ִ� ����
        {
            yield return new WaitForSeconds(3f); // ���� �ֱ�� 3��
            if (target != null)
            {
                IDamageable idm = target.GetComponentInParent<IDamageable>(); //transform���� GetComponentInParent�� ����� �θ� �������̽� �Ҵ�
                pet.transform.parent = target.parent;
                
                pet.animator.SetTrigger("Attack");
                idm.TakeDamage(15,5,2,true);
                
                pet.transform.localPosition = new Vector3(-0.05f, 0.25f, 0.88f);
                pet.transform.LookAt(target); //Ÿ���� �ٶ󺸰�
                
            }
            yield return new WaitForSeconds(1.5f);
            pet.transform.parent = characterObj.transform;
            pet.transform.localPosition = new Vector3(-1.5f, 0.33f, 2.1f);
            pet.transform.localRotation = Quaternion.identity; ;
           
        }
       
        
    }

    void ArrowShot()
    {
        if (shotCount > 0 && isReadyToShoot)
        {
            if (Input.GetMouseButtonDown(0) && isLockOn == true)
            {
             
                    
                
                ArrownDamage arrowDamage = skillArrow[0].GetComponent<ArrownDamage>(); // ĳ���� ����â �Ʒ� ȭ���� ���� �̹��� ������Ʈ
                arrowStateImage.sprite = arrowDamage.damageScriptable.arrowStateIcon;
                isReadyToShoot = false;  // �߻� �غ� ���¸� false�� �����Ͽ� �߰� �߻縦 �����ϴ�.
                bowAnimation.ArrowShot();
                isSkill = false; // ��ųȭ�� �����ÿ� �ٸ� ��ų ��� �Ұ���

                isLockOn = false; // ȭ�� �߻� �� ���ӱ���� �ٽ� �ϱ� ���� üũ
                UnityEngine.Debug.Log("��");

                Vector3 directionToTarget = (target.position - ShotPointer.transform.position).normalized;

                // Ÿ�� ������ �ٶ󺸴� �⺻ ȸ��
                Quaternion baseRotation = Quaternion.LookRotation(directionToTarget);

                // �߰����� X�� 90�� ȸ��
                Quaternion additionalRotation = Quaternion.Euler(0, 0, 0);

                // ���� ȸ�� ���
                Quaternion finalRotation = baseRotation * additionalRotation;

                // ȭ�� �ν��Ͻ� ���� �� ���� ȸ�� ����
               
                GameObject arrowInstance = Instantiate(skillArrow[skillComand], ShotPointer.transform.position, finalRotation);

                Destroy(arrowInstance, 6f); // 6�ʵ� ȭ�� �����
                Rigidbody arrowRb = arrowInstance.AddComponent<Rigidbody>(); // Rigidbody ������Ʈ ���� �߰�
                arrowRb.useGravity = false;
                ArrowPhysics(arrowRb, directionToTarget);
                shotCount--;  // shotCount�� ���⿡�� �� ���� ���ҽ�ŵ�ϴ�.
                StartCoroutine(ShotDelay());
                skillComand = 0;
                if (!isGuideShot)
                {
                    isGuideShot = true;
                    questScriptable.isGuide = true;
                }
            }
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
        yield return new WaitForSeconds(1f);  // �߻� �� 1�� ���� ����մϴ�.
        isReadyToShoot = true;  // �߻� �غ� ���¸� true�� �����Ͽ� �ٽ� �߻��� �� �ֵ��� �մϴ�.
        shotCount += 1;

    }
    //�����ð����� �� 3 ����
    IEnumerator BuffTime()
    {
        str += 3;
        buffEffect.SetActive(true);
        yield return new WaitForSeconds(10f);
        buffEffect.SetActive(false);
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
        remainHealth = Mathf.Clamp(remainHealth, 0, playerState.health); // ü���� ������ ���� �ʵ��� ����

        // Hpbar ����ui

        hp.text = $"{remainHealth}/{playerState.health}";
        hpBar.fillAmount = (float)remainHealth / playerState.health;

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
        mana.text = $"{mp}/{playerState.mp}";
        if (maxMp > 0)  // �ִ� MP�� 0���� Ŭ ���� ���
        {
            mpBar.fillAmount = (float)mp / playerState.mp;
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
        if (other.gameObject.tag == "Potal")
        {
            Quest quest = FindObjectOfType<Quest>(); // ��Ż�� ź �� Ʃ�丮���� �ϷḦ ����          
            
            Vector3 tutorial = new Vector3(-72.19f, 4.01f, 22.96f);
            if(other.gameObject.name == "Tutorial2StartZone")
            {
                if (quest.questScriptables[2].isTutorial)
                {
                    Destroy(other.gameObject);
                }
                else
                {
                    UnityEngine.Debug.Log("�ó׸ӽ�");
                    virtualCamera.gameObject.SetActive(true);
                    Destroy(other.gameObject);
                }
               


                mainCamera.depth = -1;
                
            }
            quest.questScriptables[2].isTutorial = true; //����Ʈ�� Ʈ����
            if (other.gameObject.name == "Tutorial2ClearZone")
            {

                a++;
                if (a == 1)
                {
                    other.transform.position = tutorial;
                }
                else if (a == 2)
                {
                    other.gameObject.SetActive(false);
                }

            }
        }
    }
    private int CalculateUpgradeCost(int upgradeCount)
    {// ���� 10���� �ø��� �ش� ���� �ø��� ����� 1.3�谡 ��
        return (int)(playerState.baseUpgradeCost * Mathf.Pow(1.3f, upgradeCount / 10));
    }
    // �÷��̾� �������ͽ� ��ư ���� �ż���
    //public void MaxHpUpButton()
    //{
 
    //    int cost = CalculateUpgradeCost(playerState.hpUpgradeCount);
    //    if (money.money >= cost)
    //    {
    //        IsGuideStatusUp();
    //        playerState.health += 15;
    //        initialHealth = playerState.health;            
    //        maxHP.text = playerState.health.ToString();
    //        playerState.level++;
    //        nowLevel.text = playerState.level.ToString();
    //        MoneyManager.Instance.money.money -= cost;
    //        //Inventory.instance.money.text = money.money.ToString();
    //        hp.text = $"{remainHealth}/{playerState.health}";
    //        playerState.hpUpgradeCount++; // ���׷��̵� Ƚ�� ����
    //    }


    //}
    //public void HpStatusPanel()
    //{
    //    Statuspanel.SetActive(true);
    //    panelText.text = "������ �ɷ�ġ��(Hp)(��)�� �½��ϱ�?";
    //}
    //public void MpStatusPanel()
    //{
    //    Statuspanel.SetActive(true);
    //    panelText.text = "������ �ɷ�ġ��(Mp)(��)�� �½��ϱ�?";
    //}
    //public void StrStatusPanel()
    //{
    //    Statuspanel.SetActive(true);
    //    panelText.text = "������ �ɷ�ġ��(Str)(��)�� �½��ϱ�?";
    //}
    //public void DexStatusPanel()
    //{
    //    Statuspanel.SetActive(true);
    //    panelText.text = "������ �ɷ�ġ��(Dex)(��)�� �½��ϱ�?";
    //}
    //public void MaxMpUpButton()
    //{
    //    int cost = CalculateUpgradeCost(playerState.mpUpgradeCount);
    //    if (money.money >= cost)
    //    {
    //        IsGuideStatusUp();
    //        playerState.mp += 1;
    //        mp = playerState.mp;
    //        maxMP.text = playerState.mp.ToString();
    //        playerState.level++;
    //        nowLevel.text = playerState.level.ToString();
    //        MoneyManager.Instance.money.money -= cost;
    //        //Inventory.instance.money.text = money.money.ToString();
    //        playerState.mpUpgradeCount++; // ���׷��̵� Ƚ�� ����
    //    }
    //}
    //public void MaxStrUpButton()
    //{//str���� ������ ������ ArrowDamage��ũ��Ʈ���� ����
    //    int cost = CalculateUpgradeCost(playerState.strUpgradeCount);
    //    if (money.money >= cost)
    //    {
    //        IsGuideStatusUp();
    //        playerState.str += 1;
    //        str = playerState.str;
    //        maxStr.text = playerState.str.ToString();
    //        playerState.level++;
    //        nowLevel.text = playerState.level.ToString();
    //        MoneyManager.Instance.money.money -= cost;
    //        //Inventory.instance.money.text = money.money.ToString();
    //        playerState.strUpgradeCount++; // ���׷��̵� Ƚ�� ����
    //    }

    //}
    //public void MaxDexUpButton()
    //{
    //    int cost = CalculateUpgradeCost(playerState.dexUpgradeCount);
    //    if (money.money >= cost)
    //    {
    //        IsGuideStatusUp();
    //        playerState.dex += 1;
    //        dex = playerState.dex;
    //        maxDex.text = playerState.dex.ToString();
    //        playerState.level++;
    //        nowLevel.text = playerState.level.ToString();
    //        UpdateSpeed();
    //        MoneyManager.Instance.money.money -= cost;
    //        //Inventory.instance.money.text = money.money.ToString();
    //        playerState.dexUpgradeCount++; // ���׷��̵� Ƚ�� ����
    //    }

    //}
    public void UpgradeStat(ref int stat, ref int upgradeCount, Text statText, string statName)
    {
        int cost = CalculateUpgradeCost(upgradeCount); 
        if (money.money >= cost) // ����� ������ ������
        {
            IsGuideStatusUp(); // ���̵� �Ϸ�
            stat += 1; // �ش� ���� ����
            statText.text = stat.ToString();
            playerState.level++;
            nowLevel.text = playerState.level.ToString();
            MoneyManager.Instance.money.money -= cost;
            upgradeCount++;
        }
    }
    public void MaxHpUpButton()
    {
        UpgradeStat(ref playerState.health, ref playerState.hpUpgradeCount, maxMP, "Hp");
    }
    public void MaxMpUpButton()
    {
        UpgradeStat(ref playerState.mp, ref playerState.mpUpgradeCount, maxMP, "Mp");
    }

    public void MaxStrUpButton()
    {
        UpgradeStat(ref playerState.str, ref playerState.strUpgradeCount, maxStr, "Str");
    }

    public void MaxDexUpButton()
    {
        UpgradeStat(ref playerState.dex, ref playerState.dexUpgradeCount, maxDex, "Dex");
        UpdateSpeed();
    }
    private void UpdateSpeed()
    {
        // playerState ������Ʈ�� ����Ͽ� �̵� �ӵ� ������Ʈ
        playerState.walkSpeed += 0.1f;  // dex 1�� ��ũ ���ǵ� 0.1 ����
        walkSpeed = playerState.walkSpeed; // ���� ������ ����� ���� �� ������Ʈ

        playerState.runSpeed += 0.1f;   // dex 1�� �� ���ǵ� 0.1 ����
        runSpeed = playerState.runSpeed; // ���� ������ ����� ���� �� ������Ʈ
       
    }
    public void ShowStatusPanel(string stat)
    {
        Statuspanel.SetActive(true);
        panelText.text = $"������ �ɷ�ġ��({stat})(��)�� �½��ϱ�?";
        currentStat = stat;  // ���� ������ ������ ����
    }

    private string currentStat;  // ���� ������ ������ �����ϴ� ����

    public void AgreeUpgrade()
    {
        switch (currentStat)
        {
            case "Hp":
                MaxHpUpButton();
                break;
            case "Mp":
                MaxMpUpButton();
                break;
            case "Str":
                MaxStrUpButton();
                break;
            case "Dex":
                MaxDexUpButton();
                break;
        }
        Statuspanel.SetActive(false);  // �г� �ݱ�
    }
    public void DisAgreeeUpgrade()
    {
        Statuspanel.SetActive(false);
    }
    void IsGuide() // ��ų���̵� Ŭ���� Ʈ����
    {
        if(isGuide == false && !guideMan.isGuideStart)
        {
            isGuide = true;
            questScriptable.isGuide = true;
            
        }
    }
    void IsGuideStatusUp() // ��ų���̵� Ŭ���� Ʈ����
    {
        if (!isGuidStatusUp)
        {
            isGuidStatusUp = true;
            questScriptable.isGuide = true;
        }
    }
}

