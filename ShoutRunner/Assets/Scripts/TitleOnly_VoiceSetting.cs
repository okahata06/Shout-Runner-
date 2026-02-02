using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;
using UnityEngine.SceneManagement;

//音声認識によって入力単語をテキスト化するスクリプト
public class TitleOnly_VoiceSetting : MonoBehaviour
{

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
            ChangeScene();
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


    void ChangeScene()
    {
        SceneManager.LoadScene("Main");
    }

}