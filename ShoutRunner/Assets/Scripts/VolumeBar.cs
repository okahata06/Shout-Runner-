using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeBar : MonoBehaviour
{
    [Header("Volumeゲージのイメージ")]
    [SerializeField] Image VolumeGauge;

    private VoiceSetting voice_setting;//VoiceSettingスクリプト取得用

    // Start is called before the first frame update
    void Start()
    {
        //VoiceSettingスクリプトがついたオブジェクトを取得
        voice_setting = FindFirstObjectByType<VoiceSetting>();
    }

    // Update is called once per frame
    void Update()
    {
        VolumeGauge.fillAmount = voice_setting.GetVoiceVolume;
    }
}
