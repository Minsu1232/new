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
        //시작 ui
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
        // health값이 음수가 되지 않게 제어
        health = Mathf.Clamp(health, 0, initialHealth);

        hp.text = $"{initialHealth}/{health}";
        hpBar.fillAmount = (float)health / initialHealth;
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
        Destroy(gameObject,3f);  // 게임 오브젝트 제거
    }




}
