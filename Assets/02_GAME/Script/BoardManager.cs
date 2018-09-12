using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

    // 定数
    public const int BOARD_ALL_NUM = 12;
    public const int BOARD_HEIGHT_NUM = 4;
    public const int PIECE_KIND_NUM = 4;

    // ピースタイプ
    public enum TYPE
    {
        BLUE = 0,
        GREEN,
        RED,
        YELLOW
    };

    // 構造体
    public struct PANEL_DATA
    {
        public GameObject obj;  // 
        public int arrayNum;    // 配列番号
        public int typeNum;     // 属性
        public bool mouseFlag;  // キャプチャーされているか
        public bool moveFlag;   // 動かせるか
    };

    // 変数
    public static GameObject[] Boards = new GameObject[BOARD_ALL_NUM];
    public static PANEL_DATA[] Boardpieces = new PANEL_DATA[BOARD_ALL_NUM];
    
    [SerializeField]private GameObject board;
    [SerializeField]private GameObject[] piece = new GameObject[PIECE_KIND_NUM];
    [SerializeField] private bool[] flag = new bool[BOARD_ALL_NUM];
    



    //===================================================
    // Use this for initialization
    //===================================================
    void Start () {
        CreateBoard();
	}

    //===================================================
    // Update is called once per frame
    //===================================================
    void Update () {

        MoveMausePiece();
        
	}

    //-------------------------------------------------------
    // 配置ボードの生成
    //-------------------------------------------------------
    private void CreateBoard()
    {
        Vector3 vStartPos = new Vector3(-3.8f, -1.8f, 0.0f);
        for (int i = 0; i < BOARD_ALL_NUM; i++)
        {
            int obj_num =  UnityEngine.Random.Range(0, PIECE_KIND_NUM);
            float between = 1.8f;

            GameObject boardcopy = Instantiate(board, new Vector3(vStartPos.x + between * (i % BOARD_HEIGHT_NUM), vStartPos.y - between * (i / BOARD_HEIGHT_NUM), 0.0f), Quaternion.identity);
            Boards[i] = boardcopy;

            GameObject piececopy = Instantiate(piece[obj_num], new Vector3(vStartPos.x + between * (i % BOARD_HEIGHT_NUM), vStartPos.y - between * (i / BOARD_HEIGHT_NUM), 0.0f), Quaternion.identity);
            Boardpieces[i].obj = piececopy;
            Boardpieces[i].arrayNum = i;
            Boardpieces[i].typeNum = obj_num;
            Boardpieces[i].mouseFlag = false;
            Boardpieces[i].moveFlag = false;

            // デバッグ用
            flag[i] = Boardpieces[i].moveFlag;
        }

       
    }

    //-------------------------------------------------------
    // 入れ替え可能ピースの設定
    //-------------------------------------------------------
    private void SetMovepiece()
    {
        // 念のため初期化
        for (int i = 0; i < BOARD_ALL_NUM; i++)
        {
            Boardpieces[i].moveFlag = false;
        }

        for (int i = 0; i < BOARD_ALL_NUM; i++)
        {
            if (Boardpieces[i].mouseFlag)
            {
                // 四か所できる
                if (i == 5 || i == 6)
                {
                    Boardpieces[i - 1].moveFlag = true;
                    Boardpieces[i + 1].moveFlag = true;
                    Boardpieces[i - BOARD_HEIGHT_NUM].moveFlag = true;
                    Boardpieces[i + BOARD_HEIGHT_NUM].moveFlag = true;

                    // デバッグ用
                    Boards[i - 1].GetComponent<SpriteRenderer>().color = Color.cyan;
                    Boards[i + 1].GetComponent<SpriteRenderer>().color = Color.cyan;
                    Boards[i - BOARD_HEIGHT_NUM].GetComponent<SpriteRenderer>().color = Color.cyan;
                    Boards[i + BOARD_HEIGHT_NUM].GetComponent<SpriteRenderer>().color = Color.cyan;
                }

                // 三か所できる
                if (i == 1 || i == 2)
                {
                    Boardpieces[i - 1].moveFlag = true;
                    Boardpieces[i + 1].moveFlag = true;
                    Boardpieces[i + BOARD_HEIGHT_NUM].moveFlag = true;

                    // デバッグ用
                    Boards[i - 1].GetComponent<SpriteRenderer>().color = Color.cyan;
                    Boards[i + 1].GetComponent<SpriteRenderer>().color = Color.cyan;
                    Boards[i + BOARD_HEIGHT_NUM].GetComponent<SpriteRenderer>().color = Color.cyan;
                }
                if (i == 9 || i == 10)
                {
                    Boardpieces[i - 1].moveFlag = true;
                    Boardpieces[i + 1].moveFlag = true;
                    Boardpieces[i - BOARD_HEIGHT_NUM].moveFlag = true;

                    // デバッグ用
                    Boards[i - 1].GetComponent<SpriteRenderer>().color = Color.cyan;
                    Boards[i + 1].GetComponent<SpriteRenderer>().color = Color.cyan;
                    Boards[i - BOARD_HEIGHT_NUM].GetComponent<SpriteRenderer>().color = Color.cyan;
                }
                if (i == 4)
                {
                    Boardpieces[i + 1].moveFlag = true;
                    Boardpieces[i - BOARD_HEIGHT_NUM].moveFlag = true;
                    Boardpieces[i + BOARD_HEIGHT_NUM].moveFlag = true;

                    // デバッグ用
                    Boards[i + 1].GetComponent<SpriteRenderer>().color = Color.cyan;
                    Boards[i - BOARD_HEIGHT_NUM].GetComponent<SpriteRenderer>().color = Color.cyan;
                    Boards[i + BOARD_HEIGHT_NUM].GetComponent<SpriteRenderer>().color = Color.cyan;
                }
                if (i == 7)
                {
                    Boardpieces[i - 1].moveFlag = true;
                    Boardpieces[i - BOARD_HEIGHT_NUM].moveFlag = true;
                    Boardpieces[i + BOARD_HEIGHT_NUM].moveFlag = true;

                    // デバッグ用
                    Boards[i - 1].GetComponent<SpriteRenderer>().color = Color.cyan;
                    Boards[i - BOARD_HEIGHT_NUM].GetComponent<SpriteRenderer>().color = Color.cyan;
                    Boards[i + BOARD_HEIGHT_NUM].GetComponent<SpriteRenderer>().color = Color.cyan;
                }

                // 二か所
                if (i == 0)
                {
                    Boardpieces[i + 1].moveFlag = true;
                    Boardpieces[i + BOARD_HEIGHT_NUM].moveFlag = true;

                    // デバッグ用
                    Boards[i + 1].GetComponent<SpriteRenderer>().color = Color.cyan;
                    Boards[i + BOARD_HEIGHT_NUM].GetComponent<SpriteRenderer>().color = Color.cyan;
                }
                if (i == 3)
                {
                    Boardpieces[i - 1].moveFlag = true;
                    Boardpieces[i + BOARD_HEIGHT_NUM].moveFlag = true;

                    // デバッグ用
                    Boards[i - 1].GetComponent<SpriteRenderer>().color = Color.cyan;
                    Boards[i + BOARD_HEIGHT_NUM].GetComponent<SpriteRenderer>().color = Color.cyan;
                }
                if (i == 8)
                {
                    Boardpieces[i + 1].moveFlag = true;
                    Boardpieces[i - BOARD_HEIGHT_NUM].moveFlag = true;

                    // デバッグ用
                    Boards[i + 1].GetComponent<SpriteRenderer>().color = Color.cyan;
                    Boards[i - BOARD_HEIGHT_NUM].GetComponent<SpriteRenderer>().color = Color.cyan;
                }
                if (i == 11)
                {
                    Boardpieces[i - 1].moveFlag = true;
                    Boardpieces[i - BOARD_HEIGHT_NUM].moveFlag = true;

                    // デバッグ用
                    Boards[i - 1].GetComponent<SpriteRenderer>().color = Color.cyan;
                    Boards[i - BOARD_HEIGHT_NUM].GetComponent<SpriteRenderer>().color = Color.cyan;
                }
                break;
            }
        }
    }

    //-------------------------------------------------------
    // マウスが持っているオブジェクトを動かす
    //-------------------------------------------------------
    private void MoveMausePiece()
    {
        for (int i = 0; i < BOARD_ALL_NUM; i++)
        {
            if (Boardpieces[i].mouseFlag)
            {
                Vector3 vPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f);
                Vector3 vWorldPos = Camera.main.ScreenToWorldPoint(vPos);
                Boardpieces[i].obj.GetComponent<Transform>().position = vWorldPos;
                break;
            }
        }
    }

    //-------------------------------------------------------
    //  ピースの入れ替え
    //-------------------------------------------------------
    public void Change()
    {
        for (int i = 0; i < BOARD_ALL_NUM; i++)
        {
            // マウスと当たったやつの検索
            if (Boards[i].GetComponent<SpriteRenderer>().color == Color.black)
            {
                if (Boardpieces[i].moveFlag)
                {
                    for (int j = 0; j < BOARD_ALL_NUM; j++)
                    {
                        if (Boardpieces[j].mouseFlag)
                        {
                            // 
                            PANEL_DATA save = Boardpieces[j];
                            Boardpieces[j] = Boardpieces[i];
                            Boardpieces[j].obj.GetComponent<Transform>().position = Boards[j].GetComponent<Transform>().position;
                            Boardpieces[i] = save;

                            
                            for (int k = 0; k < BOARD_ALL_NUM; k++)
                            { 
                                Boards[k].GetComponent<SpriteRenderer>().color = Color.magenta;

                            }
                            SetMovepiece();
                            break;
                        }
                    }
                }
            }
        }
    }

    //-------------------------------------------------------
    // マウスが持っているオブジェクトのセット
    //-------------------------------------------------------
    public void SetMouseObj()
    {
        Color Gray = new Color(0.5f, 0.5f, 0.5f, 0.0f);

        for (int i = 0; i < BOARD_ALL_NUM; i++)
        {
            if(Boards[i].GetComponent<SpriteRenderer>().color == Gray)
            {
                Boardpieces[i].obj.GetComponent<Piece>().Big();
                Boardpieces[i].obj.GetComponent<SpriteRenderer>().sortingOrder = 1;
                Boardpieces[i].mouseFlag = true;
                Boards[i].GetComponent<SpriteRenderer>().color = Color.magenta;
                SetMovepiece();
                break;
            }

        }
    }

    //-------------------------------------------------------
    // マウスが持っていたオブジェクトの解放
    //-------------------------------------------------------
    public void ReleaseMouseObj()
    {
        // ボードカラー初期化
        for (int i = 0; i < BOARD_ALL_NUM; i++)
        {
            Boards[i].GetComponent<SpriteRenderer>().color = Color.magenta;
        }

        for (int i = 0; i < BOARD_ALL_NUM; i++)
        {
            if (Boardpieces[i].mouseFlag)
            {
                Boardpieces[i].obj.GetComponent<Transform>().position = Boards[i].GetComponent<Transform>().position;
                Boardpieces[i].obj.GetComponent<SpriteRenderer>().sortingOrder = 0;
                Boardpieces[i].mouseFlag = false;
                break;
            }

        }
    }

    
}
