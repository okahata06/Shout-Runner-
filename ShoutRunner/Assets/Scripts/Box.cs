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

    private Transform Target;

    // Start is called before the first frame update
    void Start()
    {
        //VoiceSettingスクリプトがついたオブジェクトを取得
        voice_setting = FindFirstObjectByType<VoiceSetting>();

        //Target = Camera.main.gameObject.transform;
        Target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (voice_setting != null && voice_setting.GetVoiceVolume >= destroyVolume && this.transform.position.z - Target.position.z <= 6)
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
