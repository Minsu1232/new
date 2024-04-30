using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TooltipController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Text tooltipText; // Tooltip���� ����� Text ��ü. Unity �ν����Ϳ��� �Ҵ��ؾ� �մϴ�.
    public Canvas canvas; // Canvas ��ü. �� ��ü ���� �ν����Ϳ��� �Ҵ��ؾ� �մϴ�.

    private bool isHovering; // ���콺�� ������Ʈ ���� �ִ����� �Ǵ��ϴ� �÷���

    private Vector2 lastMousePosition;

    void Start()
    {
        if (tooltipText != null)
            tooltipText.gameObject.SetActive(false); // ������ �� Tooltip Text�� ����ϴ�.
        isHovering = false;
    }

    void Update()
    {
        if (isHovering)
        {
            Vector2 mousePosition = Input.mousePosition;
            if (Vector2.Distance(lastMousePosition, mousePosition) > 5) // ���콺�� ���� �Ÿ� �̻� �������� ���� ������Ʈ
            {
                Vector2 localPoint;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvas.transform as RectTransform,
                    mousePosition,
                    canvas.worldCamera,
                    out localPoint);

                tooltipText.rectTransform.anchoredPosition = localPoint + new Vector2(10, 10);
                lastMousePosition = mousePosition; // ������ ��ġ ������Ʈ
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (tooltipText != null)
        {
            tooltipText.gameObject.SetActive(true); // Text ��ü�� Ȱ��ȭ�Ͽ� �����ݴϴ�.
            isHovering = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltipText != null)
        {
            tooltipText.gameObject.SetActive(false); // ���콺�� ��ü ������ ������ Text ��ü�� ��Ȱ��ȭ�Ͽ� ����ϴ�.
            isHovering = false;
        }
    }
}
