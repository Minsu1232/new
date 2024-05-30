using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTransformCheck : MonoBehaviour
{
    public GameObject questTransform;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf)
        { 
            questTransform.SetActive(true);
        }
        else
        {
            questTransform.SetActive(false);
        }
    }
}
