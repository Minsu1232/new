using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public Image loading;
    float timeElapsed = 0;
    float lerpDuration = 3;
    public CanvasGroup panelCanvasGroup;

    private void OnEnable()
    {
        // 초기화
        timeElapsed = 0;
        lerpDuration = 3.0f; // 3초 동안 애니메이션 실행

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (timeElapsed < lerpDuration)
        {
            loading.fillAmount = Mathf.Lerp(0, 1, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            if(loading.fillAmount >= 0.98f)
            {
                gameObject.SetActive(false);
                StartCoroutine(FadeCanvasGroup(panelCanvasGroup, panelCanvasGroup.alpha, 0, 1.5f));
            }
        }
    }
    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float lerpTime)
    {
        float _timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - _timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;

        while (true)
        {
            timeSinceStarted = Time.time - _timeStartedLerping;
            percentageComplete = timeSinceStarted / lerpTime;

            float currentValue = Mathf.Lerp(start, end, percentageComplete);

            cg.alpha = currentValue;

            if (percentageComplete >= 1) break;

            yield return new WaitForEndOfFrame();

        }
    }
}
