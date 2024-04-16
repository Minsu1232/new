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
        // ������ �̸� �Ҵ��Ͽ� �ٲ� ���Ȱ��� ������+ ����
        player = FindObjectOfType<Player>();

        damage = 0;
        rb = GetComponent<Rigidbody>();
        //���� �ݶ��̴� �ٽ� Ŵ
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
        if (other.CompareTag("NormalMon"))  // "NormalMon" �±װ� �ִ��� Ȯ��
        {

            damage = damageScriptable.initialDamage + player.str;
            NormalMonster monster = other.GetComponent<NormalMonster>();
            if (monster != null)
            {
                Debug.Log("��Ҵ�");
                monster.TakeDamage(damage);

                Debug.Log("Arrow hit monster with trigger.");
                // ��Ʈ������
                StartCoroutine(DotDamage(monster, damageScriptable.duration, damageScriptable.damagePerSecond));
                StickArrow(other);

            }
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


        // ȭ�쵵 ������ �ڽ����� ����
        transform.SetParent(collision.transform);

        // ȭ���� ������ �ð� �Ŀ� ��������� �մϴ�.
        Destroy(gameObject, 4.0f); // 2�� �Ŀ� ȭ�� ��ü�� ����
        // duration�� ������ ��ƼŬ ����
        float particleDuration = particleInstance.main.duration + particleInstance.main.startLifetime.constantMax;
        Destroy(particleInstance.gameObject, particleDuration);
    }
    IEnumerator DotDamage(NormalMonster monster, float duration, int damagePerSecond)
    {
        float remainingTime = duration;

        yield return new WaitForSeconds(1f); // ù ��° ���� ���� ���� 1�� ���
        while (remainingTime > 0)
        {
            if (monster == null)  // ���Ͱ� null�̰ų� ������� ���� ���
            {
                yield break;  // �ڷ�ƾ ����
            }

            monster.TakeDamage(damagePerSecond);
            yield return new WaitForSeconds(1f);
            remainingTime -= 1f;

        }
    }

}
