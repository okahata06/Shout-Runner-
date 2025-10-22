using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    string recognizedText;
    VoiceToText voiceToText;

    // Start is called before the first frame update
    void Start()
    {
        voiceToText = GetComponent<VoiceToText>();
        recognizedText = voiceToText.GetRecognizedText;

    }

    // Update is called once per frame
    void Update()
    {
        // 音声認識で取得したテキストが変化したら
        if (recognizedText != voiceToText.GetRecognizedText)
        {
            recognizedText = voiceToText.GetRecognizedText;
        }
        Debug.Log("プレイヤーで取得：" + recognizedText);
        Debug.Log("voiceToTextで取得：" + voiceToText.GetRecognizedText);


        //テキストをリセット
        recognizedText = " ";
    }

}
