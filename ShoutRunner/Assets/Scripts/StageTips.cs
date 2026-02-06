using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageTips : MonoBehaviour
{
    //ステージチップのZ軸のサイズ
    const int StageTipSize = 12;
    //
    private int currentTipIndex;
    //
    public static int playerScore = 0;
    public static bool gaugeChage;
    [Header("スコアを1増加させるのに必要な距離")]
    public float distancePerScore = 0.1f;
    [Header("生成するステージを変更するために必要なスコア")]
    public int mustScore;
    //
    private float playerPos = 0;
    //
    private float lastPosition;
    //
    private float distanceAccumulated = 0f;
    [Header("ターゲットのキャラ")]
    public Transform character;
    [Header("ステージチップ格納用配列")]
    public GameObject[] stageTips;
    [Header("一定距離進んだ時用ステージチップ格納用配列")]
    public GameObject[] darkstageTips;
    [Header("最初のステージチップ生成位置")]
    public int startTipIndex;
    [Header("ステージ生成数")]
    public int preInstantiate;
    [Header("生成されたステージリスト")]
    public List<GameObject> generatedStageList = new List<GameObject>();
    [Header("スコアのテキスト")]
    public Text ScoreText;

    [Header("Bossのテキスト")]
    public GameObject BossText;

    private AudioSource audiosourse;
    [Header("SE")]
    public AudioClip destroyBox;

    private VoiceSetting voice_setting;//VoiceSettingスクリプト取得用

    private void Awake()
    {
        playerScore = 0;
    }

    void Start()
    {
        audiosourse=GetComponent<AudioSource>();

        BossText.SetActive(true);

        gaugeChage = false;

        //初期化処理
        currentTipIndex = startTipIndex - 1;
        UpdateStage(preInstantiate);

        //VoiceSettingスクリプトがついたオブジェクトを取得
        voice_setting = FindFirstObjectByType<VoiceSetting>();

        lastPosition = character.transform.position.z;
    }


    void Update()
    {
        if (Box.seOnce)
        {
            audiosourse.PlayOneShot(destroyBox);
            Box.seOnce = false;
        }

        if (PlayerMove.ismove)
        {
            BossText.SetActive(false);
        }

        if (lastPosition == 0)
        {
            gaugeChage = false;
        }
        else
        {
            gaugeChage = true;
        }

            //キャラクターの位置から現在のステージチップのインデックスを計算
            int charaPositionIndex = (int)(character.position.z / StageTipSize);
        //次のステージチップに入ったらステージの更新処理を行う
        if (charaPositionIndex + preInstantiate > currentTipIndex)
        {
            UpdateStage(charaPositionIndex + preInstantiate);
        }

        playerPos = character.transform.position.z;
        float move= Mathf.Abs(playerPos - lastPosition);
        distanceAccumulated += move;

        //累積距離が0.1を超えたらスコアを加算
        while (distanceAccumulated >= distancePerScore && PlayerHealth.currentHealth > 0)
        {
            playerScore += 1;
            distanceAccumulated -= distancePerScore;
        }

        //現在位置を記録
        lastPosition = playerPos;

        //スコアテキスト修正
        ScoreText.text = playerScore.ToString();
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
    /// <summary>
    /// 指定のインデックス位置にstageオブジェクトをランダムに生成
    /// </summary>
    /// <param name="tipIndex"></param>
    GameObject GenerateStage(int tipIndex)
    {
        //
        if (playerScore <= mustScore - ((StageTipSize * preInstantiate) * 10))
        {
            int nextStageTip = Random.Range(0, stageTips.Length);

            GameObject stageObject = Instantiate(
                stageTips[nextStageTip],
                new Vector3(0, 0, tipIndex * StageTipSize),
                Quaternion.identity);

            return stageObject;
        }
        else
        {
            int nextStageTip = Random.Range(0, darkstageTips.Length);

            GameObject stageObject = Instantiate(
                darkstageTips[nextStageTip],
                new Vector3(0, 0, tipIndex * StageTipSize),
                Quaternion.identity);

            return stageObject;
        }
    }
    /// <summary>
    /// 一番古いステージを削除
    /// </summary>
    void DestroyOldestStage()
    {
        GameObject oldStage = generatedStageList[0];
        generatedStageList.RemoveAt(0);
        Destroy(oldStage);
    }
}