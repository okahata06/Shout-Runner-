using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVoiceSettiing : MonoBehaviour
{
    [SerializeField]
    AudioSource voiceSource;

    [SerializeField]
    AudioClip[] voiceClip=new AudioClip[10];

    int voiceNumber;
    VoiceToText voiceToText;


    // Start is called before the first frame update
    void Start()
    {
        voiceSource=GetComponent<AudioSource>();
        voiceToText=GetComponent<VoiceToText>();

            voiceNumber=-1;
    }

    // Update is called once per frame
    void Update()
    {
        if(voiceNumber!=-1)
        {
            voiceSource.clip=voiceClip[voiceNumber];
            voiceSource.Play();
            voiceNumber=-1;
            Debug.Log(voiceClip[0]);
        }
    }

    public int SetVoiceNumber
    {
        set { voiceNumber = value; }
    }

    //¬Œ÷¸”s‚©‚ç”Ô†‚ğ‚Æ‚èA‘Î‰‚·‚é‰¹º‚ğÄ¶
}
