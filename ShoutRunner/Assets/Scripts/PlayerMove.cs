using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    BoxCollider boxCol;
    Rigidbody rb;
    Transform tr;
    Animator anim;
    VoiceToText voiceToText;
    VoiceToText.VoiceCommand command;
    [SerializeField, Header("移動速度")]
    float speed = 3f;
    [SerializeField, Header("跳躍力")]
    Vector3 jump_vec = new Vector3(0, 10, 0);
    [SerializeField, Header("波エフェクト")]
    ParticleSystem waveEffect;
    CharacterVoiceSettiing characterVoiceSettiing;

    float speedDecay = 6f;
    float time = 0;
    float crawlTime = 2.5f;
    float RotateSpeed = 170f;
    float centerX;
    bool isJump = false;
    bool isCurve = false;
    bool foward = false;
    bool waveEffectPlayed = false;
    public static bool ismove = true;
    public static bool isUlt = false;
    string recognizedText;
    Vector3 pos;
    Vector3 forward_rot;
    Vector3 left_rot;
    Vector3 right_rot;
    Vector3 colliderSize;
    Vector3 colliderPos;
    Rean rean = Rean.Center;

    // Start is called before the first frame update
    void Start()
    {
        boxCol=GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        voiceToText = GetComponent<VoiceToText>();
        characterVoiceSettiing = GetComponent<CharacterVoiceSettiing>();
        recognizedText = voiceToText.GetSetRecognizedText;
        command = VoiceToText.VoiceCommand.Null;
        
        colliderSize = boxCol.size;
        colliderPos = boxCol.center;
        pos = tr.position;
        centerX = pos.x;
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
        //時間経過による速度変化
        speed += 0.04f * Time.deltaTime;

        //if(foward==true&&!isCurve)
        //{
        //        rb.velocity = Vector3.forward * speed;
        //}

        //移動速度による回転速度の調整
        RotateSpeed = speed * 40f;

        //音声認識なしデバッグ用
        /*    if (Input.GetKeyDown(KeyCode.Space))
            {
                voiceToText.GetSetRecognizedText = VoiceToText.VoiceCommand.すすめ.ToString();
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
            }*/

        // 音声認識で取得したテキストが変化してたら処理
        if (recognizedText != voiceToText.GetSetRecognizedText&& command == VoiceToText.VoiceCommand.Null)
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
                boxCol.size=colliderSize;
                boxCol.center=colliderPos;
            }

        }

        Debug.Log(command);

        //1レーン分移動したら直進に戻す
        if (command == VoiceToText.VoiceCommand.ひだり)
        {
            //左端
            if (rean == Rean.Left)
            {
                isCurve = false;
                rb.velocity = Vector3.forward * speed;
                tr.position = new Vector3(centerX - 3, tr.position.y, tr.position.z);
                pos = tr.position;
                rb.velocity = Vector3.forward * speed;
                command = VoiceToText.VoiceCommand.Null;
                Debug.Log(command);
            }
            //中央
            else if (rean == Rean.Center)
            {
                isCurve = true;
                if (tr.position.x <= pos.x - 3f && foward)
                {
                    isCurve = false;
                    rean = Rean.Left;
                    rb.velocity = Vector3.forward * speed;
                tr.position = new Vector3(centerX - 3, tr.position.y, tr.position.z);
                    tr.rotation = Quaternion.Euler(forward_rot);
                    pos = tr.position;
                    rb.velocity = Vector3.forward * speed;
                    command = VoiceToText.VoiceCommand.Null;
                    foward = false;
                    time = 0;
                    Debug.Log(command);
                }
            }
            //右端
            else if (rean == Rean.Right)
            {
                Debug.Log("tr.position.x:" + tr.position.x+ "\npos.x:"+pos.x );
                Debug.Log(tr.position.x <= pos.x - 3);
                isCurve = true;

                if (tr.position.x <= pos.x - 3f && foward)
                {
                    isCurve = false;
                    rean = Rean.Center;
                    rb.velocity = Vector3.forward * speed;
                tr.position = new Vector3(centerX , tr.position.y, tr.position.z);
                    tr.rotation = Quaternion.Euler(forward_rot);
                    pos = tr.position;
                    rb.velocity = Vector3.forward * speed;
                    command = VoiceToText.VoiceCommand.Null;
                    foward = false;
                    time = 0;
                    Debug.Log(command);
                }

            }
        }
        else if (command == VoiceToText.VoiceCommand.みぎ)
        {
            //左端
            if (rean == Rean.Left)
            {

                isCurve = true;
                if (tr.position.x >= pos.x + 3f && foward)
                {
                    isCurve = false;
                    rean = Rean.Center;
                    rb.velocity = Vector3.forward * speed;
                tr.position = new Vector3(centerX, tr.position.y, tr.position.z);
                    tr.rotation = Quaternion.Euler(forward_rot);
                    pos = tr.position;
                    rb.velocity = Vector3.forward * speed;
                    command = VoiceToText.VoiceCommand.Null;
                    foward = false;
                    time = 0;
                    Debug.Log(command);
                }
            }
            //中央
            else if (rean == Rean.Center)
            {

                isCurve = true;
                if (tr.position.x >= pos.x + 3f&& foward)
                {
                    isCurve = false;
                    Debug.Log(isCurve);

                    rean = Rean.Right;
                    rb.velocity = Vector3.forward * speed;
                tr.position = new Vector3(centerX + 3, tr.position.y, tr.position.z);
                    tr.rotation = Quaternion.Euler(forward_rot);
                    pos = tr.position;
                    rb.velocity = Vector3.forward * speed;
                    command = VoiceToText.VoiceCommand.Null;
                    foward = false;
                    time = 0;
                    Debug.Log(command);
                }
            }
            //右端
            else if (rean == Rean.Right)
            {
                    isCurve = false;
                rb.velocity = Vector3.forward * speed;
                tr.position = new Vector3(centerX + 3, tr.position.y, tr.position.z);
                pos = tr.position;
                rb.velocity = Vector3.forward * speed;
                command = VoiceToText.VoiceCommand.Null;

            }
       }

        //叫び
        if (anim.GetBool(animationState.Scream.ToString()))
        {
            ScreamMove();
        }

        //ジャンプ
        if (isJump)
        {
            JumpMove();
        }

        //回転
        if (isCurve)
        {
            RotateToAngle(command);
        }
    }




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

        //方向転換など
        switch (text)
        {
            //左
            case nameof(VoiceToText.VoiceCommand.ひだり):
                command = VoiceToText.VoiceCommand.ひだり;
                rb.velocity = new Vector3(-1, 0, 1) * speed;
                break;
            //右
            case nameof(VoiceToText.VoiceCommand.みぎ):
                command = VoiceToText.VoiceCommand.みぎ;
                rb.velocity = new Vector3(1, 0, 1) * speed;
                break;
            //叫び
            case nameof(VoiceToText.VoiceCommand.なんでやねん):
                if (UltGauge.ultGauge < 1.0f)
                    return;
                command = VoiceToText.VoiceCommand.なんでやねん;
                rb.velocity = Vector3.zero;
                anim.SetBool(animationState.Scream.ToString(), true);
                break;
            //伏せ
            case nameof(VoiceToText.VoiceCommand.伏せ):
                command = VoiceToText.VoiceCommand.伏せ;
                rb.velocity = Vector3.forward * speed * 0.5f;
                boxCol.size = new Vector3(colliderSize.x, colliderSize.y / 3, colliderSize.z);
                boxCol.center = new Vector3(colliderPos.x, 0, colliderPos.z);
                anim.SetBool(animationState.Crawl.ToString(), true);
                break;
                //ジャンプ
            case nameof(VoiceToText.VoiceCommand.ジャンプ) or nameof(VoiceToText.VoiceCommand.とべ):
                command = VoiceToText.VoiceCommand.ジャンプ;
              if(!isJump)
                {
                    isJump = true;
                    rb.velocity += jump_vec;//上昇エネルギー付与
                }
                break;
                case nameof(VoiceToText.VoiceCommand.すすめ):
                command = VoiceToText.VoiceCommand.すすめ;
                rb.velocity = Vector3.forward * speed;
                break;
        }
    }

    void ScreamMove()
    {
        time += Time.deltaTime;
        if (time >= 1f && time < 2f)
        {
            if (!waveEffectPlayed)
            {
                characterVoiceSettiing.SetVoiceNumber= 12; 
                waveEffectPlayed = true;
                waveEffect.transform.position = new Vector3(tr.position.x, tr.position.y + 1.2f, tr.position.z);
                waveEffect.transform.rotation = Quaternion.Euler(new Vector3(-92, 0, 0));
                Instantiate(waveEffect, waveEffect.transform.position, waveEffect.transform.rotation);
                isUlt = true;
            }
        }
        else if (time >= 2f)
        {
            rb.velocity = Vector3.forward * speed;
            anim.SetBool(animationState.Scream.ToString(), false);
            waveEffectPlayed = false;
            time = 0;
            command = VoiceToText.VoiceCommand.Null;

        }

    }

    void JumpMove()
    {
                //ジャンプ中の処理
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

    enum animationState
    {
        Run,
        Jump,
        Crawl,
        Scream,
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
