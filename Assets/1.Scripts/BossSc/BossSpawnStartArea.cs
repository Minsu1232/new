using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawnStartArea : MonoBehaviour
{
    public GameObject boss;
    public GameObject bossUI;
    public GameObject[] torches;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!boss.activeSelf)
        {
            bossUI.SetActive(false); // �������� > Boss��ũ��Ʈ Die�ż��忡�� �ٷ��
        }
    }
    // �� ����� ������ ui on
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (!boss.activeSelf)
            {
                boss.gameObject.SetActive(true);
            }
            
            bossUI.gameObject.SetActive(true); 
        }
        StartCoroutine(torchesOn());
      
    }
    // ������ ����� ȶ��on
    IEnumerator torchesOn()
    {
        for (int i = 0; i < torches.Length; i++)
        {
            torches[i].SetActive(true);
            yield return new WaitForSeconds(0.11f);
        }
        
    }
}
