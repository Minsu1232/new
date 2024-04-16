using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vitals;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }  // �̱��� �ν��Ͻ�
    public Player player;
    public Image[] image;
    public GameObject menu;
    public Text mana;

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
        if (player.maxMp > 0)  // �ִ� MP�� 0���� Ŭ ���� ���
        {
            image[1].fillAmount = (float)player.mp / player.maxMp;
        }
        else
        {
            image[1].fillAmount = 0;  // �ִ� MP�� 0�̸� �ٸ� 0���� ����
        }
    }
}
