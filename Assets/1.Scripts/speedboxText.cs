using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class speedboxText : MonoBehaviour
{
    public RectTransform balloonImageRectTransform; // 말풍선 이미지의 RectTransform
    public TextMeshProUGUI balloonText; // 말풍선의 텍스트 컴포넌트
    public string fullText; // 전체 표시할 텍스트
    private float delay = 0.1f; // 글자가 나타나는 딜레이 시간 (초)

    private void Start()
    {
        fullText = "이제 밖으로 나가시는 군요, \n 나가기 전 해야 할 일이 있습니다.";
        StartCoroutine(ShowTextOneByOne(fullText));
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
}
