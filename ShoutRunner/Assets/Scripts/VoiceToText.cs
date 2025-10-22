using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

//音声認識によって入力単語をテキスト化するスクリプト
public class VoiceToText : MonoBehaviour
{

    KeywordRecognizer keywordRecognizer;

    string recognizedText = "";

    //認識判定したい単語
    private string[] keywords = new string[]
    { "ジャンプ", "進め", "止まれ", "攻撃" , "なんでやねん" ,
      "ひだり", "みぎ", "伏せ"
    };
    void Start()
    {
        // キーワード認識の初期化
        keywordRecognizer = new KeywordRecognizer(keywords);
        //イベントに登録
        keywordRecognizer.OnPhraseRecognized += OnPhraseRecognized;
        keywordRecognizer.Start();

        Debug.Log("音声認識開始");
    }

    //音声入力があったと判定されたときに呼ばれる　　　　　　　認識された音声データ
    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        Debug.Log($"認識された言葉: {args.text}");
        Debug.Log($"信頼度: {args.confidence}");

        recognizedText = args.text;

        // 認識された言葉に応じて処理
        //動きはPlayerMove.csで実装
        switch (args.text)
        {
            case "ジャンプ":
                Debug.Log("ジャンプ");
                break;
            case "進め":
                Debug.Log("進め");
                break;
            case "止まれ":
                Debug.Log("止まれ");
                break;
            case "攻撃":
                Debug.Log("攻撃");
                break;
            case "なんでやねん":
                Debug.Log("なんでやねん");
                break;
            case "ひだり":
                Debug.Log("ひだり");
                break;
            case "みぎ":
                Debug.Log("みぎ");
                break;
            case "伏せ":
                Debug.Log("伏せ");
                break;
        }
    }

    public string GetSetRecognizedText
    {
        get { return recognizedText; }
        set { recognizedText = value; }
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
}