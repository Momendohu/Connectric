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
    }

    private void Update () {
        //シークバー動作
        seekBar.GetComponent<Slider>().value = soundManager.GetBGMTime(gameManager.TstBGMName) / soundManager.GetBGMTimeLength(gameManager.TstBGMName);
    }

    //=============================================================
    //プレイヤーがリズムに乗る
    private IEnumerator CharacterRhythm (GameObject obj,float tempo) {
        float time = 0;
        while(true) {
            time = gameManager.GetBeatWaveTiming(soundManager.GetBGMTime("bgm001"),1,tempo);
            //time += Time.deltaTime * (tempo / 60f);
            obj.transform.localScale = new Vector3(1,CharacterRhythmAnim.Evaluate(time),1);

            if(time >= 1) {
                time = 0;
            }
            yield return null;
        }
    }
}