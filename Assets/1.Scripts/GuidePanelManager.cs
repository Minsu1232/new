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
        page = 0;  // 페이지 인덱스를 0부터 시작 (일반적으로 배열 인덱스와 일치시키기 위해)
        ShowPage(page);  // 초기 페이지를 보여줍니다.
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
            guidePages[i].SetActive(i == pageIndex);  // 페이지값에 일치하는것만 활성화
            
        }
    }

    public void pageButtonUp()
    {
        
        page += 1;
        page = Mathf.Clamp(page, 0, guidePages.Length - 1);  // 페이지 인덱스를 배열 범위 내로 제한
        ShowPage(page);  // 변경된 페이지를 활성화
        pageText.text = $"{page+1} / 3";
    }

    public void pageButtonDown()
    {
        page -= 1;
        page = Mathf.Clamp(page, 0, guidePages.Length - 1);  // 페이지 인덱스를 배열 범위 내로 제한
        ShowPage(page);  // 변경된 페이지를 활성화
        pageText.text = $"{page+1} / 3";
    }
}
