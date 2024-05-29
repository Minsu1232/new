using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GuidePanelManager : MonoBehaviour
{
    public GameObject[] guidePages;
    int page;
    public TextMeshProUGUI pageText;
    // Start is called before the first frame update
    void Start()
    {
        page = 0;  // ������ �ε����� 0���� ���� (�Ϲ������� �迭 �ε����� ��ġ��Ű�� ����)
        ShowPage(page);  // �ʱ� �������� �����ݴϴ�.
        pageText.text = $"{page+1} / 3";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowPage(int pageIndex)
    {
        for (int i = 0; i < guidePages.Length; i++)
        {
            guidePages[i].SetActive(i == pageIndex);  // ���������� ��ġ�ϴ°͸� Ȱ��ȭ
            
        }
    }

    public void pageButtonUp()
    {
        
        page += 1;
        page = Mathf.Clamp(page, 0, guidePages.Length - 1);  // ������ �ε����� �迭 ���� ���� ����
        ShowPage(page);  // ����� �������� Ȱ��ȭ
        pageText.text = $"{page+1} / 3";
    }

    public void pageButtonDown()
    {
        page -= 1;
        page = Mathf.Clamp(page, 0, guidePages.Length - 1);  // ������ �ε����� �迭 ���� ���� ����
        ShowPage(page);  // ����� �������� Ȱ��ȭ
        pageText.text = $"{page+1} / 3";
    }
}
