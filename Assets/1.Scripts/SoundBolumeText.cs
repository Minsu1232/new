using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundBolumeText : MonoBehaviour
{
    public Text soundVolume;
    public Slider soundVolumeSlider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float soundValue = soundVolumeSlider.value * 100;
        
        soundValue = (float)Math.Round(soundValue, 0); //�ݿø� �Ͽ� �Ҽ��� ó��
        soundVolume.text = soundValue.ToString();
    }
}
