using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Rigidbody rb;
    Transform tr;
    [SerializeField,Header("移動速度")]
    float speed = 3f;
    [SerializeField,Header("跳躍力")]
    Vector3 jump_vec = new Vector3(0,10,0);
    float speedDecay=2f;
    bool isJump = false;

    float left_side;

    Vector3 pos;
    Vector3 forward_rot;
    Vector3 left_rot;
    Vector3 right_rot;
    string recognizedText;
    VoiceToText voiceToText;
    VoiceToText.VoiceCommand command;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        voiceToText = GetComponent<VoiceToText>();
        recognizedText = voiceToText.GetSetRecognizedText;
        
        

        pos= tr.position;
        left_side=pos.x - 3f;
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

        //ジャンプ処理
        if (recognizedText==VoiceToText.VoiceCommand.ジャンプ.ToString()&&!isJump)
        {
            isJump = true;
           // Debug.Log("isJump=true");

            rb.velocity+= jump_vec;
        }
        //ジャンプ中の処理
        else if (isJump)
        {
            rb.velocity -= new Vector3(0, speedDecay * Time.deltaTime, 0);
            if (tr.position.y < pos.y)
            {
                isJump = false;
                command = VoiceToText.VoiceCommand.Null;
               // Debug.Log("isJump=false");

                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                tr.position = new Vector3(tr.position.x, pos.y, tr.position.z);
            }
        }
        Debug.Log("velocityddd"+rb.velocity);
        Debug.Log("pos"+tr.position);





        //1レーン分移動したら直進に戻す
        if (command==VoiceToText.VoiceCommand.ひだり)
        {
            //左端
            if (pos.x <= left_side )
            {
                rb.velocity = Vector3.forward * speed;
                command = VoiceToText.VoiceCommand.Null;
            tr.position = new Vector3(pos.x - 3, tr.position.y, tr.position.z);
                tr.rotation = Quaternion.Euler(forward_rot);
            }
            //中央
            else if (pos.x>= left_side-0.1f&& pos.x <= left_side+0.1f)
            {
                rb.velocity = Vector3.forward * speed;
                transform.rotation = Quaternion.Euler(forward_rot);
                command = VoiceToText.VoiceCommand.Null;
            }
            
        }
        if (transform.position.x > pos.x + 3 && command == VoiceToText.VoiceCommand.みぎ)
        {
            rb.velocity = Vector3.forward * speed;
            transform.rotation = Quaternion.Euler(forward_rot);
            command = VoiceToText.VoiceCommand.Null;
        }

    }

    //曲がり角で方向転換を発言した場合、通路の中心まで来てから方向転換するようにする（曲がった最初は中心レーンから）
    //T字路を実装した場合、曲がるタイミングでposを更新する必要がある


    //移動、回転処理
    void MoveByText(string text)
    {
        //移動処理をしていない状態=Null
        if (command != VoiceToText.VoiceCommand.Null)
            return;

        pos = tr.position;
        //方向転換など
        switch (text)
        {
            case nameof(VoiceToText.VoiceCommand.ひだり):
                command = VoiceToText.VoiceCommand.ひだり;
                rb.velocity = new Vector3(-1,0,1) * speed;
                transform.rotation = Quaternion.Euler(left_rot);
                break;

            case nameof(VoiceToText.VoiceCommand.みぎ):
                command = VoiceToText.VoiceCommand.みぎ;
                rb.velocity = new Vector3(1, 0, 1) * speed;
                transform.rotation = Quaternion.Euler(right_rot);
                break;
            
            case nameof(VoiceToText.VoiceCommand.なんでやねん):
                //追加の処理がないため必要なし↓
                //command = VoiceToText.VoiceCommand.なんでやねん;
                rb.velocity = Vector3.forward * speed;
                transform.rotation = Quaternion.Euler(forward_rot);
                break;
            
            case nameof(VoiceToText.VoiceCommand.伏せ):
                command = VoiceToText.VoiceCommand.伏せ;
                rb.velocity = Vector3.forward * speed * 0.5f;
                transform.rotation = Quaternion.Euler(new Vector3(90,0,0));
                break;
            
            case nameof(VoiceToText.VoiceCommand.ジャンプ)or nameof(VoiceToText.VoiceCommand.とべ):
                command = VoiceToText.VoiceCommand.ジャンプ;
                break;

        }

        

    }

    void JumpMove()
    {
    
    
    }

    //別スクでコマンドがNullかどうかで点灯するUIをつくる用
    public string GetCommandText
    {
        get { return command.ToString(); }
    }

}
