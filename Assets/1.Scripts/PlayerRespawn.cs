using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public Transform respawn;
    public GameObject loading;
    public GameObject skillBar;
    public GameObject[] quest;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")        {
           
            Player player = other.GetComponent<Player>();

            
            if (player != null && player.controller != null)
            {
                
                loading.SetActive(true);
                StartCoroutine(skillBarOff());
                player.controller.enabled = false;
                other.transform.position = respawn.position;
                player.controller.enabled = true;
                
                
            }
        }
    }
    IEnumerator skillBarOff()
    {
        List<GameObject> deactivatedQuests = new List<GameObject>();
        deactivatedQuests.Clear();
        skillBar.SetActive(false);

        for (int i = 0; i < quest.Length; i++)
        {
            if (quest[i].activeSelf)
            {
                quest[i].SetActive(false);
                deactivatedQuests.Add(quest[i]); // 켜저있는 퀘스트창만 리스트에 넣고 끈 후
            }
        }

        yield return new WaitForSeconds(3f);

        skillBar.SetActive(true);

        for (int i = 0; i < deactivatedQuests.Count; i++)
        {
            deactivatedQuests[i].SetActive(true); // 켜짐
            deactivatedQuests.Clear();
        }
    }
}
