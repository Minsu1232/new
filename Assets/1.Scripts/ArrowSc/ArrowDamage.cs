using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class ArrownDamage : MonoBehaviour
{

    [Header("Arrow")]
   
    public ArrowDamageScriptable damageScriptable;
    public Weapon bow;
    public AudioClip hitSound; 
    
    Player player;
    int damage;
    int neutralizeValu;
    int destructionValu;

    Rigidbody rb;
    Collider collider;

    AudioSource audioSource;

    bool shouldPlayAnimation;
    // Start is called before the first frame update

    
    private void OnEnable()
    {

        // ������ �̸� �Ҵ��Ͽ� �ٲ� ���Ȱ��� ������+ ����
        player = FindObjectOfType<Player>();
        rb = GetComponent<Rigidbody>();
        //���� �ݶ��̴� �ٽ� Ŵ ȭ�� ������ ������ ���°� ����
        collider = GetComponent<Collider>();
        if(collider != null)
        {
            collider.enabled = true;
        }
        


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
        Debug.Log($"Collision with: {other.gameObject.name}, Tag: {other.gameObject.tag}, Layer: {LayerMask.LayerToName(other.gameObject.layer)}");
        shouldPlayAnimation = true;


        if (other.gameObject.tag == "PouderKeg")
        {
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "BossRoom")
        {
            // ���� �� ���������� ȭ���� �����Բ�
            StickArrow(other);
            if (damageScriptable.name == "PoisonArrow")
            {
                audioSource.PlayOneShot(hitSound, 0.5f);
            }
            else if (damageScriptable.name == "FireArrow")
            {
                audioSource.PlayOneShot(hitSound, 0.1f);
            }
        }
        // �������̽��� ����� ��ũ��Ʈ ����ȭ
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damage = damageScriptable.initialDamage + player.str + bow.baseAttackPower + bow.enhancedAttackPower; // ĳ���� ������ ��� ����
            neutralizeValu = damageScriptable.neutralizeValue;
            destructionValu = damageScriptable.destructionValue;
            Debug.Log("��Ҵ�");
            // �Ϲ� ȭ���϶� �˹� ����� ����
            // �� ���� Ŭ���� �Ҹ� ����
            if(damageScriptable.name == "Arrow") 
            {
                damageable.TakeDamage(damage, neutralizeValu, destructionValu, false);
                //audioSource.PlayOneShot(hitSound, 0.3f);
            }
            else if(damageScriptable.name == "PoisonArrow")
            {
                //audioSource.PlayOneShot(hitSound, 0.5f);
                damageable.TakeDamage(damage, neutralizeValu,destructionValu,true);
            }
            // �� �� ȭ���� ����
            else if(damageScriptable.name == "FireArrow")
            {
                //audioSource.PlayOneShot(hitSound, 0.1f);
                damageable.TakeDamage(damage, neutralizeValu, destructionValu, true);
                
            }
            else
            {
                damageable.TakeDamage(damage, neutralizeValu, destructionValu, true);
            }
            
            //StartCoroutine(DotDamage(damageable, damageScriptable.duration, damageScriptable.damagePerSecond));
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
            monster.TakeDamage(damagePerSecond, neutralizeValu, destructionValu, false);
            yield return new WaitForSeconds(1f);
            remainingTime -= 1f;

        }
    }
    void StickArrow(Collider collision)
    {
        
        collider.enabled = false;
        // Rigidbody�� ��Ȱ��ȭ�Ͽ� ������ �������� ����ϴ�.
        rb.isKinematic = true;

        Vector3 particlePosition = transform.position;
        // FireArrow�� �� Z ���� -1�� ����
        if (damageScriptable.name == "FireArrow")
        {if(collision.gameObject.tag == "Boss")
            {
             particlePosition.z += 5.0f;
            }
            else
            {
                particlePosition.z += 2;
            }
            
        }
        // ��ƼŬ �ý����� �ν��Ͻ��� �����ϰ�, �θ� ���ͷ� �����մϴ�.
        ParticleSystem particleInstance = Instantiate(damageScriptable.particleEffect, particlePosition, Quaternion.identity, collision.transform);
        particleInstance.Play();
        particleInstance.Play();
        if(damageScriptable.particleEffectSecond != null) // �ΰ��� ��ƼŬ ȿ�� ����
        {
            damageScriptable.particleEffectSecond.Play();
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
