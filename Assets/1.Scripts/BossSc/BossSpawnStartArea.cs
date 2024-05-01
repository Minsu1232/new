using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawnStartArea : MonoBehaviour
{
    public GameObject boss;
    public GameObject bossUI;
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
        if(other.gameObject.tag == "Player")
        {
            boss.gameObject.SetActive(true);
            bossUI.gameObject.SetActive(true); 
        }
    }
}
