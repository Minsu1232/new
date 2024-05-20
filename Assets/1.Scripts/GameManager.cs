using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using Vitals;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }  // �̱��� �ν��Ͻ�

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

    public bool isPrologue = false; // �̹� �ôٸ� ���ѷαװ� ������ �ʰ�
    public bool isShop;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // ���� ����Ǿ �ı����� �ʵ��� ����
        }
        else
        {
            Destroy(gameObject);  // �ߺ� �ν��Ͻ� ����
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        isPrologue = false; // �׽�Ʈ
    }

    // Update is called once per frame
    void Update()
    {
        // esc�� ���� ����
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
}


