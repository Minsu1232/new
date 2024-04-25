using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class ArrownDamage : MonoBehaviour
{

    [Header("Arrow")]
    [SerializeField]
    ArrowDamageScriptable damageScriptable;
    public AudioClip hitSound; 
    
    Player player;
    int damage;
    int neutralizeValu;

    Rigidbody rb;
    Collider collider;

    AudioSource audioSource;

    bool shouldPlayAnimation = true;
    // Start is called before the first frame update

    
    private void OnEnable()
    {

        // ������ �̸� �Ҵ��Ͽ� �ٲ� ���Ȱ��� ������+ ����
        player = FindObjectOfType<Player>();
        rb = GetComponent<Rigidbody>();
        //���� �ݶ��̴� �ٽ� Ŵ ȭ�� ������ ������ ���°� ����
        collider = GetComponent<Collider>();
        collider.enabled = true;


    }
    void Start()
    {
        
        hitSound = damageScriptable.hitSound;
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        
    }

    //Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        shouldPlayAnimation = true;
        
        
        //if (other.gameObject.tag == "PouderKeg")
        //{
        //    Destroy(other.gameObject);
        //}
        if (other.gameObject.tag == "BossRoom")
        {
            StickArrow(other);
        }
        // �������̽��� ����� ��ũ��Ʈ ����ȭ
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damage = damageScriptable.initialDamage + player.str;
            neutralizeValu = damageScriptable.neutralizeValue;
            Debug.Log("��Ҵ�");
            // �Ϲ� ȭ���϶� �˹� ����� ����
            if(damageScriptable.name == "Arrow")
            {
                damageable.TakeDamage(damage, neutralizeValu, false);
                audioSource.PlayOneShot(hitSound, 0.3f);
            }           
            // �� �� ȭ���� ����
            else
            {
                audioSource.PlayOneShot(hitSound, 0.3f);
                damageable.TakeDamage(damage, neutralizeValu, true);
                
            }            
            
            StartCoroutine(DotDamage(damageable, damageScriptable.duration, damageScriptable.damagePerSecond));
            StickArrow(other);
        }
        
    }
    // ��Ʈ������ ���� �ż���

    IEnumerator DotDamage(IDamageable monster, float duration, int damagePerSecond)
    {
        float remainingTime = duration;

        shouldPlayAnimation = false;

        yield return new WaitForSeconds(1f); // ù ��° ���� ���� ���� 1�� ���
        while (remainingTime > 0)
        {
            if (monster == null)  // ���Ͱ� null�̰ų� ������� ���� ���
            {
                yield break;  // �ڷ�ƾ ����
            }
            // ��Ʈ�������� �˹��� ����
            monster.TakeDamage(damagePerSecond, neutralizeValu,false);
            yield return new WaitForSeconds(1f);
            remainingTime -= 1f;

        }
    }
    void StickArrow(Collider collision)
    {
        
        collider.enabled = false;
        // Rigidbody�� ��Ȱ��ȭ�Ͽ� ������ �������� ����ϴ�.
        rb.isKinematic = true;

        // ��ƼŬ �ý����� �ν��Ͻ��� �����ϰ�, �θ� ���ͷ� �����մϴ�.
        ParticleSystem particleInstance = Instantiate(damageScriptable.particleEffect, transform.position, Quaternion.identity, collision.transform);
        particleInstance.Play();
        if(damageScriptable.name == "FireArrow" || damageScriptable.name == "PoisonArrow")
        {
            audioSource.PlayOneShot(hitSound, 0.3f);
        }        
        // ȭ�쵵 ������ �ڽ����� ����
        transform.SetParent(collision.transform);        
        // ȭ���� ������ �ð� �Ŀ� ��������� �մϴ�.
        Destroy(gameObject, 4.0f); // 2�� �Ŀ� ȭ�� ��ü�� ����
          
        // duration�� ������ ��ƼŬ ����
        float particleDuration = particleInstance.main.duration + particleInstance.main.startLifetime.constantMax;
        Destroy(particleInstance.gameObject, particleDuration);
    }

    
   

}
