using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vitals;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }  // 싱글턴 인스턴스
    public Player player;
    public Image[] image;
    public GameObject menu;
    public Text mana;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerStamina();


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
    public void PlayerStamina()
    {
        mana.text = $"{player.maxMp}/{player.mp}";
        if (player.maxMp > 0)  // 최대 MP가 0보다 클 때만 계산
        {
            image[1].fillAmount = (float)player.mp / player.maxMp;
        }
        else
        {
            image[1].fillAmount = 0;  // 최대 MP가 0이면 바를 0으로 설정
        }
    }
}
