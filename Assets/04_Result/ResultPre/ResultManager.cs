using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultManager : MonoBehaviour {


    private GameObject gamemanager;

	// Use this for initialization
	void Start () {
        gamemanager = GameObject.Find("GameManager");
	}
	
	// Update is called once per frame
	void Update () {

        
		
	}


    // 
    // マウス
    //----------------------------------------
    public void OnClick()
    {
       gamemanager.GetComponent<GameManager>().JumpSceneResultToMusicSelect();
    }
}
