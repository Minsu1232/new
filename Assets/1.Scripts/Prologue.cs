using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Prologue : MonoBehaviour
{
    public QuestScriptable prologue;
    public TextMeshProUGUI[] textObjects;  // UI Text ������Ʈ �迭
    public float displayTime = 5f; // �ڵ� ��ȯ �ð� ����
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
        // �ڵ� ��ȯ Ÿ�̸�
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            ShowNextText();
            timer = displayTime;
        }

        // ����� �Է� ó��
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
            // ��� �ؽ�Ʈ�� �� ������ ��, ������ ���� �ܰ�� �̵�
            EndPrologue();
        }
    }

    void ShowText(int index)
    {
        // ��� �ؽ�Ʈ ������Ʈ�� ��Ȱ��ȭ
        for (int i = 0; i < textObjects.Length; i++)
        {
            textObjects[i].gameObject.SetActive(false);
        }

        // ���� �ؽ�Ʈ ������Ʈ�� Ȱ��ȭ
        textObjects[index].gameObject.SetActive(true);
    }

    void EndPrologue()
    {
        // ���ѷαװ� ������ ���� ó��
        // ��� �ؽ�Ʈ ������Ʈ�� ��Ȱ��ȭ
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
