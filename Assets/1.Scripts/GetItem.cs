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

        // �÷��̾���� �Ÿ� ���
        float distance = Vector3.Distance(player.transform.position, transform.position);
        // �÷��̾ ���� ���� ���� ������ �������� �÷��̾� ������ �̵�
        if (distance <= detectionRadius)
        {            
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // �÷��̾�� �浹���� �� ������ ����
        if (other.gameObject.CompareTag("Player"))
        {
            Inventory.instance.AddItem(item);
            Destroy(gameObject);  // ������ ���� ������Ʈ ����
        }
    }
}
