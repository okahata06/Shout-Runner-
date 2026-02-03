using System;
using System.Linq;
using UnityEngine;


//音量だけほしい
public class VoiceSetting : MonoBehaviour
{
    //使用するマイク
    //[SerializeField] private string m_DeviceName;

    private AudioClip m_AudioClip;
    private int m_LastAudioPos;
    private float m_AudioLevel;

    void Start()
    {
        //nullまたは""でデフォルトマイクに設定
        string targetDevice = "";
        //使用できるマイクの探索
        /*foreach (var device in Microphone.devices)
        {
            if (device.Contains(m_DeviceName))
            {            //使用するマイクを適用

                targetDevice = device;
            }
        }*/

        m_AudioClip = Microphone.Start(targetDevice, true, 10, 48000);
    }

    void Update()
    {
        if (!PlayerMove.ismove)
            return;
        float[] waveData = GetUpdatedAudio();
        if (waveData.Length == 0) return;
        //ボリュームデータ代入0～1
        m_AudioLevel = waveData.Average(Mathf.Abs);

        //Debug.Log(m_AudioLevel);

    }

    private float[] GetUpdatedAudio()
    {
        int nowAudioPos = Microphone.GetPosition(null);
        float[] waveData = Array.Empty<float>();

        if (m_LastAudioPos < nowAudioPos)
        {
            int audioCount = nowAudioPos - m_LastAudioPos;
            waveData = new float[audioCount];
            m_AudioClip.GetData(waveData, m_LastAudioPos);
        }
        else if (m_LastAudioPos > nowAudioPos)
        {
            int audioBuffer = m_AudioClip.samples * m_AudioClip.channels;
            int audioCount = audioBuffer - m_LastAudioPos;
            float[] wave1 = new float[audioCount];
            m_AudioClip.GetData(wave1, m_LastAudioPos);
            float[] wave2 = new float[nowAudioPos];
            if (nowAudioPos != 0)
            {
                m_AudioClip.GetData(wave2, 0);
            }
            waveData = new float[audioCount + nowAudioPos];
            wave1.CopyTo(waveData, 0);
            wave2.CopyTo(waveData, audioCount);
        }

        m_LastAudioPos = nowAudioPos;
        return waveData;
    }
    //音声ボリュームの取得

    public float GetVoiceVolume
    {
        get { return m_AudioLevel; }
    }
}