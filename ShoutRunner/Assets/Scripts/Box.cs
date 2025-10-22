using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public VoiceSetting voice_setting;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (voice_setting.GetVoiceVolume >= 0.9f)
        {
            Destroy(gameObject);
        }
    }
}
