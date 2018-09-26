using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpScreen : MonoBehaviour {
    //=============================================================
    private float beforeFrameHitPoint; //前フレームの体力(現フレームの体力と差分をとってアニメーション切り替えに使う)
    private bool isPlayerDamaged; //プレイヤーがダメージを受けたかどうか

    //=============================================================
    private GameManager gameManager;
    private SoundManager soundManager;

    private GameObject seekBar;
    private GameObject playerCharacter;
    private GameObject enemyCharacter;
    private GameObject comboNum;

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
        comboNum = transform.Find("Combo").gameObject;
    }

    //=============================================================
    private void Awake () {

    }

    private void Start () {
        Init();

        //プレイヤーがリズムに乗る
        StartCoroutine(CharacterRhythm(playerCharacter,gameManager.BGMBPM));

        //エネミーがリズムに乗る
        StartCoroutine(CharacterRhythm(enemyCharacter,gameManager.BGMBPM));
    }

    private void Update () {
        if(beforeFrameHitPoint > gameManager.CharacterStatus[gameManager.FocusCharacter].HitPoint) {
            isPlayerDamaged = true;
        }
        beforeFrameHitPoint = gameManager.CharacterStatus[gameManager.FocusCharacter].HitPoint;

        //シークバー動作
        seekBar.GetComponent<Slider>().value = soundManager.GetBGMTime(gameManager.BGMName) / soundManager.GetBGMTimeLength(gameManager.BGMName);

        //コンボ数
        comboNum.GetComponent<Text>().text = gameManager.Combo + " COMBO";
    }

    //=============================================================
    //キャラクターがリズムに乗る
    private IEnumerator CharacterRhythm (GameObject obj,float tempo) {
        float time = 0;
        while(true) {
            time = gameManager.GetBeatWaveTiming(soundManager.GetBGMTime(gameManager.BGMName),2,tempo);
            //time += Time.deltaTime * (tempo / 60f);
            obj.transform.localScale = new Vector3(1,CharacterRhythmAnim.Evaluate(time),1);

            if(time >= 1) {
                time = 0;
            }

            //キャラクターなら(雑な実装)
            if(isPlayerDamaged && obj == playerCharacter) {
                yield return CharacterDamage(obj,1f);
            }

            yield return null;
        }
    }

    //=============================================================
    //キャラクターがダメージを受ける
    private IEnumerator CharacterDamage (GameObject obj,float waitTime) {
        //キャラクターがカナデならダメージ用の画像に差し替える
        if(gameManager.CharacterDatas[gameManager.FocusCharacter].Id == 0) {
            obj.GetComponent<Image>().sprite = gameManager.CharacterImageDamage[gameManager.FocusCharacter];
        }

        float time = 0;
        while(true) {
            time += Time.deltaTime / waitTime;
            if(time >= 1) {
                break;
            }

            yield return null;
        }

        isPlayerDamaged = false;
        obj.GetComponent<Image>().sprite = gameManager.CharacterImage[gameManager.FocusCharacter];
    }
}