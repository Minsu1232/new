using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NormalMonster : MonoBehaviour
{
    [SerializeField]
    NormalMonsterScriptable normalMonsterObj;

    [SerializeField]
    ArrownDamage[] arrowDamage;

    [SerializeField]
    Image hpBar;
    
    public int initialHealth;
    public int health;
    
    int damage;
    int walkSpeed;
    int runSpeed;

    public Text hp;
    // Start is called before the first frame update
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
        //���� ui
        hp.text = $"{initialHealth}/{initialHealth}";
        hpBar.fillAmount = health / initialHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(int damage)
    {
        
        if(health > 0)
        {
            health -= damage;
        }
        // health���� ������ ���� �ʰ� ����
        health = Mathf.Clamp(health, 0, initialHealth);

        hp.text = $"{initialHealth}/{health}";
        hpBar.fillAmount = (float)health / initialHealth;
        if (health <= 0)
        {
            Die();
        }
    }

  
    // ���Ͱ� �״� �Լ�
    void Die()
    {
        Debug.Log("Monster died.");
        // ���⿡ ���� �ִϸ��̼� �Ǵ� �ı� ���� �߰�
        Destroy(gameObject,3f);  // ���� ������Ʈ ����
    }




}
