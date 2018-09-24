using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectUI : MonoBehaviour {
    //=============================================================
    private GameManager gameManager;
    private Camera _camera; //カメラ

    private GameObject leftButton; //左のボタン
    private GameObject rightButton; //右のボタン

    private Image characterImageL; //キャラクターの画像(左)
    private Image characterImageC; //キャラクターの画像(中心)
    private Image characterImageR; //キャラクターの画像(右)

    private Text nameAndLV; //名前とレベル
    private Text skillDescription; //スキル説明
    private Image instrumentTypeIcon; //楽器タイプ

    //=============================================================
    private bool isTouched; //画面がタッチされているかどうか
    private Vector3 beforeFrameTouchPosition; //前フレームのタッチポジション
    private float easingTime = 0; //イージング処理用時間
    [SerializeField]
    private Vector3[] iniPos = { new Vector3(-800,-50,0),new Vector3(0,-50,0),new Vector3(800,-50,0) }; //初期位置(左、中心、右)
    private bool characterShiftFlag; //キャラクターのシフトフラグ

    //=============================================================
    private void Init () {
        CRef();
    }

    //=============================================================
    private void CRef () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        _camera = GameObject.Find("CameraUI").GetComponent<Camera>();

        leftButton = transform.Find("LeftButton").gameObject;
        rightButton = transform.Find("RightButton").gameObject;

        //オブジェクト生成を行う
        characterImageL = CreateCharacterImage("L",iniPos[0]).GetComponent<Image>();
        characterImageC = CreateCharacterImage("C",iniPos[1]).GetComponent<Image>();
        characterImageR = CreateCharacterImage("R",iniPos[2]).GetComponent<Image>();

        nameAndLV = transform.Find("Image/Text").GetComponent<Text>();
        skillDescription = transform.Find("Image2/Text").GetComponent<Text>();
        instrumentTypeIcon = transform.Find("Type/Icon").GetComponent<Image>();
    }

    //=============================================================
    private void Awake () {
        Init();
    }

    private void Start () {

    }

    private void Update () {
        GameManager.CharacterData characterData = gameManager.CharacterDatas[gameManager.FocusCharacter];

        characterImageL.sprite = gameManager.CharacterImage[GetFocusCharacterNum(gameManager.FocusCharacter - 1)];
        characterImageC.sprite = gameManager.CharacterImage[gameManager.FocusCharacter];
        characterImageR.sprite = gameManager.CharacterImage[GetFocusCharacterNum(gameManager.FocusCharacter + 1)];

        nameAndLV.text = characterData.Name + " LV " + gameManager.CharacterStatus[gameManager.FocusCharacter].Level;
        skillDescription.text = "ActiveSkill - " + characterData.ActiveSkill + "\nPassiveSkill - " + characterData.PassiveSkill;
        instrumentTypeIcon.sprite = gameManager.PieceLinkImage[(int)characterData.InstrumentType];

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
            //Debug.Log("1:" + characterImageC.GetComponent<RectTransform>().position.x);
            //Debug.Log("2:" + _camera.orthographicSize / 3);

            if(characterImageC.GetComponent<RectTransform>().position.x >= _camera.orthographicSize / 3) {
                //ShiftFocusCharacter(1);
                characterShiftFlag = true;
            }

            //キャラクターのシフトフラグがたったら
            if(characterShiftFlag) {
                //元の場所に戻す
                easingTime += Time.deltaTime;

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
            } else {
                //元の場所に戻す
                easingTime += Time.deltaTime;

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
    //キャラクターセレクトのボタンが押された時の処理
    //progressNum -> どれだけ参照を進めるか
    public void ShiftFocusCharacter (int progressNum) {
        gameManager.FocusCharacter = GetFocusCharacterNum(gameManager.FocusCharacter + progressNum);
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
}