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
        if(gameObject.name == "Healing circle" && player != null) // healingcircle�ߵ�
        {
            player.remainHealth = Mathf.Min(player.remainHealth+ 20, player.initialHealth); // �ִ�ü���� �ѱ��� �ʰ� ȸ��
            player.hp.text = $"{player.remainHealth}/{player.initialHealth}";
            player.hpBar.fillAmount = (float)player.remainHealth / player.initialHealth; // player UI����
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
            Debug.Log("��Ż ��");

            // Player ������Ʈ�� ã���ϴ�.
            Player player = other.GetComponent<Player>();

            // player�� null�� �ƴϰ�, player�� controller�� null�� �ƴ��� Ȯ���մϴ�.
            if (player != null && player.controller != null)
            {
                player.controller.enabled = false;
                other.transform.position = tranferPosition.position;
                player.controller.enabled = true;
                gameObject.SetActive(false);
                
            }
        }
    }
    // IceAge�ߵ�
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
