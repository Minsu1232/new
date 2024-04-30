using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PouderKeg : MonoBehaviour
{
    public ParticleSystem explosion;
    public GameObject itemPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void DestroyObject()
    {
        // ������ ������ �����������
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + 3, transform.position.z);
        GameObject item = Instantiate(itemPrefab,spawnPosition, Quaternion.identity);
        Rigidbody rb = item.GetComponent<Rigidbody>();
        Vector3 forceDirection = new Vector3(Random.Range(-1f, 1f), 1, Random.Range(-1f, 1f));
        rb.AddForce(forceDirection.normalized * 30f, ForceMode.Impulse);
        Debug.Log("������");
        Destroy(gameObject);  // ������Ʈ �ı�
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "FireArrow(Clone)")
        {
            DestroyObject();
            if (gameObject.name == "PouderKegBundle")
            {
                explosion.gameObject.SetActive(true);
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, 14f);
                foreach (Collider hitCollider in hitColliders)
                {
                    // ȭ���빶ġ ���߽� ������                   
                   Boss enemy = hitCollider.GetComponent<Boss>();
                    if (enemy != null)
                    {
                        enemy.remainHealth -= 20;
                        enemy.StartCoroutine(enemy.Gethit()); // �ش� ������Ʈ�� �ı��Ǳ� ������
                    }
                }
                Destroy(gameObject);
            }
            explosion.gameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
    // ����� ����� ������ ���� üũ
    //void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, 13f);
    //}

    //private void OnDisable()
    //{
    //    if(itemPrefab != null)
    //    {
    //        DestroyObject();
    //    }   
        
                    
    //}
}
