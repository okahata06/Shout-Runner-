using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;
using UnityEngine.SceneManagement;
using static VoiceToText;

public class Result : MonoBehaviour
{
    float total = 0;
    private bool _sceneChange = true;//コルーチンを1度だけ処理する用
    private bool _returnTitle;

    [SerializeField] GameObject IrisPanel;
    [SerializeField] RectTransform unmask;

    readonly Vector2 IRIS_MID_SCALE1 = new Vector2(1.0f, 1.0f);
    readonly Vector2 IRIS_MID_SCALE2 = new Vector2(3.0f, 3.0f);

    [Header("スコアのテキスト")]
    public Text ScoreText;

    [Header("ボリュームボーナスのテキスト")]
    public Text VolumeText;

    [Header("トータルのテキスト")]
    public Text TotalText;

    [Header("ランキング")]
    public GameObject Ranking;

    RankingManager ranking;

    KeywordRecognizer keywordRecognizer;

    string recognizedText = "";

    //認識判定したい単語
    private string[] keywords = new string[]
    { VoiceCommand.タイトル.ToString()
    };

    private void Awake()
    {
        ranking = FindFirstObjectByType<RankingManager>();

        //IrisPanel.SetActive(false);
        //Ranking.SetActive(false);
    }

    private void Start()
    {
        IrisPanel.SetActive(false);
        Ranking.SetActive(false);
    }

    private void OnEnable()
    {
        // キーワード認識の初期化
        keywordRecognizer = new KeywordRecognizer(keywords);
        //イベントに登録
        keywordRecognizer.OnPhraseRecognized += OnPhraseRecognized;
        keywordRecognizer.Start();

        StartCoroutine(ResultText());

        _returnTitle = false;
    }

    void OnDestroy()
    {
        // クリーンアップ
        if (keywordRecognizer != null && keywordRecognizer.IsRunning)
        {
            keywordRecognizer.Stop();
        }
        keywordRecognizer?.Dispose();

    }

    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        // Debug.Log($"認識された言葉: {args.text}");
        // Debug.Log($"信頼度: {args.confidence}");

        // 認識された言葉に応じて処理
        if (args.text == nameof(VoiceCommand.タイトル) && _sceneChange && _returnTitle)
        {
            Debug.Log("スタート");
            StartCoroutine(MainTitle());
            _sceneChange = false;
        }

    }

    public void IrisOut()
    {
        unmask.DOScale(IRIS_MID_SCALE1, 0.2f).SetEase(Ease.InCubic);
        unmask.DOScale(IRIS_MID_SCALE2, 0.2f).SetDelay(0.2f).SetEase(Ease.OutCubic);
        unmask.DOScale(new Vector2(0, 0), 0.4f).SetDelay(0.4f).SetEase(Ease.InCubic);
    }

    enum VoiceCommand
    {
        タイトル,
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

        yield return new WaitForSeconds(3);

        _returnTitle = true;
    }

    private IEnumerator MainTitle()
    {
        IrisPanel.SetActive(true);
        IrisOut();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Title");
    }
}
