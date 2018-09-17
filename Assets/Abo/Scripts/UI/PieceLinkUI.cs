using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class PieceLinkUI : MonoBehaviour {
    //=============================================================
    //ピースリンクのタイプの選択
    protected virtual void SelectPieceLinkType (int[,] pieceLink,ref GameManager.PIECE_LINK_TYPE pieceLinkType) {
        //左下がなくてアクティブなピースリンクの数が2なら横長タイプ
        if(pieceLink[0,1] == -1 && ActivePieceLinkNum(pieceLink) == 2) {
            pieceLinkType = GameManager.PIECE_LINK_TYPE.H;
            return;
        }

        //右上がなくてアクティブなピースリンクの数が2なら縦長タイプ
        if(pieceLink[1,0] == -1 && ActivePieceLinkNum(pieceLink) == 2) {
            pieceLinkType = GameManager.PIECE_LINK_TYPE.V;
            return;
        }

        //そのほかは四角タイプ(またはLタイプ)
        pieceLinkType = GameManager.PIECE_LINK_TYPE.O;
    }

    //=============================================================
    //アクティブなピースリンクの数を取得
    protected virtual int ActivePieceLinkNum (int[,] pieceLink) {
        int num = 0;
        for(int i = 0;i < pieceLink.GetLength(0);i++) {
            for(int j = 0;j < pieceLink.GetLength(1);j++) {
                if(pieceLink[i,j] != -1) {
                    num++;
                }
            }
        }

        return num;
    }

    //=============================================================
    //ピースリンクのイメージがアクティブかどうか
    protected virtual bool IsActivePieceLinkImage (GameObject linkObj,string path) {
        GameObject obj = RefPieceLinkImage(linkObj,path);
        if(obj == null) {
            return false;
        } else {
            return true;
        }
    }

    //=============================================================
    //ピースリンクのイメージの参照を返す
    protected virtual GameObject RefPieceLinkImage (GameObject linkObj,string path) {
        return linkObj.transform.Find(path).gameObject;
    }

    //=============================================================
    //ピースリンクの決定
    protected virtual void DecidePieceLink (int[,] pieceLink) {
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