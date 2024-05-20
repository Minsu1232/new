using System.Collections;
using System.Collections.Generic;
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



    [Header("Transform")]
    public Transform bossStart;
    public Transform questStart;

    public bool isPrologue = false; // 이미 봤다면 프롤로그가 나오지 않게
    public bool isShop;
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
        isPrologue = false; // 테스트
    }

    // Update is called once per frame
    void Update()
    {
        // esc로 끌수 있음
        if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
        {
            shop.SetActive(false);
            isShop = false;
        }
        StatusOpen();
        InventoryOpen();

    }
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
    public void StatusOpen()
    {
        if(UnityEngine.Input.GetKeyDown(KeyCode.E)) 
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
}


