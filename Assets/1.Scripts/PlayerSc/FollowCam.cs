using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform player; // 플레이어의 Transform 정보를 받을 변수
    public float rotationSpeed = 5f; // 카메라의 회전 속도

    private Vector3 offset = new Vector3(0f, 5f, -10f); // 카메라의 초기 위치를 조정하기 위한 오프셋

    void FixedUpdate()
    {
        // 마우스의 X 축 입력 값을 받아 플레이어 주위를 회전시킴
        float mouseX = Input.GetAxis("Mouse X");
        Quaternion camTurnAngle = Quaternion.Euler(0f, mouseX * rotationSpeed, 0f);

        // 카메라의 위치를 플레이어를 중심으로 회전하면서 이동
        offset = camTurnAngle * offset;
        Vector3 desiredPosition = player.position + offset;

        // 카메라가 플레이어를 바라보도록 회전
        transform.position = desiredPosition;
        transform.LookAt(player);
    }
}
