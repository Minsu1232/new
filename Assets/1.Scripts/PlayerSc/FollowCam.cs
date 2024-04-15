using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform player; // �÷��̾��� Transform ������ ���� ����
    public float rotationSpeed = 5f; // ī�޶��� ȸ�� �ӵ�

    private Vector3 offset = new Vector3(0f, 5f, -10f); // ī�޶��� �ʱ� ��ġ�� �����ϱ� ���� ������

    void FixedUpdate()
    {
        // ���콺�� X �� �Է� ���� �޾� �÷��̾� ������ ȸ����Ŵ
        float mouseX = Input.GetAxis("Mouse X");
        Quaternion camTurnAngle = Quaternion.Euler(0f, mouseX * rotationSpeed, 0f);

        // ī�޶��� ��ġ�� �÷��̾ �߽����� ȸ���ϸ鼭 �̵�
        offset = camTurnAngle * offset;
        Vector3 desiredPosition = player.position + offset;

        // ī�޶� �÷��̾ �ٶ󺸵��� ȸ��
        transform.position = desiredPosition;
        transform.LookAt(player);
    }
}
