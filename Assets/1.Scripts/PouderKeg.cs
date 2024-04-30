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
        // 아이템 생성시 복셀방식으로
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + 3, transform.position.z);
        GameObject item = Instantiate(itemPrefab,spawnPosition, Quaternion.identity);
        Rigidbody rb = item.GetComponent<Rigidbody>();
        Vector3 forceDirection = new Vector3(Random.Range(-1f, 1f), 1, Random.Range(-1f, 1f));
        rb.AddForce(forceDirection.normalized * 30f, ForceMode.Impulse);
        Debug.Log("복셀중");
        Destroy(gameObject);  // 오브젝트 파괴
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
                    // 화약통뭉치 폭발시 데미지                   
                   Boss enemy = hitCollider.GetComponent<Boss>();
                    if (enemy != null)
                    {
                        enemy.remainHealth -= 20;
                        enemy.StartCoroutine(enemy.Gethit()); // 해당 오브젝트가 파괴되기 때문에
                    }
                }
                Destroy(gameObject);
            }
            explosion.gameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
    // 기즈모를 사용해 데미지 범위 체크
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
