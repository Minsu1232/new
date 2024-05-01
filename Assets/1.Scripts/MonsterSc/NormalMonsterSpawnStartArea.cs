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
    // 존 통과시 보스와 ui on
    private void OnTriggerEnter(Collider other)
    {   
        if (other.gameObject.tag == "Player")
        {
           monster.gameObject.SetActive(true);
           
        }
    }
}
