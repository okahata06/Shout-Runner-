using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//OBJに当たった時の体力管理
public class PlayerHealth : MonoBehaviour
{
    [SerializeField, Header("HP1_Image")]
    Image Hp1;
    [SerializeField, Header("HP2_Image")]
    Image Hp2;
    [SerializeField, Header("HP3_Image")]
    Image Hp3;
    [SerializeField, Header("HPRest_Image")]
    Sprite HpRest;
    [SerializeField, Header("HPEmpty_Image")]
    Sprite HpEmpty;
    [SerializeField, Header("HP_Max")]
    int maxHP = 3;
    [SerializeField, Header("Result")]
    GameObject Result;
    public static int currentHealth;
    BoxCollider boxCol;
    Animator anim;
    [Header("HitSE")]
    public AudioClip hit;
    private AudioSource audiosourse;

    CharacterVoiceSettiing characterVoiceSetting;
    // Start is called before the first frame update
    void Start()
    {
        audiosourse = GetComponent<AudioSource>();
        characterVoiceSetting = GetComponent<CharacterVoiceSettiing>();
        Result.SetActive(false);
        currentHealth = maxHP;
        boxCol = GetComponent<BoxCollider>();
        anim = GetComponent<Animator>();
        anim.SetInteger("HP", maxHP);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth == 3)
        {
            Hp1.sprite = HpRest;
            Hp2.sprite = HpRest;
            Hp3.sprite = HpRest;
        }
        else if(currentHealth == 2)
        {
            Hp1.sprite = HpRest;
            Hp2.sprite = HpRest;
            Hp3.sprite = HpEmpty;
        }
        else if (currentHealth == 1)
        {
            Hp1.sprite = HpRest;
            Hp2.sprite = HpEmpty;
            Hp3.sprite = HpEmpty;
        }
        else if (currentHealth == 0)
        {
            Hp1.sprite = HpEmpty;
            Hp2.sprite = HpEmpty;
            Hp3.sprite = HpEmpty;

            if (Result.activeSelf == false)
                Result.SetActive(true);
        }
    }
    
    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag(TrapType.Box.ToString()) ||
            col.gameObject.CompareTag(TrapType.Trap.ToString()))
        {
            audiosourse.PlayOneShot(hit);
            currentHealth--;
            characterVoiceSetting.SetVoiceNumber = 11; // ダメージボイス
            anim.SetInteger("HP", currentHealth);
            Debug.Log("Player HP: " + currentHealth);
            // 死亡処理
            if (currentHealth <= 0)
            {
                // Debug.Log("Player Dead");
                characterVoiceSetting.SetVoiceNumber = 10; // 死亡ボイス
                boxCol.enabled = false; // 例: 衝突判定を無効化
                PlayerMove.ismove = false;
                anim.applyRootMotion= true;
            }
        }
    }

    enum TrapType
    {
        Spike,
        Box,
        Lazer,
        Trap
    }
}
