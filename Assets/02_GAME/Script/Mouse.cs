﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour {

    private const float RIMIT_TOP = 0.8f;
    private const float RIMIT_BOTTOM = -8.8f;
    private const float RIMIT_LEFT = -5.0f;
    private const float RIMIT_RIGHT = 5.0f;

    [SerializeField] private Vector3 cursol_world_pos;
    public Vector3 CursolWorldPos
    {
        get { return cursol_world_pos; }
        set { cursol_world_pos = value; }
    }

    [SerializeField] private bool tapFlag;
    [SerializeField] private bool oldTapFlag;
    [SerializeField] private bool is_TriggerTapFlag;
    [SerializeField] private bool is_ReleaseTapFlag;
    [SerializeField] private GameObject board;
    [SerializeField] public static bool CaptureFlag;

    private GameObject game_manager;

    // Use this for initialization
    void Start () {
        oldTapFlag = false;
        tapFlag = false;
        CaptureFlag = false;

        // 実機で操作するか否か
        if (Application.isEditor)
        {
            MouseInfo();
            Debug.Log("mause");
        }
        else
        {
            TouchInfo();
            Debug.Log("実機");
        }

        game_manager = GameObject.Find("GameManager");
    }

    // Update is called once per frame
    void FixedUpdate () {

        if (game_manager.GetComponent<GameManager>().IsGameClear || game_manager.GetComponent<GameManager>().IsGameOver ||
               game_manager.GetComponent<GameManager>().IsPause) { return; }


        // 実機で操作するか否か
        if (Application.isEditor)
        {
            MouseInfo();
            Debug.Log("mause");
        }
        else
        {
            TouchInfo();
            Debug.Log("実機");
        }


        if (!PlayScreenCheck() || is_ReleaseTapFlag)
        {
            board.GetComponent<BoardManager>().ReleaseMouseObj();
            oldTapFlag = false;
            tapFlag = false;
            CaptureFlag = false;
        }

    }

    //---------------------------------------------------------
    // マウスの座標やクリックの管理
    //---------------------------------------------------------
    private void MouseInfo () {

        Vector3 vPos = new Vector3(Input.mousePosition.x,Input.mousePosition.y,10.0f);
        cursol_world_pos = Camera.main.ScreenToWorldPoint(vPos);

        tapFlag = Input.GetMouseButton(0);          // 左ボタンクリック
        is_TriggerTapFlag = !oldTapFlag && tapFlag; // トリガー
        is_ReleaseTapFlag = oldTapFlag && !tapFlag; // リリース 
        oldTapFlag = tapFlag;                       // 過去のクリック
        this.transform.position = new Vector3(cursol_world_pos.x, cursol_world_pos.y, cursol_world_pos.z);
    }

    //---------------------------------------------------------
    // タッチ座標やクリックの管理
    //---------------------------------------------------------
    private void TouchInfo()
    {

        Vector3 vPos = new Vector3(0.0f, 0.0f, 10.0f);
        cursol_world_pos = Camera.main.ScreenToWorldPoint(vPos);
        tapFlag = false;
        is_TriggerTapFlag = false; // トリガー
        is_ReleaseTapFlag = false; // リリース 

        // タッチしているかチェック
        if (Input.touchCount > 0)
        {
            tapFlag = true;

            // タッチ情報の取得
            Touch touch = Input.GetTouch(0);
            vPos = new Vector3(touch.position.x, touch.position.y, 10.0f);
            cursol_world_pos = Camera.main.ScreenToWorldPoint(vPos);

            if (!oldTapFlag)
            {
                if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    is_TriggerTapFlag = true;
                }
            }
            if (oldTapFlag)
            {
                if (touch.phase == TouchPhase.Ended)
                {
                    is_ReleaseTapFlag = true;
                }
            }
        }

        this.transform.position = new Vector3(cursol_world_pos.x, cursol_world_pos.y, cursol_world_pos.z);
        oldTapFlag = tapFlag;
    }

    //--------------------------------------------------------
    // 侵入検知(領域に入っているとき)
    //--------------------------------------------------------
    void OnTriggerEnter2D(Collider2D other)
    {

        if (game_manager.GetComponent<GameManager>().IsGameClear || game_manager.GetComponent<GameManager>().IsGameOver ||
               game_manager.GetComponent<GameManager>().IsPause) { return; }

        // クリック時一度のみ
        if (is_TriggerTapFlag)
        {
            // ボードのキャプチャー
            if (other.tag == "Board")
            {
                if (PlayScreenCheck())
                {
                    CaptureFlag = true;
                }
                other.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 0.0f);
                board.GetComponent<BoardManager>().SetMouseObj();

            }
        }

        // クリックしている間
        if (tapFlag && !is_TriggerTapFlag)
        {
            if (other.tag == "Board")
            {
                other.GetComponent<SpriteRenderer>().color = Color.black;
                board.GetComponent<BoardManager>().Change();
            }
        }
    }


    void OnTriggerStay2D (Collider2D other) {

        if (game_manager.GetComponent<GameManager>().IsGameClear || game_manager.GetComponent<GameManager>().IsGameOver ||
               game_manager.GetComponent<GameManager>().IsPause) { return; }


        // クリック時一度のみ
        if (is_TriggerTapFlag) {
            // ボードのキャプチャー
            if(other.tag == "Board") {
                if( PlayScreenCheck() ) {
                    CaptureFlag = true;
                }
                other.GetComponent<SpriteRenderer>().color = new Color(0.5f,0.5f,0.5f,0.0f);
                board.GetComponent<BoardManager>().SetMouseObj();
            }
        }

        // クリックしている間
        if(tapFlag && !is_TriggerTapFlag) {
            if(other.tag == "Board") {
                other.GetComponent<SpriteRenderer>().color = new Color(0.0f,0.0f,0.0f,0.0f);
                board.GetComponent<BoardManager>().Change();
            }
        }

    }

    //--------------------------------------------------------
    // 画面操作範囲外チェック
    //--------------------------------------------------------
    public bool PlayScreenCheck()
    {
        return ( cursol_world_pos.x > RIMIT_LEFT && cursol_world_pos.x < RIMIT_RIGHT && 
                 cursol_world_pos.y > RIMIT_BOTTOM && cursol_world_pos.y < RIMIT_TOP );
    }

}