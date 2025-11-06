using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//OBJに当たった時の体力管理
public class PlayerHealth : MonoBehaviour
{
    [SerializeField, Header("HP_Image")]
    Sprite image;
    [SerializeField, Header("HP_Max")]
    int maxHP = 3;
    int currentHealth;
    BoxCollider boxCol;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHP;
        boxCol = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter(Collider col)
    {

        //トラップの数だけループさせてチェック
        for(int i = 0;i<Enum.GetValues(typeof(TrapType)).Length;i++)
        {

        }
        if(col.gameObject.CompareTag(TrapType.Box.ToString()) ||
            col.gameObject.CompareTag(TrapType.Trap.ToString()))
        {
            currentHealth--;
            Debug.Log("Player HP: " + currentHealth);
            // 死亡処理
            if (currentHealth <= 0)
            {
               // Debug.Log("Player Dead");
                boxCol.enabled = false; // 例: 衝突判定を無効化
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
