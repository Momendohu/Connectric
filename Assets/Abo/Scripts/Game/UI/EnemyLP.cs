using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyLP : MonoBehaviour {
    //=============================================================
    private GameManager gameManager;
    private Slider lp;


    //=============================================================
    private void Init () {
        CRef();
    }

    //=============================================================
    private void CRef () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        lp = GetComponent<Slider>();
    }

    //=============================================================
    private void Awake () {
        Init();
    }

    private void Update () {
        //LPゲージにデータ適用
        GameManager.CharacterState status = gameManager.EnemyStatus[gameManager.FocusEnemy];
        lp.value = status.HitPoint / (status.MaxHitPoint != 0 ? status.MaxHitPoint : 1);
    }
}