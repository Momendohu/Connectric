using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutIn : MonoBehaviour {
    //=============================================================
    public Sprite[] CharacterImage; //キャラクターイメージ(立ち絵)

    //=============================================================
    [System.NonSerialized]
    public string DisplayText; //表示テキスト

    [System.NonSerialized]
    public int Id; //id

    //=============================================================
    private GameManager gameManager;

    private GameObject under;
    private GameObject over;

    private GameObject text;
    private GameObject character;

    //=============================================================
    private void Init () {
        CRef();
    }

    //=============================================================
    private void CRef () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        under = transform.Find("Under").gameObject;
        over = transform.Find("Over").gameObject;

        text = transform.Find("Under/Text").gameObject;
        character = transform.Find("Under/Character").gameObject;
    }

    //=============================================================
    private void Awake () {
        Init();
    }

    private void Start () {
        character.GetComponent<Image>().sprite = CharacterImage[Id]; //イメージの適用
        text.GetComponent<Text>().text = DisplayText; //表示テキストの適用

        text.SetActive(false);
        character.SetActive(false);

        StartCoroutine(CutInAnimations(0.3f,0.8f,0.3f,0.9f));
    }

    private void Update () {

    }

    //=============================================================
    private IEnumerator CutInAnimations (float interval1,float interval2,float interval3,float characterSpeed) {
        float time = 0;

        //カットインを出す
        while(true) {
            time += gameManager.TimeForGame() / interval1;
            if(time >= 1) {
                under.GetComponent<RectTransform>().localScale = Vector3.one;
                over.GetComponent<RectTransform>().localScale = Vector3.one;

                time = 0;
                break;
            }

            Vector3 applyingScale = new Vector3(1,time,1);
            under.GetComponent<RectTransform>().localScale = applyingScale;
            over.GetComponent<RectTransform>().localScale = applyingScale;

            yield return null;
        }

        //キャラクター、テキスト表示
        text.SetActive(true);
        character.SetActive(true);

        //キャラクターを微妙に動かす
        while(true) {
            time += gameManager.TimeForGame() / interval2;
            if(time >= 1) {
                time = 0;
                break;
            }

            character.GetComponent<RectTransform>().localPosition += Vector3.left * characterSpeed;

            yield return null;
        }

        //透過して消滅
        while(true) {
            time += gameManager.TimeForGame() / interval3;
            if(time >= 1) {
                time = 0;
                break;
            }

            Color col = new Color(1,1,1,1 - time);
            under.GetComponent<Image>().color = col;
            over.GetComponent<Image>().color = col;
            text.GetComponent<Text>().color = col;
            character.GetComponent<Image>().color = col;

            yield return null;
        }

        Destroy(this.gameObject);
    }
}