using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

//音声認識によって入力単語をテキスト化するスクリプト
public class VoiceToText : MonoBehaviour
{
    [SerializeField, Header("音声認識結果表示用テキスト")]
    Text text;
    [SerializeField, Header("キャラ反応表示用OBJ")]
    GameObject characterReactionOBJ;

    //  [SerializeField, Header("最低取得音量")]
    //float minVoiceVolume = 0.01f;
    Text characterReactionText;

    VoiceSetting voiceSetting;
    CharacterVoiceSettiing characterVoiceSettiing;

    public static float maxVolume = 0;
    float TextDisplayTime = 1.3f;
    float textdisplayTimeCount = 0f;
    float TimeCount = 2f;
    float noSpeakTime = 3.5f;
    bool TextDisplaying = false;
    bool isSpeaking = false;

    //リアクション
    string[] reaction_Success = new string[]{
        ReactionSuccess.オーケー.ToString()+"！",ReactionSuccess.かしこまり.ToString()+"！",ReactionSuccess.了解.ToString()+"！",

    };
    string[] reaction_Failure = new string[]{
        ReactionFailure.え.ToString()+"？",ReactionFailure.なんて.ToString()+"？",ReactionFailure.もっかい言って.ToString()+"!",
        ReactionFailure.聞き取られへんて.ToString()+"！",ReactionFailure.ちゃんと喋れ.ToString()+"！"
    };

    KeywordRecognizer keywordRecognizer;
    //DictationRecognizer dictationRecognizer;

    string recognizedText = "";

    //認識判定したい単語
    private string[] keywords = new string[]
    { VoiceCommand.ジャンプ.ToString(),VoiceCommand.伏せ.ToString(),VoiceCommand.なんでやねん.ToString(),
      VoiceCommand.ひだり.ToString(),VoiceCommand.みぎ.ToString()
    };
    void Start()
    {
        voiceSetting = GetComponent<VoiceSetting>();
        characterVoiceSettiing = GetComponent<CharacterVoiceSettiing>();

        characterReactionText = characterReactionOBJ.GetComponentInChildren<Text>();
        text.text = "音声認識待機中";

        characterReactionOBJ.SetActive(false);
        //dictationRecognizer=new DictationRecognizer();
        //dictationRecognizer.DictationResult += DictationRecResult;
        // dictationRecognizer.DictationError += DictationRecError;
        //dictationRecognizer.DictationComplete += DictationRecComplete;
   
        // キーワード認識の初期化
        keywordRecognizer = new KeywordRecognizer(keywords);
        //イベントに登録
        keywordRecognizer.OnPhraseRecognized += OnPhraseRecognized;
        keywordRecognizer.Start();

        Debug.Log("keyword音声認識開始");

        //dictationRecognizer.Start();
        //Debug.Log("dictation音声認識開始");


    }

    void Update()
    {
        //Debug.Log("音声入力音量:" + voiceSetting.GetVoiceVolume);

        if (maxVolume < voiceSetting.GetVoiceVolume)
            maxVolume = voiceSetting.GetVoiceVolume;

        if (TextDisplaying==true)
        {    
            textdisplayTimeCount += Time.deltaTime;

            if (textdisplayTimeCount >= TextDisplayTime)
            {
                characterReactionOBJ.SetActive(false);
                TextDisplaying = false;
                textdisplayTimeCount = 0f;
            }
        }

        TimeCount += Time.deltaTime;
        //失敗リアクションを表示
        if ( TimeCount >= noSpeakTime                 //時間経過
            ) 
        {
            SuccessOrFilure(false);

            //Debug.Log("認識失敗リアクション表示");
            TimeCount = 0f;
        }
    }

    //音声入力があったと判定されたときに呼ばれる　　　　　　　認識された音声データ
    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        // Debug.Log($"認識された言葉: {args.text}");
        // Debug.Log($"信頼度: {args.confidence}");

        recognizedText = text.text = args.text;

        //if (minVoiceVolume < voiceSetting.GetVoiceVolume)
        //{
        //    isSpeaking = true;
        //}

        SuccessOrFilure(true); //認識成功リアクション表示

        TimeCount = 0f; //認識されたのでタイムカウントリセット

        // 認識された言葉に応じて処理
        //動きはPlayerMove.csで実装
        switch (args.text)
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
        }
    }

    //命令成功失敗リアクション表示
    void SuccessOrFilure(bool isSuccess)
    {
        characterReactionOBJ.SetActive(true);
        TextDisplaying = true;
        if (isSuccess)
        {
            int index = Random.Range(0, reaction_Success.Length);
            characterReactionText.text = reaction_Success[index];
            //Debug.Log("認識成功リアクション表示");
            characterVoiceSettiing.SetVoiceNumber = index;
            isSpeaking = false;
        }
        else
        {
            int index = Random.Range(0, reaction_Failure.Length);
            characterReactionText.text = reaction_Failure[index];
            //Debug.Log("認識失敗リアクション表示");
            characterVoiceSettiing.SetVoiceNumber = index + 4;
            isSpeaking = false;
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

    public enum ReactionSuccess
    {
        オーケー,
        了解です,
        かしこまり,
        了解,
    }
    public enum ReactionFailure
    {
        え,
        なんて,
        もっかい言って,
        聞き取られへんて,
        ちゃんと喋れ,
    }


}