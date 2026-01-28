using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UltGauge : MonoBehaviour
{
    [Header("ウルトゲージのイメージ")]
    [SerializeField] Image UltGaugeImage;

    public static float ultGauge = 0;

    // Start is called before the first frame update
    void Start()
    {
        UltGaugeImage.fillAmount = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (UltGaugeImage.fillAmount <= 1.0)
        {
            UltGaugeImage.fillAmount += Time.deltaTime / 50;
        }

        if(PlayerMove.isUlt)
        {
            UltGaugeImage.fillAmount = 0;
            PlayerMove.isUlt = false;
        }

        ultGauge = UltGaugeImage.fillAmount;

        Debug.Log(ultGauge);
    }
}
