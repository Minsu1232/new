using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenemanager : MonoBehaviour
{
    public QuestScriptable prologue;
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

        // prologue.isPrologue�� ���� ���� ������ ���� �ε��մϴ�.
        if (prologue.isPrologue == false)
        {
            SceneManager.LoadScene("Prologue");
            prologue.isPrologue = true;
        }
        else
        {
            SceneManager.LoadScene("Village");
        }
    }
}
