using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Rigidbody rb;
    Transform tr;
    Animator anim;
    [SerializeField, Header("移動速度")]
    float speed = 3f;
    [SerializeField, Header("跳躍力")]
    Vector3 jump_vec = new Vector3(0, 10, 0);
    float speedDecay = 6f;
    bool isJump = false;
    bool isCurve = false;

    bool foward = false;

    float time = 0;
    float crawlTime = 2.5f;
    Vector3 pos;
    Vector3 forward_rot;
    Vector3 left_rot;
    Vector3 right_rot;
    string recognizedText;
    float RotateSpeed = 170f;
    Rean rean = Rean.Center;
    VoiceToText voiceToText;
    VoiceToText.VoiceCommand command;

    public static bool ismove=true;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        voiceToText = GetComponent<VoiceToText>();
        recognizedText = voiceToText.GetSetRecognizedText;
        command = VoiceToText.VoiceCommand.Null;

        pos = tr.position;
        forward_rot = tr.eulerAngles;
        left_rot = new Vector3(forward_rot.x, forward_rot.y - 45, forward_rot.z);
        right_rot = new Vector3(forward_rot.x, forward_rot.y + 45, forward_rot.z);


        
    }

    // Update is called once per frame
    void Update()
    {
        if (!ismove)
        {
            rb.velocity = Vector3.zero;
            return;
        }
        //速度による回転速度の調整
        RotateSpeed = speed * 40f;

        //音声認識なしデバッグ用
        if (Input.GetKeyDown(KeyCode.Space))
        {
            voiceToText.GetSetRecognizedText = VoiceToText.VoiceCommand.なんでやねん.ToString();
            MoveByText(command.ToString());
            Debug.Log(command.ToString());
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            voiceToText.GetSetRecognizedText = VoiceToText.VoiceCommand.ひだり.ToString();
            MoveByText(command.ToString());
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            voiceToText.GetSetRecognizedText = VoiceToText.VoiceCommand.みぎ.ToString();
            MoveByText(command.ToString());
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            voiceToText.GetSetRecognizedText = VoiceToText.VoiceCommand.ジャンプ.ToString();
            MoveByText(command.ToString());
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            voiceToText.GetSetRecognizedText = VoiceToText.VoiceCommand.伏せ.ToString();
            MoveByText(command.ToString());
        }

        // 音声認識で取得したテキストが変化してたら処理
        if (recognizedText != voiceToText.GetSetRecognizedText)
        {
            MoveByText(recognizedText);
            recognizedText = voiceToText.GetSetRecognizedText;
            voiceToText.GetSetRecognizedText = "";
            //Debug.Log("プレイヤーで取得：" + recognizedText);
            //Debug.Log("voiceToTextで取得：" + voiceToText.GetSetRecognizedText);


        }


        //伏せ処理
        if (anim.GetBool(animationState.Crawl.ToString()))
        {
            time += Time.deltaTime;
            if (time >= crawlTime)
            {
                anim.SetBool(animationState.Crawl.ToString(), false);
                time = 0;
                command = VoiceToText.VoiceCommand.Null;
                rb.velocity = Vector3.forward * speed;
            }

        }


        //ジャンプ処理
        if (recognizedText == VoiceToText.VoiceCommand.ジャンプ.ToString() && !isJump)
        {
            isJump = true;

            rb.velocity += jump_vec;
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
        //Debug.Log("velocityddd"+rb.velocity);
        //Debug.Log("pos"+tr.position);

        //Debug.Log("command:" + command);



        //1レーン分移動したら直進に戻す
        if (command == VoiceToText.VoiceCommand.ひだり)
        {
            //左端
            if (rean == Rean.Left)
            {
                rb.velocity = Vector3.forward * speed;
                command = VoiceToText.VoiceCommand.Null;
            }
            //中央
            else if (rean == Rean.Center)
            {
                isCurve = true;
                if (tr.position.x <= pos.x - 3)
                {
                    rean = Rean.Left;
                    rb.velocity = Vector3.forward * speed;
                    tr.rotation = Quaternion.Euler(forward_rot);
                    command = VoiceToText.VoiceCommand.Null;
                    foward = false;
                    time = 0;
                }
            }
            //右端
            else if (rean == Rean.Right)
            {
                isCurve = true;
                if (tr.position.x <= pos.x - 3)
                {
                    rean = Rean.Center;
                    rb.velocity = Vector3.forward * speed;
                    tr.rotation = Quaternion.Euler(forward_rot);
                    command = VoiceToText.VoiceCommand.Null;
                    foward = false;
                    time = 0;
                }

            }



        }
        else if (command == VoiceToText.VoiceCommand.みぎ)
        {
            //左端
            if (rean == Rean.Left)
            {
                isCurve = true;
                if (tr.position.x >= pos.x + 3)
                {
                    rean = Rean.Center;
                    rb.velocity = Vector3.forward * speed;
                    tr.rotation = Quaternion.Euler(forward_rot);
                    command = VoiceToText.VoiceCommand.Null;
                    foward = false;
                    time = 0;
                }
            }
            //中央
            else if (rean == Rean.Center)
            {
                isCurve = true;
                if (tr.position.x >= pos.x + 3)
                {
                    rean = Rean.Right;
                    rb.velocity = Vector3.forward * speed;
                    tr.rotation = Quaternion.Euler(forward_rot);
                    command = VoiceToText.VoiceCommand.Null;
                    foward = false;
                    time = 0;
                }
            }
            //右端
            else if (rean == Rean.Right)
            {
                rb.velocity = Vector3.forward * speed;
                command = VoiceToText.VoiceCommand.Null;

            }
        }

        if (isCurve)
        {
            RotateToAngle(command);
        }
    }


    //曲がり角で方向転換を発言した場合、通路の中心まで来てから方向転換するようにする（曲がった最初は中心レーンから）
    //T字路を実装した場合、曲がるタイミングでposを更新する必要がある


    //現在のアングルから指定アングルまで徐々に回転させる
    void RotateToAngle(VoiceToText.VoiceCommand direction)
    {
        //左
        if (direction== VoiceToText.VoiceCommand.ひだり&&!foward)
        {

            tr.rotation = Quaternion.RotateTowards(tr.rotation, Quaternion.Euler(left_rot), RotateSpeed * Time.deltaTime);
        if(tr.rotation== Quaternion.Euler(left_rot))
            {
                foward = true;
            }
        }
        else if(foward==true)
        {
            time+= Time.deltaTime;
            if (time <= 2.0f/(speed*1.5f))
                return;
                tr.rotation = Quaternion.RotateTowards(tr.rotation, Quaternion.Euler(forward_rot), RotateSpeed * Time.deltaTime);
        
        if(tr.rotation== Quaternion.Euler(forward_rot))
            {
                isCurve = false;
            }
        }
        //右
        if (direction == VoiceToText.VoiceCommand.みぎ && !foward)
        {

            tr.rotation = Quaternion.RotateTowards(tr.rotation, Quaternion.Euler(right_rot), RotateSpeed * Time.deltaTime);
            if (tr.rotation == Quaternion.Euler(right_rot))
            {
                foward = true;
            }
        }
        else if (foward == true)
        {
            time += Time.deltaTime;
            if (time <= 1.5f / speed)
                return;
            tr.rotation = Quaternion.RotateTowards(tr.rotation, Quaternion.Euler(forward_rot), RotateSpeed * Time.deltaTime);

            if (tr.rotation == Quaternion.Euler(forward_rot))
            {
                isCurve = false;
            }
        }

    }


    //移動
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
                rb.velocity = new Vector3(-1, 0, 1) * speed;
                break;

            case nameof(VoiceToText.VoiceCommand.みぎ):
                command = VoiceToText.VoiceCommand.みぎ;
                rb.velocity = new Vector3(1, 0, 1) * speed;
                break;

            case nameof(VoiceToText.VoiceCommand.なんでやねん):
                //追加の処理がないため必要なし↓
                //command = VoiceToText.VoiceCommand.なんでやねん;
                rb.velocity = Vector3.forward * speed;
                break;

            case nameof(VoiceToText.VoiceCommand.伏せ):
                command = VoiceToText.VoiceCommand.伏せ;
                rb.velocity = Vector3.forward * speed * 0.5f;
                anim.SetBool(animationState.Crawl.ToString(), true);
                break;

            case nameof(VoiceToText.VoiceCommand.ジャンプ) or nameof(VoiceToText.VoiceCommand.とべ):
                command = VoiceToText.VoiceCommand.ジャンプ;
                break;

        }



    }



    void JumpMove()
    {


    }

    enum animationState
    {
        Run,
        Jump,
        Crawl,
        LeftCurve,
        RightCurve,
    }

    //レーン位置保存用
    enum Rean
    {
        Left,
        Center,
        Right,
    }


    //別スクでコマンドがNullかどうかで点灯するUIをつくる用
    public string GetCommandText
    {
        get { return command.ToString(); }
    }

}
