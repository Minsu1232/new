using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenemanager : MonoBehaviour
{
    public QuestScriptable prologue;
    public GameObject loadSCene;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void LoadMainScene()
    {
        if (!prologue.isPrologue)
        {
            SceneManager.LoadScene("Prologue");
        }
        else
        {
            //loadSCene.SetActive(true);// ���ѷα׸� �̹� �ôٸ� ���������ε��� �ٷ� �ε�
            SceneManager.LoadScene("TestScene");
        }
      
            
       
    }
    public void TestScene()
    {
        SceneManager.LoadScene("TestScene");
    }
}
