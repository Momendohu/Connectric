﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextFrame : PieceLinkUI {
    //=============================================================
    private GameObject linkAll;
    private GameObject linkV;
    private GameObject linkH;
    private GameObject linkO;

    //=============================================================
    private GameManager.PIECE_LINK_TYPE pieceLinkType = GameManager.PIECE_LINK_TYPE.V;

    //ピースリンク
    private int[,] pieceLink = new int[2,2];
    public int[,] PieceLink {
        get { return pieceLink; }
        set { pieceLink = value; }
    }

    //=============================================================
    private void Init () {
        CRef();

        pieceLink = gameManager.GetNextPieceLink();
        base.SelectPieceLinkType(pieceLink,ref pieceLinkType); //ピースリンクのタイプの選択
        SetPieceLinkDisplay(); //リンクに応じて表示形式と色を変える
    }


    //=============================================================
    protected override void CRef () {
        base.CRef();

        linkAll = transform.Find("PieceLink_NextFrame").gameObject;
        linkV = transform.Find("PieceLink_NextFrame/V").gameObject;
        linkH = transform.Find("PieceLink_NextFrame/H").gameObject;
        linkO = transform.Find("PieceLink_NextFrame/O").gameObject;
    }

    //=============================================================
    private void Awake () {
        Init();
    }

    //=============================================================
    private void Update () {
        if(!gameManager.IsPause) {
            linkAll.SetActive(true);

            pieceLink = gameManager.GetNextPieceLink();
            base.SelectPieceLinkType(pieceLink,ref pieceLinkType); //ピースリンクのタイプの選択
            SetPieceLinkDisplay(); //リンクに応じて表示形式と色を変える
        } else {
            linkAll.SetActive(false);
        }
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