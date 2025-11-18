using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

//音声認識によって入力単語をテキスト化するスクリプト
public class VoiceToText : MonoBehaviour
{

    KeywordRecognizer keywordRecognizer;
    DictationRecognizer dictationRecognizer;

    string recognizedText = "";

    //認識判定したい単語
    private string[] keywords = new string[]
    { VoiceCommand.ジャンプ.ToString(),VoiceCommand.伏せ.ToString(),VoiceCommand.なんでやねん.ToString(),
      VoiceCommand.ひだり.ToString(),VoiceCommand.みぎ.ToString()
    };
    void Start()
    {
        dictationRecognizer=new DictationRecognizer();
        dictationRecognizer.DictationResult += DictationRecResult;
        dictationRecognizer.DictationError += DictationRecError;
        // キーワード認識の初期化
        keywordRecognizer = new KeywordRecognizer(keywords);
        //イベントに登録
        keywordRecognizer.OnPhraseRecognized += OnPhraseRecognized;
       // keywordRecognizer.Start();

        Debug.Log("keyword音声認識開始");
        dictationRecognizer.Start();
        Debug.Log("dictation音声認識開始");
    }

    void Update()
    {

    }
    //音声入力があったと判定されたときに呼ばれる　　　　　　　認識された音声データ
    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
       // Debug.Log($"認識された言葉: {args.text}");
       // Debug.Log($"信頼度: {args.confidence}");

        recognizedText = args.text;

        // 認識された言葉に応じて処理
        //動きはPlayerMove.csで実装
        /*switch (args.text)
        {
            case nameof(VoiceCommand.ジャンプ) or nameof(VoiceCommand.とべ):
                Debug.Log("ジャンプ");
                break;
            case nameof(VoiceCommand.なんでやねん):
                Debug.Log("進め");
                break;
            case nameof(VoiceCommand.ひだり):
                Debug.Log("ひだり");
                break;
            case nameof(VoiceCommand.みぎ):
                Debug.Log("みぎ");
                break;
            case nameof(VoiceCommand.伏せ):
                Debug.Log("伏せ");
                break;
        }*/
    }
    private void DictationRecResult(string text, ConfidenceLevel confidence)
    {
        Debug.Log($"認識した音声： {text}");
    }
    public string GetSetRecognizedText
    {
        get { return recognizedText; }
        set { recognizedText = value; }
    }
    //何かしらのエラーが起きた時に発生するイベント
    private void DictationRecError(string error, int hresult)
    {
        Debug.Log($"エラー：{error}, {hresult}");
    }
    void OnDestroy()
    {
        // クリーンアップ
        if (keywordRecognizer != null && keywordRecognizer.IsRunning)
        {
            keywordRecognizer.Stop();
        }
        keywordRecognizer?.Dispose();

        dictationRecognizer.Stop();
        dictationRecognizer.Dispose();
    }

    public enum VoiceCommand
    {
        ジャンプ,
        とべ,
        なんでやねん,
        ひだり,
        みぎ,
        伏せ,
        Null,
    }


}