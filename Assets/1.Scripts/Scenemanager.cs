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

        // prologue.isPrologue의 값에 따라 적절한 씬을 로드합니다.
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
