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
        // �ڷ�ƾ�� �����Ͽ� �񵿱������� ���� ���� �ε��մϴ�.
        StartCoroutine(LoadMainSceneAsync());
    }

    IEnumerator LoadMainSceneAsync()
    {
        // ���� ���� �񵿱������� �ε��մϴ�.
        AsyncOperation operation = SceneManager.LoadSceneAsync("Village");

        // �ε��� �Ϸ�� ������ �ݺ��մϴ�.
        while (!operation.isDone)
        {
            // �ε� ���� ���¸� �����̴��� �ؽ�Ʈ�� ������Ʈ�մϴ�.
            float progress = Mathf.Clamp01(operation.progress / 1f);
            progressBar.fillAmount = progress;
           

            yield return null;
        }
    }
}
