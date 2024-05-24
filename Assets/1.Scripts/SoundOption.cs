using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundOption : MonoBehaviour
{
    public Slider volumeSlider;  // 에디터에서 할당할 슬라이더
    public AudioSource audioSource;  // 에디터에서 할당할 오디오 소스

    void Start()
    {
        // 초기 볼륨을 슬라이더의 값과 동기화
        if (audioSource != null && volumeSlider != null)
            audioSource.volume = volumeSlider.value;
    }

    public void OnVolumeChange()
    {
        // 슬라이더 값이 변경될 때 오디오 볼륨 업데이트
        if (audioSource != null && volumeSlider != null)
            audioSource.volume = volumeSlider.value;
    }
}
