using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public Image loading;
    float timeElapsed = 0;
    float lerpDuration = 3;
    public CanvasGroup panelCanvasGroup;
    public GameObject image;    
    public TextMeshProUGUI progressText;
    public GameObject playerhpbar;

    private void OnEnable()
    {// Loading ��Ȱ��
        timeElapsed = 0;
        lerpDuration = 3.0f; // �ִϸ��̼� �ð� �ʱ�ȭ
        loading.fillAmount = 0; // �ε� �� �ʱ�ȭ
        image.SetActive(true); // �̹��� Ȱ��ȭ
        panelCanvasGroup.alpha = 1; // �г� ���� �ʱ�ȭ
        if(playerhpbar.activeSelf)
        {
            playerhpbar.SetActive(false);
        }
       
    }
    // Start is called before the first frame update
    void Start()
    {
        int a = Random.Range(0, 2);
        if (a == 0)
        {
            progressText.text = "Tip ���� �ܿ� ������ �ȴٸ� �ı��� �����մϴ�.";
        }
        else
        {
            progressText.text = "Tip ��������Ʈ�� ����ٸ� �ʵ忡�� �����غ�����.";
        }
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
    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float lerpTime) // ȭ���� ������ �������
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
                if (!playerhpbar.activeSelf)
                {
                    playerhpbar.SetActive(true);
                }
                gameObject.SetActive(false); // ���̵�ƿ� �� ���� ������Ʈ ��Ȱ��ȭ
                break;
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
