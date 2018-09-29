using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-120)]
public class TimingBar : MonoBehaviour {
    //=============================================================
    private GameManager gameManager;
    private SoundManager soundManager;
    private Image image;

    private GameObject pieceLinkObj;

    //=============================================================
    private Vector3 iniPos = new Vector3(-250,750,0);
    private Vector3 goalPos = new Vector3(250,750,0);

    private int destroyCountLength = 1;

    //=============================================================
    //アニメーションの種類
    private enum DestroyAnimationType {
        NotEstablished = 0,
        Established = 1
    }
    private DestroyAnimationType destroyAnimationType = DestroyAnimationType.NotEstablished;

    //=============================================================
    //破壊フラグ
    private bool destroyFlag;
    public bool DestroyFlag {
        get { return destroyFlag; }
    }

    //リンクが成立したかどうかのフラグ
    private bool linkEstablished;
    public bool LinkEstablished {
        set { linkEstablished = value; }
    }

    private int notesWave;
    private int destroyCount;

    private float BPM;
    private float bgmTime;
    private int waveInterval;

    //=============================================================
    private void Init () {
        CRef();

        BPM = gameManager.BGMBPM;
        bgmTime = soundManager.GetBGMTime(gameManager.BGMName);
        waveInterval = 8;

        notesWave = gameManager.GetBeatWaveNum(bgmTime,waveInterval,BPM);
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
        bgmTime = soundManager.GetBGMTime(gameManager.BGMName);

        if(!destroyFlag) {
            //消滅までのカウントを進める
            if(notesWave != gameManager.GetBeatWaveNum(bgmTime,waveInterval,BPM)) {
                notesWave = gameManager.GetBeatWaveNum(bgmTime,waveInterval,BPM);
                destroyCount++;
            }

            //リンクが成立しているならリンク成立フラグon
            if(linkEstablished) {
                destroyAnimationType = DestroyAnimationType.Established;
            }

            //カウントが進んだら消す
            if(destroyCount == destroyCountLength + 1) {
                destroyFlag = true;

                //リンクが成立したかどうかでアニメーションを変化させる
                StartCoroutine(DestroyRoutine(destroyAnimationType,2));

                //リンクの状態を解除する
                if(linkEstablished) {
                    linkEstablished = false;
                    destroyAnimationType = DestroyAnimationType.NotEstablished;
                }
            }
        }

        //位置の更新
        GetComponent<RectTransform>().localPosition = Vector3.Lerp(
            iniPos - (goalPos - iniPos) * (destroyCountLength - destroyCount),
            goalPos - (goalPos - iniPos) * (destroyCountLength - destroyCount),
            gameManager.GetBeatWaveTiming(bgmTime,waveInterval,BPM)
            );
    }

    //=============================================================
    //オブジェクト破壊時の処理(アニメーション)
    private IEnumerator DestroyRoutine (DestroyAnimationType type,float speed) {
        switch(type) {
            case DestroyAnimationType.NotEstablished:
            List<Image> pieceLinkImages = pieceLinkObj.GetComponent<PieceLink_UpScreen>().GetPieceLinkImageComponent();
            Color iniColor = image.color;
            float time = 0;

            while(true) {
                time += Time.deltaTime * speed;
                if(time >= 1) {
                    break;
                }

                Color presentColor = new Color(iniColor.r,iniColor.g,iniColor.b,Mathf.Clamp01(iniColor.a - time));
                image.color = presentColor;
                for(int i = 0;i < pieceLinkImages.Count;i++) {
                    pieceLinkImages[i].color = presentColor;
                }

                yield return null;
            }

            Destroy(this.gameObject);
            break;

            case DestroyAnimationType.Established:
            break;

            default:
            Debug.Log("タイミングバーのアニメーション指定が変だよ");
            break;
        }

        yield break;
    }
}