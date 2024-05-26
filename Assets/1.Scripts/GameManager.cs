using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using Vitals;

public class GameManager : MonoBehaviour
{
   
    public static GameManager Instance { get; private set; }  // 싱글턴 인스턴스

    [Header("Player UI")]
    public Player player;
    public Image[] image;
    public Text mana;
    public GameObject status;
    public GameObject skillBar;
    

    [Header("Game UI")]
    public GameObject menu;
    public GameObject shopMenu;
    public GameObject shop;
    public GameObject dungeonPanel;
    public GameObject transPanel;
    public GameObject inventory;
    public CanvasGroup panelCanvasGroup;
    public GameObject guidePanel;
    public GameObject optionPanel;
    public GameObject questPanel;
    //public Outline questOutline;
    //public Outline shopOutline;


    [Header("Transform")]
    public Transform bossStart;
    public Transform questStart;

    public bool isPrologue = false; // 이미 봤다면 프롤로그가 나오지 않게
    public bool isShop;
    public QuestScriptable tutorial;
    public TextMeshProUGUI Guide;

    public float duration = 2.0f; // 알파값 변화에 소요되는 시간 (초)
    private float startTime; // 알파값 변화 시작 시간
    private bool isFadingIn = false; // 페이드 인 상태 관리

    public GameObject tutorial2startzone;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // 씬이 변경되어도 파괴되지 않도록 설정
        }
        else
        {
            Destroy(gameObject);  // 중복 인스턴스 방지
        }
    }
    // Start is called before the first frame update
    void Start()
    {


        if (!tutorial.isTutorial)
        {
            Guide.gameObject.SetActive(true);
            StartFadingIn2();
        }
        else
        {
            Destroy(tutorial2startzone);
        }

        isPrologue = false; // 테스트
    }

    // Update is called once per frame
    void Update()
    {
        // esc로 끌수 있음
        if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
        {
            shop.SetActive(false);
            menu.SetActive(false);
            shopMenu.SetActive(false); 
            dungeonPanel.SetActive(false);
            status.SetActive(false);
            inventory.SetActive(false);
            guidePanel.SetActive(false);
            optionPanel.SetActive(false);
            isShop = false;

        }
        StatusOpen();
        InventoryOpen();
        TutorialOn();

    }
    //public void OnMouseEnter2()
    //{
    //    questOutline.enabled = true;
    //}
    public void InventoryOpen()
    {
        
        if (UnityEngine.Input.GetKeyDown(KeyCode.I))
        {
            if (inventory.activeSelf)
            {
                inventory.SetActive(false);
            }
            else
            {
                inventory.SetActive(true);
            }

        }

    }
    public void InventoryOpenButton()
    {
       inventory.gameObject.SetActive(true);
    }
    public void OptionPanelOpen()
    {
        optionPanel.SetActive(true);
    }
    public void OptionPanelOff()
    {
        optionPanel.SetActive(false);
    }
    public void StatusOpen()
    {
        
        if (UnityEngine.Input.GetKeyDown(KeyCode.E))
        {
            if (!status.activeSelf)
            {
                status.gameObject.SetActive(true);
            }
            else
            {
                status.gameObject.SetActive(false);
            }

        }
    }
    public void StatusOpenButton()
    {
        status.gameObject.SetActive(true);
    }
    public void GuideOpen()
    {
        guidePanel.SetActive(true);
    }
    public void GuideOff()
    {
        guidePanel.SetActive(false);
    }

    public void StatusOpenByMouse()
    {

        if (!status.activeSelf)
        {
            status.gameObject.SetActive(true);
        }
     
        else
        {
            status.gameObject.SetActive(false);
        }


    }
    public void StatusClose()
    {
        status.gameObject.SetActive(false);
    }
    public void MenuOpen()
    {
        if (!menu.activeSelf)
        {
            menu.SetActive(true);
        }
        else
        {
            menu.SetActive(false);
        }

    }
    public void ShopOpen()
    {

        if (!shop.activeSelf)
        {
            isShop = false;
        }
        else
        {
            isShop = true;
        }
    }
    public void ShopExit()
    {
        shop.SetActive(false);

        if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
        {
            shop.SetActive(false);
        }
        isShop = false;
    }
    public void BossZoneTrans()
    {
        CharacterController controller = player.GetComponent<CharacterController>();

        if (controller != null)
        {
            StartCoroutine(skillBarOff());
            // CharacterController를 잠시 비활성화
            controller.enabled = false;

            // 캐릭터의 위치를 변경
            player.transform.position = bossStart.transform.position;

            // CharacterController를 다시 활성화
            controller.enabled = true;

            // 패널 꺼짐
            dungeonPanel.gameObject.SetActive(false);
        }
        else
        {
            // CharacterController가 없는 경우, 직접 위치 설정
            player.transform.position = bossStart.transform.position;
        }
    }
    public void QuestZoneTrans()
    {
        CharacterController controller = player.GetComponent<CharacterController>();

        if (controller != null)
        {
            StartCoroutine(skillBarOff());
            // CharacterController를 잠시 비활성화
            controller.enabled = false;

            // 캐릭터의 위치를 변경
            player.transform.position = questStart.transform.position;

            // CharacterController를 다시 활성화
            controller.enabled = true;

            // 패널 꺼짐
            dungeonPanel.gameObject.SetActive(false);
        }
        else
        {
            // CharacterController가 없는 경우, 직접 위치 설정
            player.transform.position = bossStart.transform.position;
        }
    }
    public void PanelClose()
    {
        dungeonPanel.gameObject.SetActive(false);
    }
    public void StartFadeIn()
    {
        if (!transPanel.activeSelf)
        {
            transPanel.gameObject.SetActive(true);
        }


    }

    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float lerpTime)
    {
        float _timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - _timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;

        while (true)
        {
            timeSinceStarted = Time.time - _timeStartedLerping;
            percentageComplete = timeSinceStarted / lerpTime;

            float currentValue = Mathf.Lerp(start, end, percentageComplete);

            cg.alpha = currentValue;

            if (percentageComplete >= 1) break;

            yield return new WaitForEndOfFrame();

        }
    }
    IEnumerator skillBarOff()
    {
        skillBar.SetActive(false);
        yield return new WaitForSeconds(3f);
        skillBar.SetActive(true);
    }
    public void TutorialOn()
    {
        if (!tutorial.isTutorial)
        {
            if (isFadingIn)
            {
                float time = (Time.time - startTime) / duration; // 정규화된 시간 계산
                float alpha = Mathf.Lerp(0, 1, time); // 알파값 계산
                Color newColor = Guide.color;
                newColor.a = alpha;
                Guide.color = newColor; // 알파값 적용

                // 페이드 인 완료 체크
                if (time >= 1.0f)
                {
                    isFadingIn = false;
                    Destroy(Guide.gameObject);
                }
            }
        }
    }

    private void StartFadingIn2()
    {
        startTime = Time.time;
        isFadingIn = true;
    }
    public void Quit()
    {
        Application.Quit();
    }
    //플레이어의 카메라 제어용 매서드
    public bool IsAnyUIActive()
    {
        return shop.activeSelf || inventory.activeSelf || optionPanel.activeSelf || menu.activeSelf || shopMenu.activeSelf || dungeonPanel.activeSelf || guidePanel.activeSelf || questPanel.activeSelf;
    }

}


