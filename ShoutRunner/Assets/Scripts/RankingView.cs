using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class RankingView : MonoBehaviour
{
    [Header("ランキング管理")]
    public RankingManager rankingManager;

    [Header("表示用Text（Legacy）")]
    public Text rankingText;

    void OnEnable()
    {
        ShowRanking();
    }

    public void ShowRanking()
    {
        List<int> scores = rankingManager.GetScores();

        rankingText.text = "";

        for (int i = 0; i < 10; i++)
        {
            //データがある場合
            if (i < scores.Count)
            {
                rankingText.text +=
                    (i + 1).ToString("00") + "位  " +
                    scores[i].ToString() + "\n";
            }
            //まだ埋まっていない順位
            else
            {
                rankingText.text +=
                    (i + 1).ToString("00") + "位  ----\n";
            }
        }
    }
}
