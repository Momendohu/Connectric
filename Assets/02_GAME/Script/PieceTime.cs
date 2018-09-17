using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceTime : MonoBehaviour {
    
    public bool finAnimFrag = false;
	// Use this for initialization
	void Start () {
        GetComponent<Animator>().SetTrigger("CountDownTrigger");
    }
	
	// Update is called once per frame
	void Update () {

        if (!Mouse.ChaptureFlag)
        {
            this.GetComponent<Transform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }


        if (this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            finAnimFrag = true;
        }

    }

    //-------------------------------------------
    // キャプチャ中に大きくする
    //-------------------------------------------
    public void Big()
    {
        this.GetComponent<Transform>().localScale = new Vector3(0.6f, 0.6f, 0.6f);
    }

    //--------------------------------------------------------------
    // アニメーション終了のお知らせ
    //--------------------------------------------------------------
    public bool GetFinAnim()
    {
        return finAnimFrag;
    }
}
