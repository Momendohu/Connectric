using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectUI : MonoBehaviour {
    //=============================================================
    private GameManager gameManager;

    private Image characterImage; //キャラクターの画像
    private Text nameAndLV; //名前とレベル
    private Text skillDescription; //スキル説明
    private Image instrumentTypeIcon; //楽器タイプ

    //=============================================================
    [System.NonSerialized]
    public int FocusCharacter = 0; //フォーカスするキャラクター

    //=============================================================
    private void Init () {
        CRef();
    }

    //=============================================================
    private void CRef () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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
        GameManager.CharacterData characterData = gameManager.CharacterDatas[0];

        characterImage.sprite = gameManager.CharacterImage[characterData.Id];
        nameAndLV.text = characterData.Name + " LV999";
        skillDescription.text = "ActiveSkill - " + characterData.ActiveSkill + "\nPassiveSkill - " + characterData.PassiveSkill;
        instrumentTypeIcon.sprite = gameManager.PieceLinkImage[(int)characterData.InstrumentType];
    }
}