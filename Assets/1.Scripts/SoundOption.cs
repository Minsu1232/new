using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundOption : MonoBehaviour
{
    public Slider volumeSlider;  // �����Ϳ��� �Ҵ��� �����̴�
    public AudioSource audioSource;  // �����Ϳ��� �Ҵ��� ����� �ҽ�
    public Text soundVolum;

    void Start()
    {
        // �ʱ� ������ �����̴��� ���� ����ȭ
        if (audioSource != null && volumeSlider != null)
            audioSource.volume = volumeSlider.value;
    }

    public void OnVolumeChange()
    {
        // �����̴� ���� ����� �� ����� ���� ������Ʈ
        if (audioSource != null && volumeSlider != null)
        {
            float scaledVolume = volumeSlider.value / 100f; // �����̴��� �������

            audioSource.volume = scaledVolume;

            float text = scaledVolume * 100;
            
            soundVolum.text = text.ToString("F0");
        }
            
    }
}
