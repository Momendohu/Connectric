using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectUI : MonoBehaviour {
    //=============================================================
    private GameManager gameManager;

    //private Button leftButton; //左のボタン
    //private Button rightButton; //右のボタン

    private Image characterImage; //キャラクターの画像
    private Text nameAndLV; //名前とレベル
    private Text skillDescription; //スキル説明
    private Image instrumentTypeIcon; //楽器タイプ

    //=============================================================
    private void Init () {
        CRef();
    }

    //=============================================================
    private void CRef () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        //leftButton = transform.Find("LeftButton").GetComponent<Button>();
        //rightButton = transform.Find("RightButton").GetComponent<Button>();

        characterImage = transform.Find("Character").GetComponent<Image>();
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

        characterImage.sprite = gameManager.CharacterImage[gameManager.FocusCharacter];
        nameAndLV.text = characterData.Name + " LV " + gameManager.CharacterStatus[gameManager.FocusCharacter].Level;
        skillDescription.text = "ActiveSkill - " + characterData.ActiveSkill + "\nPassiveSkill - " + characterData.PassiveSkill;
        instrumentTypeIcon.sprite = gameManager.PieceLinkImage[(int)characterData.InstrumentType];
    }

    //=============================================================
    //キャラクターセレクトのボタンが押された時の処理
    //progressNum -> どれだけ参照を進めるか
    public void PushSelectButton (int progressNum) {
        gameManager.FocusCharacter += progressNum;
        if(gameManager.FocusCharacter < 0) {
            gameManager.FocusCharacter = gameManager.CharacterDatas.Length - 1;
        }

        if(gameManager.FocusCharacter > gameManager.CharacterDatas.Length - 1) {
            gameManager.FocusCharacter = 0;
        }
    }
}