using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gradeNPC : MonoBehaviour
{
    public GameObject gradePanel;
    public Outline outline;
    // Start is called before the first frame update
    void Start()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseEnter()
    {
        outline.enabled = true;
    }
    private void OnMouseExit()
    {
        outline.enabled = false;
    }
    private void OnMouseDown()
    {
        gradePanel.SetActive(true);
    }
    public void GradePanelOff()
    {
        gradePanel.SetActive(false);
    }
}
