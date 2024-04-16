using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMonster : MonoBehaviour
{
    [SerializeField]
    NormalMonsterScriptable normalMonsterObj;

    [SerializeField]
    ArrownDamage[] arrowDamage;
    
    public int health;
    int damage;
    int walkSpeed;
    int runSpeed;
    // Start is called before the first frame update
    private void OnEnable()
    {
        health = normalMonsterObj.health;
        damage = normalMonsterObj.damage;
        walkSpeed = normalMonsterObj.walkSpeed;
        runSpeed = normalMonsterObj.runSpeed;
        
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Monster took damage: " + damage + ", Remaining Health: " + health);
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
        Destroy(gameObject);  // ���� ������Ʈ ����
    }




}
