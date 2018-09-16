using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieceLink_UpScreen : PieceLinkUI {
    //=============================================================
    private GameManager gameManager;

    private GameObject linkV;
    private GameObject linkH;
    private GameObject linkO;

    //=============================================================
    private GameManager.PIECE_LINK_TYPE pieceLinkType = GameManager.PIECE_LINK_TYPE.V;

    //ピースリンク
    private int[,] pieceLink = new int[2,2];
    public int[,] PieceLink {
        get { return pieceLink; }
    }

    //=============================================================
    private void Init () {
        CRef();

        base.DecidePieceLink(pieceLink); //ピースリンクの情報の決定
        base.SelectPieceLinkType(pieceLink,ref pieceLinkType); //ピースリンクのタイプの選択

        //リンクに応じて表示形式と色を変える
        linkV.SetActive(false);
        linkH.SetActive(false);
        linkO.SetActive(false);

        switch(pieceLinkType) {
            case GameManager.PIECE_LINK_TYPE.V:
            linkV.SetActive(true);
            SetPieceLinkImage(linkV,"Piece_UpScreenU",0,0);
            SetPieceLinkImage(linkV,"Piece_UpScreenD",0,1);
            break;

            case GameManager.PIECE_LINK_TYPE.H:
            linkH.SetActive(true);
            SetPieceLinkImage(linkH,"Piece_UpScreenL",0,0);
            SetPieceLinkImage(linkH,"Piece_UpScreenR",1,0);
            break;

            case GameManager.PIECE_LINK_TYPE.O:
            linkO.SetActive(true);
            SetPieceLinkImage(linkO,"Piece_UpScreenLU",0,0);

            if(PieceLink[1,0] != -1) {
                SetPieceLinkImage(linkO,"Piece_UpScreenRU",1,0);
            } else {
                linkO.transform.Find("Piece_UpScreenRU").gameObject.SetActive(false);
            }

            if(PieceLink[0,1] != -1) {
                SetPieceLinkImage(linkO,"Piece_UpScreenLD",0,1);
            } else {
                linkO.transform.Find("Piece_UpScreenLD").gameObject.SetActive(false);
            }

            if(PieceLink[1,1] != -1) {
                SetPieceLinkImage(linkO,"Piece_UpScreenRD",1,1);
            } else {
                linkO.transform.Find("Piece_UpScreenRD").gameObject.SetActive(false);
            }

            break;

            default:
            Debug.Log("謎のピースリンクが設定されてるよ");
            break;
        }
    }

    //=============================================================
    //ピースリンクに画像を設定する
    private void SetPieceLinkImage (GameObject obj,string path,int x,int y) {
        obj.transform.Find(path).GetComponent<Image>().sprite = gameManager.PieceLinkImage[PieceLink[x,y]];
    }

    //=============================================================
    private void CRef () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        linkV = transform.Find("PieceLinkV_UpScreen").gameObject;
        linkH = transform.Find("PieceLinkH_UpScreen").gameObject;
        linkO = transform.Find("PieceLinkO_UpScreen").gameObject;
    }

    //=============================================================
    private void Awake () {
        Init();
    }
}