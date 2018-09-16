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

        SetPieceLinkDisplay(); //リンクに応じて表示形式と色を変える
    }

    //=============================================================
    private void CRef () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        linkV = transform.Find("V").gameObject;
        linkH = transform.Find("H").gameObject;
        linkO = transform.Find("O").gameObject;
    }

    //=============================================================
    private void Awake () {
        Init();
    }

    //=============================================================
    //ピースリンクのイメージのコンポーネントを取得する
    public List<Image> GetPieceLinkImageComponent () {
        List<Image> objs = new List<Image>();
        if(base.IsActivePieceLinkImage(linkV,"U")) {
            objs.Add(base.RefPieceLinkImage(linkV,"U").GetComponent<Image>());
        }

        if(base.IsActivePieceLinkImage(linkV,"D")) {
            objs.Add(base.RefPieceLinkImage(linkV,"D").GetComponent<Image>());
        }

        if(base.IsActivePieceLinkImage(linkH,"L")) {
            objs.Add(base.RefPieceLinkImage(linkH,"L").GetComponent<Image>());
        }

        if(base.IsActivePieceLinkImage(linkH,"R")) {
            objs.Add(base.RefPieceLinkImage(linkH,"R").GetComponent<Image>());
        }

        if(base.IsActivePieceLinkImage(linkO,"LU")) {
            objs.Add(base.RefPieceLinkImage(linkO,"LU").GetComponent<Image>());
        }

        if(base.IsActivePieceLinkImage(linkO,"LD")) {
            objs.Add(base.RefPieceLinkImage(linkO,"LD").GetComponent<Image>());
        }

        if(base.IsActivePieceLinkImage(linkO,"RU")) {
            objs.Add(base.RefPieceLinkImage(linkO,"RU").GetComponent<Image>());
        }

        if(base.IsActivePieceLinkImage(linkO,"RD")) {
            objs.Add(base.RefPieceLinkImage(linkO,"RD").GetComponent<Image>());
        }

        return objs;
    }

    //=============================================================
    //ピースリンクに画像を設定する
    private void SetPieceLinkImage (GameObject obj,string path,int x,int y) {
        obj.transform.Find(path).gameObject.SetActive(true);
        obj.transform.Find(path).GetComponent<Image>().sprite = gameManager.PieceLinkImage[PieceLink[x,y]];
    }

    //=============================================================
    //リンクに応じて表示形式と色を変える
    private void SetPieceLinkDisplay () {
        linkV.SetActive(false);
        linkH.SetActive(false);
        linkO.SetActive(false);

        switch(pieceLinkType) {
            case GameManager.PIECE_LINK_TYPE.V:
            linkV.SetActive(true);
            SetPieceLinkImage(linkV,"U",0,0);
            SetPieceLinkImage(linkV,"D",0,1);
            break;

            case GameManager.PIECE_LINK_TYPE.H:
            linkH.SetActive(true);
            SetPieceLinkImage(linkH,"L",0,0);
            SetPieceLinkImage(linkH,"R",1,0);
            break;

            case GameManager.PIECE_LINK_TYPE.O:
            linkO.SetActive(true);

            if(PieceLink[0,0] != -1) {
                SetPieceLinkImage(linkO,"LU",0,0);
            } else {
                linkO.transform.Find("LU").gameObject.SetActive(false);
            }

            if(PieceLink[1,0] != -1) {
                SetPieceLinkImage(linkO,"RU",1,0);
            } else {
                linkO.transform.Find("RU").gameObject.SetActive(false);
            }

            if(PieceLink[0,1] != -1) {
                SetPieceLinkImage(linkO,"LD",0,1);
            } else {
                linkO.transform.Find("LD").gameObject.SetActive(false);
            }

            if(PieceLink[1,1] != -1) {
                SetPieceLinkImage(linkO,"RD",1,1);
            } else {
                linkO.transform.Find("RD").gameObject.SetActive(false);
            }

            break;

            default:
            Debug.Log("謎のピースリンクが設定されてるよ");
            break;
        }
    }
}