using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectUI : MonoBehaviour {
    //=============================================================
    private GameManager gameManager;
    private SoundManager soundManager;
    private Camera _camera; //カメラ

    private GameObject leftButton; //左のボタン
    private GameObject rightButton; //右のボタン

    private GameObject characterImageL; //キャラクターの画像(左)
    private GameObject characterImageC; //キャラクターの画像(中心)
    private GameObject characterImageR; //キャラクターの画像(右)

    private Text nameAndLV; //名前とレベル
    private Text skillDescription; //スキル説明
    private Image instrumentTypeIcon; //楽器タイプ

    private AudioSource selectSE;

    //=============================================================
    private bool isTouched; //画面がタッチされているかどうか
    private Vector3 beforeFrameTouchPosition; //前フレームのタッチポジション
    private float easingTime = 0; //イージング処理用時間
    private Vector3[] iniPos = { new Vector3(-800,-50,0),new Vector3(0,-50,0),new Vector3(800,-50,0) }; //初期位置(左、中心、右)
    private bool characterShiftFlagL; //キャラクターのシフトフラグ
    private bool characterShiftFlagR; //キャラクターのシフトフラグ
    private float shiftLerpSpeed = 4; //シフト移動のスピード

    //=============================================================
    private void Init () {
        CRef();
    }

    //=============================================================
    private void CRef () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();

        _camera = GameObject.Find("Camera").GetComponent<Camera>();

        leftButton = transform.Find("LeftButton").gameObject;
        rightButton = transform.Find("RightButton").gameObject;

        //オブジェクト生成を行う
        characterImageL = CreateCharacterImage("L",iniPos[0]);
        characterImageC = CreateCharacterImage("C",iniPos[1]);
        characterImageR = CreateCharacterImage("R",iniPos[2]);

        nameAndLV = transform.Find("Image/Text").GetComponent<Text>();
        skillDescription = transform.Find("Image/Image2/Text").GetComponent<Text>();
        instrumentTypeIcon = transform.Find("Image/Type/Icon").GetComponent<Image>();

        selectSE = GameObject.Find("SelectSE").GetComponent<AudioSource>();
    }

    //=============================================================
    private void Awake () {
        Init();
    }

    private void Start () {

    }

    private void Update () {
        //画面タッチ状態ならフラグをon
        if(TouchUtil.GetTouch() == TouchUtil.TouchInfo.Moved) {
            isTouched = true;
        } else {
            isTouched = false;
        }

        //画面タッチの状態に応じてキャラクターやボタンの状態を変える
        if(isTouched) {
            //前フレームとタッチした場所の座標が違うなら
            if(beforeFrameTouchPosition != GetTouchPosition()) {
                characterImageL.GetComponent<RectTransform>().position += new Vector3((GetTouchPosition().x - beforeFrameTouchPosition.x),0,0);
                characterImageC.GetComponent<RectTransform>().position += new Vector3((GetTouchPosition().x - beforeFrameTouchPosition.x),0,0);
                characterImageR.GetComponent<RectTransform>().position += new Vector3((GetTouchPosition().x - beforeFrameTouchPosition.x),0,0);

                //ボタンのアクティブを切る
                leftButton.SetActive(false);
                rightButton.SetActive(false);
            }

            easingTime = 0;
        } else {
            easingTime += Time.deltaTime * shiftLerpSpeed;

            //左に移動させるかどうかの判定
            if(!characterShiftFlagL) {
                if(characterImageC.GetComponent<RectTransform>().position.x <= -_camera.orthographicSize / 4) {
                    selectSE.PlayOneShot(selectSE.clip);
                    characterShiftFlagL = true;
                }
            }

            //左に移動させたときの動作
            if(characterShiftFlagL) {
                characterImageC.GetComponent<RectTransform>().localPosition = Vector3.Lerp(
                   characterImageC.GetComponent<RectTransform>().localPosition,
                   iniPos[0],
                   easingTime
                   );

                characterImageR.GetComponent<RectTransform>().localPosition = Vector3.Lerp(
                   characterImageR.GetComponent<RectTransform>().localPosition,
                   iniPos[1],
                   easingTime
                   );

                //一定時間が経過したら
                if(easingTime >= 1) {
                    easingTime = 0;
                    characterShiftFlagL = false;

                    //フォーカスするキャラクターのシフト
                    ShiftFocusCharacter(1);
                    //位置の初期化
                    characterImageL.GetComponent<RectTransform>().localPosition = iniPos[0];
                    characterImageC.GetComponent<RectTransform>().localPosition = iniPos[1];
                    characterImageR.GetComponent<RectTransform>().localPosition = iniPos[2];
                }
            }

            //右に移動させるかどうかの判定
            if(!characterShiftFlagR) {
                if(characterImageC.GetComponent<RectTransform>().position.x >= _camera.orthographicSize / 4) {
                    selectSE.PlayOneShot(selectSE.clip);
                    characterShiftFlagR = true;
                }
            }

            //右に移動させたときの動作
            if(characterShiftFlagR) {
                characterImageC.GetComponent<RectTransform>().localPosition = Vector3.Lerp(
                   characterImageC.GetComponent<RectTransform>().localPosition,
                   iniPos[2],
                   easingTime
                   );

                characterImageL.GetComponent<RectTransform>().localPosition = Vector3.Lerp(
                   characterImageL.GetComponent<RectTransform>().localPosition,
                   iniPos[1],
                   easingTime
                   );

                //一定時間が経過したら
                if(easingTime >= 1) {
                    easingTime = 0;
                    characterShiftFlagR = false;

                    //フォーカスするキャラクターのシフト
                    ShiftFocusCharacter(-1);
                    //位置の初期化
                    characterImageL.GetComponent<RectTransform>().localPosition = iniPos[0];
                    characterImageC.GetComponent<RectTransform>().localPosition = iniPos[1];
                    characterImageR.GetComponent<RectTransform>().localPosition = iniPos[2];
                }
            }

            //通常動作
            if(!(characterShiftFlagL || characterShiftFlagR)) {
                characterImageL.GetComponent<RectTransform>().localPosition = Vector3.Lerp(
                    characterImageL.GetComponent<RectTransform>().localPosition,
                    iniPos[0],
                    easingTime
                    );

                characterImageC.GetComponent<RectTransform>().localPosition = Vector3.Lerp(
                    characterImageC.GetComponent<RectTransform>().localPosition,
                    iniPos[1],
                    easingTime
                    );

                characterImageR.GetComponent<RectTransform>().localPosition = Vector3.Lerp(
                    characterImageR.GetComponent<RectTransform>().localPosition,
                    iniPos[2],
                    easingTime
                    );

                //ボタンのアクティブ化
                leftButton.SetActive(true);
                rightButton.SetActive(true);
            }
        }

        beforeFrameTouchPosition = GetTouchPosition();
    }

    private void LateUpdate () {
        //キャラクターデータの適用
        GameManager.CharacterData characterData = gameManager.CharacterDatas[gameManager.FocusCharacter];
        nameAndLV.text = characterData.Name + " LV " + gameManager.CharacterStatus[gameManager.FocusCharacter].Level;
        skillDescription.text = "スキル - " + gameManager.SkillDatas[gameManager.CharacterDatas[gameManager.FocusCharacter].SkillId].Name;
        instrumentTypeIcon.sprite = gameManager.PieceLinkImage[(int)characterData.InstrumentType];

        //イメージを適用
        characterImageL.GetComponent<Image>().sprite = gameManager.CharacterImage[GetFocusCharacterNum(gameManager.FocusCharacter - 1)];
        characterImageC.GetComponent<Image>().sprite = gameManager.CharacterImage[GetFocusCharacterNum(gameManager.FocusCharacter)];
        characterImageR.GetComponent<Image>().sprite = gameManager.CharacterImage[GetFocusCharacterNum(gameManager.FocusCharacter + 1)];
    }

    //=============================================================
    //現在のタッチしたポジションを取得する
    private Vector3 GetTouchPosition () {
        return TouchUtil.GetTouchWorldPosition(_camera);
    }

    //=============================================================
    //フォーカスしているキャラクターの数字を返す
    private int GetFocusCharacterNum (int num) {
        if(num > gameManager.CharacterDatas.Length - 1) {
            return 0;
        }

        if(num < 0) {
            return gameManager.CharacterDatas.Length - 1;
        }

        return num;
    }

    //=============================================================
    //プレイボタンを押したときの処理
    public void PushPlayButton () {
        gameManager.JumpSceneCharacterSelectToGame();
    }

    //=============================================================
    //キャラクターフォーカスを変更する
    //progressNum -> どれだけ参照を進めるか
    private void ShiftFocusCharacter (int progressNum) {
        gameManager.FocusCharacter = GetFocusCharacterNum(gameManager.FocusCharacter + progressNum);
    }

    //=============================================================
    //シフトする方向
    private enum ShiftDirection {
        LEFT = -1,
        RIGHT = 1
    }

    //キャラクターセレクトのボタンが押された時の処理
    public void OnClick (int shiftDirection) {
        switch(shiftDirection) {
            case (int)ShiftDirection.LEFT:
            selectSE.PlayOneShot(selectSE.clip);
            characterShiftFlagL = true;
            break;

            case (int)ShiftDirection.RIGHT:
            selectSE.PlayOneShot(selectSE.clip);
            characterShiftFlagR = true;
            break;
        }
    }

    //=============================================================
    //キャラクターの画像を生成する
    private GameObject CreateCharacterImage (string name,Vector3 iniPos) {
        GameObject obj = Instantiate(Resources.Load("Prefabs/UI/CharacterSelectImage")) as GameObject;
        obj.name = name;
        obj.GetComponent<RectTransform>().localPosition = iniPos;

        obj.transform.SetParent(GameObject.Find("Canvas/CharacterSelectUI/Characters").transform,false);

        return obj;
    }

    //=============================================================
    //戻るボタンを押したときの動作
    public void OnClickReturnButton () {
        soundManager.TriggerSE("Cancel01");
        gameManager.JumpSceneCharacterSelectToSelectSound();
    }
}