using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLifeAndSkillGauge : MonoBehaviour {
    //=============================================================
    private int skillGaugeNum = 4; //スキルゲージの数

    //=============================================================
    private GameManager gameManager;
    private GameObject lifePoint;
    private GameObject[] skillGauge;

    //=============================================================
    private void Init () {
        CRef();
    }

    //=============================================================
    private void CRef () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        lifePoint = transform.Find("LP").gameObject;
        skillGauge = new GameObject[skillGaugeNum];
        skillGauge[0] = transform.Find("Skill1").gameObject;
        skillGauge[1] = transform.Find("Skill2").gameObject;
        skillGauge[2] = transform.Find("Skill3").gameObject;
        skillGauge[3] = transform.Find("Skill4").gameObject;
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

        for(int i = 0;i < skillGaugeNum;i++) {
            //Debug.Log(status.Voltage);
            //Debug.Log(i + "::" + ((status.Voltage * skillGaugeNum / status.MaxVoltage) - i));
            skillGauge[(skillGaugeNum - 1) - i].GetComponent<Image>().fillAmount = (status.Voltage * skillGaugeNum / status.MaxVoltage) - i;
        }
    }
}