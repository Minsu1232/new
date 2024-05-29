
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
    // 캐릭터의 정보
    public int initialHealth;
    public int remainHealth;
    public int str;
    public int dex;
    public float walkSpeed;
    public float runSpeed;
    public int maxMp;
    public int mp;
    public int level;
    public float recovery; // 캐릭터 마나 회복 시간

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
    public CharacterController controller;  // 지형에 맞는 움직임을 자연스럽게 하기위해 사용
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


    // 컴포넌트
    Rigidbody rb;
    Animator animator;
  
    public Boss boss;

    // 캐릭터컨트롤러 관리용 변수
    private Vector3 playerVelocity; //캐릭터의 y값 체크용
    private bool isGrounded;
    float gravity = -8f; //받을 중력값

    // 연속샷 방지
    int shotCount;
    bool isReadyToShoot = true;

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

    
    int a = 0; // tutorial 퀘스트 위치 활성화오프용 변수
    // 스킬 쿨타임 체크용
    bool isSkill;

    AudioSource audioSource;
    bool isSound;
    public speedboxText guideMan;
    bool isGuide; // 스킬가이드 클리어 트리거
    bool isGuideShot; // 조준과 샷 클리어 트리거
    bool isGuidStatusUp; // /스탯 관리 클리어 트리거
    [SerializeField] QuestScriptable questScriptable;
    public float detectionRadius = 5f; // 검색 반경
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
        // 스크립터블 초기화
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
            if (!GameManager.Instance.IsAnyUIActive())
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
        if (skillComand == 3 && isLockOn) // 골드화살과 락온일때만 트루
        {
            wings.gameObject.SetActive(true);
        }
        else if(!isLockOn&&wings.activeSelf) // 켜저있으면 펄스
        {
            
            wings.gameObject.SetActive(false);
        }

    }
    IEnumerator InitializePlayer()
    {
        while (!DataManager.Instance.isDataLoaded)
        {
            yield return null; // 데이터 로드 완료 대기
        }

        // 데이터 로드 후 초기화 로직
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
            if (isWalk == true && !animator.GetCurrentAnimatorStateInfo(0).IsName("StandUp")) // 캐릭터 생성시 모션 동안 움직일수 없음
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
        //StartCoroutine(SkillCool(a, b, c)); a = 스킬쿨타임,b = 스킬쿨의 인덱스, c = 화살종류 인덱스
        if (mp >= 15 && isLockOn == false && isSkill == false && cool[0] == false && Input.GetKeyDown(KeyCode.Alpha1))
        {
            //버프 (10초간 힘 3 증가)
            mp -= 15;
            isSkill = true;
            
            audioSource.PlayOneShot(buffSound);
            StartCoroutine(SkillCool(20, 0, 4));
            skillArrow[4].gameObject.SetActive(true);
            ArrownDamage arrowDamage = skillArrow[0].GetComponent<ArrownDamage>(); // 캐릭터 상태창 아래 화살의 상태 이미지 업데이트
            arrowStateImage.sprite = arrowDamage.damageScriptable.arrowStateIcon;
            skillComand = 0;
            StartCoroutine(BuffTime());
            IsGuide();
        }
        else if (mp >= 8 && isLockOn == false && isSkill == false && cool[1] == false && Input.GetKeyDown(KeyCode.Alpha2))
        {
            //불화살

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
            //독화살
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
            //금화살
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
            //기본화살
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
            // 카운터패턴사용때 스킬 사용시 일시적인 경직
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

                // 쿨타임은 3초
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

    IEnumerator SkillCool(int coolTime, int skillCoolIndex, int skillArrowIndex) // 스킬 쿨타임 및 UI 관리
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
    IEnumerator PetUsing()
    {
        pet.gameObject.SetActive(true);
        StartCoroutine(PetAttack()); // 공격 코루틴
        yield return new WaitForSeconds(14f);
        if(pet != null)
        {
            pet.animator.SetTrigger("Leave");
        }        
        yield return new WaitForSeconds(1.3f);
        StopCoroutine(PetAttack()); // 명시적으로 코루틴 중지
        pet.gameObject.SetActive(false);
    }
    IEnumerator PetAttack()
    {
        while (pet.gameObject.activeSelf) // 스킬이 활성화 되어 있는 동안
        {
            yield return new WaitForSeconds(3f); // 공격 주기는 3초
            if (target != null)
            {
                IDamageable idm = target.GetComponentInParent<IDamageable>(); //transform값에 GetComponentInParent를 사용해 부모에 인터페이스 할당
                pet.transform.parent = target.parent;
                
                pet.animator.SetTrigger("Attack");
                idm.TakeDamage(15,5,2,true);
                
                pet.transform.localPosition = new Vector3(-0.05f, 0.25f, 0.88f);
                pet.transform.LookAt(target); //타겟을 바라보게
                
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
             
                    
                
                ArrownDamage arrowDamage = skillArrow[0].GetComponent<ArrownDamage>(); // 캐릭터 상태창 아래 화살의 상태 이미지 업데이트
                arrowStateImage.sprite = arrowDamage.damageScriptable.arrowStateIcon;
                isReadyToShoot = false;  // 발사 준비 상태를 false로 설정하여 추가 발사를 막습니다.
                bowAnimation.ArrowShot();
                isSkill = false; // 스킬화살 장전시엔 다른 스킬 사용 불가용

                isLockOn = false; // 화살 발사 후 에임기능을 다시 하기 위한 체크
                UnityEngine.Debug.Log("쏨");

                Vector3 directionToTarget = (target.position - ShotPointer.transform.position).normalized;

                // 타겟 방향을 바라보는 기본 회전
                Quaternion baseRotation = Quaternion.LookRotation(directionToTarget);

                // 추가적인 X축 90도 회전
                Quaternion additionalRotation = Quaternion.Euler(0, 0, 0);

                // 최종 회전 계산
                Quaternion finalRotation = baseRotation * additionalRotation;

                // 화살 인스턴스 생성 시 최종 회전 적용
               
                GameObject arrowInstance = Instantiate(skillArrow[skillComand], ShotPointer.transform.position, finalRotation);

                Destroy(arrowInstance, 6f); // 6초뒤 화살 사라짐
                Rigidbody arrowRb = arrowInstance.AddComponent<Rigidbody>(); // Rigidbody 컴포넌트 동적 추가
                arrowRb.useGravity = false;
                ArrowPhysics(arrowRb, directionToTarget);
                shotCount--;  // shotCount를 여기에서 한 번만 감소시킵니다.
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
        yield return new WaitForSeconds(1f);  // 발사 후 1초 동안 대기합니다.
        isReadyToShoot = true;  // 발사 준비 상태를 true로 설정하여 다시 발사할 수 있도록 합니다.
        shotCount += 1;

    }
    //버프시간동안 힘 3 증가
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
    /// 플레이어의 피격 함수, NormalMounster에서 호출해서 피격
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        if (isInvincible) return; // 무적 상태일 때는 피해를 받지 않음
        remainHealth -= damage; // 피해량을 먼저 적용
        remainHealth = Mathf.Clamp(remainHealth, 0, playerState.health); // 체력이 음수가 되지 않도록 제어

        // Hpbar 감소ui

        hp.text = $"{remainHealth}/{playerState.health}";
        hpBar.fillAmount = (float)remainHealth / playerState.health;

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
        mana.text = $"{mp}/{playerState.mp}";
        if (maxMp > 0)  // 최대 MP가 0보다 클 때만 계산
        {
            mpBar.fillAmount = (float)mp / playerState.mp;
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
        if (other.gameObject.tag == "Potal")
        {
            Quest quest = FindObjectOfType<Quest>(); // 포탈을 탄 후 튜토리얼의 완료를 위해          
            
            Vector3 tutorial = new Vector3(-72.19f, 4.01f, 22.96f);
            if(other.gameObject.name == "Tutorial2StartZone")
            {
                if (quest.questScriptables[2].isTutorial)
                {
                    Destroy(other.gameObject);
                }
                else
                {
                    UnityEngine.Debug.Log("시네머신");
                    virtualCamera.gameObject.SetActive(true);
                    Destroy(other.gameObject);
                }
               


                mainCamera.depth = -1;
                
            }
            quest.questScriptables[2].isTutorial = true; //퀘스트의 트리거
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
    {// 스탯 10번을 올리면 해당 스탯 올리는 비용이 1.3배가 됨
        return (int)(playerState.baseUpgradeCost * Mathf.Pow(1.3f, upgradeCount / 10));
    }
    // 플레이어 스테이터스 버튼 관리 매서드
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
    //        playerState.hpUpgradeCount++; // 업그레이드 횟수 증가
    //    }


    //}
    //public void HpStatusPanel()
    //{
    //    Statuspanel.SetActive(true);
    //    panelText.text = "선택한 능력치가(Hp)(이)가 맞습니까?";
    //}
    //public void MpStatusPanel()
    //{
    //    Statuspanel.SetActive(true);
    //    panelText.text = "선택한 능력치가(Mp)(이)가 맞습니까?";
    //}
    //public void StrStatusPanel()
    //{
    //    Statuspanel.SetActive(true);
    //    panelText.text = "선택한 능력치가(Str)(이)가 맞습니까?";
    //}
    //public void DexStatusPanel()
    //{
    //    Statuspanel.SetActive(true);
    //    panelText.text = "선택한 능력치가(Dex)(이)가 맞습니까?";
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
    //        playerState.mpUpgradeCount++; // 업그레이드 횟수 증가
    //    }
    //}
    //public void MaxStrUpButton()
    //{//str관련 데미지 로직은 ArrowDamage스크립트에서 관리
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
    //        playerState.strUpgradeCount++; // 업그레이드 횟수 증가
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
    //        playerState.dexUpgradeCount++; // 업그레이드 횟수 증가
    //    }

    //}
    public void UpgradeStat(ref int stat, ref int upgradeCount, Text statText, string statName)
    {
        int cost = CalculateUpgradeCost(upgradeCount); 
        if (money.money >= cost) // 비용이 돈보다 적으면
        {
            IsGuideStatusUp(); // 가이드 완료
            stat += 1; // 해당 스탯 증가
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
        // playerState 오브젝트를 사용하여 이동 속도 업데이트
        playerState.walkSpeed += 0.1f;  // dex 1당 워크 스피드 0.1 증가
        walkSpeed = playerState.walkSpeed; // 게임 내에서 사용할 실제 값 업데이트

        playerState.runSpeed += 0.1f;   // dex 1당 런 스피드 0.1 증가
        runSpeed = playerState.runSpeed; // 게임 내에서 사용할 실제 값 업데이트
       
    }
    public void ShowStatusPanel(string stat)
    {
        Statuspanel.SetActive(true);
        panelText.text = $"선택한 능력치가({stat})(이)가 맞습니까?";
        currentStat = stat;  // 현재 선택한 스탯을 저장
    }

    private string currentStat;  // 현재 선택한 스탯을 저장하는 변수

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
        Statuspanel.SetActive(false);  // 패널 닫기
    }
    public void DisAgreeeUpgrade()
    {
        Statuspanel.SetActive(false);
    }
    void IsGuide() // 스킬가이드 클리어 트리거
    {
        if(isGuide == false && !guideMan.isGuideStart)
        {
            isGuide = true;
            questScriptable.isGuide = true;
            
        }
    }
    void IsGuideStatusUp() // 스킬가이드 클리어 트리거
    {
        if (!isGuidStatusUp)
        {
            isGuidStatusUp = true;
            questScriptable.isGuide = true;
        }
    }
}

