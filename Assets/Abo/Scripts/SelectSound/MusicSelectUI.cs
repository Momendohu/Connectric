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

    private bool onceFirstBGM; //シーン起動時1回bgmを起動するためのフラグ(startでbgmがならなかったから使用)(startの段階でsoundmanagerが2つある可能性?)

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

    private void Update () {
        if(!onceFirstBGM) {
            soundManager.TriggerBGM(soundManager.BGMDatas[gameManager.FocusBGM].Name,true);
            onceFirstBGM = true;
        }

        //各bgmデータを画面に適用
        info_musicTitle.transform.Find("Text").GetComponent<Text>().text = soundManager.BGMDatas[gameManager.FocusBGM].DisplayName;
        info_musicArtist.transform.Find("Text").GetComponent<Text>().text = soundManager.BGMDatas[gameManager.FocusBGM].ArtistName;

        musicImage.sprite = MusicImages[gameManager.FocusBGM];
    }

    //=============================================================
    //次のシーン(キャラクター選択)に遷移するボタン
    public void OnClickPlayButton () {
        gameManager.JumpSceneSelectSoundToCharacterSelect();
    }

    //=============================================================
    //ホームに戻るボタン
    public void OnClickReturnButton () {
        //bgmを止める
        soundManager.StopBGM(soundManager.BGMDatas[gameManager.FocusBGM].Name);

        soundManager.TriggerSE("Cancel01");
        StartCoroutine(FadeToCharacterSelect());
    }

    //=============================================================
    //右ボタン
    public void OnClickRight () {
        //bgmを止める
        soundManager.StopBGM(soundManager.BGMDatas[gameManager.FocusBGM].Name);

        int f = gameManager.FocusBGM + 1;
        if(f > soundManager.BGMNum - 1) {
            f = 0;
        }

        gameManager.FocusBGM = f;

        gameManager.ApplyToBGMData(f);

        //bgmを再生
        soundManager.TriggerBGM(soundManager.BGMDatas[gameManager.FocusBGM].Name,true);
    }

    //=============================================================
    //左ボタン
    public void OnClickLeft () {
        //bgmを止める
        soundManager.StopBGM(soundManager.BGMDatas[gameManager.FocusBGM].Name);

        int f = gameManager.FocusBGM - 1;
        if(f < 0) {
            f = soundManager.BGMNum - 1;
        }

        gameManager.FocusBGM = f;

        gameManager.ApplyToBGMData(f);

        //bgmを再生
        soundManager.TriggerBGM(soundManager.BGMDatas[gameManager.FocusBGM].Name,true);
    }

    //=============================================================
    //キャラクターセレクトシーンに行くための橋渡し
    private IEnumerator FadeToCharacterSelect () {
        float time = 0;
        while(true) {
            time += Time.deltaTime;
            if(time >= 1) {
                break;
            }

            yield return null;
        }

        gameManager.JumpSceneSelectSoundToHome();
    }
}