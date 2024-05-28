using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class speedboxText : MonoBehaviour
{
    public RectTransform balloonImageRectTransform; // 말풍선 이미지의 RectTransform
    public TextMeshProUGUI balloonText; // 말풍선의 텍스트 컴포넌트
    public GameObject textImage;
    public GameObject questImage;
    public string fullText; // 전체 표시할 텍스트
    private float delay = 0.05f; // 글자가 나타나는 딜레이 시간 (초)
    public bool isGuideStart;
    private Coroutine currentCoroutine; //진행중인 코루틴 할당

    public QuestScriptable questScriptable;

    private void OnEnable()
    {
        questScriptable.UpdateQuestDetail();
    }
    private void Start()
    {
        isGuideStart = false;
         fullText = questScriptable.questDetail;
        //StartCoroutine(ShowTextOneByOne(fullText));
        currentCoroutine = StartCoroutine(ShowTextOneByOne(fullText)); //진행중인 코루틴 할당
    }
    public void Update()
    {
        if (questScriptable.isGuide) 
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine); // 현재 실행 중인 코루틴 중지
                balloonText.text = ""; // 텍스트 초기화
            }
            questScriptable.CheckQuestCompletion();
            fullText = questScriptable.questDetail;
            currentCoroutine = StartCoroutine(ShowTextOneByOne(fullText)); // 새 코루틴 시작

        }
    }

    private IEnumerator ShowTextOneByOne(string completeText)
    {
        balloonText.text = ""; // 초기 텍스트를 비웁니다.
        
        foreach (char letter in completeText)
        {
            balloonText.text += letter; // 하나의 글자를 추가합니다.
            UpdateBalloonSize(); // 말풍선 크기를 업데이트합니다.
            yield return new WaitForSeconds(delay); // 다음 글자가 나타나기 전 딜레이
        
        }
    }

    // 말풍선 크기를 업데이트하는 함수
    private void UpdateBalloonSize()
    {
        // 텍스트 내용에 맞게 말풍선 크기 조절
        Vector2 newSize = new Vector2(balloonText.preferredWidth, balloonText.preferredHeight);
        balloonImageRectTransform.sizeDelta = newSize;
    }
    private void OnMouseDown()
    {
        if (!isGuideStart) // 첫 가이드의 시작은 npc 클릭
        {
            //questScriptable.isGuide = true;
            if (!textImage.activeSelf)
            {
                textImage.gameObject.SetActive(true);
                questImage.gameObject.SetActive(false);
                isGuideStart = true;
            }
            
        }
        else 
        {
            questScriptable.isGuide = true;
            isGuideStart = false;
        }
        
    }
}
