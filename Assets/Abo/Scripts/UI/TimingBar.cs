using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimingBar : MonoBehaviour {
    //=============================================================
    private GameManager gameManager;
    private SoundManager soundManager;
    private Image image;

    private GameObject pieceLinkObj;

    //=============================================================
    private Vector3 iniPos = new Vector3(-240,0,0);
    private Vector3 goalPos = new Vector3(240,0,0);

    private int destroyCountLength = 1;

    //=============================================================
    private bool destroyFlag;

    private int notesWave;
    private int destroyCount;

    private float BPM;
    private float bgmTime;
    private int waveInterval;

    //ピースリンク
    private int[,] pieceLink = new int[2,2];
    public int[,] PieceLink {
        get { return pieceLink; }
    }

    //=============================================================
    private void Init () {
        CRef();

        BPM = gameManager.TstBGMBPM;
        bgmTime = soundManager.GetBGMTime("bgm001");
        waveInterval = 8;

        notesWave = gameManager.GetBeatWaveNum(bgmTime,waveInterval,BPM);
        DecidePieceLink(); //ピースリンクの情報の決定
        pieceLinkObj.GetComponent<PieceLink_UpScreen>().SelectPieceLinkType(pieceLink); //ピースリンクのタイプの決定
    }

    //=============================================================
    private void CRef () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        image = GetComponent<Image>();

        pieceLinkObj = transform.Find("PieceLink_UpScreen").gameObject;
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
            gameManager.GetBeatWaveTiming(bgmTime,waveInterval,BPM)
            );

        if(!destroyFlag) {
            //消滅までのカウントを進める
            if(notesWave != gameManager.GetBeatWaveNum(bgmTime,waveInterval,BPM)) {
                notesWave = gameManager.GetBeatWaveNum(bgmTime,waveInterval,BPM);
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

    //=============================================================
    //ピースリンクの決定
    private void DecidePieceLink () {
        //初期状態
        for(int i = 0;i < pieceLink.GetLength(0);i++) {
            for(int j = 0;j < pieceLink.GetLength(1);j++) {
                pieceLink[i,j] = -1;
            }
        }
        //左上
        pieceLink[0,0] = Random.Range(0,4);

        int branch1 = Random.Range(0,2);
        int branch2 = 0/*Random.Range(0,3)*/;
        int branch3 = 0/*Random.Range(0,2)*/;

        if(branch1 == 0) {
            //右上
            pieceLink[1,0] = Random.Range(0,4);

            switch(branch2) {
                case 0:
                //終了
                return;

                case 1:
                //左下
                pieceLink[0,1] = Random.Range(0,4);

                if(branch3 == 0) {
                    //終了
                    return;
                } else {
                    //右下
                    pieceLink[1,1] = Random.Range(0,4);
                }

                break;

                case 2:
                //右下
                pieceLink[1,1] = Random.Range(0,4);

                if(branch3 == 0) {
                    //終了
                    return;
                } else {
                    //左下
                    pieceLink[0,1] = Random.Range(0,4);
                }
                break;
            }
        } else {
            //左下
            pieceLink[0,1] = Random.Range(0,4);

            switch(branch2) {
                case 0:
                //終了
                return;

                case 1:
                //右上
                pieceLink[1,0] = Random.Range(0,4);

                if(branch3 == 0) {
                    //終了
                    return;
                } else {
                    //右下
                    pieceLink[1,1] = Random.Range(0,4);
                }

                break;

                case 2:
                //右下
                pieceLink[1,1] = Random.Range(0,4);

                if(branch3 == 0) {
                    //終了
                    return;
                } else {
                    //右上
                    pieceLink[1,0] = Random.Range(0,4);
                }
                break;
            }
        }
    }
}