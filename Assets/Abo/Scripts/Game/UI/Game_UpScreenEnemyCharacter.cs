using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_UpScreenEnemyCharacter : MonoBehaviour {
    //=============================================================
    private GameManager gameManager;
    private Image shadow; //影

    private Vector3 iniPosShadow = new Vector3(0,0,0);
    private Vector3 goalPosShadow = new Vector3(0,-400,0);

    //=============================================================
    private bool isFirstEnemy;
    public bool IsFirstEnemy {
        get { return isFirstEnemy; }
        set { isFirstEnemy = value; }
    }

    //=============================================================
    private void Init () {
        CRef();
    }

    //=============================================================
    private void CRef () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        shadow = transform.Find("Shadow").GetComponent<Image>();
    }

    //=============================================================
    private void Awake () {
        Init();
    }

    private void Start () {
        StartCoroutine(ShadowMove(3));
    }

    private void Update () {

    }

    //=============================================================
    private IEnumerator ShadowMove (float speed) {
        float time = 0;
        while(true) {
            if(isFirstEnemy) {
                time = 1;
            }

            shadow.GetComponent<RectTransform>().localPosition = Vector3.Lerp(iniPosShadow,goalPosShadow,time);

            time += gameManager.TimeForGame() * speed;
            if(time >= 1) {
                break;
            }

            yield return null;
        }
    }
}