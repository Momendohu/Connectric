using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

    // 定数
    public const int BOARD_ALL_NUM = 25;
    public const int BOARD_WIDTH_NUM = 5;
    public const int BOARD_HEIGHT_NUM = 5;
    public const float between = 1.85f;

    // ピースタイプ
    public enum INSTRUMENT_TYPE
    {
        GUITAR = 0,
        DRUM,
        VOCAL,
        DJ,
        MAX
    };

    // 構造体
    public struct PANEL_DATA
    {
        public GameObject obj;       // 
        public int arrayWidthNum;    // 配列番号
        public int arrayHeightNum;   // 配列番号
        public int typeNum;          // 属性
        public bool mouseFlag;       // キャプチャーされているか
        public bool moveFlag;        // 動かせるか
    };

    // 変数
    public static GameObject[,] Boards = new GameObject[BOARD_WIDTH_NUM, BOARD_HEIGHT_NUM];
    public static PANEL_DATA[,] Boardpieces = new PANEL_DATA[BOARD_WIDTH_NUM, BOARD_HEIGHT_NUM];

    [SerializeField]private GameObject board;
    [SerializeField]private GameObject[] piece = new GameObject[(int)INSTRUMENT_TYPE.MAX];
    [SerializeField] private bool[] flag = new bool[BOARD_ALL_NUM];
    
    // リンクテスト用




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
       
        Vector3 vStartPos = new Vector3(-3.7f, 1.2f, 0.0f);
        for (int height = 0; height < BOARD_HEIGHT_NUM; height++)
        {
            for (int width = 0; width < BOARD_WIDTH_NUM; width++)
            {
                int obj_num = UnityEngine.Random.Range(0, (int)INSTRUMENT_TYPE.MAX);

                // ボード（あたり判定）
                Boards[width,height] = Instantiate(board, new Vector3(vStartPos.x + between * width, vStartPos.y - between * height, 0.0f), Quaternion.identity);

                // ピース
                Boardpieces[width, height].obj = Instantiate(piece[obj_num], new Vector3(vStartPos.x + between * width, vStartPos.y - between * height, 0.0f), Quaternion.identity);
                Boardpieces[width, height].arrayWidthNum = width;
                Boardpieces[width, height].arrayHeightNum = height;
                Boardpieces[width, height].typeNum = obj_num;
                Boardpieces[width, height].mouseFlag = false;
                Boardpieces[width, height].moveFlag = false;
                
                // デバッグ用
                flag[width + height * 5] = Boardpieces[width, height].moveFlag;
            }
        }

       
    }

    //-------------------------------------------------------
    // 入れ替え可能ピースの設定
    //-------------------------------------------------------
    private void SetMovepiece()
    {
        //念のため初期化
        for (int height = 0; height < BOARD_HEIGHT_NUM; height++)
        {
            for (int width = 0; width < BOARD_WIDTH_NUM; width++)
            {
                Boardpieces[width, height].moveFlag = false;
            }
        }

        // 入れ替え
        for (int height = 0; height < BOARD_HEIGHT_NUM; height++)
        {
            for (int width = 0; width < BOARD_WIDTH_NUM; width++)
            {
                if (Boardpieces[width,height].mouseFlag)
                {
                    // 左上角
                    if(width == 0 && height == 0)
                    {
                        Boardpieces[1, 0].moveFlag = true;
                        Boardpieces[0, 1].moveFlag = true;

                        // デバッグ用
                        Boards[1,0].GetComponent<SpriteRenderer>().color = Color.cyan;
                        Boards[0,1].GetComponent<SpriteRenderer>().color = Color.cyan;
                    }

                    // 右上角
                    else if(width == (BOARD_WIDTH_NUM - 1) && height == 0 )
                    {
                        Boardpieces[BOARD_WIDTH_NUM - 2, 0].moveFlag = true;
                        Boardpieces[BOARD_WIDTH_NUM - 1, 1].moveFlag = true;

                        // デバッグ用
                        Boards[BOARD_WIDTH_NUM - 2, 0].GetComponent<SpriteRenderer>().color = Color.cyan;
                        Boards[BOARD_WIDTH_NUM - 1, 1].GetComponent<SpriteRenderer>().color = Color.cyan;

                    }

                    // 左下角
                    else if (width == 0 && height == (BOARD_HEIGHT_NUM - 1))
                    {
                        Boardpieces[0, BOARD_HEIGHT_NUM - 2].moveFlag = true;
                        Boardpieces[1, BOARD_HEIGHT_NUM - 1].moveFlag = true;

                        // デバッグ用
                        Boards[0, BOARD_HEIGHT_NUM - 2].GetComponent<SpriteRenderer>().color = Color.cyan;
                        Boards[1, BOARD_HEIGHT_NUM - 1].GetComponent<SpriteRenderer>().color = Color.cyan;

                    }

                    // 右下角
                    else if (width == (BOARD_WIDTH_NUM - 1) && height == (BOARD_HEIGHT_NUM - 1))
                    {
                        Boardpieces[BOARD_WIDTH_NUM - 2, BOARD_HEIGHT_NUM -1].moveFlag = true;
                        Boardpieces[BOARD_WIDTH_NUM - 1, BOARD_HEIGHT_NUM -2].moveFlag = true;

                        // デバッグ用
                        Boards[BOARD_WIDTH_NUM - 2, BOARD_HEIGHT_NUM -1].GetComponent<SpriteRenderer>().color = Color.cyan;
                        Boards[BOARD_WIDTH_NUM - 1, BOARD_HEIGHT_NUM - 2].GetComponent<SpriteRenderer>().color = Color.cyan;
                    }

                    // 上列
                    else if( (width != 0 && height == 0) || (width != (BOARD_WIDTH_NUM - 1) && height == 0) )
                    {
                        Boardpieces[width - 1, height].moveFlag = true;
                        Boardpieces[width + 1, height].moveFlag = true;
                        Boardpieces[width, height + 1].moveFlag = true;

                        // デバッグ用
                        Boards[width - 1, height].GetComponent<SpriteRenderer>().color = Color.cyan;
                        Boards[width + 1, height].GetComponent<SpriteRenderer>().color = Color.cyan;
                        Boards[width, height + 1].GetComponent<SpriteRenderer>().color = Color.cyan;
                    }

                    // 下列
                    else if( (width != 0 && height == (BOARD_HEIGHT_NUM - 1) ) || (width != (BOARD_WIDTH_NUM - 1) && height == (BOARD_HEIGHT_NUM - 1)) )
                    {
                        Boardpieces[width - 1, height].moveFlag = true;
                        Boardpieces[width + 1, height].moveFlag = true;
                        Boardpieces[width, height - 1].moveFlag = true;

                        // デバッグ用
                        Boards[width - 1, height].GetComponent<SpriteRenderer>().color = Color.cyan;
                        Boards[width + 1, height].GetComponent<SpriteRenderer>().color = Color.cyan;
                        Boards[width, height - 1].GetComponent<SpriteRenderer>().color = Color.cyan;
                    }

                    // 左列
                    else if ((width == 0 && height != 0) || (width == 0 && height == (BOARD_HEIGHT_NUM - 1)))
                    {
                        Boardpieces[width, height - 1].moveFlag = true;
                        Boardpieces[width, height + 1].moveFlag = true;
                        Boardpieces[width + 1, height].moveFlag = true;

                        // デバッグ用
                        Boards[width, height - 1].GetComponent<SpriteRenderer>().color = Color.cyan;
                        Boards[width, height + 1].GetComponent<SpriteRenderer>().color = Color.cyan;
                        Boards[width + 1, height].GetComponent<SpriteRenderer>().color = Color.cyan;
                    }

                    // 右列
                    else if ((width == (BOARD_WIDTH_NUM - 1) && height != 0) || (width == (BOARD_WIDTH_NUM - 1) && height == (BOARD_HEIGHT_NUM - 1)))
                    {
                        Boardpieces[width, height - 1].moveFlag = true;
                        Boardpieces[width, height + 1].moveFlag = true;
                        Boardpieces[width - 1, height].moveFlag = true;

                        // デバッグ用
                        Boards[width, height - 1].GetComponent<SpriteRenderer>().color = Color.cyan;
                        Boards[width, height + 1].GetComponent<SpriteRenderer>().color = Color.cyan;
                        Boards[width - 1, height].GetComponent<SpriteRenderer>().color = Color.cyan;
                    }

                    // 四か所できる
                    else 
                    {
                        Boardpieces[width - 1, height].moveFlag = true;
                        Boardpieces[width + 1, height].moveFlag = true;
                        Boardpieces[width, height - 1].moveFlag = true;
                        Boardpieces[width, height + 1].moveFlag = true;

                        // デバッグ用
                        Boards[width - 1, height].GetComponent<SpriteRenderer>().color = Color.cyan;
                        Boards[width + 1, height].GetComponent<SpriteRenderer>().color = Color.cyan;
                        Boards[width, height - 1].GetComponent<SpriteRenderer>().color = Color.cyan;
                        Boards[width, height + 1].GetComponent<SpriteRenderer>().color = Color.cyan;
                    }

                    
                    break;
                }
            }
        }
    }

    //-------------------------------------------------------
    // マウスが持っているオブジェクトを動かす
    //-------------------------------------------------------
    private void MoveMausePiece()
    {

        for(int height = 0; height < BOARD_HEIGHT_NUM; height++ )
        {
            for (int width = 0; width < BOARD_WIDTH_NUM; width++)
            {
                if (Boardpieces[width,height].mouseFlag)
                {
                    Vector3 vPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f);
                    Vector3 vWorldPos = Camera.main.ScreenToWorldPoint(vPos);
                    Boardpieces[width,height].obj.GetComponent<Transform>().position = vWorldPos;
                    break;
                }
            }
        }
      
    }

    //-------------------------------------------------------
    //  ピースの入れ替え
    //-------------------------------------------------------
    public void Change()
    {

        for (int height = 0; height < BOARD_HEIGHT_NUM; height++)
        {
            for (int width = 0; width < BOARD_WIDTH_NUM; width++)
            {
                //マウスと当たったやつの検索
                if (Boards[width, height].GetComponent<SpriteRenderer>().color == Color.black)
                {
                    if (Boardpieces[width, height].moveFlag)
                    {
                        for (int height2 = 0; height2 < BOARD_HEIGHT_NUM; height2++)
                        {
                            for (int width2 = 0; width2 < BOARD_WIDTH_NUM; width2++)
                            {
                                if (Boardpieces[width2,height2].mouseFlag)
                                {

                                    PANEL_DATA save = Boardpieces[width2,height2];
                                    Boardpieces[width2, height2] = Boardpieces[width,height];
                                    Boardpieces[width2, height2].obj.GetComponent<Transform>().position = Boards[width2, height2].GetComponent<Transform>().position;
                                    Boardpieces[width,height] = save;


                                    for (int height3 = 0; height3 < BOARD_HEIGHT_NUM; height3++)
                                    {
                                        for (int width3 = 0; width3 < BOARD_WIDTH_NUM; width3++)
                                        {
                                            Boards[width3,height3].GetComponent<SpriteRenderer>().color = Color.magenta;
                                        }
                                    }
                                    SetMovepiece();
                                    break;
                                }
                            }
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

        for (int height = 0; height < BOARD_HEIGHT_NUM; height++)
        {
            for (int width = 0; width < BOARD_WIDTH_NUM; width++)
            {
                if (Boards[width,height].GetComponent<SpriteRenderer>().color == Gray)
                {
                    Boardpieces[width,height].obj.GetComponent<Piece>().Big();
                    Boardpieces[width,height].obj.GetComponent<SpriteRenderer>().sortingOrder = 1;
                    Boardpieces[width,height].mouseFlag = true;
                    Boards[width,height].GetComponent<SpriteRenderer>().color = Color.magenta;
                    SetMovepiece();
                    break;
                }
            }
        }
    }


    //-------------------------------------------------------
    // マウスが持っていたオブジェクトの解放
    //-------------------------------------------------------
    public void ReleaseMouseObj()
    {
        // ボードカラー初期化
        for (int height = 0; height < BOARD_HEIGHT_NUM; height++)
        {
            for (int width = 0; width < BOARD_WIDTH_NUM; width++)
            {
                Boards[width, height].GetComponent<SpriteRenderer>().color = Color.magenta;
            }
        }

        for (int height = 0; height < BOARD_HEIGHT_NUM; height++)
        {
            for (int width = 0; width < BOARD_WIDTH_NUM; width++)
            {
                Boardpieces[width, height].obj.GetComponent<Transform>().position = Boards[width, height].GetComponent<Transform>().position;
                Boardpieces[width, height].obj.GetComponent<SpriteRenderer>().sortingOrder = 0;
                Boardpieces[width, height].mouseFlag = false;
            }
        }
    }

    //-------------------------------------------------------
    // パズルのリンク
    //-------------------------------------------------------

}
