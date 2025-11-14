using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//OBJ‚É“–‚½‚Á‚½‚Ì‘Ì—ÍŠÇ—
public class PlayerHealth : MonoBehaviour
{
    [SerializeField, Header("HP_Image")]
    Sprite image;
    [SerializeField, Header("HP_Max")]
    int maxHP = 3;
    int currentHealth;
    BoxCollider boxCol;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHP;
        boxCol = GetComponent<BoxCollider>();
        anim = GetComponent<Animator>();
        anim.SetInteger("HP", maxHP);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter(Collider col)
    {

        if(col.gameObject.CompareTag(TrapType.Box.ToString()) ||
            col.gameObject.CompareTag(TrapType.Trap.ToString()))
        {
            currentHealth--;
            anim.SetInteger("HP", currentHealth);
            Debug.Log("Player HP: " + currentHealth);
            // €–Sˆ—
            if (currentHealth <= 0)
            {
               // Debug.Log("Player Dead");
                boxCol.enabled = false; // —á: Õ“Ë”»’è‚ğ–³Œø‰»
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
