﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class PauseButton : MonoBehaviour {
    //=============================================================
    private GameManager gameManager;

    //=============================================================
    private void Init () {
        CRef();
    }

    //=============================================================
    private void CRef () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    //=============================================================
    private void Awake () {
        Init();
    }

    //=============================================================
    public void OnClick () {
        gameManager.IsPause = true;
    }
}