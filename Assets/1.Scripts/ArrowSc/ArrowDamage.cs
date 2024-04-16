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

                StickArrow(other);
                
            }
        }
    }
    void StickArrow(Collider collision)
    {
        // Rigidbody를 비활성화하여 물리적 움직임을 멈춥니다.
        rb.isKinematic = true;

        // 화살의 부모를 몬스터로 설정하여 화살이 몬스터와 함께 움직이게 합니다.
        transform.SetParent(collision.transform);

        // 화살이 지정된 시간 후에 사라지도록 합니다.
        Destroy(gameObject, 2.0f); // 2초 후에 화살 객체를 제거
    }

}
