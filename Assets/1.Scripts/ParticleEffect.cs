using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect : MonoBehaviour
{    public Boss boss;
     public Player player;
    public Transform tranferPosition;
    bool isSlow;
    // Start is called before the first frame update
    void Start()
    {
        if(gameObject.name == "Healing circle" && player != null) // healingcircle발동
        {
            player.remainHealth = Mathf.Min(player.remainHealth+ 20, player.initialHealth); // 최대체력은 넘기지 않게 회복
            player.hp.text = $"{player.remainHealth}/{player.initialHealth}";
            player.hpBar.fillAmount = (float)player.remainHealth / player.initialHealth; // player UI갱신
        }
        isSlow = false;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && gameObject.name == "PortalToPortal")
        {
            Debug.Log("포탈 인");

            // Player 컴포넌트를 찾습니다.
            Player player = other.GetComponent<Player>();

            // player가 null이 아니고, player의 controller도 null이 아닌지 확인합니다.
            if (player != null && player.controller != null)
            {
                player.controller.enabled = false;
                other.transform.position = tranferPosition.position;
                player.controller.enabled = true;
                gameObject.SetActive(false);
                
            }
        }
    }
    // IceAge발동
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Boss" && !isSlow)
        {
            boss.navMeshAgent.speed -= 3;
            isSlow = true;
        }
      
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Boss")
        {
            boss.navMeshAgent.speed = boss.walkSpeed;            
        }
    }


}
