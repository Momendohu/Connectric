using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimingBar : MonoBehaviour {
    //=============================================================
    private GameManager gameManager;
    private SoundManager soundManager;
    private Image image;

    //=============================================================
    private Vector3 iniPos = new Vector3(-160,0,0);
    private Vector3 goalPos = new Vector3(160,0,0);

    private int destroyCountLength = 1;

    //=============================================================
    private bool destroyFlag;

    private int notesWave4;
    private int destroyCount;

    private float BPM;
    private float bgmTime;


    //=============================================================
    private void Init () {
        CRef();

        BPM = gameManager.TstBGMBPM;
        bgmTime = soundManager.GetBGMTime("bgm001");

        notesWave4 = gameManager.GetBeatWaveNum(bgmTime,4,BPM);
    }

    //=============================================================
    private void CRef () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        image = GetComponent<Image>();
    }

    //=============================================================
    private void Awake () {
        Init();
    }

    private void Update () {
        //時間の取得
        bgmTime = soundManager.GetBGMTime("bgm001");

        //位置の更新
        GetComponent<RectTransform>().localPosition = Vector3.Lerp(
            iniPos - (goalPos - iniPos) * (destroyCountLength - destroyCount),
            goalPos - (goalPos - iniPos) * (destroyCountLength - destroyCount),
            gameManager.GetBeatWaveTiming(bgmTime,4,BPM)
            );

        if(!destroyFlag) {
            //消滅までのカウントを進める
            if(notesWave4 != gameManager.GetBeatWaveNum(bgmTime,4,BPM)) {
                notesWave4 = gameManager.GetBeatWaveNum(bgmTime,4,BPM);
                destroyCount++;
            }

            //カウントが進んだら消す
            if(destroyCount == destroyCountLength + 1) {
                destroyFlag = true;
                StartCoroutine(DestroyRoutine());
            }
        }
    }

    //=============================================================
    //オブジェクト破壊時の処理(アニメーション)
    private IEnumerator DestroyRoutine () {
        Color iniColor = image.color;
        float time = 0;

        while(true) {
            time += Time.deltaTime;
            if(time >= 1) {
                break;
            }

            image.color = new Color(iniColor.r,iniColor.g,iniColor.b,1 - time);

            yield return null;
        }

        Destroy(this.gameObject);
    }
}