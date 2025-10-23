using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    private VoiceSetting voice_setting;//VoiceSettingスクリプト取得用

    [Header("Boxを破壊するのに必要なVolume")]
    public float destroyVolume;

    [Header("壊れた時のエフェクト")]
    public GameObject woodEffectPrefab;
    public GameObject dustEffectPrefab;

    // Start is called before the first frame update
    void Start()
    {
        //VoiceSettingスクリプトがついたオブジェクトを取得
        voice_setting = FindFirstObjectByType<VoiceSetting>();
    }

    // Update is called once per frame
    void Update()
    {
        if (voice_setting != null && voice_setting.GetVoiceVolume >= destroyVolume)
            BreakBox();
    }

    /// <summary>
    /// Box破壊用関数
    /// </summary>
    void BreakBox()
    {
        if (woodEffectPrefab != null)
            Instantiate(woodEffectPrefab, transform.position, Quaternion.identity);

        if (dustEffectPrefab != null)
            Instantiate(dustEffectPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

}
