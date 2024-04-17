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
    public int health;
    
    int damage;
    int walkSpeed;
    int runSpeed;

    public bool isAttack;
    public bool isDie;



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
        health = initialHealth;
        
    }

    private void Start()
    {
        isDie = false;
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = walkSpeed;
        //시작 ui
        hp.text = $"{initialHealth}/{initialHealth}";
        hpBar.fillAmount = health / initialHealth;
    }

    // Update is called once per frame
    void Update()
    {
        MonsterMove();
        Attack(); 
        
    }
 
   
    public void TakeDamage(int damage)
    {
        
        if(health > 0)
        {
            health -= damage;
        }
        // health값이 음수가 되지 않게 제어
        health = Mathf.Clamp(health, 0, initialHealth);
        hp.text = $"{initialHealth}/{health}";
        hpBar.fillAmount = (float)health / initialHealth;
        StartCoroutine(Gethit());
        if (health <= 0 && !isDie)
        {               
            Die();
        }
    }

  
    // 몬스터가 죽는 함수
    void Die()
    {
        Debug.Log("Monster died.");
        animator.SetTrigger("Die");
        isDie = true;
        
        Destroy(gameObject,3f);  // 게임 오브젝트 제거
    }
   void MonsterMove()
    {
        if(isAttack == false || !isDie)
        {            
            navMeshAgent.enabled = true;
            navMeshAgent.SetDestination(player.transform.position);
            animator.SetBool("Move", true);
            animator.SetBool("Attack", false);            
        }
        
       
    }
    void Attack()
    {
        // 플레이어와의 거리 계산
        float distance = Vector3.Distance(gameObject.transform.position, player.transform.position);

        // 거리가 5f 이하일 경우 Attack 메서드 호출
        if (distance <= 3f)
        {
            animator.SetBool("Attack", true);
            animator.SetBool("Move", false);
            isAttack = true;
            navMeshAgent.enabled = false;
        }
        else
        {
            isAttack = false;           
            animator.SetBool("Move", true);
            animator.SetBool("Attack", false);

        }
        
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
