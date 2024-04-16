using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ArrownDamage : MonoBehaviour
{
    [SerializeField]
    ArrowDamageScriptable damageScriptable;    
    


    public int damage;

    [SerializeField]
    Transform ShotPointer;
    [SerializeField]
    Transform target;

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        damage = damageScriptable.initialDamage;
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        
    }

    //Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NormalMon"))  // "NormalMon" 태그가 있는지 확인
        {
            NormalMonster monster = other.GetComponent<NormalMonster>();
            if (monster != null)
            {
                Debug.Log("닿았다");
                monster.TakeDamage(damage);
                Debug.Log("Arrow hit monster with trigger.");
                
            }
        }
    }
        
}
