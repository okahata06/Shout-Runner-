using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class UltGauge : MonoBehaviour
{
    [Header("ウルトゲージのイメージ")]
    [SerializeField] Image UltGaugeImage;

    KeywordRecognizer keywordRecognizer;

    string recognizedText = "";

    //認識判定したい単語
    private string[] keywords = new string[]
    { VoiceCommand.まつお.ToString()
    };

    public static float ultGauge = 0;

    // Start is called before the first frame update
    void Start()
    {
        UltGaugeImage.fillAmount = 0;

        // キーワード認識の初期化
        keywordRecognizer = new KeywordRecognizer(keywords);
        //イベントに登録
        keywordRecognizer.OnPhraseRecognized += OnPhraseRecognized;
        keywordRecognizer.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (UltGaugeImage.fillAmount <= 1.0)
        {
            UltGaugeImage.fillAmount += Time.deltaTime / 30;
        }

        if(PlayerMove.isUlt)
        {
            UltGaugeImage.fillAmount = 0;
            PlayerMove.isUlt = false;
        }

        ultGauge = UltGaugeImage.fillAmount;

        //Debug.Log(ultGauge);
    }

    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        // Debug.Log($"認識された言葉: {args.text}");
        // Debug.Log($"信頼度: {args.confidence}");

        // 認識された言葉に応じて処理
        if (args.text == nameof(VoiceCommand.まつお) && UltGaugeImage.fillAmount < 1.0)
        {
            UltGaugeImage.fillAmount = 1.0f;
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
        まつお,
    }
}
