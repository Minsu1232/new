using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    // ������ ���� üũ
    bool isAttack;
    bool isDie;
    bool isKill;
    bool isGettingHit;
    bool isAnimalsHit;

    [SerializeField]
    private float hitRecoveryTime = 5.0f; // �ǰ� �� ȸ�� �ð�
    private float currentHitRecoveryTimer;

    [Header("Animals Specific Attributes")]
    public float patrolRadius = 10f;  // �ʱ� �̵� ����
    public float detectionRadius = 15f;  // �÷��̾� ���� ����

    private Vector3 initialPosition; // �ʱ���ġ
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
            player = FindObjectOfType<Player>(); // ���� player�� �Ҵ��� �ȵǾ�������
        }
        isDie = false;
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        if(navMeshAgent != null)
        {
            navMeshAgent.speed = walkSpeed;
        }
        // �ʱ� ��ġ ����
        initialPosition = transform.position;
        SetNewRandomTarget();
        //���� ui
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
                if (Vector3.Distance(transform.position, player.transform.position) <= detectionRadius || isAnimalsHit ==true) // isAnimalsHit�� �ǰݽ� ��÷� player�� �ѱ� ���� boolŸ�� 
                {                                                                                                              // takedamage�ߵ��� �ߵ�
                    // �÷��̾ ���� ���� ���� ���� ��
                    navMeshAgent.enabled = true;
                    Attack();
                    MonsterMove();
                }
                else
                {
                    // �÷��̾ ���� ���� �ۿ� ���� ��
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
 /// ////////////////////////////// animal�̵��� �ż���
 /// </summary>
    void SetNewRandomTarget()
    {
        float randomX = Random.Range(-detectionRadius, detectionRadius);
        float randomZ = Random.Range(-detectionRadius, detectionRadius);
        targetPosition = initialPosition + new Vector3(randomX, 0, randomZ);
        // ������ ��ġ�� ���� �� �ص�
    }
    void MoveTowardsTarget()
    {
        // �̵�
        transform.position = Vector3.MoveTowards(transform.position, targetPosition,normalMonsterObj.walkSpeed * Time.deltaTime);
        transform.LookAt(new Vector3(targetPosition.x, transform.position.y, targetPosition.z)); // ������ȯ�� ���溸�Բ�
        CheckAndSetNewTargetIfNeeded();
    }
    void CheckAndSetNewTargetIfNeeded()
    {
        // �Ÿ��ȿ� ������ �ٽ� ��ġ ����
        if (Vector3.Distance(transform.position, targetPosition) < 1f)
        {
            SetNewRandomTarget();
        }
    }
    ////////////////////////////////////////



    /// <summary>
    /// ������ �ǰ� �Լ� ArrowDamage���� ȣ��.
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage, int neutralizeValu, int destruction, bool shouldTriggerHitAnimation = true)
    {

        isAnimalsHit = true;
        if (remainHealth > 0)
        {
            remainHealth -= damage;
            if (gameObject.CompareTag("Animals") && navMeshAgent.enabled == false) // animals�鸸 �ش�
            {
                currentHitRecoveryTimer = hitRecoveryTime; // �ǰ� �� ȸ�� Ÿ�̸� �ʱ�ȭ
                navMeshAgent.enabled = true;
               
            }
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
                Destroy(gameObject, 3f); // �߻������� �ı��ϸ� �����
            }
            else
            {
                StartCoroutine(DeactivateAfterDelay());//3�ʵ�false ( �������� ȣ�������� �ı� x )
            }
            
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
