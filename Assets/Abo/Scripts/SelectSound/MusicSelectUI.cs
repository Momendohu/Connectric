using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicSelectUI : MonoBehaviour {
    //=============================================================
    private GameManager gameManager;
    private SoundManager soundManager;

    private GameObject info_musicTitle;
    private GameObject info_musicArtist;
    private GameObject info_record;

    private GameObject leftButton;
    private GameObject rightButton;

    private Image musicImage;

    //=============================================================
    public Sprite[] MusicImages;

    //=============================================================
    private void Init () {
        CRef();
    }

    //=============================================================
    private void CRef () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();

        info_musicTitle = GameObject.Find("Canvas/Information/MusicTitle");
        info_musicArtist = GameObject.Find("Canvas/Information/MusicArtists");
        info_record = GameObject.Find("Canvas/Information/Record");

        leftButton = GameObject.Find("Canvas/LeftButton");
        rightButton = GameObject.Find("Canvas/RightButton");

        musicImage = GameObject.Find("Canvas/Music/Image").GetComponent<Image>();
    }

    //=============================================================
    private void Awake () {
        Init();
    }

    private void Start () {

    }

    private void Update () {
        //各bgmデータを画面に適用
        info_musicTitle.transform.Find("Text").GetComponent<Text>().text = soundManager.BGMDatas[gameManager.FocusBGM].DisplayName;
        info_musicArtist.transform.Find("Text").GetComponent<Text>().text = soundManager.BGMDatas[gameManager.FocusBGM].ArtistName;

        musicImage.sprite = MusicImages[gameManager.FocusBGM];
    }

    //=============================================================
    //次のシーン(キャラクター選択)に遷移するボタン
    public void OnClickPlayButton () {
        //gameManager.ApplyToBGMData(1);
        gameManager.JumpSceneSelectSoundToCharacterSelect();
    }

    //=============================================================
    //ホームに戻るボタン
    public void OnClickReturnButton () {
        gameManager.JumpSceneSelectSoundToHome();
    }

    //=============================================================
    //右ボタン
    public void OnClickRight () {
        int f = gameManager.FocusBGM + 1;
        if(f > soundManager.BGMNum - 1) {
            f = 0;
        }

        gameManager.FocusBGM = f;

        gameManager.ApplyToBGMData(f);
    }

    //=============================================================
    //左ボタン
    public void OnClickLeft () {
        int f = gameManager.FocusBGM - 1;
        if(f < 0) {
            f = soundManager.BGMNum - 1;
        }

        gameManager.FocusBGM = f;

        gameManager.ApplyToBGMData(f);
    }
}