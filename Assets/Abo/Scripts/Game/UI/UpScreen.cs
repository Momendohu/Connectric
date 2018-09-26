﻿using System.Collections;
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
            yield return null;
        }
    }

    //=============================================================
    //キャラクターがダメージを受ける
    private IEnumerator CharacterDamage (GameObject obj) {
        //キャラクターがカナデならダメージ用の画像に差し替える
        if(gameManager.CharacterDatas[gameManager.FocusCharacter].Id == 0) {
            obj.GetComponent<Image>().sprite = gameManager.CharacterImage[3];
        }
        while(true) {

            yield return null;
        }

    }
}