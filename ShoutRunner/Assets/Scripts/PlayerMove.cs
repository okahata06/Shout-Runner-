using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Rigidbody rb;
    string recognizedText;
    VoiceToText voiceToText;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        voiceToText = GetComponent<VoiceToText>();
        recognizedText = voiceToText.GetSetRecognizedText;

    }

    // Update is called once per frame
    void Update()
    {
        // 音声認識で取得したテキストが変化してたら処理
        if (recognizedText != voiceToText.GetSetRecognizedText)
        {
            MoveByText(recognizedText);
            recognizedText = voiceToText.GetSetRecognizedText;
            voiceToText.GetSetRecognizedText = "";
            Debug.Log("プレイヤーで取得：" + recognizedText);
            Debug.Log("voiceToTextで取得：" + voiceToText.GetSetRecognizedText);


        }

    }

    void MoveByText(string text)
    {
        switch (text)
        {
            case "ひだり":
                rb.velocity = Vector3.left * 5;
                break;
            case "みぎ":
                rb.velocity = Vector3.right * 5;
                break;
            case "なんでやねん":
                rb.velocity = Vector3.forward * 5;
                break;

        }


    }

}
