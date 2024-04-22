using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

// �������̽� ����� �ǰ� ����
public class Boss : MonoBehaviour, IDamageable
{
    [Header("Boss Info")]
    [SerializeField]
    BossScriptable bossScriptable;
    public int initialHealth; // ���� ü��
    public int remainHealth; // ���� ü��
    public int damage;
    public int neutralizeValue; // ���� ����ȭ
    public int remainNeutralizeVlaue; // ���� ����ȭ 
    public int destructionValue; // ���� �ı� ��ġ    
    public int walkSpeed;

    [Header("Boss Attributes")]
    [SerializeField]
    NavMeshAgent navMeshAgent;
    public GameObject[] gimmickWalls;
    public Transform gimmickStartPosition;
    public int gimmickCount; // 0�� �Ǹ� ����




    [Header("Monster UI")]
    public Text hp;       
    public Image hpBar;
    public Image neutralizeBar;
    public Image destructionBar;
    [Header("Player Attributes")]
    public Player player;

    [Header("Check Attributes")]
    public bool isgimmick; // ������Ʈ�� ���� ����    
    public bool isGimmicked; // ��� ���� ����
    public bool isAttack;
    public bool isKill;
    public bool isDie;
    public bool isGroggy;

    Animator animator;
    bool isGettingHit;  // ���ο� ���� ���� �߰�

    private Coroutine damageCoroutine; //����� �޸��� ������ �ֱ⸦ ���� ����

    // Start is called before the first frame update

    private void OnEnable()
    {

        initialHealth = bossScriptable.health;
        damage = bossScriptable.damage;
        walkSpeed = bossScriptable.walkSpeed;
        neutralizeValue = bossScriptable.neutralizeValue;
        destructionValue = bossScriptable.destructionValue;

        // ���� value üũ��
        remainHealth = initialHealth;
        remainNeutralizeVlaue = neutralizeValue;


    }
    void Start()
    {

        isGettingHit = false;
        isDie = false;
        isGroggy = false;
        gimmickCount = 4;

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
            if (!isgimmick)
            {
                Groggy();
                MonsterMove();
                Attack();
                neutralizeBar.fillAmount = (float)remainNeutralizeVlaue / neutralizeValue;
                Gimmick();
            }
            GimmicDamage();
            GimmicEnd();


        }       

    }
    /// <summary>
    /// ������ �ǰ� �Լ� ArrowDamage���� ȣ��.
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage, int neutralizeValu, bool shouldTriggerHitAnimation = true)
    {
        // ���� ü���� 0���� ũ�� ������ ����
        if (remainHealth > 0)
        {
            remainHealth -= damage;
        }
        // ���� ����ȭ �������� 0���� ũ�� ����ȭ������ ����
        if (remainNeutralizeVlaue > 0)
        {
            remainNeutralizeVlaue -= neutralizeValu;
        }
       
        // health���� ������ ���� �ʰ� ����
        remainHealth = Mathf.Clamp(remainHealth, 0, initialHealth);
        hp.text = $"{remainHealth}/{initialHealth}";
        hpBar.fillAmount = (float)remainHealth / initialHealth;
        neutralizeBar.fillAmount = (float)remainNeutralizeVlaue / neutralizeValue;
        if (shouldTriggerHitAnimation)
        {// ������ ��Ʈ�������� ������ ����
            StartCoroutine(Gethit());
        }
        if (remainHealth <= 0)
        {
            Die();
        }
    }
    void MonsterMove()
    {
        if (isAttack == false || !isDie && player != null && !isgimmick)
        {
            navMeshAgent.enabled = true;
            navMeshAgent.SetDestination(player.transform.position);
            animator.SetBool("Move", true);
            animator.SetBool("Attack", false);
        }


    }
    // ������ �����Լ� �ִϸ��̼� �̺�Ʈ�� ���� �������� ��
    void Attack()
    {
        // �÷��̾���� �Ÿ� ���
        if (player != null)
        {
            float distance = Vector3.Distance(gameObject.transform.position, player.transform.position);

            // �Ÿ��� 3f �����̰�, �÷��̾��� ü���� 0���� Ŭ ��
            if (distance <= 3f && player.remainHealth > 0)
            {
                // �÷��̾ ����
                if (!isKill)
                {
                    if (!isAttack) // ������ ���� ���۵��� �ʾҴٸ� ���� �ε����� ����
                    {
                        int randomIndex = UnityEngine.Random.Range(0, 3);  // 0, 1, �Ǵ� 2
                        animator.SetInteger("AttackInt", randomIndex);
                    }

                    animator.SetBool("Attack", true);
                    animator.SetBool("Move", false);
                    isAttack = true;
                    navMeshAgent.enabled = false;
                }
            }
            else if (player.remainHealth <= 0) // �÷��̾��� ü���� 0 ���ϸ�
            {
                // �÷��̾ �׾��� ���� �ൿ �� �ִϸ��̼�
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
    // �ִϸ��̼� �۵� Ʈ����
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
    // �ִϸ��̼� �̺�Ʈ (�÷��̾� ������)
    void Hit()
    {
        player.TakeDamage(damage);        
    }

    // �׷α� ���� �ż���
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
        navMeshAgent.speed = 0;  // �ӵ��� 0���� ����

        yield return new WaitForSeconds(4f);  // �ǰ� �ִϸ��̼� ��� �ð� ���� ���
        animator.SetBool("IsGroggy", false);
        navMeshAgent.speed = walkSpeed;  // �ӵ� ����        
        isGroggy = false;  // �ǰ� ���� ����
        remainNeutralizeVlaue = neutralizeValue;
        
     

    }

    // ����� ���� ������ �ż���
    void Gimmick()
    {
        // ü���� 50�� �Ʒ��϶�
        if (remainHealth < initialHealth * 0.5)
        {
            if (!isGimmicked)
            {
                navMeshAgent.enabled = false;
                animator.SetTrigger("Rage");
                isgimmick = true; 
                // �� pillar�� ���� MovePillar �ڷ�ƾ ����
                foreach (GameObject gimmickWall in gimmickWalls)
                {
                    StartCoroutine(MovePillar(gimmickWall, 8f, 1f)); // 1�� ���� 8�� �̵�
                }
                isGimmicked = true; 
                StartCoroutine(Gimmicking());
            }

        }
    }
    void GimmicDamage()
    {
        bool isGimmickRunning = animator.GetBool("GimmickRun");
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (isGimmickRunning && distanceToPlayer < 5f)
        {
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(GimmickDamagePlayer());
            }
        }
        else
        {
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }
    // ��� ���� �ż���
    void GimmicEnd()
    {
        if(gimmickCount == 0)
        {
            animator.SetBool("GimmickRun", false);
            animator.SetBool("IsGroggy", true);
            StartCoroutine(GimmickEndCorutine());
        }
    }

    // �Ͽ︵ �� ��ͽ����������� �̵� �ϴ� �ڷ�ƾ
    IEnumerator Gimmicking()
    {
        yield return new WaitForSeconds(2f); // ��ȿ �� �̵� ���� �ð�����
        animator.SetBool("GimmickRun", true);
        Vector3 startPosition = gameObject.transform.position;  // ���� ��ġ
        Vector3 endPosition = gimmickStartPosition.position;    // �� ��ġ
        float closeEnoughDistance = 0.5f; // ����� ����� �Ÿ�

        while (true) // ���� ������ ����ϰ� break�� ������ ����
        {
            // ������Ʈ ��ġ ����
            transform.position = Vector3.MoveTowards(transform.position, endPosition, Time.deltaTime * 10); // 10�� ���� ������ �ӵ� ��

            // ������Ʈ�� endPosition�� �ٶ󺸵��� ����
            Vector3 direction = (endPosition - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(direction);

            // ���� ��ġ�� ��ǥ ��ġ�� ����� ������� Ȯ��
            if (Vector3.Distance(transform.position, endPosition) < closeEnoughDistance)
            {
                Debug.Log("����");
                animator.SetBool("GimmickRun", false);
                animator.SetTrigger("Observ"); // 2�ʰ� �θ��� �Ÿ� ��
                yield return new WaitForSeconds(3.12f);
                walkSpeed = bossScriptable.runSpeed; // ��� ���۽� ���ǵ� ����
                animator.SetBool("GimmickRun", true); // �÷��̾ ���� �޷���
                isgimmick = false;
                              
                break; // ��ǥ�� �����ϸ� ���� ����
            }

            yield return null;
        }

        // ���� ��ġ �� ���� ����
        transform.position = endPosition;
        transform.rotation = Quaternion.LookRotation((endPosition - startPosition).normalized);
        
    }

    //ü�� 50�۽� ����� �������� �ڷ�ƾ
    IEnumerator MovePillar(GameObject gimmickwall, float targetY, float duration)
    {
        float time = 0;
        Vector3 startPosition = gimmickwall.transform.position; // ���� ��ġ
        Vector3 endPosition = new Vector3(gimmickwall.transform.position.x, targetY, gimmickwall.transform.position.z); // ���� ��ġ

        while (time < duration)
        {
            // �����Լ��� ���� ���� ��ġ ����
            gimmickwall.transform.position = Vector3.Lerp(startPosition, endPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        gimmickwall.transform.position = endPosition; // ���� ��ġ Ȯ��
    }
    // �ʴ� 20�� ������
    IEnumerator GimmickDamagePlayer()
    {
        while (true)
        {
            player.TakeDamage(20);
            yield return new WaitForSeconds(1f);
        }
    }
    IEnumerator GimmickEndCorutine()
    {
        navMeshAgent.speed = 0;
        yield return new WaitForSeconds(5f);
        navMeshAgent.speed = bossScriptable.walkSpeed; // �ٽ� ���� �ӵ��� ���ư� ���� ������ �𸣰����� �ȵ��ư�
        animator.SetBool("IsGroggy", false);
    }
    private void OnTriggerEnter(Collider other)
    {
        // ��� ���� �Ҷ����� �μ���
        if(other.gameObject.tag == "GimmickWalls" && !isgimmick)
        {
            Destroy(other.gameObject);
            gimmickCount--;
        }
    }

}
