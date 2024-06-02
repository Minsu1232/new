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
        // ������ �� ���� ���
        int a = Random.Range(0, 2);
        if(a == 0)
        {
            progressText.text = "Tip ��������Ʈ ��ġ���� �ı��� ������ ������Ʈ�� �ֽ��ϴ�.";
        }
        else
        {
            progressText.text = "Tip ��������Ʈ�� ����ٸ� �ʵ忡�� �����غ�����.";
        }
        // �ڷ�ƾ�� �����Ͽ� �񵿱������� ���� ���� �ε��մϴ�.
        StartCoroutine(LoadMainSceneAsync());
    }

    IEnumerator LoadMainSceneAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("Village");
        operation.allowSceneActivation = false; // �� ��ȯ�� �ڵ����� ���� �ʵ��� ����

        float targetProgress = 0;
        while (!operation.isDone)
        {
            // operation.progress�� 0.9���� ��������, ����ڿ��Դ� 1�� ���̰� �մϴ�.
            targetProgress = operation.progress < 0.9f ? operation.progress : 1.0f;
            progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, targetProgress, Time.deltaTime * 5); // 5�� ���� �ӵ�, ���� ����

            if (operation.progress >= 0.9f && progressBar.fillAmount >= 0.99f) // progressBar.fillAmount�� ���� 1�� ����� ��
            {
                operation.allowSceneActivation = true; // �� ��ȯ ���
            }
            yield return null;
        }
    }
}
