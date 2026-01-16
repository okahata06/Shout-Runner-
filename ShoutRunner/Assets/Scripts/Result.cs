using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.Impl;

public class Result : MonoBehaviour
{
    float total = 0;

    [Header("スコアのテキスト")]
    public Text ScoreText;

    [Header("スコアのテキスト")]
    public Text VolumeText;

    [Header("スコアのテキスト")]
    public Text TotalText;

    private void OnEnable()
    {
        Debug.Log(VoiceToText.maxVolume);
        float volume = Mathf.Clamp01(VoiceToText.maxVolume);

        // 倍率計算
        float rawMultiplier = 1.0f + volume;

        // 小数第1位で切り捨て
        float scoreMultiplier = Mathf.Floor(rawMultiplier * 10f) / 10f;

        // スコア加算
        total += StageTips.playerScore * scoreMultiplier;

        ScoreText.text = StageTips.playerScore.ToString();
        VolumeText.text = scoreMultiplier.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private IEnumerator ResultText()
    //{

    //}
}
