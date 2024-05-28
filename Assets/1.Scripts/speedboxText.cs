using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class speedboxText : MonoBehaviour
{
    public RectTransform balloonImageRectTransform; // ��ǳ�� �̹����� RectTransform
    public TextMeshProUGUI balloonText; // ��ǳ���� �ؽ�Ʈ ������Ʈ
    public GameObject textImage;
    public GameObject questImage;
    public string fullText; // ��ü ǥ���� �ؽ�Ʈ
    private float delay = 0.05f; // ���ڰ� ��Ÿ���� ������ �ð� (��)
    public bool isGuideStart;
    private Coroutine currentCoroutine; //�������� �ڷ�ƾ �Ҵ�

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
        currentCoroutine = StartCoroutine(ShowTextOneByOne(fullText)); //�������� �ڷ�ƾ �Ҵ�
    }
    public void Update()
    {
        if (questScriptable.isGuide) 
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine); // ���� ���� ���� �ڷ�ƾ ����
                balloonText.text = ""; // �ؽ�Ʈ �ʱ�ȭ
            }
            questScriptable.CheckQuestCompletion();
            fullText = questScriptable.questDetail;
            currentCoroutine = StartCoroutine(ShowTextOneByOne(fullText)); // �� �ڷ�ƾ ����

        }
    }

    private IEnumerator ShowTextOneByOne(string completeText)
    {
        balloonText.text = ""; // �ʱ� �ؽ�Ʈ�� ���ϴ�.
        
        foreach (char letter in completeText)
        {
            balloonText.text += letter; // �ϳ��� ���ڸ� �߰��մϴ�.
            UpdateBalloonSize(); // ��ǳ�� ũ�⸦ ������Ʈ�մϴ�.
            yield return new WaitForSeconds(delay); // ���� ���ڰ� ��Ÿ���� �� ������
        
        }
    }

    // ��ǳ�� ũ�⸦ ������Ʈ�ϴ� �Լ�
    private void UpdateBalloonSize()
    {
        // �ؽ�Ʈ ���뿡 �°� ��ǳ�� ũ�� ����
        Vector2 newSize = new Vector2(balloonText.preferredWidth, balloonText.preferredHeight);
        balloonImageRectTransform.sizeDelta = newSize;
    }
    private void OnMouseDown()
    {
        if (!isGuideStart) // ù ���̵��� ������ npc Ŭ��
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
