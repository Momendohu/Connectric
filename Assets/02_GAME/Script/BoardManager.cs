using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-200)]
public class BoardManager : MonoBehaviour {

    // 定数
    public const int BOARD_ALL_NUM = 25;
    public const int BOARD_WIDTH_NUM = 5;
    public const int BOARD_HEIGHT_NUM = 5;
    public const float between = 1.85f;
    public Vector3 vStartPos = new Vector3(-3.7f, 1.2f, 0.0f);

    public const float DEBUG_COLOR = 0.0f;

    // ピースタイプ
    public enum INSTRUMENT_TYPE {
        GUITAR = 0,
        DRUM,
        VOCAL,
        DJ,
        TIME,
        MAX
    };

    // リンクターゲットタイプ
    public enum TARGET_FORM {
        TATE = 0,
        YOKO,
        MAX
    }

    // 構造体
    public struct PANEL_DATA {
        public GameObject obj;       // 
        public int arrayWidthNum;    // 配列番号
        public int arrayHeightNum;   // 配列番号
        public int typeNum;          // 属性
        public bool mouseFlag;       // キャプチャーされているか
        public bool moveFlag;        // 動かせるか
        public bool linkflag;        // リンクしているかの確認
        public bool deletePrepareFrag;
    };

    // 変数
    public static GameObject[,] Boards = new GameObject[BOARD_WIDTH_NUM, BOARD_HEIGHT_NUM];
    public static PANEL_DATA[,] Boardpieces = new PANEL_DATA[BOARD_WIDTH_NUM, BOARD_HEIGHT_NUM];

    private List<GameObject> linkFlames = new List<GameObject>();

    [SerializeField] private GameObject board;
    [SerializeField] private GameObject linkFlame;
    [SerializeField] private GameObject[] piece = new GameObject[(int)INSTRUMENT_TYPE.MAX];
    [SerializeField] private bool[] flag = new bool[BOARD_ALL_NUM];
    [SerializeField] private int[,] Target = new int[2, 2];       // リンクテスト
    [SerializeField] private bool skill = false;

    private GameObject game_manager;
    private GameObject mouse;
    private GameObject sound;


    [SerializeField] private int combo = 0;                       // コンボ数
    public int Combo
    {
        get { return combo; }
        set { combo = value; }
    }
    


    //===================================================
    // Use this for initialization
    //===================================================
    void Start() {

        CreateBoard();
        game_manager = GameObject.Find("GameManager");
        mouse = GameObject.Find("Mouse");
        sound = GameObject.Find("SoundManager");
    }

    //===================================================
    // Update is called once per frame
    //===================================================
    void Update() {


        MoveMausePiece();

        // 削除準備
        if (game_manager.GetComponent<GameManager>().IsBeatChange)
        {

            for (int height = 0; height < BOARD_HEIGHT_NUM; height++)
            {
                for (int width = 0; width < BOARD_WIDTH_NUM; width++)
                {
                    if (Boardpieces[width, height].linkflag)
                    {
                        Boardpieces[width, height].deletePrepareFrag = true;
                        Boardpieces[width, height].obj.GetComponent<Piece>().SmallFrag = true;
                        Debug.Log("削除準備");
                    }
                }
            }
            LinkFlameDelete();      // リンク背景削除
            game_manager.GetComponent<GameManager>().IsBeatChange = false;

        }

        // 小さくする＆ピースを消す(演出)
        for (int height = 0; height < BOARD_HEIGHT_NUM; height++)
        {
            for (int width = 0; width < BOARD_WIDTH_NUM; width++)
            {
                if (Boardpieces[width, height].deletePrepareFrag)
                {
                    Boardpieces[width, height].obj.GetComponent<Piece>().Small();
                    if(Boardpieces[width, height].obj.GetComponent<Piece>().DeleteFrag)
                    {
                        PieceDelete(width, height);
                    }
                }
            }
        }



        if (skill)
        {
            SkillActiveTime();
        }


        LinkDo();
        Replenishment();    // 補充

    }

    //-------------------------------------------------------
    // 配置ボードの生成
    //-------------------------------------------------------
    private void CreateBoard() {
        for (int height = 0; height < BOARD_HEIGHT_NUM; height++) {
            for (int width = 0; width < BOARD_WIDTH_NUM; width++) {
                int obj_num = UnityEngine.Random.Range(0, (int)INSTRUMENT_TYPE.MAX - 1);

                // ボード（あたり判定）
                Boards[width, height] = Instantiate(board, new Vector3(vStartPos.x + between * width, vStartPos.y - between * height, 0.0f), Quaternion.identity);

                // ピース
                Boardpieces[width, height].obj = Instantiate(piece[obj_num], new Vector3(vStartPos.x + between * width, vStartPos.y - between * height, 0.0f), Quaternion.identity);
                Boardpieces[width, height].arrayWidthNum = width;
                Boardpieces[width, height].arrayHeightNum = height;
                Boardpieces[width, height].typeNum = obj_num;
                Boardpieces[width, height].mouseFlag = false;
                Boardpieces[width, height].moveFlag = false;
                Boardpieces[width, height].linkflag = false;
                Boardpieces[width, height].deletePrepareFrag  = false;

                // デバッグ用
                flag[width + height * 5] = Boardpieces[width, height].moveFlag;
            }
        }


    }

    //-------------------------------------------------------
    // 入れ替え可能ピースの設定
    //-------------------------------------------------------
    private void SetMovepiece() {

        Color cyan = new Color(0.0f, 1.0f, 1.0f, DEBUG_COLOR);

        //念のため初期化
        for (int height = 0; height < BOARD_HEIGHT_NUM; height++) {
            for (int width = 0; width < BOARD_WIDTH_NUM; width++) {
                Boardpieces[width, height].moveFlag = false;
            }
        }

        // 入れ替え
        for (int height = 0; height < BOARD_HEIGHT_NUM; height++) {
            for (int width = 0; width < BOARD_WIDTH_NUM; width++) {
                if (Boardpieces[width, height].mouseFlag) {
                    // 左上角
                    if (width == 0 && height == 0) {
                        Boardpieces[1, 0].moveFlag = true;
                        Boardpieces[0, 1].moveFlag = true;

                        // デバッグ用
                        Boards[1, 0].GetComponent<SpriteRenderer>().color = cyan;
                        Boards[0, 1].GetComponent<SpriteRenderer>().color = cyan;

                        // 斜めも対応
                        Boardpieces[1, 1].moveFlag = true;
                        Boards[1, 1].GetComponent<SpriteRenderer>().color = cyan;
                    }

                    // 右上角
                    else if (width == (BOARD_WIDTH_NUM - 1) && height == 0) {
                        Boardpieces[BOARD_WIDTH_NUM - 2, 0].moveFlag = true;
                        Boardpieces[BOARD_WIDTH_NUM - 1, 1].moveFlag = true;

                        // デバッグ用
                        Boards[BOARD_WIDTH_NUM - 2, 0].GetComponent<SpriteRenderer>().color = cyan;
                        Boards[BOARD_WIDTH_NUM - 1, 1].GetComponent<SpriteRenderer>().color = cyan;

                        // 斜めも対応
                        Boardpieces[BOARD_WIDTH_NUM - 2, 1].moveFlag = true;
                        Boards[BOARD_WIDTH_NUM - 2, 1].GetComponent<SpriteRenderer>().color = cyan;

                    }

                    // 左下角
                    else if (width == 0 && height == (BOARD_HEIGHT_NUM - 1)) {
                        Boardpieces[0, BOARD_HEIGHT_NUM - 2].moveFlag = true;
                        Boardpieces[1, BOARD_HEIGHT_NUM - 1].moveFlag = true;

                        // デバッグ用
                        Boards[0, BOARD_HEIGHT_NUM - 2].GetComponent<SpriteRenderer>().color = cyan;
                        Boards[1, BOARD_HEIGHT_NUM - 1].GetComponent<SpriteRenderer>().color = cyan;

                        // 斜めも対応
                        Boardpieces[1, BOARD_HEIGHT_NUM - 2].moveFlag = true;
                        Boards[1, BOARD_HEIGHT_NUM - 2].GetComponent<SpriteRenderer>().color = cyan;

                    }

                    // 右下角
                    else if (width == (BOARD_WIDTH_NUM - 1) && height == (BOARD_HEIGHT_NUM - 1)) {
                        Boardpieces[BOARD_WIDTH_NUM - 2, BOARD_HEIGHT_NUM - 1].moveFlag = true;
                        Boardpieces[BOARD_WIDTH_NUM - 1, BOARD_HEIGHT_NUM - 2].moveFlag = true;

                        // デバッグ用
                        Boards[BOARD_WIDTH_NUM - 2, BOARD_HEIGHT_NUM - 1].GetComponent<SpriteRenderer>().color = cyan;
                        Boards[BOARD_WIDTH_NUM - 1, BOARD_HEIGHT_NUM - 2].GetComponent<SpriteRenderer>().color = cyan;

                        // 斜めも対応
                        Boardpieces[BOARD_WIDTH_NUM - 2, BOARD_HEIGHT_NUM - 2].moveFlag = true;
                        Boards[BOARD_WIDTH_NUM - 2, BOARD_HEIGHT_NUM - 2].GetComponent<SpriteRenderer>().color = cyan;
                    }

                    // 上列
                    else if ((width != 0 && height == 0) || (width != (BOARD_WIDTH_NUM - 1) && height == 0)) {
                        Boardpieces[width - 1, height].moveFlag = true;
                        Boardpieces[width + 1, height].moveFlag = true;
                        Boardpieces[width, height + 1].moveFlag = true;

                        // デバッグ用
                        Boards[width - 1, height].GetComponent<SpriteRenderer>().color = cyan;
                        Boards[width + 1, height].GetComponent<SpriteRenderer>().color = cyan;
                        Boards[width, height + 1].GetComponent<SpriteRenderer>().color = cyan;

                        // 斜めも対応
                        Boardpieces[width - 1, height + 1].moveFlag = true;
                        Boardpieces[width + 1, height + 1].moveFlag = true;
                        Boards[width - 1, height + 1].GetComponent<SpriteRenderer>().color = cyan;
                        Boards[width + 1, height + 1].GetComponent<SpriteRenderer>().color = cyan;
                    }

                    // 下列
                    else if ((width != 0 && height == (BOARD_HEIGHT_NUM - 1)) || (width != (BOARD_WIDTH_NUM - 1) && height == (BOARD_HEIGHT_NUM - 1))) {
                        Boardpieces[width - 1, height].moveFlag = true;
                        Boardpieces[width + 1, height].moveFlag = true;
                        Boardpieces[width, height - 1].moveFlag = true;

                        // デバッグ用
                        Boards[width - 1, height].GetComponent<SpriteRenderer>().color = cyan;
                        Boards[width + 1, height].GetComponent<SpriteRenderer>().color = cyan;
                        Boards[width, height - 1].GetComponent<SpriteRenderer>().color = cyan;

                        // 斜めも対応
                        Boardpieces[width - 1, height - 1].moveFlag = true;
                        Boardpieces[width + 1, height - 1].moveFlag = true;
                        Boards[width - 1, height - 1].GetComponent<SpriteRenderer>().color = cyan;
                        Boards[width + 1, height - 1].GetComponent<SpriteRenderer>().color = cyan;
                    }

                    // 左列
                    else if ((width == 0 && height != 0) || (width == 0 && height == (BOARD_HEIGHT_NUM - 1))) {
                        Boardpieces[width, height - 1].moveFlag = true;
                        Boardpieces[width, height + 1].moveFlag = true;
                        Boardpieces[width + 1, height].moveFlag = true;

                        // デバッグ用
                        Boards[width, height - 1].GetComponent<SpriteRenderer>().color = cyan;
                        Boards[width, height + 1].GetComponent<SpriteRenderer>().color = cyan;
                        Boards[width + 1, height].GetComponent<SpriteRenderer>().color = cyan;

                        // 斜めも対応
                        Boardpieces[width + 1, height - 1].moveFlag = true;
                        Boardpieces[width + 1, height + 1].moveFlag = true;
                        Boards[width + 1, height - 1].GetComponent<SpriteRenderer>().color = cyan;
                        Boards[width + 1, height + 1].GetComponent<SpriteRenderer>().color = cyan;
                    }

                    // 右列
                    else if ((width == (BOARD_WIDTH_NUM - 1) && height != 0) || (width == (BOARD_WIDTH_NUM - 1) && height == (BOARD_HEIGHT_NUM - 1))) {
                        Boardpieces[width, height - 1].moveFlag = true;
                        Boardpieces[width, height + 1].moveFlag = true;
                        Boardpieces[width - 1, height].moveFlag = true;

                        // デバッグ用
                        Boards[width, height - 1].GetComponent<SpriteRenderer>().color = cyan;
                        Boards[width, height + 1].GetComponent<SpriteRenderer>().color = cyan;
                        Boards[width - 1, height].GetComponent<SpriteRenderer>().color = cyan;

                        // 斜めも対応
                        Boardpieces[width - 1, height - 1].moveFlag = true;
                        Boardpieces[width - 1, height + 1].moveFlag = true;
                        Boards[width - 1, height - 1].GetComponent<SpriteRenderer>().color = cyan;
                        Boards[width - 1, height + 1].GetComponent<SpriteRenderer>().color = cyan;
                    }

                    // 四か所できる
                    else {
                        Boardpieces[width - 1, height].moveFlag = true;
                        Boardpieces[width + 1, height].moveFlag = true;
                        Boardpieces[width, height - 1].moveFlag = true;
                        Boardpieces[width, height + 1].moveFlag = true;

                        // デバッグ用
                        Boards[width - 1, height].GetComponent<SpriteRenderer>().color = cyan;
                        Boards[width + 1, height].GetComponent<SpriteRenderer>().color = cyan;
                        Boards[width, height - 1].GetComponent<SpriteRenderer>().color = cyan;
                        Boards[width, height + 1].GetComponent<SpriteRenderer>().color = cyan;

                        // 斜めの対応
                        Boardpieces[width - 1, height - 1].moveFlag = true;
                        Boardpieces[width + 1, height - 1].moveFlag = true;
                        Boardpieces[width - 1, height + 1].moveFlag = true;
                        Boardpieces[width + 1, height + 1].moveFlag = true;
                        Boards[width - 1, height - 1].GetComponent<SpriteRenderer>().color = cyan;
                        Boards[width + 1, height - 1].GetComponent<SpriteRenderer>().color = cyan;
                        Boards[width - 1, height + 1].GetComponent<SpriteRenderer>().color = cyan;
                        Boards[width + 1, height + 1].GetComponent<SpriteRenderer>().color = cyan;
                    }

                }
            }
        }
    }

    //-------------------------------------------------------
    // マウスが持っているオブジェクトを動かす
    //-------------------------------------------------------
    private void MoveMausePiece() {

        for (int height = 0; height < BOARD_HEIGHT_NUM; height++) {
            for (int width = 0; width < BOARD_WIDTH_NUM; width++) {
                if (Boardpieces[width, height].mouseFlag) {
                    Boardpieces[width, height].obj.GetComponent<Transform>().position = mouse.GetComponent<Mouse>().CursolWorldPos;
                    break;
                }
            }
        }

    }

    //-------------------------------------------------------
    //  ピースの入れ替え
    //-------------------------------------------------------
    public void Change() {

        for (int height = 0; height < BOARD_HEIGHT_NUM; height++) {
            for (int width = 0; width < BOARD_WIDTH_NUM; width++) {
                //マウスと当たったやつの検索
                if (Boards[width, height].GetComponent<SpriteRenderer>().color == new Color(0.0f, 0.0f, 0.0f, DEBUG_COLOR)) {
                    if (Boardpieces[width, height].moveFlag) {
                        for (int height2 = 0; height2 < BOARD_HEIGHT_NUM; height2++) {
                            for (int width2 = 0; width2 < BOARD_WIDTH_NUM; width2++) {
                                if (Boardpieces[width2, height2].mouseFlag) {

                                    PANEL_DATA save = Boardpieces[width2, height2];
                                    Boardpieces[width2, height2] = Boardpieces[width, height];
                                    Boardpieces[width2, height2].obj.GetComponent<Transform>().position = Boards[width2, height2].GetComponent<Transform>().position;
                                    Boardpieces[width, height] = save;


                                    // パネル入れかえサウンド
                                    sound.GetComponent<SoundManager>().TriggerSE("puzzlemove");


                                    for (int height3 = 0; height3 < BOARD_HEIGHT_NUM; height3++) {
                                        for (int width3 = 0; width3 < BOARD_WIDTH_NUM; width3++) {
                                            Boards[width3, height3].GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 1.0f, DEBUG_COLOR);
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
    public void SetMouseObj() {
        Color Gray = new Color(0.5f, 0.5f, 0.5f, DEBUG_COLOR);

        for (int height = 0; height < BOARD_HEIGHT_NUM; height++) {
            for (int width = 0; width < BOARD_WIDTH_NUM; width++) {
                if (Boards[width, height].GetComponent<SpriteRenderer>().color == Gray) {
                    if (Boardpieces[width, height].typeNum == (int)INSTRUMENT_TYPE.TIME) {
                        Boardpieces[width, height].obj.GetComponent<PieceTime>().Big();
                    } else {
                        Boardpieces[width, height].obj.GetComponent<Piece>().Big();
                    }


                    Boardpieces[width, height].obj.GetComponent<SpriteRenderer>().sortingOrder = 101;
                    Boardpieces[width, height].mouseFlag = true;
                    Boards[width, height].GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 1.0f, DEBUG_COLOR);
                    SetMovepiece();
                    break;
                }
            }
        }
    }

    //-------------------------------------------------------
    // マウスが持っていたオブジェクトの解放
    //-------------------------------------------------------
    public void ReleaseMouseObj() {
        // ボードカラー初期化
        for (int height = 0; height < BOARD_HEIGHT_NUM; height++) {
            for (int width = 0; width < BOARD_WIDTH_NUM; width++) {
                Boards[width, height].GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 1.0f, DEBUG_COLOR);
            }
        }

        for (int height = 0; height < BOARD_HEIGHT_NUM; height++) {
            for (int width = 0; width < BOARD_WIDTH_NUM; width++) {
                if (Boardpieces[width, height].mouseFlag) {
                    Boardpieces[width, height].obj.GetComponent<Transform>().position = Boards[width, height].GetComponent<Transform>().position;
                    Boardpieces[width, height].obj.GetComponent<SpriteRenderer>().sortingOrder = 100;
                    Boardpieces[width, height].mouseFlag = false;
                    break;
                }

            }
        }
    }

    //-------------------------------------------------------
    // リンクチェックの実行
    //-------------------------------------------------------
    private void LinkDo() {
        // ノーツリンクの取得
        Target = game_manager.GetComponent<GameManager>().GetLatestPieceLink();

        Debug.Log(Target[0, 0]);
        Debug.Log(Target[0, 1]);
        Debug.Log(Target[1, 0]);
        Debug.Log(Target[1, 1]);


        // 念のため初期化
        for (int height = 0; height < BOARD_HEIGHT_NUM; height++) {
            for (int width = 0; width < BOARD_WIDTH_NUM; width++) {
                Boardpieces[width, height].linkflag = false;
            }
        }

        LinkFlameDelete();

        combo = Link();
    }

    //-------------------------------------------------------
    // リンクチェック
    //-------------------------------------------------------
    private int Link() {
        int linknum = 0;

        for (int height = 0; height < BOARD_HEIGHT_NUM; height++) {
            for (int width = 0; width < BOARD_WIDTH_NUM; width++) {
                // 左上の一致
                if (Boardpieces[width, height].typeNum == Target[0, 0]) {
                    //if(!Boardpieces[width, height].mouseFlag)
                    {
                        // 縦と判断
                        if (Target[1, 0] == -1 && Target[1, 1] == -1) {
                            if (height == (BOARD_HEIGHT_NUM - 1)) { continue; }
                            if (Boardpieces[width, height + 1].typeNum == Target[0, 1]) {
                                //if(!Boardpieces[width, height + 1].mouseFlag) { continue; }
                                linknum++;
                                Boardpieces[width, height].linkflag = true;
                                Boardpieces[width, height + 1].linkflag = true;

                                linkFlames.Add(CreateLinkFlame_t(width, height));
                                continue;
                            }

                        }
                        // 横と判断
                        else if (Target[0, 1] == -1 && Target[1, 1] == -1) {

                            if (width == (BOARD_WIDTH_NUM - 1)) { continue; }
                            if (Boardpieces[width + 1, height].typeNum == Target[1, 0]) {
                                //if (!Boardpieces[width + 1, height].mouseFlag) { continue; }
                                linknum++;
                                Boardpieces[width, height].linkflag = true;
                                Boardpieces[width + 1, height].linkflag = true;

                                linkFlames.Add(CreateLinkFlame_y(width, height));
                                continue;
                            }
                        }
                    }
                }
            }
        }
        return linknum;
    }

    //-------------------------------------------------------
    // リンク削除
    //-------------------------------------------------------
    private void PieceDelete(int width, int height) {

        Destroy(Boardpieces[width, height].obj);

        // パネル削除
        sound.GetComponent<SoundManager>().TriggerSE("puzzledelete");


        Boardpieces[width, height].obj = Instantiate(piece[(int)INSTRUMENT_TYPE.TIME], new Vector3(vStartPos.x + between * width, vStartPos.y - between * height, 0.0f), Quaternion.identity);
        Boardpieces[width, height].arrayWidthNum = width;
        Boardpieces[width, height].arrayHeightNum = height;
        Boardpieces[width, height].typeNum = (int)INSTRUMENT_TYPE.TIME;
        Boardpieces[width, height].mouseFlag = false;
        Boardpieces[width, height].moveFlag = false;
        Boardpieces[width, height].linkflag = false;
        Boardpieces[width, height].deletePrepareFrag = false;
        
    }

    //-------------------------------------------------------
    // 補充
    //-------------------------------------------------------
    private void Replenishment() {
        for (int height = 0; height < BOARD_HEIGHT_NUM; height++) {
            for (int width = 0; width < BOARD_WIDTH_NUM; width++) {
                if (Boardpieces[width, height].typeNum == (int)INSTRUMENT_TYPE.TIME) {
                    if (Boardpieces[width, height].obj.GetComponent<PieceTime>().FinAnim) {
                        Destroy(Boardpieces[width, height].obj);

                        int obj_num = UnityEngine.Random.Range(0, (int)INSTRUMENT_TYPE.MAX - 1);

                        // ピース
                        Boardpieces[width, height].obj = Instantiate(piece[obj_num], new Vector3(vStartPos.x + between * width, vStartPos.y - between * height, 0.0f), Quaternion.identity);
                        Boardpieces[width, height].arrayWidthNum = width;
                        Boardpieces[width, height].arrayHeightNum = height;
                        Boardpieces[width, height].typeNum = obj_num;
                        Boardpieces[width, height].mouseFlag = false;
                        Boardpieces[width, height].moveFlag = false;
                        Boardpieces[width, height].linkflag = false;
                        Boardpieces[width, height].deletePrepareFrag = false;
                    }
                }
            }
        }
    }

    //-------------------------------------------------------
    // リンクフレーム（縦）
    //-------------------------------------------------------
    private GameObject CreateLinkFlame_t(int width, int height)
    {
        GameObject link_flame;
        Vector3 linkFlamePos = Boards [width, height].GetComponent<Transform>().position;
        linkFlamePos.y -= 1.0f;
        link_flame = Instantiate(linkFlame, linkFlamePos, Quaternion.identity);
        link_flame.GetComponent<SpriteRenderer>().sortingOrder = 1 + linkFlames.Count;
        return link_flame;
    }

    //-------------------------------------------------------
    // リンクフレーム（横）
    //-------------------------------------------------------
    private GameObject CreateLinkFlame_y(int width, int height)
    {
        GameObject link_flame;
        Vector3 linkFlamePos = Boards[width, height].GetComponent<Transform>().position;
        linkFlamePos.x += 1.0f;
        Quaternion linkFlameRot = Quaternion.Euler(0.0f, 0.0f, 90.0f);
        link_flame = Instantiate(linkFlame, linkFlamePos, linkFlameRot);
        link_flame.GetComponent<SpriteRenderer>().sortingOrder = 1 + linkFlames.Count;
        return link_flame;
    }

    //-------------------------------------------------------
    // リンクフレーム削除
    //-------------------------------------------------------
    private void LinkFlameDelete()
    {
        for (int i = 0; i < linkFlames.Count; i++)
        {
            Destroy(linkFlames[i]);
        }
        linkFlames.Clear();
    }

    //-------------------------------------------------------
    // スキル発動（タイム）
    //-------------------------------------------------------
    private void SkillActiveTime()
    {
        Debug.Log("スキル発動");
        for (int height = 0; height < BOARD_HEIGHT_NUM; height++)
        {
            for (int width = 0; width < BOARD_WIDTH_NUM; width++)
            {
                if (Boardpieces[width, height].typeNum == (int)INSTRUMENT_TYPE.TIME)
                {
                    Boardpieces[width, height].obj.GetComponent<PieceTime>().FinAnim = true;
                    Replenishment();
                }
            }
        }
    }
}
