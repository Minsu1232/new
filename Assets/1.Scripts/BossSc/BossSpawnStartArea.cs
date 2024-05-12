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
        if (!boss.activeSelf)
        {
            bossUI.SetActive(false); // 수정예정 > Boss스크립트 Die매서드에서 다루기
        }
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
