using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

[DefaultExecutionOrder(-100)]
/// <summary>
/// ゲームマネージャー
/// </summary>
public class GameManager : SingletonMonoBehaviour<GameManager> {
    //=============================================================
    private SoundManager soundManager;

    //=============================================================
    public float TstBGMBPM = 130f;
    public string TstBGMName = "bgm001";

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
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    //=============================================================
    private void Awake () {
        Init();
    }

    private void Start () {
        soundManager.TriggerBGM(TstBGMName,false);
    }

    private void Update () {

    }
}