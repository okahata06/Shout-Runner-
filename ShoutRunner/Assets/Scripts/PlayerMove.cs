using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Rigidbody rb;
    Transform tr;
    Vector3 forward_rot;
    Vector3 left_rot;
    Vector3 right_rot;
    string recognizedText;
    VoiceToText voiceToText;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        voiceToText = GetComponent<VoiceToText>();
        recognizedText = voiceToText.GetSetRecognizedText;

        forward_rot = tr.eulerAngles;
        left_rot = new Vector3(forward_rot.x, forward_rot.y - 45,forward_rot.z);
        right_rot = new Vector3(forward_rot.x, forward_rot.y + 45, forward_rot.z);

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

    //移動、回転処理
    void MoveByText(string text)
    {
        switch (text)
        {
            case "ひだり":
                rb.velocity = new Vector3(-1,0,1) * 5;
                transform.rotation = Quaternion.Euler(left_rot);
                break;
            case "みぎ":
                rb.velocity = new Vector3(1, 0, 1) * 5;
                transform.rotation = Quaternion.Euler(right_rot);
                break;
            case "なんでやねん":
                rb.velocity = Vector3.forward * 5;
                transform.rotation = Quaternion.Euler(forward_rot);
                break;

        }


    }

}
