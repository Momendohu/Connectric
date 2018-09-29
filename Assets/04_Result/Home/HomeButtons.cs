using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeButtons : MonoBehaviour {

    //--------------------------------------------
    // オブジェクト
    private GameObject tabObj;
    private GameObject homeManager;

	// Use this for initialization
	void Start () {
        tabObj = GameObject.Find("tab");
        homeManager = GameObject.Find("HomeManager");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // クリック分岐処理
    public void OnClick(int num)
    {
        // タグ表示中は更新しない
        if (tabObj.GetComponent<Tab>().IsTabIndicateFlag) { return; }

        homeManager.GetComponent<HomeManager>().Selectmode = num;
    }
}
