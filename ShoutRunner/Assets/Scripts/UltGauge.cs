using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class UltGauge : MonoBehaviour
{
    [Header("ウルトゲージのイメージ")]
    [SerializeField] Image UltGaugeImage;

    private AudioSource audiosourse;
    [Header("SE")]
    public AudioClip gaugeMax;

    private bool seOnce = true;

    bool MatuoCommand = false; 
    bool AiruCommand = false;


    public static float ultGauge = 0;

    // Start is called before the first frame update
    void Start()
    {
        audiosourse = GetComponent<AudioSource>();

        UltGaugeImage.fillAmount = 0;
        UltGaugeImage.color = new Color(1f, 1f, 1f, 0.5f);

    }

    // Update is called once per frame
    void Update()
    {
        // まつおコマンドでゲージ満タン
        if (MatuoCommand && StageTips.gaugeChage) {
            UltGaugeImage.fillAmount = 1.0f;
            MatuoCommand = false;
        }

        if (UltGaugeImage.fillAmount <= 1.0 && StageTips.gaugeChage)
        {
            UltGaugeImage.fillAmount += Time.deltaTime / 30;
        }

        if(PlayerMove.isUlt)
        {
            UltGaugeImage.fillAmount = 0;
            PlayerMove.isUlt = false;
            seOnce = true;
        }

        ultGauge = UltGaugeImage.fillAmount;
      
        // ゲージが満タンになったら色を変える
        if (UltGaugeImage.fillAmount == 1f)
        {
            if (seOnce)
            {
                audiosourse.PlayOneShot(gaugeMax);
                seOnce = false;
            }

            UltGaugeImage.color = new Color(0.9f, 0.9f, 0.2f, 0.5f);
        }
        else
        {
            UltGaugeImage.color = new Color(1f, 1f, 1f, 0.5f);
        }


        //Debug.Log(ultGauge);
    }

    public bool SetMatuoCommand
    {
       set { MatuoCommand = value; }
    }

    /*private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        
        // 認識された言葉に応じて処理
        if (args.text == nameof(VoiceCommand.まつお) && UltGaugeImage.fillAmount < 1.0)
        {
            UltGaugeImage.fillAmount = 1.0f;
        }

    }
    */

}
