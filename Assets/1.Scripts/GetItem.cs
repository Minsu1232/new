using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItem : MonoBehaviour
{
    public Player player;
    public Item item;
    public float speed = 15f;
    public float detectionRadius = 30f;
    Rigidbody rb;

    private void Start()
    {        
        player = FindObjectOfType<Player>();
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {

        // 플레이어와의 거리 계산
        float distance = Vector3.Distance(player.transform.position, transform.position);
        // 플레이어가 검출 범위 내에 있으면 아이템을 플레이어 쪽으로 이동
        if (distance <= detectionRadius)
        {            
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // 플레이어와 충돌했을 때 아이템 제거
        if (other.gameObject.CompareTag("Player"))
        {
            Inventory.instance.AddItem(item);
            Destroy(gameObject);  // 아이템 게임 오브젝트 제거
        }
    }
}
