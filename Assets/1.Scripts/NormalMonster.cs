using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class NormalMonster : MonoBehaviour
{
    [SerializeField]
    NormalMonsterScriptable normalMonsterObj;

    [SerializeField]
    ArrownDamage[] arrowDamage;

    [SerializeField]
    Image hpBar;

    [SerializeField]
    NavMeshAgent navMeshAgent;

    Animator animator;


    public Player player;
    public int initialHealth;
    public int remainHealth;
    
   public int damage;
    int walkSpeed;
    int runSpeed;

    public bool isAttack;
    public bool isDie;
    public bool isKill;



    public Text hp;
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
            MonsterMove();
            Attack();
        }
        
        
    }
 
   /// <summary>
   /// 몬스터의 피격 함수 ArrowDamage에서 호출해서 피격
   /// </summary>
   /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        
        if(remainHealth > 0)
        {
            remainHealth -= damage;
        }
        // health값이 음수가 되지 않게 제어
        remainHealth = Mathf.Clamp(remainHealth, 0, initialHealth);
        hp.text = $"{remainHealth}/{initialHealth}";
        hpBar.fillAmount = (float)remainHealth / initialHealth;
        StartCoroutine(Gethit());
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
            Debug.Log("Monster died.");
            animator.SetTrigger("Die");
            isDie = true;
            navMeshAgent.enabled = false;
            Destroy(gameObject, 3f);
        }

    }
    //navmesh 이동 함수
    void MonsterMove()
    {
        if(isAttack == false || !isDie && player != null)
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
            if(!isKill)
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
    // 애니메이션 이벤트용 매서드
    void Hit()
    {
        player.TakeDamage(damage);


    }
    IEnumerator Gethit()
    {
        animator.SetBool("Gethit", true);
        navMeshAgent.speed = 0;
        yield return new WaitForSeconds(1.3f);
        navMeshAgent.speed = walkSpeed;
        animator.SetBool("Gethit", false);
    }
 




}
