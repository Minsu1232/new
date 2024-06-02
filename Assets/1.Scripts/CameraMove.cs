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
    float tolerance = 0.1f; // 허용 오차 범위
    

    void Update()
    {
        // Dolly Cart의 위치를 경로를 따라 이동시킴
        
        dollyCart.m_Position += speed * Time.deltaTime;
        

        //Mathf.Abs(dollyCart.m_Position - 8f)는 dollyCart.m_Position과 8f의 차이의 절대값을 반환 tolerance값보다 작아질때 true
        if (Mathf.Abs(dollyCart.m_Position - 8f) < tolerance)
        {
            storyText.gameObject.SetActive(true);
            storyText.text = "저 아저씨는 여전히 기대고있네";
        }
        else if (Mathf.Abs(dollyCart.m_Position - 25f) < tolerance)
        {
            storyText.text = "사람이 뭔가 없는거 같은데...";
            
        }
        else if (Mathf.Abs(dollyCart.m_Position - 36f) < tolerance)
        {
            storyText.text = "음? 무슨 소리지? 화약 창고?";
        }
        else if (Mathf.Abs(dollyCart.m_Position - 70f) < tolerance)
        {
            storyText.text = "그새 또 무슨 일이 일어났군 \n 가까이 가서 들어보자";
        }
        // dollyCart가 최대 거리에 도달했는지 확인
        if (dollyCart.m_Position >= dollyCart.m_Path.PathLength)
        {
            // virtualCamera를 비활성화하고 mainCamera의 depth를 0으로 설정
            storyText.gameObject.SetActive(false);
            player.controller.enabled = false;
            player.transform.position = gameObject.transform.position;
            player.controller.enabled = true;
            mainCamera.depth = 0;
            gameObject.SetActive(false);
            
            
        }
    }
}
