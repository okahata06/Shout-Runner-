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

    [Header("ボリュームボーナスのテキスト")]
    public Text VolumeText;

    [Header("トータルのテキスト")]
    public Text TotalText;

    [Header("ランキング")]
    public GameObject Ranking;

    RankingManager ranking;

    private void Awake()
    {
        ranking = FindFirstObjectByType<RankingManager>();

        Ranking.SetActive(false);
    }

    private void OnEnable()
    {
        StartCoroutine(ResultText());
    }

    private IEnumerator ResultText()
    {
        float volume = Mathf.Clamp01(VoiceToText.maxVolume);

        //倍率計算
        float rawMultiplier = 1.0f + volume;

        //小数第1位で切り捨て
        float scoreMultiplier = Mathf.Floor(rawMultiplier * 10f) / 10f;

        //スコア加算
        total = Mathf.Round(StageTips.playerScore * scoreMultiplier);
        ranking.AddScore((int)total);

        ScoreText.text = StageTips.playerScore.ToString();

        yield return new WaitForSeconds(2);

        VolumeText.text = "×"　+ scoreMultiplier.ToString();

        yield return new WaitForSeconds(2);

        TotalText.text = total.ToString();

        yield return new WaitForSeconds(3);

        Ranking.SetActive(true); 
    }
}
