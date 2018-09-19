using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLP_Voltage : MonoBehaviour {
    //=============================================================
    private GameManager gameManager;
    private Slider lp;
    private Slider voltage;

    //=============================================================
    private void Init () {
        CRef();
    }

    //=============================================================
    private void CRef () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        lp = transform.Find("LP").GetComponent<Slider>();
        voltage = transform.Find("Voltage").GetComponent<Slider>();
    }

    //=============================================================
    private void Awake () {
        Init();
    }

    private void Start () {

    }

    private void Update () {
        //LPゲージにデータ適用
        GameManager.CharacterState status = gameManager.CharacterStatus[gameManager.FocusCharacter];
        lp.value = status.HitPoint / status.MaxHitPoint;
    }
}