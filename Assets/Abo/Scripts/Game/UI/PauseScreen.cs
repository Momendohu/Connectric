using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class PauseScreen : MonoBehaviour {
    //=============================================================
    private GameManager gameManager;
    private GameObject waitVail;

    //=============================================================
    private void Init () {
        CRef();
    }

    //=============================================================
    private void CRef () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        waitVail = transform.Find("WaitVail").gameObject;
    }

    //=============================================================
    private void Awake () {
        Init();
    }

    private void Start () {

    }

    private void Update () {
        if(gameManager.IsPause) {
            waitVail.SetActive(true);
        } else {
            waitVail.SetActive(false);
        }
    }

    //=============================================================
    //ゲームに戻るボタン
    public void OnClickToGame () {
        gameManager.IsPause = false;
    }

    //=============================================================
    //曲選択に戻るボタン
    public void OnClickToSelect () {

    }
}