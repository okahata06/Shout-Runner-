using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVoiceSettiing : MonoBehaviour
{
    [SerializeField]
    AudioSource voiceSource;

    [SerializeField]
    AudioClip[] voiceClip=new AudioClip[15];

    int voiceNumber;


    // Start is called before the first frame update
    void Start()
    {
        voiceSource=GetComponent<AudioSource>();

        voiceSource.clip = voiceClip[9];//始まりのボス音声
        voiceSource.Play();

        voiceNumber = -1;
        PlayerMove.ismove = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!voiceSource.isPlaying)
        {
            PlayerMove.ismove = true;
        }
        if(voiceNumber!=-1&&PlayerMove.ismove)
        {
            voiceSource.clip=voiceClip[voiceNumber];
            voiceSource.Play();
            voiceNumber=-1;
        }
    }

    public int SetVoiceNumber
    {
        set { voiceNumber = value; }
    }

    //成功失敗から番号をとり、対応する音声を再生
}
