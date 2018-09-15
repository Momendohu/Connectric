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
    // ピースタイプ
    public enum INSTRUMENT_TYPE {
        GUITAR = 0,
        DRUM,
        VOCAL,
        DJ,
        MAX
    };

    //=============================================================
    private SoundManager soundManager;

    //=============================================================
    public float TstBGMBPM = 130f;
    public string TstBGMName = "bgm001";

    //ピースリンク
    private INSTRUMENT_TYPE[,] pieceLink = new INSTRUMENT_TYPE[2,2];
    public INSTRUMENT_TYPE[,] PieceLink {
        get { return pieceLink; }
    }

    //=============================================================
    private int notesWaveForTimingBar;

    //=============================================================
    private void Init () {
        if(this != Instance) {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this.gameObject);

        CRef();

        //タイミングバー用のウェーブ指定
        notesWaveForTimingBar = GetBeatWaveNum(soundManager.GetBGMTime("bgm001"),4,TstBGMBPM);
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
        //タイミングバー生成
        if(notesWaveForTimingBar != GetBeatWaveNum(soundManager.GetBGMTime("bgm001"),4,TstBGMBPM)) {
            notesWaveForTimingBar = GetBeatWaveNum(soundManager.GetBGMTime("bgm001"),4,TstBGMBPM);

            CreateTimingBar();
        }
    }

    //=============================================================
    //タイミングバーの作成
    private void CreateTimingBar () {
        GameObject obj = Instantiate(Resources.Load("Prefabs/UI/TimingBar")) as GameObject;
        obj.transform.SetParent(GameObject.Find("Canvas/UpScreen").transform,false);
    }

    //=============================================================
    //1ビート毎の秒数を取得
    public float GetTimePerBeats (float BPM) {
        return 60 / BPM;
    }

    //=============================================================
    //ビートウェイブを取得
    public float GetBeatWave (float time,int interval,float BPM) {
        return time / (GetTimePerBeats(BPM) * interval);
    }

    //=============================================================
    //ビートウェイブ(何Waveか)を取得
    public int GetBeatWaveNum (float time,int interval,float BPM) {
        return Mathf.FloorToInt(GetBeatWave(time,interval,BPM));
    }

    //=============================================================
    //ビートウェイブのタイミングを取得(0-1)
    public float GetBeatWaveTiming (float time,int interval,float BPM) {
        return GetBeatWave(time,interval,BPM) - GetBeatWaveNum(time,interval,BPM);
    }
}