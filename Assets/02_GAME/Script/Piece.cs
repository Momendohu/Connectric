using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour {

    //--------------------------------------
    //
    //--------------------------------------
    private bool smallFrag;
    public bool SmallFrag
    {
        get { return smallFrag; }
        set { smallFrag = value; }
    }

    private bool deleteFrag;
    public bool DeleteFrag
    {
        get { return deleteFrag; }
        set { deleteFrag = value; }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(!Mouse.CaptureFlag && !smallFrag)
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

    //-------------------------------------------
    // 削除演出
    //-------------------------------------------
    public void Small()
    {
        Vector3 sub = new Vector3(0.02f, 0.02f, 0.02f);
        this.GetComponent<Transform>().localScale = this.GetComponent<Transform>().localScale - sub;
        if(this.GetComponent<Transform>().localScale.x < 0.0f)
        {
            DeleteFrag = true;
        }
    }
}
