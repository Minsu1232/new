using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vitals;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }  // 싱글턴 인스턴스

    [Header("Player UI")]
    public Player player;
    public Image[] image;    
    public Text mana;

    [Header("Game UI")]
    public GameObject menu;

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
   
}
