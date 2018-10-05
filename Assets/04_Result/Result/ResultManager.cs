using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour {
    public Sprite[] CharacterImages;

    private GameManager gamemanager;
    private GameObject totalscore;
    private GameObject hi_combo;
    private GameObject hi_hit;
    private GameObject enemy_defeat;

    private GameObject tatie;

    private void Start () {
        tatie = GameObject.Find("Canvas/Chara_I");
        gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();

        /*Debug.Log(
        "combo:" + gamemanager.GameRecordStatus.Combo +
        "maxcombo:" + gamemanager.GameRecordStatus.MaxCombo +
        "maxhit:" + gamemanager.GameRecordStatus.MaxHit +
        "score:" + gamemanager.GameRecordStatus.Score +
        "sep:" + gamemanager.GameRecordStatus.SeparateCombo +
        "sepN:" + gamemanager.GameRecordStatus.SeparateComboSeparateNum +
        "defeat:" + gamemanager.GameRecordStatus.DefeatEnemyNum
        );*/

        // テキスト
        totalscore = GameObject.Find("TOTALSCORE");
        hi_combo = GameObject.Find("HI_COMBO");
        hi_hit = GameObject.Find("HI_HIT");
        enemy_defeat = GameObject.Find("ENEMY_defeat");

        // テキストの書き込み
        totalscore.GetComponent<Text>().text   = "トータルスコア     " + gamemanager.GameRecordStatus.Score;
        hi_combo.GetComponent<Text>().text     = "最大コンボ        <color=#ff0000>" + gamemanager.GameRecordStatus.MaxCombo + "  COMBO</color>";
        hi_hit.GetComponent<Text>().text       = "最大ヒット        <color=#ff5500>" + gamemanager.GameRecordStatus.MaxHit + "  HIT </color>";
        enemy_defeat.GetComponent<Text>().text = "敵の撃破数      <color=#ffff00>" + gamemanager.GameRecordStatus.DefeatEnemyNum + "</color>";

    }

    private void Update () {
        tatie.GetComponent<Image>().sprite = CharacterImages[gamemanager.GetComponent<GameManager>().FocusCharacter];
    }


    //---------------------------------------- 
    // マウス
    //----------------------------------------
    public void OnClick () {
        gamemanager.GetComponent<GameManager>().JumpSceneResultToMusicSelect();
    }
}
