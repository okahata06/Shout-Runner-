using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageTips : MonoBehaviour
{
    //ステージチップのサイズ
    const int StageTipSize = 12;
    private int currentTipIndex;
    [Header("ターゲットのキャラ")]
    public Transform character;
    [Header("ステージチップ格納用配列")]
    public GameObject[] stageTips;
    [Header("最初のステージチップ生成位置")]
    public int startTipIndex;
    [Header("ステージ生成数")]
    public int preInstantiate;
    [Header("生成されたステージリスト")]
    public List<GameObject> generatedStageList = new List<GameObject>();

    void Start()
    {
        //初期化処理
        currentTipIndex = startTipIndex - 1;
        UpdateStage(preInstantiate);
    }


    void Update()
    {
        //キャラクターの位置から現在のステージチップのインデックスを計算
        int charaPositionIndex = (int)(character.position.z / StageTipSize);
        //次のステージチップに入ったらステージの更新処理を行う
        if (charaPositionIndex + preInstantiate > currentTipIndex)
        {
            UpdateStage(charaPositionIndex + preInstantiate);
        }

    }
    //指定のインデックスまでのステージチップを生成して管理下におく
    void UpdateStage(int toTipIndex)
    {
        if (toTipIndex <= currentTipIndex) return;

        //指定のステージチップまで生成
        for (int i = currentTipIndex + 1; i <= toTipIndex; i++)
        {
            GameObject stageObject = GenerateStage(i);
            //生成したステージチップを管理リストに追加
            generatedStageList.Add(stageObject);
        }
        //ステージ保持上限になるまで古いステージを削除
        while (generatedStageList.Count > preInstantiate + 2) DestroyOldestStage();

        currentTipIndex = toTipIndex;
    }
    //指定のインデックス位置にstageオブジェクトをランダムに生成
    GameObject GenerateStage(int tipIndex)
    {
        int nextStageTip = Random.Range(0, stageTips.Length);

        GameObject stageObject = Instantiate(
            stageTips[nextStageTip],
            new Vector3(0, 0, tipIndex * StageTipSize),
            Quaternion.identity);
        return stageObject;
    }
    //一番古いステージを削除
    void DestroyOldestStage()
    {
        GameObject oldStage = generatedStageList[0];
        generatedStageList.RemoveAt(0);
        Destroy(oldStage);
    }
}