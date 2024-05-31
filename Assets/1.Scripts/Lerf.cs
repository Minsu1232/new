using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Lerf : MonoBehaviour
{
    public TextMeshProUGUI dieText;
    // Start is called before the first frame update
    private void OnEnable()
    {
        StartCoroutine(FadeTextToFullAlpha(1.5f, dieText));  // 1.5�� ���� ���̵�
    }
    void Start()
    {

       

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator FadeTextToFullAlpha(float t, TextMeshProUGUI text)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);  // �ʱ� ������ 0���� ����
        while (text.color.a < 1.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }
}
