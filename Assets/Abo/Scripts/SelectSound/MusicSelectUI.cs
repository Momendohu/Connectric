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
}