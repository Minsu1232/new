using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuBarManager : MonoBehaviour
{
    public RectTransform panel; // �г��� RectTransform
    public GameObject icon; // �پ��� ������ ��Ȱ��ȭ
    public Image ResizeButtonImage; // ���������ư �̹���
    public Sprite[] directionImage; // �������� ��ư ��������Ʈ
    public float targetSize = 200; // ��ǥ ũ��
    public float duration = 0.3f; // ��ȯ�� �ҿ�Ǵ� �ð�
    private float originalSize; // ���� ũ�⸦ ������ ����
    private float currentTime = 0; // ���� �ð� ���� ����
    private bool isAnimating = false; // �ִϸ��̼� ���� ������ Ȯ���ϴ� �÷���


    void Start()
    {
        originalSize = panel.sizeDelta.x; // �ʱ� ũ�� ����
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
                isAnimating = false; // �ִϸ��̼��� �������� ǥ��
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
            originalSize = panel.sizeDelta.x; // ���� ũ�⸦ ���� ũ��� ����
            targetSize = (originalSize == 200) ? 1000 : 200; // ��ǥ ũ�� ���
            currentTime = 0; // �ð� ����
            isAnimating = true; // �ִϸ��̼� ����
        }
    }
}
