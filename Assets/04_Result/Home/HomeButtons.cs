using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeButtons : MonoBehaviour {

    // 
    private GameObject homeManager;

	// Use this for initialization
	void Start () {
        homeManager = GameObject.Find("HomeManager");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // クリック分岐処理
    public void OnClick(int num)
    {
        homeManager.GetComponent<HomeManager>().Selectmode = num;
    }
}
