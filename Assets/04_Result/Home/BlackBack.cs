using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackBack : MonoBehaviour {

    //------------------------------------------------------
    // 背景の表示
    //------------------------------------------------------
    private bool isBackBlackFlag = false;
    public bool IsBackBlackFlag
    {
        get { return isBackBlackFlag; }
        set { isBackBlackFlag = value; }
    }

    // Use this for initialization
    void Start () {

        isBackBlackFlag = false;
        this.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        this.GetComponent<RectTransform>().localScale = new Vector3(0.0f, 0.0f, 0.0f);
    }
	
	// Update is called once per frame
	void Update () {

        if (!isBackBlackFlag)
        {
            Start();
            return;
        }

        if(this.GetComponent<Image>().color.a < 0.7f)
        {
            this.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
            this.GetComponent<Image>().color += new Color(0.0f,0.0f,0.0f,0.1f);
        }
	}
}
