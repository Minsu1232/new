using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class speedboxText : MonoBehaviour
{
    public RectTransform balloonImageRectTransform; // ��ǳ�� �̹����� RectTransform
    public TextMeshProUGUI balloonText; // ��ǳ���� �ؽ�Ʈ ������Ʈ
    public string fullText; // ��ü ǥ���� �ؽ�Ʈ
    private float delay = 0.1f; // ���ڰ� ��Ÿ���� ������ �ð� (��)

    private void Start()
    {
        fullText = "���� ������ �����ô� ����, \n ������ �� �ؾ� �� ���� �ֽ��ϴ�.";
        StartCoroutine(ShowTextOneByOne(fullText));
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
}
