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
    public int destructionValue; // ���� �ı� ��ġ    
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

    Animator animator;
    bool isGettingHit;  // ���ο� ���� ���� �߰�


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
        
    }
    void Start()
    {

        isGettingHit = false;
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

    public void TakeDamage(int damage)
    {

        if (remainHealth > 0)
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
    void Hit()
    {
        player.TakeDamage(damage);

    }
}
