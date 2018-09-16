using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour {

    [SerializeField]private  float cursol_X;
    [SerializeField]private  float cursol_Y;
    [SerializeField]private  bool tapFlag;
    [SerializeField]private  bool oldTapFlag;
    [SerializeField]private  bool is_TriggerTapFlag;
    [SerializeField]private  bool is_ReleaseTapFlag;
    [SerializeField]private GameObject board;
    [SerializeField]public static bool ChaptureFlag;

    // Use this for initialization
    void Start () {
        oldTapFlag = false;
        tapFlag = false;
        ChaptureFlag = false;
        MouseInfo();
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        MouseInfo();

        if(is_ReleaseTapFlag) { ChaptureFlag = false; }
    }

    //---------------------------------------------------------
    // マウスの座標やクリックの管理
    //---------------------------------------------------------
    private void MouseInfo()
    {

        Vector3 vPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f);
        Vector3 vWorldPos = Camera.main.ScreenToWorldPoint(vPos);
        cursol_X = vWorldPos.x;
        cursol_Y = vWorldPos.y;

        tapFlag = Input.GetMouseButton(0);          // 左ボタンクリック
        is_TriggerTapFlag = !oldTapFlag && tapFlag; // トリガー
        is_ReleaseTapFlag = oldTapFlag && !tapFlag; // リリース 
        oldTapFlag = tapFlag;                       // 過去のクリック
        this.transform.position = new Vector3(cursol_X, cursol_Y, vWorldPos.z);
    }

    //--------------------------------------------------------
    // 侵入検知(領域に入っているとき)
    //--------------------------------------------------------
    void OnTriggerStay2D(Collider2D other)
    {
        // クリック時一度のみ
        if(is_TriggerTapFlag)
        {
            // ボードのキャプチャー
            if (other.tag == "Board")
            {
                ChaptureFlag = true;
                other.GetComponent<SpriteRenderer>().color = new Color(0.5f,0.5f,0.5f,0.0f);
                board.GetComponent<BoardManager>().SetMouseObj();
            }
        }

        // クリックしている間
        if(tapFlag && !is_TriggerTapFlag)
        {
            if(other.tag == "Board")
            {
                other.GetComponent<SpriteRenderer>().color = Color.black;
                board.GetComponent<BoardManager>().Change();
            }
        }

        // リリースした時
        if(is_ReleaseTapFlag)
        {
            board.GetComponent<BoardManager>().ReleaseMouseObj();
        }

    }

    //// 入力されたクリック(タップ)位置から最も近いピースの位置を返す
    //public Piece GetNearestPiece(Vector3 input)
    //{
    //    var minDist = float.MaxValue;
    //    Piece nearestPiece = null;

    //    // 入力値と盤面のピース位置との距離を計算し、一番距離が短いピースを探す
    //    foreach (var p in board)
    //    {
    //        var dist = Vector3.Distance(input, p.transform.position);
    //        if (dist < minDist)
    //        {
    //            minDist = dist;
    //            nearestPiece = p;
    //        }
    //    }

    //    return nearestPiece;
    //}

}
