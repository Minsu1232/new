using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TooltipController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Text tooltipText; // Tooltip으로 사용할 Text 객체. Unity 인스펙터에서 할당해야 합니다.
    public Canvas canvas; // Canvas 객체. 이 객체 또한 인스펙터에서 할당해야 합니다.

    private bool isHovering; // 마우스가 오브젝트 위에 있는지를 판단하는 플래그

    private Vector2 lastMousePosition;

    void Start()
    {
        if (tooltipText != null)
            tooltipText.gameObject.SetActive(false); // 시작할 때 Tooltip Text를 숨깁니다.
        isHovering = false;
    }

    void Update()
    {
        if (isHovering)
        {
            Vector2 mousePosition = Input.mousePosition;
            if (Vector2.Distance(lastMousePosition, mousePosition) > 5) // 마우스가 일정 거리 이상 움직였을 때만 업데이트
            {
                Vector2 localPoint;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvas.transform as RectTransform,
                    mousePosition,
                    canvas.worldCamera,
                    out localPoint);

                tooltipText.rectTransform.anchoredPosition = localPoint + new Vector2(10, 10);
                lastMousePosition = mousePosition; // 마지막 위치 업데이트
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (tooltipText != null)
        {
            tooltipText.gameObject.SetActive(true); // Text 객체를 활성화하여 보여줍니다.
            isHovering = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltipText != null)
        {
            tooltipText.gameObject.SetActive(false); // 마우스가 객체 밖으로 나가면 Text 객체를 비활성화하여 숨깁니다.
            isHovering = false;
        }
    }
}
