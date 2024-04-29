using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemRb : MonoBehaviour
{
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "BossRoom")
        {
            rb.isKinematic = true;
            Collider collider = gameObject.GetComponent<Collider>();
            collider.isTrigger = true;
        }       
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
