using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingMainScene : MonoBehaviour
{
    public Image progressBar;
    public TextMeshProUGUI progressText;

    void Start()
    {
        // 랜덤한 팁 문구 출력
        int a = Random.Range(0, 2);
        if(a == 0)
        {
            progressText.text = "Tip 메인퀘스트 위치에는 파괴가 가능한 오브젝트가 있습니다.";
        }
        else
        {
            progressText.text = "Tip 메인퀘스트가 힘들다면 필드에서 성장해보세요.";
        }
        // 코루틴을 시작하여 비동기적으로 메인 씬을 로드합니다.
        StartCoroutine(LoadMainSceneAsync());
    }

    IEnumerator LoadMainSceneAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("Village");
        operation.allowSceneActivation = false; // 씬 전환을 자동으로 하지 않도록 설정

        float targetProgress = 0;
        while (!operation.isDone)
        {
            // operation.progress는 0.9에서 멈추지만, 사용자에게는 1로 보이게 합니다.
            targetProgress = operation.progress < 0.9f ? operation.progress : 1.0f;
            progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, targetProgress, Time.deltaTime * 5); // 5는 보간 속도, 조절 가능

            if (operation.progress >= 0.9f && progressBar.fillAmount >= 0.99f) // progressBar.fillAmount가 거의 1에 가까울 때
            {
                operation.allowSceneActivation = true; // 씬 전환 허용
            }
            yield return null;
        }
    }
}
