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
        if (other.CompareTag("NormalMon"))  // "NormalMon" �±װ� �ִ��� Ȯ��
        {
            NormalMonster monster = other.GetComponent<NormalMonster>();
            if (monster != null)
            {
                Debug.Log("��Ҵ�");
                monster.TakeDamage(damage);
                Debug.Log("Arrow hit monster with trigger.");

                StickArrow(other);
                
            }
        }
    }
    void StickArrow(Collider collision)
    {
        // Rigidbody�� ��Ȱ��ȭ�Ͽ� ������ �������� ����ϴ�.
        rb.isKinematic = true;

        // ��ƼŬ �ý����� �ν��Ͻ��� �����ϰ�, �θ� ���ͷ� �����մϴ�.
        ParticleSystem particleInstance = Instantiate(damageScriptable.particleEffect, transform.position, Quaternion.identity, collision.transform);
        particleInstance.Play();
        
        
        // ȭ�쵵 ������ �ڽ����� ����
        transform.SetParent(collision.transform);

        // ȭ���� ������ �ð� �Ŀ� ��������� �մϴ�.
        Destroy(gameObject, 2.0f); // 2�� �Ŀ� ȭ�� ��ü�� ����
        float particleDuration = particleInstance.main.duration + particleInstance.main.startLifetime.constantMax;
        Destroy(particleInstance.gameObject, particleDuration);
    }

}
