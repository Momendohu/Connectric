using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_PlayerCharacter : MonoBehaviour {
    //=============================================================
    private GameManager gameManager;
    private Image image;

    //=============================================================
    private void Init () {
        CRef();
    }

    //=============================================================
    private void CRef () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        image = GetComponent<Image>();
    }

    //=============================================================
    private void Awake () {
        Init();
    }

    private void Start () {

    }

    private void Update () {
        image.sprite = gameManager.CharacterImage[gameManager.FocusCharacter];
        if(gameManager.FocusCharacter == 2) {
            GetComponent<RectTransform>().eulerAngles = new Vector3(0,180,0);
        } else {
            GetComponent<RectTransform>().eulerAngles = Vector3.zero;
        }
    }
}