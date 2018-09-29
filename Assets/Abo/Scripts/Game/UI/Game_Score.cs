using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_Score : MonoBehaviour {
    //=============================================================
    private GameManager gameManager;
    private Text text;

    //=============================================================
    private void Init () {
        CRef();
    }

    //=============================================================
    private void CRef () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        text = GetComponent<Text>();
    }

    //=============================================================
    private void Awake () {
        Init();
    }

    private void Start () {

    }

    private void Update () {
        text.text = "score:" + gameManager.GameRecordStatus.Score;
    }
}