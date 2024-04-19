using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class ArrownDamage : MonoBehaviour
{

    [Header("Arrow")]
    [SerializeField]
    ArrowDamageScriptable damageScriptable;
    
    Player player;
    int damage;
    int neutralizeValu;

    Rigidbody rb;
    Collider collider;
    // Start is called before the first frame update

  
    private void OnEnable()
    {

        // 시작전 미리 할당하여 바뀐 스탯값에 데미지+ 적용
        player = FindObjectOfType<Player>();
        rb = GetComponent<Rigidbody>();
        //꺼진 콜라이더 다시 킴 화살 박힐때 데미지 들어가는거 방지
        collider = GetComponent<Collider>();
        collider.enabled = true;


    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        Debug.Log(damageScriptable);
    }

    //Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        // 인터페이스를 사용해 스크립트 동기화
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damage = damageScriptable.initialDamage + player.str;
            neutralizeValu = damageScriptable.neutralizeValue;
            Debug.Log("닿았다");
            damageable.TakeDamage(damage,neutralizeValu);
            
            StartCoroutine(DotDamage(damageable, damageScriptable.duration, damageScriptable.damagePerSecond));
            StickArrow(other);
        }
    }
    // 도트데미지 구현 매서드
    IEnumerator DotDamage(IDamageable monster, float duration, int damagePerSecond)
    {
        float remainingTime = duration;

        yield return new WaitForSeconds(0.5f); // 첫 번째 피해 적용 전에 1초 대기
        while (remainingTime > 0)
        {
            if (monster == null)  // 몬스터가 null이거나 살아있지 않은 경우
            {
                yield break;  // 코루틴 종료
            }
            
            monster.TakeDamage(damagePerSecond, neutralizeValu);
            yield return new WaitForSeconds(0.5f);
            remainingTime -= 1f;

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
        Destroy(gameObject, 4.0f); // 2초 후에 화살 객체를 제거
          
        // duration이 끝나면 파티클 제거
        float particleDuration = particleInstance.main.duration + particleInstance.main.startLifetime.constantMax;
        Destroy(particleInstance.gameObject, particleDuration);
    }

    
   

}
