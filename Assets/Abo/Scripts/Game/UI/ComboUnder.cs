using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboUnder : MonoBehaviour {
    //=============================================================
    public int Combo;

    //=============================================================
    private GameManager gameManager;

    private RectTransform mainText;
    private RectTransform shadowText;
    private RectTransform maskedImage; //マスクされている画像

    //=============================================================
    private Vector3 maskedImageIniPos = new Vector3(-500,0,0);
    private Vector3 maskedImageGoalPos = new Vector3(500,0,0);

    //=============================================================
    private void Init () {
        CRef();
    }

    //=============================================================
    private void CRef () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        mainText = transform.Find("Main").GetComponent<RectTransform>();
        shadowText = transform.Find("Shadow").GetComponent<RectTransform>();
        maskedImage = transform.Find("Main/Masked").GetComponent<RectTransform>();
    }

    //=============================================================
    private void Awake () {
        Init();
    }

    private void Start () {
        mainText.gameObject.GetComponent<Text>().text = Combo + " COMBO";
        shadowText.gameObject.GetComponent<Text>().text = Combo + " COMBO";

        StartCoroutine(Display(0.1f,3f,0.1f));
    }

    //=============================================================
    //表示
    private IEnumerator Display (float interval1,float interval2,float interval3) {
        float time = 0;

        //拡大
        while(true) {
            time += gameManager.TimeForGame() / interval1;
            if(time >= 1) {
                time = 0;
                break;
            }

            mainText.localScale = Vector3.Lerp(Vector3.right,Vector3.one,time);
            shadowText.localScale = Vector3.Lerp(Vector3.right,Vector3.one,time);

            yield return null;
        }

        //マスクイメージの動作
        while(true) {
            time += gameManager.TimeForGame() / interval2;
            if(time >= 1) {
                time = 0;
                break;
            }

            maskedImage.localPosition = Vector3.Lerp(maskedImageIniPos,maskedImageGoalPos,time);

            yield return null;
        }

        //縮小
        while(true) {
            time += gameManager.TimeForGame() / interval3;
            if(time >= 1) {
                time = 0;
                break;
            }

            mainText.localScale = Vector3.Lerp(Vector3.one,Vector3.right,time);
            shadowText.localScale = Vector3.Lerp(Vector3.one,Vector3.right,time);

            yield return null;
        }

        Destroy(this.gameObject);
    }
}