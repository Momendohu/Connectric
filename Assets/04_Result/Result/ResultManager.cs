using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour {


    private GameObject gamemanager;
    private GameObject totalscore;
    private GameObject hi_combo;
    private GameObject hi_hit;
    private GameObject enemy_break;

	// Use this for initialization
	void Start () {
        gamemanager = GameObject.Find("GameManager");

        // テキスト
        totalscore = GameObject.Find("TOTALSCORE");
        hi_combo = GameObject.Find("HI_COMBO");
        hi_hit = GameObject.Find("HI_HIT");
        enemy_break = GameObject.Find("ENEMY_break");

        // テキストの書き込み
        totalscore.GetComponent<Text>().text  = "トータルスコア      < " + gamemanager.GetComponent<GameManager>().GameRecordStatus.Score + " >";
        hi_combo.GetComponent<Text>().text    = "最大コンボ      < <color=#ff0000>" + gamemanager.GetComponent<GameManager>().GameRecordStatus.MaxCombo + "  COMBO</color> >";
        hi_hit.GetComponent<Text>().text      = "最大ヒット          < <color=#ff5500>" + gamemanager.GetComponent<GameManager>().GameRecordStatus.MaxHit + "  HIT </color>>";
        enemy_break.GetComponent<Text>().text = "敵の撃破数   < <color=#ffff00>"+ gamemanager.GetComponent<GameManager>().GameRecordStatus.DefeatEnemyNum + "</color> >";

    }
	
	// Update is called once per frame
	void Update () {
        
	}


    //---------------------------------------- 
    // マウス
    //----------------------------------------
    public void OnClick()
    {
       gamemanager.GetComponent<GameManager>().JumpSceneResultToMusicSelect();
    }
}
