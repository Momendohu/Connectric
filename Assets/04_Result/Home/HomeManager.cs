using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeManager : MonoBehaviour {

    //--------------------------------------------
    // モード切替
    //--------------------------------------------
    private int selectmode = -1;
    public int Selectmode
    {
        get { return selectmode; }
        set { selectmode = value; }
    }

    //--------------------------------------------
    // タブ
    private GameObject tabObj;

    //--------------------------------------------
    // 黒い背景
    private GameObject blackBackObj;

    //--------------------------------------------
    // ゲームマネージャー
    private GameObject gameManagerObj;
    

    // Use this for initialization
    void Start () {
        tabObj = GameObject.Find("tab");
        blackBackObj = GameObject.Find("BlackBack");
        gameManagerObj = GameObject.Find("GameManager");
    }
	
	// Update is called once per frame
	void Update () {

        // 黒背景の表示 / 非表示
        if(!tabObj.GetComponent<Tab>().IsTabIndicateFlag)
        {
            blackBackObj.GetComponent<BlackBack>().IsBackBlackFlag = false;
        }

        CheckMode();
    }
    

    //--------------------------------------------
    // クリック処理
    //--------------------------------------------
    private void CheckMode()
    {

        // タブ表示中は更新しない
        if (tabObj.GetComponent<Tab>().IsTabIndicateFlag) { return; }

        switch(selectmode)
        {
            // バトル
            case 0:
                Debug.Log("バトル");
                gameManagerObj.GetComponent<GameManager>().JumpSceneHomeToSelectSound();
                break;

            // クエスト
            case 1:
                Debug.Log("クエスト");
                TagIndicate();
                break;

            // 強化
            case 2:
                Debug.Log("強化");
                TagIndicate();
                break;

            // ホーム
            case 3:
                Debug.Log("ホーム");
                TagIndicate();
                break;

            // ガチャ
            case 4:
                Debug.Log("ガチャ");
                TagIndicate();
                break;

            // 設定
            case 5:
                Debug.Log("設定");
                TagIndicate();
                break;

            // 設定
            case 6:
                Debug.Log("お知らせ");
                TagIndicate();
                break;

            // 設定
            case 7:
                Debug.Log("メール");
                TagIndicate();
                break;

            // 設定
            case 8:
                Debug.Log("フレンド");
                TagIndicate();
                break;

            // その他
            default:
                break;
        }

        selectmode = -1;
    }

    //-------------------------------------------------
    // タブ表示
    //-------------------------------------------------
    private void TagIndicate()
    {
        tabObj.GetComponent<Tab>().IsTabIndicateFlag = true;
        blackBackObj.GetComponent<BlackBack>().IsBackBlackFlag = true;
    }


}
