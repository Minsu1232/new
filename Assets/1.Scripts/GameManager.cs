using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine.Windows;
using Vitals;

public class GameManager : MonoBehaviour
{
   
    public static GameManager Instance { get; private set; }  // 싱글턴 인스턴스

   public DataManager dataManager;
   //public  Inventory inventorySave;
   //public MoneyManager moneyManager;

    [Header("Player UI")]
    public Player player;
    public Image[] image;
    public Text mana;
    public GameObject status;
    public GameObject skillBar;
    

    [Header("Game UI")]
    
    public GameObject shopMenu;
    public GameObject shop;
    public GameObject dungeonPanel;
    public GameObject transPanel;
    public GameObject inventory;
    public CanvasGroup panelCanvasGroup;
    public GameObject guidePanel;
    public GameObject optionPanel;
    public GameObject questPanel;
    public GameObject goHomePanel;
    public GameObject savePanel;
    public GameObject quitPanel;
    public GameObject gradegradePanel;
    public GameObject gradePanel;
    public TextMeshProUGUI nowtime;
    public BowEnHancementSystem bowManager;
    public Weapon weapon;
    //public Outline questOutline;
    //public Outline shopOutline;

    private float timeToUpdate = 1f; // 시간을 1초마다 갱신
    private float timer = 0f; // 시간 누적
    private string lastDisplayedMinute;
    [Header("Transform")]
    public Transform bossStart;
    public Transform questStart;
    public Transform home;

    public bool isPrologue = false; // 이미 봤다면 프롤로그가 나오지 않게
    public bool isShop;
    public QuestScriptable tutorial;
    public TextMeshProUGUI Guide;

    public float duration = 2.0f; // 알파값 변화에 소요되는 시간 (초)
    private float startTime; // 알파값 변화 시작 시간
    private bool isFadingIn = false; // 페이드 인 상태 관리

    public GameObject tutorial2startzone;

    string bowEnHancementPath;
    private void OnEnable()
    {
        BowLoadGameData();
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // 씬이 변경되어도 파괴되지 않도록 설정
            dataManager = DataManager.Instance; // DataManager 싱글톤 인스턴스를 참조
            //inventorySave = Inventory.instance; // Inventory 싱글톤 인스턴스를 참조
            //moneyManager = MoneyManager.Instance; // moneymanager 참조
        }
        else
        {
            Destroy(gameObject);  // 중복 인스턴스 방지
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        bowEnHancementPath = Path.Combine(Application.persistentDataPath, "bow.json");

        if (!tutorial.isTutorial)
        {
            Guide.gameObject.SetActive(true);
            StartFadingIn2();
        }
        else
        {
            Destroy(tutorial2startzone);
        }
        lastDisplayedMinute = DateTime.Now.ToString("HH:mm");
        UpdateTimeDisplay(); // 초기 시간 설정        
        BowLoadGameData();
        bowManager.UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        // esc로 끌수 있음
        if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
        {
            shop.SetActive(false);            
            shopMenu.SetActive(false); 
            dungeonPanel.SetActive(false);
            status.SetActive(false);
            inventory.SetActive(false);
            guidePanel.SetActive(false);
            optionPanel.SetActive(false);
            questPanel.SetActive(false);
            goHomePanel.SetActive(false);
            savePanel.SetActive(false);
            questPanel.SetActive(false);
            gradegradePanel.SetActive(false);
            gradePanel.SetActive(false);
            isShop = false;

        }
        StatusOpen();
        InventoryOpen();
        TutorialOn();
        string currentMinute = DateTime.Now.ToString("HH:mm");
        if (currentMinute != lastDisplayedMinute)
        {
            UpdateTimeDisplay();
            lastDisplayedMinute = currentMinute;
        }

    }
    public void BowSaveGameData() // 무기스탯 저장
    {
        string json = JsonUtility.ToJson(weapon, true);
        System.IO.File.WriteAllText(bowEnHancementPath, json);
        Debug.Log("Game data saved to " + bowEnHancementPath);
    }
    public void BowLoadGameData()
    {
        if (System.IO.File.Exists(bowEnHancementPath))
        {
            string json = System.IO.File.ReadAllText(bowEnHancementPath);
            JsonUtility.FromJsonOverwrite(json, weapon);
            Debug.Log("Game data loaded from " + bowEnHancementPath);
            
        }
        //else
        //{
        //    Debug.Log("No save file found, loading default settings.");
        //    playerState = CreateInstance<PlayerState>(); // 초기 설정 로드 또는 새 인스턴스 생성
        //}
    }
    // 데이터 저장을 한번에 하는 매서드 (버튼 할당용)
    public void SaveAll()
    {
        if (dataManager != null)
        {
            dataManager.PlayerSaveGameData();
            dataManager.PrologueSaveGameData();
            dataManager.TutorialSaveGameData();
            dataManager.MainSaveGameData();
            dataManager.DailySaveGameData();
            Inventory.instance.SaveInventory();
            MoneyManager.Instance.SaveMoney();
            BowSaveGameData();
        }
        else
        {
            Debug.LogError("DataManager instance is not found.");
        }

        //if (inventorySave != null)
        //{
        //    inventorySave.SaveInventory();
        //}
        //else
        //{
        //    Debug.LogError("Inventory instance is not found.");
        //}
        //if(moneyManager != null)
        //{
        //    moneyManager.SaveMoney();
        //}
        //else
        //{
        //    Debug.LogError("MoneyManager instance is not found.");
        //}
        Debug.Log("All data saved");
        savePanel.SetActive(false);
    }
    public void GradePanelOn()
    {
        gradePanel.SetActive(true);
    }
    public void GradePanelOff()
    {
        gradePanel.SetActive(false);
    }
    public void SavePanelOn()
    {
        savePanel.SetActive(true);
    }
    public void SavePaenlOff()
    {
        savePanel.SetActive(false);
    }
    void UpdateTimeDisplay()
    {
        if (nowtime != null)
        {
            nowtime.text = DateTime.Now.ToString("HH:mm");  
        }
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
    public void GoHomePanelOpen()
    {
        goHomePanel.SetActive(true);
    }
    public void GoHomePanelOff()
    {
        goHomePanel.SetActive(false);
    }
    public void GoHomeAgree()
    {
        goHomePanel.SetActive(false);
        player.controller.enabled = false;
        player.transform.position = home.transform.position;
        player.controller.enabled = true;
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
    public void ShopOpenButton()
    {
        shop.SetActive(true);
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
        Debug.Log("게임 종료");
    }
    public void QuitPanelOpen()
    {
        quitPanel.SetActive(true);
    }
    public void QuitPanelOff()
    {
        quitPanel.SetActive(false);
    }
    //플레이어의 카메라 제어용 매서드
    public bool IsAnyUIActive()
    {
        
        return status.activeSelf || goHomePanel.activeSelf || gradegradePanel.activeSelf || shop.activeSelf || inventory.activeSelf || optionPanel.activeSelf || shopMenu.activeSelf || dungeonPanel.activeSelf || guidePanel.activeSelf || questPanel.activeSelf || savePanel.activeSelf;

    }
    private void OnApplicationQuit()
    {
        BowSaveGameData();
    }

}


