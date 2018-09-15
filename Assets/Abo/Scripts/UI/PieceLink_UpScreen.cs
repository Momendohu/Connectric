using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

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

    //=============================================================
    public Sprite[] Image;

    //=============================================================
    private void Init () {
        CRef();

        linkV.SetActive(false);
        linkH.SetActive(false);
        linkO.SetActive(false);

        switch(pieceLinkType) {
            case PIECE_LINK_TYPE.V:
            linkV.SetActive(true);
            break;

            case PIECE_LINK_TYPE.H:
            linkH.SetActive(true);
            break;

            case PIECE_LINK_TYPE.O:
            linkO.SetActive(true);
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
}