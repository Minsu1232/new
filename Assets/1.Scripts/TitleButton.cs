using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleButton : MonoBehaviour
{
    public GameObject buttonParticle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
