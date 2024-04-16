using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ArrownDamage : MonoBehaviour
{
    [SerializeField]
    ArrowDamageScriptable damageScriptable;
    [SerializeField]
    PlayerState playerState;
    
    


    public int damage;

    [SerializeField]
    Transform ShotPointer;
    [SerializeField]
    Transform target;

    Rigidbody rb;
    Collider collider;
    // Start is called before the first frame update
    private void OnEnable()
    {
        
        collider = GetComponent<Collider>();
        collider.enabled = true;
        rb = GetComponent<Rigidbody>();

    }
    void Start()
    {
        damage = damageScriptable.initialDamage + playerState.str;
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        
    }

    //Update is called once per frame
    void OnTriggerEnter (Collider other)
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
       
        collider.enabled = false;
        // Rigidbody를 비활성화하여 물리적 움직임을 멈춥니다.
        rb.isKinematic = true;
       
        // 파티클 시스템의 인스턴스를 생성하고, 부모를 몬스터로 설정합니다.
        ParticleSystem particleInstance = Instantiate(damageScriptable.particleEffect, transform.position, Quaternion.identity, collision.transform);
        particleInstance.Play();
        
        
        // 화살도 몬스터의 자식으로 설정
        transform.SetParent(collision.transform);

        // 화살이 지정된 시간 후에 사라지도록 합니다.
        Destroy(gameObject, 2.0f); // 2초 후에 화살 객체를 제거
        // duration이 끝나면 파티클 제거
        float particleDuration = particleInstance.main.duration + particleInstance.main.startLifetime.constantMax;
        Destroy(particleInstance.gameObject, particleDuration);
    }

}
