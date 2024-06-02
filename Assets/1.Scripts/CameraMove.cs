using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CameraMove : MonoBehaviour
{
    public CinemachineDollyCart dollyCart;
    public TextMeshProUGUI storyText;
    public float speed = 0.7f;
    public Camera mainCamera;
    public Player player;
    float tolerance = 0.1f; // ��� ���� ����
    

    void Update()
    {
        // Dolly Cart�� ��ġ�� ��θ� ���� �̵���Ŵ
        
        dollyCart.m_Position += speed * Time.deltaTime;
        

        //Mathf.Abs(dollyCart.m_Position - 8f)�� dollyCart.m_Position�� 8f�� ������ ���밪�� ��ȯ tolerance������ �۾����� true
        if (Mathf.Abs(dollyCart.m_Position - 8f) < tolerance)
        {
            storyText.gameObject.SetActive(true);
            storyText.text = "�� �������� ������ �����ֳ�";
        }
        else if (Mathf.Abs(dollyCart.m_Position - 25f) < tolerance)
        {
            storyText.text = "����� ���� ���°� ������...";
            
        }
        else if (Mathf.Abs(dollyCart.m_Position - 36f) < tolerance)
        {
            storyText.text = "��? ���� �Ҹ���? ȭ�� â��?";
        }
        else if (Mathf.Abs(dollyCart.m_Position - 70f) < tolerance)
        {
            storyText.text = "�׻� �� ���� ���� �Ͼ�� \n ������ ���� ����";
        }
        // dollyCart�� �ִ� �Ÿ��� �����ߴ��� Ȯ��
        if (dollyCart.m_Position >= dollyCart.m_Path.PathLength)
        {
            // virtualCamera�� ��Ȱ��ȭ�ϰ� mainCamera�� depth�� 0���� ����
            storyText.gameObject.SetActive(false);
            player.controller.enabled = false;
            player.transform.position = gameObject.transform.position;
            player.controller.enabled = true;
            mainCamera.depth = 0;
            gameObject.SetActive(false);
            
            
        }
    }
}
