using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpScreen : MonoBehaviour {
    //=============================================================
    private float beforeFrameHitPoint; //前フレームの体力(現フレームの体力と差分をとってアニメーション切り替えに使う)
    private bool isPlayerDamaged; //プレイヤーがダメージを受けたかどうか

    private float beforeFrameHitPointEnemy; //前フレームの敵の体力
    private bool isEnemyDamaged; //エネミーがダメージを受けたかどうか

    //=============================================================
    private GameManager gameManager;
    private SoundManager soundManager;

    private GameObject seekBar;
    private GameObject playerCharacter;
    private GameObject enemyCharacter;
    private GameObject comboNum;

    //=============================================================
    public AnimationCurve CharacterRhythmAnim;
    public AnimationCurve CharacterDamageAnim;

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
        StartCoroutine(CharacterAnim(playerCharacter,gameManager.BGMBPM));

        //エネミーがリズムに乗る
        StartCoroutine(CharacterAnim(enemyCharacter,gameManager.BGMBPM));

        //フォーカスしているキャラクターに応じて画像を切り替える
        playerCharacter.GetComponent<Image>().sprite = gameManager.CharacterImage[gameManager.FocusCharacter];
        if(gameManager.FocusCharacter == 2) {
            playerCharacter.GetComponent<RectTransform>().eulerAngles = new Vector3(0,180,0);
        } else {
            playerCharacter.GetComponent<RectTransform>().eulerAngles = Vector3.zero;
        }
    }

    private void Update () {
        //プレイヤーがダメージを受けたかどうかを判定
        if(beforeFrameHitPoint > gameManager.CharacterStatus[gameManager.FocusCharacter].HitPoint) {
            isPlayerDamaged = true;
        }
        beforeFrameHitPoint = gameManager.CharacterStatus[gameManager.FocusCharacter].HitPoint;

        //敵がダメージを受けたかどうかを判定
        if(beforeFrameHitPointEnemy > gameManager.EnemyStatus[gameManager.FocusEnemy].HitPoint) {
            isEnemyDamaged = true;
        }
        beforeFrameHitPointEnemy = gameManager.EnemyStatus[gameManager.FocusEnemy].HitPoint;

        //シークバー動作
        seekBar.GetComponent<Slider>().value = soundManager.GetBGMTime(gameManager.BGMName) / soundManager.GetBGMTimeLength(gameManager.BGMName);

        //コンボ数
        comboNum.GetComponent<Text>().text = gameManager.GameRecordStatus.Combo + " COMBO";
    }

    //=============================================================
    //キャラクターがアニメーションする
    private IEnumerator CharacterAnim (GameObject obj,float tempo) {
        float time = 0;
        while(true) {
            time = gameManager.GetBeatWaveTiming(soundManager.GetBGMTime(gameManager.BGMName),2,tempo);
            obj.transform.localScale = new Vector3(1,CharacterRhythmAnim.Evaluate(time),1);

            if(time >= 1) {
                time = 0;
            }

            //キャラクターなら(雑な実装)
            if(obj == playerCharacter) {
                //スキル時画像の切り替え
                if(gameManager.IsSkillMode) {
                    obj.GetComponent<Image>().sprite = gameManager.CharacterImageSmile[gameManager.FocusCharacter];
                } else {
                    obj.GetComponent<Image>().sprite = gameManager.CharacterImage[gameManager.FocusCharacter];
                }

                if(isPlayerDamaged) {
                    yield return CharacterDamage(obj,1f);
                }
            }

            //エネミーなら(雑な実装)
            if(isEnemyDamaged && obj == enemyCharacter) {
                yield return EnemyDamage(obj,1f);
            }

            yield return null;
        }
    }

    //=============================================================
    //エネミーがダメージを受ける
    private IEnumerator EnemyDamage (GameObject obj,float waitTime) {
        //スケールの初期化
        obj.transform.localScale = Vector3.one;

        //角度の調整
        obj.GetComponent<RectTransform>().localEulerAngles = new Vector3(0,0,-10);

        float time = 0;
        while(true) {
            time += gameManager.TimeForGame() / waitTime;

            obj.GetComponent<Image>().color = new Color(1,1,1,CharacterDamageAnim.Evaluate(time));

            if(time >= 1) {
                isEnemyDamaged = false;

                obj.GetComponent<RectTransform>().localEulerAngles = Vector3.zero;
                break;
            }

            yield return null;
        }
    }

    //=============================================================
    //キャラクターがダメージを受ける
    private IEnumerator CharacterDamage (GameObject obj,float waitTime) {
        //スケールの初期化
        obj.transform.localScale = Vector3.one;

        //角度の調整
        if(gameManager.FocusCharacter != 2) {
            obj.GetComponent<RectTransform>().localEulerAngles = new Vector3(0,0,10);
        } else {
            obj.GetComponent<RectTransform>().localEulerAngles = new Vector3(0,180,-10);
        }

        //ダメージ差分に画像を切り替え
        obj.GetComponent<Image>().sprite = gameManager.CharacterImageDamage[gameManager.FocusCharacter];

        float time = 0;
        while(true) {
            time += gameManager.TimeForGame() / waitTime;

            obj.GetComponent<Image>().color = new Color(1,1,1,CharacterDamageAnim.Evaluate(time));

            if(time >= 1) {
                isPlayerDamaged = false;

                if(gameManager.FocusCharacter != 2) {
                    obj.GetComponent<RectTransform>().localEulerAngles = Vector3.zero;
                } else {
                    obj.GetComponent<RectTransform>().localEulerAngles = new Vector3(0,180,0);
                }

                obj.GetComponent<Image>().sprite = gameManager.CharacterImage[gameManager.FocusCharacter];
                break;
            }

            yield return null;
        }
    }

    //=============================================================
    //キャラクターがダメージを受ける
}