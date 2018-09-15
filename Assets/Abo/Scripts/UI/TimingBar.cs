using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class TimingBar : MonoBehaviour {
    //=============================================================
    private GameManager gameManager;
    private SoundManager soundManager;

    //=============================================================
    private Vector3 iniPos = new Vector3(-160,0,0);
    private Vector3 goalPos = new Vector3(160,0,0);

    private int destroyCountLength = 1;

    //=============================================================
    private int notesWave4;
    private int destroyCount;

    private float BPM;
    private float time;


    //=============================================================
    private void Init () {
        CRef();

        BPM = gameManager.TstBGMBPM;
        time = soundManager.GetBGMTime("bgm001");

        notesWave4 = gameManager.GetBeatWaveNum(time,4,BPM);
    }

    //=============================================================
    private void CRef () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    //=============================================================
    private void Awake () {
        Init();
    }

    private void Update () {
        //時間の取得
        time = soundManager.GetBGMTime("bgm001");

        //位置の更新
        GetComponent<RectTransform>().localPosition = Vector3.Lerp(
            iniPos - (goalPos - iniPos) * (destroyCountLength - destroyCount),
            goalPos - (goalPos - iniPos) * (destroyCountLength - destroyCount),
            gameManager.GetBeatWaveTiming(time,4,BPM)
            );

        //消滅までのカウントを進める
        if(notesWave4 != gameManager.GetBeatWaveNum(time,4,BPM)) {
            notesWave4 = gameManager.GetBeatWaveNum(time,4,BPM);
            destroyCount++;
        }

        //カウントが進んだら消す
        if(destroyCount == destroyCountLength + 1) {
            Destroy(this.gameObject);
        }
    }
}