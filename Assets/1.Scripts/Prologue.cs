using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Prologue : MonoBehaviour
{
    public QuestScriptable prologue;
    public TextMeshProUGUI[] textObjects;  // UI Text 오브젝트 배열
    public float displayTime = 5f; // 자동 전환 시간 간격
    public GameObject mainLoading;

    private int currentIndex = 0;
    private float timer;

    void Start()
    {
        if (prologue.isPrologue)
        {
            EndPrologue();
        }
        else
        {
            prologue.isPrologue = true;
        }
        
        if (textObjects.Length > 0)
        {
            ShowText(currentIndex);
            timer = displayTime;
        }
    }

    void Update()
    {
        // 자동 전환 타이머
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            ShowNextText();
            timer = displayTime;
        }

        // 사용자 입력 처리
        if (Input.anyKeyDown)
        {
            ShowNextText();
        }
    }

    void ShowNextText()
    {
        currentIndex++;
        if (currentIndex < textObjects.Length)
        {
            ShowText(currentIndex);
        }
        else
        {
            // 모든 텍스트를 다 보여준 후, 게임의 다음 단계로 이동
            EndPrologue();
        }
    }

    void ShowText(int index)
    {
        // 모든 텍스트 오브젝트를 비활성화
        for (int i = 0; i < textObjects.Length; i++)
        {
            textObjects[i].gameObject.SetActive(false);
        }

        // 현재 텍스트 오브젝트만 활성화
        textObjects[index].gameObject.SetActive(true);
    }

    void EndPrologue()
    {
        // 프롤로그가 끝났을 때의 처리
        // 모든 텍스트 오브젝트를 비활성화
        for (int i = 0; i < textObjects.Length; i++)
        {
            if (textObjects[i].enabled)
            {
                textObjects[i].gameObject.SetActive(false);
            }
            textObjects[i].gameObject.SetActive(false);
        }
        mainLoading.gameObject.SetActive(true);
        Debug.Log("Prologue Ended");
    }
}
