using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuBarManager : MonoBehaviour
{
    public RectTransform panel; // 패널의 RectTransform
    public GameObject icon; // 줄어들면 아이콘 비활성화
    public Image ResizeButtonImage; // 리사이즈버튼 이미지
    public Sprite[] directionImage; // 리사이즈 버튼 스프라이트
    public float targetSize = 200; // 목표 크기
    public float duration = 0.3f; // 전환에 소요되는 시간
    private float originalSize; // 원래 크기를 저장할 변수
    private float currentTime = 0; // 현재 시간 갱신 변수
    private bool isAnimating = false; // 애니메이션 진행 중인지 확인하는 플래그


    void Start()
    {
        originalSize = panel.sizeDelta.x; // 초기 크기 설정
    }

    void Update()
    {
        if (isAnimating)
        {
            if (currentTime < duration)
            {
                float newSize = Mathf.Lerp(originalSize, targetSize, currentTime / duration);
                panel.sizeDelta = new Vector2(newSize, panel.sizeDelta.y);
                currentTime += Time.deltaTime;
               
            }
            else
            {
                panel.sizeDelta = new Vector2(targetSize, panel.sizeDelta.y);
                isAnimating = false; // 애니메이션이 끝났음을 표시
            }
        }
    }

    public void ToggleSize()
    {
        if (!isAnimating)
        {
            if (icon.activeSelf)
            {
                icon.SetActive(false);
                ResizeButtonImage.sprite = directionImage[1];
            }
            else
            {
                icon.SetActive(true);
                ResizeButtonImage.sprite = directionImage[0];
            }
            originalSize = panel.sizeDelta.x; // 현재 크기를 원래 크기로 설정
            targetSize = (originalSize == 200) ? 1000 : 200; // 목표 크기 토글
            currentTime = 0; // 시간 리셋
            isAnimating = true; // 애니메이션 시작
        }
    }
}
