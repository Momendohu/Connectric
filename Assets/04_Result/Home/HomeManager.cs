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
    // タグ
    private GameObject tabObj;

    // Use this for initialization
    void Start () {
        tabObj = GameObject.Find("tab");
    }
	
	// Update is called once per frame
	void Update () {
        CheckMode();
    }
    

    //--------------------------------------------
    // クリック処理
    //--------------------------------------------
    private void CheckMode()
    {

        // タグ表示中は更新しない
        if (tabObj.GetComponent<Tab>().IsTabIndicateFlag) { return; }

        switch(selectmode)
        {
            // バトル
            case 0:
                Debug.Log("バトル");
                break;

            // クエスト
            case 1:
                Debug.Log("クエスト");
                tabObj.GetComponent<Tab>().IsTabIndicateFlag = true;
                break;

            // 強化
            case 2:
                Debug.Log("強化");
                tabObj.GetComponent<Tab>().IsTabIndicateFlag = true;
                break;

            // ホーム
            case 3:
                Debug.Log("ホーム");
                tabObj.GetComponent<Tab>().IsTabIndicateFlag = true;
                break;

            // ガチャ
            case 4:
                Debug.Log("ガチャ");
                tabObj.GetComponent<Tab>().IsTabIndicateFlag = true;
                break;

            // 設定
            case 5:
                Debug.Log("設定");
                tabObj.GetComponent<Tab>().IsTabIndicateFlag = true;
                break;

            // 設定
            case 6:
                Debug.Log("お知らせ");
                tabObj.GetComponent<Tab>().IsTabIndicateFlag = true;
                break;

            // 設定
            case 7:
                Debug.Log("メール");
                tabObj.GetComponent<Tab>().IsTabIndicateFlag = true;
                break;

            // 設定
            case 8:
                Debug.Log("フレンド");
                tabObj.GetComponent<Tab>().IsTabIndicateFlag = true;
                break;

            // その他
            default:
                break;
        }

        selectmode = -1;
    }


}
