using System.Collections.Generic;
using UnityEngine;

public class RankingManager : MonoBehaviour
{
    const int MAX_RANK = 10;
    const string SAVE_KEY = "RANKING_SCORE";

    List<int> scores = new List<int>();

    void Awake()
    {
        Load();
    }

    //スコア追加
    public void AddScore(int score)
    {
        scores.Add(score);

        //降順ソート
        scores.Sort((a, b) => b.CompareTo(a));

        //10位まで
        if (scores.Count > MAX_RANK)
        {
            scores.RemoveAt(scores.Count - 1);
        }

        Save();
    }

    //ランキング取得
    public List<int> GetScores()
    {
        return scores;
    }

    void Save()
    {
        string json = JsonUtility.ToJson(new ScoreWrapper(scores));
        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save();
    }

    void Load()
    {
        if (!PlayerPrefs.HasKey(SAVE_KEY))
            return;

        string json = PlayerPrefs.GetString(SAVE_KEY);
        scores = JsonUtility.FromJson<ScoreWrapper>(json).scores;
    }

    [System.Serializable]
    class ScoreWrapper
    {
        public List<int> scores;
        public ScoreWrapper(List<int> scores)
        {
            this.scores = scores;
        }
    }
}
