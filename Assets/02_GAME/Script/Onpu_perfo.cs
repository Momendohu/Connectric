using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Onpu_perfo : MonoBehaviour {

    // 
    private const int TIME = 60; // 敵にたどり着くまでの時間
    private float counter = 0;
    private Vector3 startPos;
    private Vector3 endPos;     // 敵の座標の設定
    private int piece_type;

	// Use this for initialization
	void Start () {
        endPos = new Vector3(2.5f, 4.5f, 10.0f);
    }
	
	// Update is called once per frame
	void Update () {

      float time = 1.0f * counter / TIME;
      //Debug.Log(time);/////////////////////////////////////
      counter++;
      //this.transform.position = Vector3.Lerp(startPos, endPos, time);
      this.transform.position = Vector3.Slerp(startPos, endPos, time);//////////////////////////////////
        transform.eulerAngles += new Vector3(0,Random.Range(3,20),0);/////////////////////////////////////

      if(piece_type == 4)
      {

      }
      
	}

    // スタート位置の設定
    public void SetPos(Vector3 pos)
    {
        startPos = pos;
    }

    // 削除
    public void DeleteOnpu()
    {
        Destroy(this.gameObject);
    }

    // 正規化したタイム
    public float GetTime()
    {
        float time = 1.0f * counter / TIME;
        return (time);
    }


    // 色の設定
    public void SetColor(int type)
    {
        /*if(type == 0)
        {
            this.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else if(type == 1)
        {
            this.GetComponent<SpriteRenderer>().color = Color.cyan;
        }
        else if(type == 2)
        {
            this.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        else if(type == 3)
        {
            this.GetComponent<SpriteRenderer>().color = Color.green;
        }
        else
        {
            piece_type = 4;
        }*/

        if(type == 0) {
            this.GetComponent<SpriteRenderer>().color = Color.red;
            this.GetComponent<SpriteRenderer>().color = new Color(1,0,0,0.5f);
        } else if(type == 1) {
            this.GetComponent<SpriteRenderer>().color = new Color(0,1,1,0.5f);
        } else if(type == 2) {
            this.GetComponent<SpriteRenderer>().color = new Color(1,0.92f,0.016f,0.5f);
        } else if(type == 3) {
            this.GetComponent<SpriteRenderer>().color = new Color(0,1,0,0.5f);
        } else {
            piece_type = 4;
            this.GetComponent<SpriteRenderer>().color = new Color(0,0,0,0.5f);
        }


    }
}
