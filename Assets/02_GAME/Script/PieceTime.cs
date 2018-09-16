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

        if(this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            finAnimFrag = true;
        }

    }

    //--------------------------------------------------------------
    // アニメーション終了のお知らせ
    //--------------------------------------------------------------
    public bool GetFinAnim()
    {
        return finAnimFrag;
    }
}
