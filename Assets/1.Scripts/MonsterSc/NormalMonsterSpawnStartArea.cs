using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMonsterSpawnStartArea : MonoBehaviour
{
    public GameObject monster;    
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    // �� ����� ������ ui on
    private void OnTriggerEnter(Collider other)
    {   
        if (other.gameObject.tag == "Player")
        {
           monster.gameObject.SetActive(true);
           
        }
    }
}
