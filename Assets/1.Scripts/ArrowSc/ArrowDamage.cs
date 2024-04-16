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
    Player player;




    int damage;

    [SerializeField]
    Transform ShotPointer;
    [SerializeField]
    Transform target;

    Rigidbody rb;
    Collider collider;
    // Start is called before the first frame update
    private void OnEnable()
    {
        // 시작전 미리 할당하여 바뀐 스탯값에 데미지+ 적용
        player = FindObjectOfType<Player>();

        damage = 0;
        rb = GetComponent<Rigidbody>();
        //꺼진 콜라이더 다시 킴
        collider = GetComponent<Collider>();
        collider.enabled = true;


    }
    void Start()
    {

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

            damage = damageScriptable.initialDamage + player.str;
            NormalMonster monster = other.GetComponent<NormalMonster>();
            if (monster != null)
            {
                Debug.Log("닿았다");
                monster.TakeDamage(damage);

                Debug.Log("Arrow hit monster with trigger.");
                // 도트데미지
                StartCoroutine(DotDamage(monster, damageScriptable.duration, damageScriptable.damagePerSecond));
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
        Destroy(gameObject, 4.0f); // 2초 후에 화살 객체를 제거
        // duration이 끝나면 파티클 제거
        float particleDuration = particleInstance.main.duration + particleInstance.main.startLifetime.constantMax;
        Destroy(particleInstance.gameObject, particleDuration);
    }
    IEnumerator DotDamage(NormalMonster monster, float duration, int damagePerSecond)
    {
        float remainingTime = duration;

        yield return new WaitForSeconds(1f); // 첫 번째 피해 적용 전에 1초 대기
        while (remainingTime > 0)
        {
            if (monster == null)  // 몬스터가 null이거나 살아있지 않은 경우
            {
                yield break;  // 코루틴 종료
            }

            monster.TakeDamage(damagePerSecond);
            yield return new WaitForSeconds(1f);
            remainingTime -= 1f;

        }
    }

}
