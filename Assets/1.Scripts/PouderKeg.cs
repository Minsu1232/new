using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PouderKeg : MonoBehaviour
{
    public ParticleSystem explosion;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "FireArrow(Clone)")
        {
            Destroy(gameObject);
        }
    }
    private void OnDisable()
    {
        explosion.gameObject.SetActive(true);       
    }
}
