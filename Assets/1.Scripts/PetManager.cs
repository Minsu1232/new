using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetManager : MonoBehaviour
{
   public Animator animator;

    Vector3 initialPet;
    public Transform initObj;

    // Start is called before the first frame update
    private void OnEnable()
    {
       
    }
    void Start()
    {
        //initTransform();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //void initTransform()
    //{
    //    initialPet = gameObject.transform.position;
    //}
    //void ResetPetTransform()
    //{
    //    gameObject.transform.position = initialPet;
    //    gameObject.transform.parent = initObj.transform;
    //}
    private void OnDisable()
    {
   ;
      
        
    }
}
