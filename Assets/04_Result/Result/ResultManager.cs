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
        totalscore.GetComponent<Text>().text  = "TOTAL SCORE      < " + 99999 + " >";
        hi_combo.GetComponent<Text>().text    = "HI COMBO      < <color=#ff0000>" + 20 + "  COMBO</color> >";
        hi_hit.GetComponent<Text>().text      = "HI HIT          < <color=#ff5500>" + 21 + "  HIT </color>>";
        enemy_break.GetComponent<Text>().text = "ENEMY_break   < <color=#ffff00>CLEAR</color> >";
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
