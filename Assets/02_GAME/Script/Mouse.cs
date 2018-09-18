using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour {

    private const float RIMIT_TOP = 2.5f;
    private const float RIMIT_BOTTOM = -7.0f;
    private const float RIMIT_LEFT = -4.8f;
    private const float RIMIT_RIGHT = 4.8f;


    [SerializeField] private float cursol_X;
    [SerializeField] private float cursol_Y;
    [SerializeField] private bool tapFlag;
    [SerializeField] private bool oldTapFlag;
    [SerializeField] private bool is_TriggerTapFlag;
    [SerializeField] private bool is_ReleaseTapFlag;
    [SerializeField] private GameObject board;
    [SerializeField] public static bool CaptureFlag;

    // Use this for initialization
    void Start () {
        oldTapFlag = false;
        tapFlag = false;
        CaptureFlag = false;
        MouseInfo();
    }

    // Update is called once per frame
    void FixedUpdate () {

        MouseInfo();

        if(cursol_X < RIMIT_LEFT || cursol_X > RIMIT_RIGHT || cursol_Y < RIMIT_BOTTOM || cursol_Y > RIMIT_TOP || is_ReleaseTapFlag) {
            CaptureFlag = false;
        }
        Debug.Log(CaptureFlag);

    }

    //---------------------------------------------------------
    // マウスの座標やクリックの管理
    //---------------------------------------------------------
    private void MouseInfo () {

        Vector3 vPos = new Vector3(Input.mousePosition.x,Input.mousePosition.y,0.0f);
        Vector3 vWorldPos = Camera.main.ScreenToWorldPoint(vPos);
        cursol_X = vWorldPos.x;
        cursol_Y = vWorldPos.y;

        tapFlag = Input.GetMouseButton(0);          // 左ボタンクリック
        is_TriggerTapFlag = !oldTapFlag && tapFlag; // トリガー
        is_ReleaseTapFlag = oldTapFlag && !tapFlag; // リリース 
        oldTapFlag = tapFlag;                       // 過去のクリック
        this.transform.position = new Vector3(cursol_X,cursol_Y,vWorldPos.z);
    }

    //--------------------------------------------------------
    // 侵入検知(領域に入っているとき)
    //--------------------------------------------------------
    void OnTriggerStay2D (Collider2D other) {
        // クリック時一度のみ
        if(is_TriggerTapFlag) {
            // ボードのキャプチャー
            if(other.tag == "Board") {
                if(cursol_X > RIMIT_LEFT && cursol_X < RIMIT_RIGHT && cursol_Y > RIMIT_BOTTOM & cursol_Y < RIMIT_TOP) {
                    CaptureFlag = true;
                }
                other.GetComponent<SpriteRenderer>().color = new Color(0.5f,0.5f,0.5f,0.0f);
                board.GetComponent<BoardManager>().SetMouseObj();
            }
        }

        // クリックしている間
        if(tapFlag && !is_TriggerTapFlag) {
            if(other.tag == "Board") {
                other.GetComponent<SpriteRenderer>().color = Color.black;
                board.GetComponent<BoardManager>().Change();
            }
        }

        // リリースした時
        if(/*is_ReleaseTapFlag && */!CaptureFlag) {
            board.GetComponent<BoardManager>().ReleaseMouseObj();
        }

    }

}