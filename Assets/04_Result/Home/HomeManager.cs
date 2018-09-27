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

    // Use this for initialization
    void Start () {
		
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
        switch(selectmode)
        {
            // バトル
            case 0:
                Debug.Log("バトル");
                break;

            // クエスト
            case 1:
                Debug.Log("クエスト");
                break;

            // 強化
            case 2:
                Debug.Log("強化");
                break;

            // ホーム
            case 3:
                Debug.Log("ホーム");
                break;

            // ガチャ
            case 4:
                Debug.Log("ガチャ");
                break;

            // 設定
            case 5:
                Debug.Log("設定");
                break;

            // その他
            default:
                break;
        }

        selectmode = -1;
    }


}
