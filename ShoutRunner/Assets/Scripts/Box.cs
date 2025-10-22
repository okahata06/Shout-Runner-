using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
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
        if (voice_setting != null && voice_setting.GetVoiceVolume >= 0.9f)
        {
            Destroy(gameObject);
        }
    }
}
