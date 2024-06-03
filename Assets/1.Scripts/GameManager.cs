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
   
    public static GameManager Instance { get; private set; }  // �̱��� �ν��Ͻ�

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

    private float timeToUpdate = 1f; // �ð��� 1�ʸ��� ����
    private float timer = 0f; // �ð� ����
    private string lastDisplayedMinute;
    [Header("Transform")]
    public Transform bossStart;
    public Transform questStart;
    public Transform home;

    public bool isPrologue = false; // �̹� �ôٸ� ���ѷαװ� ������ �ʰ�
    public bool isShop;
    public QuestScriptable tutorial;
    public TextMeshProUGUI Guide;

    public float duration = 2.0f; // ���İ� ��ȭ�� �ҿ�Ǵ� �ð� (��)
    private float startTime; // ���İ� ��ȭ ���� �ð�
    private bool isFadingIn = false; // ���̵� �� ���� ����

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
            DontDestroyOnLoad(gameObject);  // ���� ����Ǿ �ı����� �ʵ��� ����
            dataManager = DataManager.Instance; // DataManager �̱��� �ν��Ͻ��� ����
            //inventorySave = Inventory.instance; // Inventory �̱��� �ν��Ͻ��� ����
            //moneyManager = MoneyManager.Instance; // moneymanager ����
        }
        else
        {
            Destroy(gameObject);  // �ߺ� �ν��Ͻ� ����
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
        UpdateTimeDisplay(); // �ʱ� �ð� ����        
        BowLoadGameData();
        bowManager.UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        // esc�� ���� ����
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
    public void BowSaveGameData() // ���⽺�� ����
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
        //    playerState = CreateInstance<PlayerState>(); // �ʱ� ���� �ε� �Ǵ� �� �ν��Ͻ� ����
        //}
    }
    // ������ ������ �ѹ��� �ϴ� �ż��� (��ư �Ҵ��)
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
            // CharacterController�� ��� ��Ȱ��ȭ
            controller.enabled = false;

            // ĳ������ ��ġ�� ����
            player.transform.position = bossStart.transform.position;

            // CharacterController�� �ٽ� Ȱ��ȭ
            controller.enabled = true;

            // �г� ����
            dungeonPanel.gameObject.SetActive(false);
        }
        else
        {
            // CharacterController�� ���� ���, ���� ��ġ ����
            player.transform.position = bossStart.transform.position;
        }
    }
    public void QuestZoneTrans()
    {
        CharacterController controller = player.GetComponent<CharacterController>();

        if (controller != null)
        {
            StartCoroutine(skillBarOff());
            // CharacterController�� ��� ��Ȱ��ȭ
            controller.enabled = false;

            // ĳ������ ��ġ�� ����
            player.transform.position = questStart.transform.position;

            // CharacterController�� �ٽ� Ȱ��ȭ
            controller.enabled = true;

            // �г� ����
            dungeonPanel.gameObject.SetActive(false);
        }
        else
        {
            // CharacterController�� ���� ���, ���� ��ġ ����
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
                float time = (Time.time - startTime) / duration; // ����ȭ�� �ð� ���
                float alpha = Mathf.Lerp(0, 1, time); // ���İ� ���
                Color newColor = Guide.color;
                newColor.a = alpha;
                Guide.color = newColor; // ���İ� ����

                // ���̵� �� �Ϸ� üũ
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
        Debug.Log("���� ����");
    }
    public void QuitPanelOpen()
    {
        quitPanel.SetActive(true);
    }
    public void QuitPanelOff()
    {
        quitPanel.SetActive(false);
    }
    //�÷��̾��� ī�޶� ����� �ż���
    public bool IsAnyUIActive()
    {
        
        return status.activeSelf || goHomePanel.activeSelf || gradegradePanel.activeSelf || shop.activeSelf || inventory.activeSelf || optionPanel.activeSelf || shopMenu.activeSelf || dungeonPanel.activeSelf || guidePanel.activeSelf || questPanel.activeSelf || savePanel.activeSelf;

    }
    private void OnApplicationQuit()
    {
        BowSaveGameData();
    }

}


