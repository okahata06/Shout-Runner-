using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//OBJÇ…ìñÇΩÇ¡ÇΩéûÇÃëÃóÕä«óù
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

    // Start is called before the first frame update
    void Start()
    {
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
            currentHealth--;
            anim.SetInteger("HP", currentHealth);
            Debug.Log("Player HP: " + currentHealth);
            // éÄñSèàóù
            if (currentHealth <= 0)
            {
               // Debug.Log("Player Dead");
                boxCol.enabled = false; // ó·: è’ìÀîªíËÇñ≥å¯âª
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
