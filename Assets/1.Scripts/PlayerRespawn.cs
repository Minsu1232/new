using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public Transform respawn;
    public GameObject loading;
    public GameObject skillBar;
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
        skillBar.SetActive(false);
        yield return new WaitForSeconds(3f);
        skillBar.SetActive(true);
        gameObject.SetActive(false);
    }
}
