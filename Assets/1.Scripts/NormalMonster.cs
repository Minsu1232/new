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
        //���� ui
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
   /// ������ �ǰ� �Լ� ArrowDamage���� ȣ���ؼ� �ǰ�
   /// </summary>
   /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        
        if(remainHealth > 0)
        {
            remainHealth -= damage;
        }
        // health���� ������ ���� �ʰ� ����
        remainHealth = Mathf.Clamp(remainHealth, 0, initialHealth);
        hp.text = $"{remainHealth}/{initialHealth}";
        hpBar.fillAmount = (float)remainHealth / initialHealth;
        StartCoroutine(Gethit());
        if (remainHealth <= 0)
        {               
            Die();
        }
    }


    // ���Ͱ� �״� �Լ�
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
    //navmesh �̵� �Լ�
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
    // ������ �����Լ�
    void Attack()
    {
        // �÷��̾���� �Ÿ� ���
        float distance = Vector3.Distance(gameObject.transform.position, player.transform.position);

        // �Ÿ��� 3f ������ ��� Attack �޼��� ȣ��
        if (distance <= 3f && player.remainHealth > 0)
        {
            // �÷��̾ ����
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
            // player�� �׾������� �ൿ
            if(!isKill)
            {
                animator.SetTrigger("Kill");
                isKill = true;
            }            
            
        }
        else
        {
            // �Ÿ��� �־����� �ٽ� �̵�
            isAttack = false;           
            animator.SetBool("Move", true);
            animator.SetBool("Attack", false);

        }
        
    }
    // �ִϸ��̼� �̺�Ʈ�� �ż���
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
