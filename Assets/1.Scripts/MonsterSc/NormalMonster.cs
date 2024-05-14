using System.Collections;
using System.Collections.Generic;
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




    // Start is called before the first frame update

    private void Awake()
    {
        isAttack = false;
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
        isDie = false;
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        if(navMeshAgent != null)
        {
            navMeshAgent.speed = walkSpeed;
        }
       
        //시작 ui
        hp.text = $"{initialHealth}/{initialHealth}";
        hpBar.fillAmount = remainHealth / initialHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDie)
        {
            MonsterMove();
            Attack();
        }


    }

    /// <summary>
    /// 몬스터의 피격 함수 ArrowDamage에서 호출.
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage, int neutralizeValu, int destruction, bool shouldTriggerHitAnimation = true)
    {

        if (remainHealth > 0)
        {
            remainHealth -= damage;
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
            if(gameObject.name == "training_dummy")
            {
                Destroy(gameObject);
            }
            portal.gameObject.SetActive(true);
            if(quest != null)
            {
                quest.killed += 1;
            }
            
            Debug.Log("Monster died.");
            animator.SetTrigger("Die");
            isDie = true;
            navMeshAgent.enabled = false;
            StartCoroutine(DeactivateAfterDelay());//3초뒤false ( 갈때마다 호출을위해 파괴 x )
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
