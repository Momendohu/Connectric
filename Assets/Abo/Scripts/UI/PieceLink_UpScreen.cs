using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieceLink_UpScreen : MonoBehaviour {
    //=============================================================
    private GameObject linkV;
    private GameObject linkH;
    private GameObject linkO;

    //=============================================================
    public enum PIECE_LINK_TYPE {
        V = 0,
        H = 1,
        O = 2,
    }

    //=============================================================
    private PIECE_LINK_TYPE pieceLinkType = PIECE_LINK_TYPE.V;

    //ピースリンク
    private int[,] pieceLink = new int[2,2];
    public int[,] PieceLink {
        get { return pieceLink; }
    }

    //=============================================================
    public Sprite[] Image;

    //=============================================================
    private void Init () {
        CRef();

        DecidePieceLink(); //ピースリンクの情報の決定
        SelectPieceLinkType(pieceLink); //ピースリンクのタイプの選択

        //リンクに応じて表示形式と色を変える
        linkV.SetActive(false);
        linkH.SetActive(false);
        linkO.SetActive(false);

        switch(pieceLinkType) {
            case PIECE_LINK_TYPE.V:
            linkV.SetActive(true);
            linkV.transform.Find("Piece_UpScreenU").GetComponent<Image>().sprite = Image[PieceLink[0,0]];
            linkV.transform.Find("Piece_UpScreenD").GetComponent<Image>().sprite = Image[PieceLink[0,1]];
            break;

            case PIECE_LINK_TYPE.H:
            linkH.SetActive(true);
            linkH.transform.Find("Piece_UpScreenL").GetComponent<Image>().sprite = Image[PieceLink[0,0]];
            linkH.transform.Find("Piece_UpScreenR").GetComponent<Image>().sprite = Image[PieceLink[1,0]];
            break;

            case PIECE_LINK_TYPE.O:
            linkO.SetActive(true);
            linkV.transform.Find("Piece_UpScreenLU").GetComponent<Image>().sprite = Image[PieceLink[0,0]];
            linkV.transform.Find("Piece_UpScreenRU").GetComponent<Image>().sprite = Image[PieceLink[1,0]];
            linkV.transform.Find("Piece_UpScreenLD").GetComponent<Image>().sprite = Image[PieceLink[0,1]];
            linkV.transform.Find("Piece_UpScreenRD").GetComponent<Image>().sprite = Image[PieceLink[1,1]];
            break;

            default:
            Debug.Log("謎のピースリンクが設定されてるよ");
            break;
        }
    }

    //=============================================================
    private void CRef () {
        linkV = transform.Find("PieceLinkV_UpScreen").gameObject;
        linkH = transform.Find("PieceLinkH_UpScreen").gameObject;
        linkO = transform.Find("PieceLinkO_UpScreen").gameObject;
    }

    //=============================================================
    private void Awake () {
        Init();
    }

    private void Start () {

    }


    private void Update () {

    }

    //=============================================================
    //ピースリンクのタイプの選択
    public void SelectPieceLinkType (int[,] pieceLink) {
        //左下がないなら横長タイプ
        if(pieceLink[0,1] == -1) {
            pieceLinkType = PIECE_LINK_TYPE.H;
            return;
        }

        //右上がないなら縦長タイプ
        if(pieceLink[1,0] == -1) {
            pieceLinkType = PIECE_LINK_TYPE.V;
            return;
        }

        //そのほかは四角タイプ(またはLタイプ)
        pieceLinkType = PIECE_LINK_TYPE.O;
    }

    //=============================================================
    //ピースリンクの決定
    private void DecidePieceLink () {
        //初期状態
        for(int i = 0;i < pieceLink.GetLength(0);i++) {
            for(int j = 0;j < pieceLink.GetLength(1);j++) {
                pieceLink[i,j] = -1;
            }
        }
        //左上
        pieceLink[0,0] = Random.Range(0,4);

        int branch1 = Random.Range(0,2);
        int branch2 = 0/*Random.Range(0,3)*/;
        int branch3 = 0/*Random.Range(0,2)*/;

        if(branch1 == 0) {
            //右上
            pieceLink[1,0] = Random.Range(0,4);

            switch(branch2) {
                case 0:
                //終了
                return;

                case 1:
                //左下
                pieceLink[0,1] = Random.Range(0,4);

                if(branch3 == 0) {
                    //終了
                    return;
                } else {
                    //右下
                    pieceLink[1,1] = Random.Range(0,4);
                }

                break;

                case 2:
                //右下
                pieceLink[1,1] = Random.Range(0,4);

                if(branch3 == 0) {
                    //終了
                    return;
                } else {
                    //左下
                    pieceLink[0,1] = Random.Range(0,4);
                }
                break;
            }
        } else {
            //左下
            pieceLink[0,1] = Random.Range(0,4);

            switch(branch2) {
                case 0:
                //終了
                return;

                case 1:
                //右上
                pieceLink[1,0] = Random.Range(0,4);

                if(branch3 == 0) {
                    //終了
                    return;
                } else {
                    //右下
                    pieceLink[1,1] = Random.Range(0,4);
                }

                break;

                case 2:
                //右下
                pieceLink[1,1] = Random.Range(0,4);

                if(branch3 == 0) {
                    //終了
                    return;
                } else {
                    //右上
                    pieceLink[1,0] = Random.Range(0,4);
                }
                break;
            }
        }
    }
}