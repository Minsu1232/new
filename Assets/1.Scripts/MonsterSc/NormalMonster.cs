using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

// �������̽� ����� �ǰ� ����
public class NormalMonster : MonoBehaviour, IDamageable
{
    [Header("Monster Attributes")]
    [SerializeField]
    NormalMonsterScriptable normalMonsterObj;
    public int initialHealth; // ���� ü��
    public int remainHealth; // ���� ü��
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

    // ������ ���� üũ
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
    /// ������ �ǰ� �Լ� ArrowDamage���� ȣ��.
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage, int neutralizeValu, int destruction, bool shouldTriggerHitAnimation = true)
    {

        if (remainHealth > 0)
        {
            remainHealth -= damage;
        }
        // health���� ������ ���� �ʰ� ����
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


    // ���Ͱ� �״� �Լ�
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
            StartCoroutine(DeactivateAfterDelay());//3�ʵ�false ( �������� ȣ�������� �ı� x )
        }

    }
    IEnumerator DeactivateAfterDelay()
    {
        // 3�ʰ� ���
        yield return new WaitForSeconds(3f);

        // ������Ʈ ��Ȱ��ȭ
        gameObject.SetActive(false);
    }
    //navmesh �̵� �Լ�
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
    // ������ �����Լ�
    void Attack()
    {
        if (animator != null)
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
                if (!isKill)
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
    }
       
    // �ִϸ��̼� �̺�Ʈ�� �ż���
    void Hit()
    {
        player.TakeDamage(damage);

    }
    IEnumerator Gethit()
    {
        if (isGettingHit)  // �̹� �ǰ� ������ ��� �� �̻� �������� ����
            yield break;

        isGettingHit = true;
        animator.SetBool("Gethit", true);
        navMeshAgent.speed = 0;  // �ӵ��� 0���� ����

        yield return new WaitForSeconds(1.3f);  // �ǰ� �ִϸ��̼� ��� �ð� ���� ���

        navMeshAgent.speed = walkSpeed;  // �ӵ� ����
        animator.SetBool("Gethit", false);
        isGettingHit = false;  // �ǰ� ���� ����
    }





}
