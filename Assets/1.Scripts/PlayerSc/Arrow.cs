using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField]
    GameObject[] rbArrow;

    Rigidbody rbRigidbody;
    // Start is called before the first frame update
    void Start()
    {
        rbRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ArrowPhysics()
    {
        for(int i = 0; i < rbArrow.Length; i++)
        {
            rbRigidbody.AddForce(transform.forward * 100f);
        }
    }
}
