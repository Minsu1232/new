using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ī�޶� �и� ���� ��ũ��Ʈ
/// </summary>
public class FollowCam : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    void Update()
    {
        transform.position = target.position + offset;
    }


  
}
