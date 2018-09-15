using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpScreen : MonoBehaviour {
    //=============================================================
    private GameManager gameManager;
    private SoundManager soundManager;

    private GameObject seekBar;
    private GameObject playerCharacter;
    private GameObject enemyCharacter;

    //=============================================================
    private int notesWave4forTimingBar;

    //=============================================================
    public AnimationCurve CharacterRhythmAnim;

    //=============================================================
    private void Init () {
        CRef();
    }

    //=============================================================
    private void CRef () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();

        seekBar = transform.Find("SeekBar").gameObject;
        playerCharacter = transform.Find("PlayerCharacter").gameObject;
        enemyCharacter = transform.Find("EnemyCharacter").gameObject;
    }

    //=============================================================
    private void Awake () {
        Init();
    }

    private void Start () {
        //プレイヤーがリズムに乗る
        StartCoroutine(CharacterRhythm(playerCharacter,gameManager.TstBGMBPM));

        //エネミーがリズムに乗る
        StartCoroutine(CharacterRhythm(enemyCharacter,gameManager.TstBGMBPM));

        //タイミングバー用のウェーブ指定
        notesWave4forTimingBar = gameManager.GetBeatWaveNum(soundManager.GetBGMTime("bgm001"),4,gameManager.TstBGMBPM);
    }

    private void Update () {
        //シークバー動作
        seekBar.GetComponent<Slider>().value = soundManager.GetBGMTime(gameManager.TstBGMName) / soundManager.GetBGMTimeLength(gameManager.TstBGMName);

        //タイミングバー生成
        if(notesWave4forTimingBar != gameManager.GetBeatWaveNum(soundManager.GetBGMTime("bgm001"),4,gameManager.TstBGMBPM)) {
            notesWave4forTimingBar = gameManager.GetBeatWaveNum(soundManager.GetBGMTime("bgm001"),4,gameManager.TstBGMBPM);

            CreateTimingBar();
        }
    }

    //=============================================================
    //キャラクターがリズムに乗る
    private IEnumerator CharacterRhythm (GameObject obj,float tempo) {
        float time = 0;
        while(true) {
            time = gameManager.GetBeatWaveTiming(soundManager.GetBGMTime("bgm001"),2,tempo);
            //time += Time.deltaTime * (tempo / 60f);
            obj.transform.localScale = new Vector3(1,CharacterRhythmAnim.Evaluate(time),1);

            if(time >= 1) {
                time = 0;
            }
            yield return null;
        }
    }

    //=============================================================
    //タイミングバーの作成
    private void CreateTimingBar () {
        GameObject obj = Instantiate(Resources.Load("Prefabs/UI/TimingBar")) as GameObject;
        obj.transform.SetParent(this.transform,false);
    }
}