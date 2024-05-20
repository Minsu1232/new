using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

// 인터페이스 사용해 피격 판정
public class NormalMonster : MonoBehaviour, IDamageable
{
    [Header("Monster Attributes")]
    [SerializeField]
    NormalMonsterScriptable normalMonsterObj;
    public int initialHealth; // 시작 체력
    public int remainHealth; // 남은 체력
    public int damage;
    [SerializeField]
    NavMeshAgent navMeshAgent;
    [SerializeField]
    public GameObject portal;
    [SerializeField]
    Money money;

    [Header("Monster UI")]
    public Text hp;
    [SerializeField]
    Image hpBar;

    [Header("Player Attributes")]
    public Player player;

    [Header("Quest")]
    public QuestScriptable quest;

    Animator animator;

    int walkSpeed;
    int runSpeed;

    // 몬스터의 상태 체크
    bool isAttack;
    bool isDie;
    bool isKill;
    bool isGettingHit;
    bool isAnimalsHit;

    [SerializeField]
    private float hitRecoveryTime = 5.0f; // 피격 후 회복 시간
    private float currentHitRecoveryTimer;

    [Header("Animals Specific Attributes")]
    public float patrolRadius = 10f;  // 초기 이동 범위
    public float detectionRadius = 15f;  // 플레이어 감지 범위

    private Vector3 initialPosition; // 초기위치
    private Vector3 targetPosition;


    // Start is called before the first frame update

    private void Awake()
    {
        isAttack = false;
        isAnimalsHit = false;


    }
    private void OnEnable()
    {

        initialHealth = normalMonsterObj.health;
        damage = normalMonsterObj.damage;
        walkSpeed = normalMonsterObj.walkSpeed;
        runSpeed = normalMonsterObj.runSpeed;
        remainHealth = initialHealth;

    }

    private void Start()
    {
        if(player == null)
        {
            player = FindObjectOfType<Player>(); // 만약 player가 할당이 안되어있으면
        }
        isDie = false;
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        if(navMeshAgent != null)
        {
            navMeshAgent.speed = walkSpeed;
        }
        // 초기 위치 저장
        initialPosition = transform.position;
        SetNewRandomTarget();
        //시작 ui
        hp.text = $"{initialHealth}/{initialHealth}";
        hpBar.fillAmount = remainHealth / initialHealth;
    }

    // Update is called once per frame
    void Update()
    {
       
        if (!isDie)
        {
            if (gameObject.CompareTag("Animals"))
            {
                if (Vector3.Distance(transform.position, player.transform.position) <= detectionRadius || isAnimalsHit ==true) // isAnimalsHit는 피격시 상시로 player를 쫓기 위한 bool타입 
                {                                                                                                              // takedamage발동시 발동
                    // 플레이어가 감지 범위 내에 있을 때
                    navMeshAgent.enabled = true;
                    Attack();
                    MonsterMove();
                }
                else
                {
                    // 플레이어가 감지 범위 밖에 있을 때
                    navMeshAgent.enabled = false;
                    MoveTowardsTarget();
                }
            }
            else
            {
                MonsterMove();
                Attack();
            }
            if (currentHitRecoveryTimer > 0)
            {
                currentHitRecoveryTimer -= Time.deltaTime;
            }

        }


    }
 /// <summary>
 /// ////////////////////////////// animal이동용 매서드
 /// </summary>
    void SetNewRandomTarget()
    {
        float randomX = Random.Range(-detectionRadius, detectionRadius);
        float randomZ = Random.Range(-detectionRadius, detectionRadius);
        targetPosition = initialPosition + new Vector3(randomX, 0, randomZ);
        // 랜덤한 위치를 선정 해 준뒤
    }
    void MoveTowardsTarget()
    {
        // 이동
        transform.position = Vector3.MoveTowards(transform.position, targetPosition,normalMonsterObj.walkSpeed * Time.deltaTime);
        transform.LookAt(new Vector3(targetPosition.x, transform.position.y, targetPosition.z)); // 방향전환시 전방보게끔
        CheckAndSetNewTargetIfNeeded();
    }
    void CheckAndSetNewTargetIfNeeded()
    {
        // 거리안에 없으면 다시 위치 지정
        if (Vector3.Distance(transform.position, targetPosition) < 1f)
        {
            SetNewRandomTarget();
        }
    }
    ////////////////////////////////////////



    /// <summary>
    /// 몬스터의 피격 함수 ArrowDamage에서 호출.
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage, int neutralizeValu, int destruction, bool shouldTriggerHitAnimation = true)
    {

        isAnimalsHit = true;
        if (remainHealth > 0)
        {
            remainHealth -= damage;
            if (gameObject.CompareTag("Animals") && navMeshAgent.enabled == false) // animals들만 해당
            {
                currentHitRecoveryTimer = hitRecoveryTime; // 피격 후 회복 타이머 초기화
                navMeshAgent.enabled = true;
               
            }
        }
        // health값이 음수가 되지 않게 제어
        remainHealth = Mathf.Clamp(remainHealth, 0, initialHealth);
        hp.text = $"{remainHealth}/{initialHealth}";
        hpBar.fillAmount = (float)remainHealth / initialHealth;
        if(gameObject.name != "training_dummy")
        {
            StartCoroutine(Gethit());
        }
        
        if (remainHealth <= 0)
        {
            Die();
        }
    }


    // 몬스터가 죽는 함수
    void Die()
    {
        if (!isDie)
        {
            money.money += normalMonsterObj.dropMoney;
            if(gameObject.name == "training_dummy")
            {
                Destroy(gameObject);
            }
            if (portal != null)
            {
                portal.gameObject.SetActive(true);
            }
            
            if(quest != null)
            {
                quest.killed += 1;
            }
            
            Debug.Log("Monster died.");
            animator.SetTrigger("Die");
            isDie = true;
            navMeshAgent.enabled = false;
            if (gameObject.CompareTag("Animals"))
            {
                Destroy(gameObject, 3f); // 야생동물을 파괴하며 재생성
            }
            else
            {
                StartCoroutine(DeactivateAfterDelay());//3초뒤false ( 갈때마다 호출을위해 파괴 x )
            }
            
        }

    }
    IEnumerator DeactivateAfterDelay()
    {
        // 3초간 대기
        yield return new WaitForSeconds(3f);

        // 오브젝트 비활성화
        gameObject.SetActive(false);
    }
    //navmesh 이동 함수
    void MonsterMove()
    {
        if (isAttack == false && !isDie && player != null && navMeshAgent != null)
        {
            navMeshAgent.enabled = true;

            navMeshAgent.SetDestination(player.transform.position);
            animator.SetBool("Move", true);
            animator.SetBool("Attack", false);
        }


    }
    // 몬스터의 공격함수
    void Attack()
    {
        if (animator != null)
        {
            // 플레이어와의 거리 계산
            float distance = Vector3.Distance(gameObject.transform.position, player.transform.position);

            // 거리가 3f 이하일 경우 Attack 메서드 호출
            if (distance <= 3f && player.remainHealth > 0)
            {
                // 플레이어를 공격
                if (!isKill)
                {
                    animator.SetBool("Attack", true);
                    animator.SetBool("Move", false);
                    isAttack = true;
                    navMeshAgent.enabled = false;
                }


            }
            else if (player.remainHealth <= 0)
            {
                // player가 죽었을때의 행동
                if (!isKill)
                {
                    animator.SetTrigger("Kill");
                    isKill = true;
                }

            }
            else
            {
                // 거리가 멀어지면 다시 이동
                isAttack = false;
                animator.SetBool("Move", true);
                animator.SetBool("Attack", false);

            }

        }
    }
       
    // 애니메이션 이벤트용 매서드
    void Hit()
    {
        player.TakeDamage(damage);

    }
    IEnumerator Gethit()
    {
        if (isGettingHit)  // 이미 피격 상태인 경우 더 이상 진행하지 않음
            yield break;

        isGettingHit = true;
        animator.SetBool("Gethit", true);
        navMeshAgent.speed = 0;  // 속도를 0으로 설정

        yield return new WaitForSeconds(1.3f);  // 피격 애니메이션 재생 시간 동안 대기

        navMeshAgent.speed = walkSpeed;  // 속도 복원
        animator.SetBool("Gethit", false);
        isGettingHit = false;  // 피격 상태 해제
    }





}
