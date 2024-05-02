using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickWall : MonoBehaviour
{
    public ParticleSystem[] particles;
   
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
        if (other.gameObject.tag == "BossRoom")
        {
            particles[0].Play();
            particles[1].gameObject.SetActive(true);
                        
            Debug.Log("dust");
        }
    }
    private void OnDisable()
    {
        particles[2].gameObject.SetActive(true);
    }

}
