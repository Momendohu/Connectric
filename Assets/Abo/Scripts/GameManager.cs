using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

/// <summary>
/// ゲームマネージャー
/// </summary>
public class GameManager : SingletonMonoBehaviour<GameManager> {
    //=============================================================


    //=============================================================
    private void Init () {
        if(this != Instance) {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this.gameObject);

        CRef();
    }

    //=============================================================
    private void CRef () {

    }

    //=============================================================
    private void Awake () {
        Init();
    }

    private void Start () {

    }

    private void Update () {

    }
}