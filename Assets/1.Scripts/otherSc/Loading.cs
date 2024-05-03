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
    public GameObject image;

    private void OnEnable()
    {// Loading 재활용
        timeElapsed = 0;
        lerpDuration = 3.0f; // 애니메이션 시간 초기화
        loading.fillAmount = 0; // 로딩 바 초기화
        image.SetActive(true); // 이미지 활성화
        panelCanvasGroup.alpha = 1; // 패널 투명도 초기화

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
        
            if (loading != null)
            {
                loading.fillAmount = Mathf.Lerp(0, 1, timeElapsed / lerpDuration);
                
            }
            
            timeElapsed += Time.deltaTime;
            if(loading.fillAmount >= 0.98f)
            {
                image.SetActive(false);
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

            cg.alpha = Mathf.Lerp(start, end, percentageComplete);

            if (percentageComplete >= 1)
            {
                gameObject.SetActive(false); // 페이드아웃 후 게임 오브젝트 비활성화
                break;
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
