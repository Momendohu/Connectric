using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        //totalscore.GetComponent<>()
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
