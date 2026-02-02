using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

//音声認識によって入力単語をテキスト化するスクリプト
public class TitleOnly_VoiceSetting : MonoBehaviour
{
    [SerializeField] GameObject IrisPanel;
    [SerializeField] RectTransform unmask;

    private bool _sceneChange = true;//コルーチンを1度だけ処理する用

    readonly Vector2 IRIS_MID_SCALE1 = new Vector2(1.0f, 1.0f);
    readonly Vector2 IRIS_MID_SCALE2 = new Vector2(3.0f, 3.0f);

    KeywordRecognizer keywordRecognizer;

    string recognizedText = "";

    //認識判定したい単語
    private string[] keywords = new string[]
    { VoiceCommand.スタート.ToString()
    };

    void Start()
    {
        // キーワード認識の初期化
        keywordRecognizer = new KeywordRecognizer(keywords);
        //イベントに登録
        keywordRecognizer.OnPhraseRecognized += OnPhraseRecognized;
        keywordRecognizer.Start();

        IrisPanel.SetActive(false);
        _sceneChange = true;
    }

    void Update()
    { 

    }

    //音声入力があったと判定されたときに呼ばれる　　　　　　　認識された音声データ
    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        // Debug.Log($"認識された言葉: {args.text}");
        // Debug.Log($"信頼度: {args.confidence}");

        // 認識された言葉に応じて処理
        if(args.text==nameof(VoiceCommand.スタート))
        {
            Debug.Log("スタート");
            StartCoroutine(TitleSelect());
        }

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

    enum VoiceCommand
    {
        スタート,
    }

    public void IrisOut()
    {
        unmask.DOScale(IRIS_MID_SCALE1, 0.2f).SetEase(Ease.InCubic);
        unmask.DOScale(IRIS_MID_SCALE2, 0.2f).SetDelay(0.2f).SetEase(Ease.OutCubic);
        unmask.DOScale(new Vector2(0, 0), 0.4f).SetDelay(0.4f).SetEase(Ease.InCubic);
    }

    //void ChangeScene()
    //{
    //    SceneManager.LoadScene("Main");
    //}

    private IEnumerator TitleSelect()
    {
        IrisPanel.SetActive(true);
        IrisOut();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Main");
    }

}