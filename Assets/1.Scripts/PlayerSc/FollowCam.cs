using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 카메라 밀림 방지 스크립트
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
