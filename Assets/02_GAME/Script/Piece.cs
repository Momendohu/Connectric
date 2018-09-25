﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour {



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(!Mouse.CaptureFlag)
        {
            this.GetComponent<Transform>().localScale = new Vector3(0.2f, 0.2f, 0.2f);
        }
    }

    //-------------------------------------------
    // キャプチャ中に大きくする
    //-------------------------------------------
    public void Big()
    {
        this.GetComponent<Transform>().localScale = new Vector3(0.22f, 0.22f, 0.22f);
    }
}
