using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tab : MonoBehaviour {

    // 定数
    private const float INDICATE_SPEED = 0.2f;


    private AudioSource soundSE;

    //------------------------------------------------------
    // タブの表示
    //------------------------------------------------------
    private bool isTabIndicateFlag = false;
    public bool IsTabIndicateFlag
    {
        get { return isTabIndicateFlag; }
        set { isTabIndicateFlag = value; }
    }

    // Use this for initialization
    void Start () {
        isTabIndicateFlag = false;
        this.GetComponent<Transform>().localScale = new Vector3(0.0f, 0.0f, 0.0f);
        soundSE = GameObject.Find("HomeSE1").GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {

        if (!isTabIndicateFlag) { return; }

        if (this.GetComponent<Transform>().localScale.x <= 0.8f)
        {
            this.GetComponent<Transform>().localScale += new Vector3(INDICATE_SPEED, INDICATE_SPEED, INDICATE_SPEED);
        } else {
            this.GetComponent<Transform>().localScale = Vector3.one*0.8f;
        }
	}


    //========================================================
    // クリック処理
    //========================================================
    public void OnClick()
    {
        Start();
        soundSE.PlayOneShot(soundSE.clip);
    }
}
