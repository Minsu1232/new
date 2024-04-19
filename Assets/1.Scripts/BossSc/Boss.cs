using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

// 인터페이스 사용해 피격 판정
public class Boss : MonoBehaviour, IDamageable
{
    [Header("Boss Info")]
    [SerializeField]
    BossScriptable bossScriptable;
    public int initialHealth; // 시작 체력
    public int remainHealth; // 남은 체력
    public int damage;
    public int neutralizeValue; // 시작 무력화
    public int remainNeutralizeVlaue; // 남은 무력화 
    public int destructionValue; // 시작 파괴 수치    
    public int walkSpeed;

    [Header("Boss Attributes")]
    [SerializeField]
    NavMeshAgent navMeshAgent;
    

    [Header("Monster UI")]
    public Text hp;       
    public Image hpBar;
    public Image neutralizeBar;
    public Image destructionBar;
    [Header("Player Attributes")]
    public Player player;

    [Header("Check Attributes")]
    public bool isgimmick;
    public bool isAttack;
    public bool isKill;
    public bool isDie;
    public bool isGroggy;

    Animator animator;
    bool isGettingHit;  // 새로운 상태 변수 추가


    // Start is called before the first frame update

    private void OnEnable()
    {

        initialHealth = bossScriptable.health;
        damage = bossScriptable.damage;
        walkSpeed = bossScriptable.walkSpeed;
        neutralizeValue = bossScriptable.neutralizeValue;
        destructionValue = bossScriptable.destructionValue;

        // 남은 value 체크용
        remainHealth = initialHealth;
        remainNeutralizeVlaue = neutralizeValue;


    }
    void Start()
    {

        isGettingHit = false;
        isDie = false;
        isGroggy = false;

        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = walkSpeed;
        //시작 ui
        hp.text = $"{initialHealth}/{initialHealth}";
        hpBar.fillAmount = remainHealth / initialHealth;

    }

    // Update is called once per frame
    void Update()
    {
        if (!isDie)
        {
            Groggy();
            MonsterMove();
            Attack();
            neutralizeBar.fillAmount = (float)remainNeutralizeVlaue / neutralizeValue;
        }       
        Debug.Log(isGroggy);


    }
    /// <summary>
    /// 몬스터의 피격 함수 ArrowDamage에서 호출.
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage, int neutralizeValu)
    {
        // 남은 체력이 0보다 크면 데미지 적용
        if (remainHealth > 0)
        {
            remainHealth -= damage;
        }
        // 남은 무력화 게이지가 0보다 크면 무력화데미지 적용
        if (remainNeutralizeVlaue > 0)
        {
            remainNeutralizeVlaue -= neutralizeValu;
        }
       
        // health값이 음수가 되지 않게 제어
        remainHealth = Mathf.Clamp(remainHealth, 0, initialHealth);
        hp.text = $"{remainHealth}/{initialHealth}";
        hpBar.fillAmount = (float)remainHealth / initialHealth;
        neutralizeBar.fillAmount = (float)remainNeutralizeVlaue / neutralizeValue;
        StartCoroutine(Gethit());
        if (remainHealth <= 0)
        {
            Die();
        }
    }
    void MonsterMove()
    {
        if (isAttack == false || !isDie && player != null)
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
        // 플레이어와의 거리 계산
        float distance = Vector3.Distance(gameObject.transform.position, player.transform.position);

        // 거리가 3f 이하일 경우 Attack 메서드 호출
        if (distance <= 3f && player.remainHealth > 0) // player의 health값이 0보다 클때
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
        else if (player.remainHealth <= 0) // player의 health값이 0보다 같거나 낮아지면
        {
            // player가 죽었을때의 행동 및 애니메이션
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
    private void Die()
    {
        if (!isDie)
        {
            Debug.Log("Monster died.");
            animator.SetTrigger("Die");
            isDie = true;
            navMeshAgent.enabled = false;
            Destroy(gameObject, 3f);
        }
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
    void Hit()
    {
        player.TakeDamage(damage);        
    }

    // 그로기 관련 매서드
    void Groggy()
    {
        if(remainNeutralizeVlaue <= 0)
        {
            StartCoroutine(GroggyTime());
        }
    }
    IEnumerator GroggyTime()
    {
    
        if(!isGroggy)
        {
            animator.SetTrigger("Groggy");
            animator.SetBool("IsGroggy", true);
        }
        isGroggy = true;               
        navMeshAgent.speed = 0;  // 속도를 0으로 설정

        yield return new WaitForSeconds(4f);  // 피격 애니메이션 재생 시간 동안 대기
        animator.SetBool("IsGroggy", false);
        navMeshAgent.speed = walkSpeed;  // 속도 복원        
        isGroggy = false;  // 피격 상태 해제
        remainNeutralizeVlaue = neutralizeValue;
        
     

    }       
       
    
}
