using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleButton : MonoBehaviour
{
    public GameObject buttonParticle;
    public GameObject optionPanel;
    public GameObject porologuePanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            optionPanel.SetActive(false);
            porologuePanel.SetActive(false);
        }
    }
    public void particleOn() // �̺�ƮƮ���� ����� ���� ��ƼŬ�� ��ġ ��ȯȰ��
    {
        buttonParticle.transform.parent = gameObject.transform;
        buttonParticle.transform.localPosition = new Vector3(118.84f, -20.6f, -31.90001f);
        buttonParticle.SetActive(true);
        
    }
    public void ParticleOff()
    {
        buttonParticle.SetActive(false);

    }
    public void OtionPanelOn()
    {
        optionPanel.SetActive(true);
    } 
    public void OtionPanelOff()
    {
        optionPanel.SetActive(false);
    }
    public void ProloguePanelOn()
    {
        porologuePanel.SetActive(true);
    }
    public void ProloguePanelOff()
    {
        porologuePanel.SetActive(false);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
