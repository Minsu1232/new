using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingMainScene : MonoBehaviour
{
    public Image progressBar;
    public Text progressText;

    void Start()
    {
        // 코루틴을 시작하여 비동기적으로 메인 씬을 로드합니다.
        StartCoroutine(LoadMainSceneAsync());
    }

    IEnumerator LoadMainSceneAsync()
    {
        // 메인 씬을 비동기적으로 로드합니다.
        AsyncOperation operation = SceneManager.LoadSceneAsync("Village");

        // 로딩이 완료될 때까지 반복합니다.
        while (!operation.isDone)
        {
            // 로딩 진행 상태를 슬라이더와 텍스트에 업데이트합니다.
            float progress = Mathf.Clamp01(operation.progress / 1f);
            progressBar.fillAmount = progress;
           

            yield return null;
        }
    }
}
