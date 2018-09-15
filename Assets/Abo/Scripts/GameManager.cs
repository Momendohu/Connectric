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