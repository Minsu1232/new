using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
    public GameObject itemPrefab;  // 아이템 프리팹 설정

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
        GameObject item = Instantiate(itemPrefab, transform.position, Quaternion.identity);
        Rigidbody rb = item.GetComponent<Rigidbody>();
        Vector3 forceDirection = new Vector3(Random.Range(-1f, 1f), 1, Random.Range(-1f, 1f));
        rb.AddForce(forceDirection.normalized * Random.Range(10f, 20f), ForceMode.Impulse);
        Destroy(gameObject);  // 오브젝트 파괴
    }
}
