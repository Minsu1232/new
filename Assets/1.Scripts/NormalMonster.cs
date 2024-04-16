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

  
    // 몬스터가 죽는 함수
    void Die()
    {
        Debug.Log("Monster died.");
        // 여기에 죽음 애니메이션 또는 파괴 로직 추가
        Destroy(gameObject);  // 게임 오브젝트 제거
    }




}
