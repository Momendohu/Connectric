using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLifeAndSkillGauge : MonoBehaviour {
    //=============================================================
    private GameManager gameManager;
    private GameObject lifePoint;


    //=============================================================
    private void Init () {
        CRef();
    }

    //=============================================================
    private void CRef () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        lifePoint = transform.Find("LP").gameObject;
    }

    //=============================================================
    private void Awake () {
        Init();
    }

    private void Start () {

    }

    private void Update () {
        GameManager.CharacterState status = gameManager.CharacterStatus[gameManager.FocusCharacter];
        lifePoint.GetComponent<Image>().fillAmount = status.HitPoint / (status.MaxHitPoint != 0 ? status.MaxHitPoint : 1);
    }
}