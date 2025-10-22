using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceToText : MonoBehaviour
{

    KeywordRecognizer keywordRecognizer;

    string recognizedText = "";

    //認識判定したい単語
    private string[] keywords = new string[] { "ジャンプ", "進め", "止まれ", "攻撃" , "なんでやねん" };
    void Start()
    {
        // キーワード認識の初期化
        keywordRecognizer = new KeywordRecognizer(keywords);
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
        switch (args.text)
        {
            case "ジャンプ":
                Jump();
                break;
            case "進め":
                MoveForward();
                break;
            case "止まれ":
                Stop();
                break;
            case "攻撃":
                Attack();
                break;
        }
    }

    public string GetRecognizedText
    {
        get { return recognizedText; }
    }  

    void Jump()
    {
        Debug.Log("ジャンプ実行！");
        // ジャンプ処理
    }

    void MoveForward()
    {
        Debug.Log("前進！");
        // 移動処理
    }

    void Stop()
    {
        Debug.Log("停止！");
        // 停止処理
    }

    void Attack()
    {
        Debug.Log("攻撃！");
        // 攻撃処理
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