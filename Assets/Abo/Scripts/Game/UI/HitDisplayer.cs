using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitDisplayer : MonoBehaviour {
    //=============================================================
    public int HitNum;

    //=============================================================
    private float speed = 5f;
    private float rotateSpeed = -0.1f;
    private float grav = -10f;
    private float alpha = 3f;
    private float alphaSpeed = 3f;

    //=============================================================
    private GameManager gameManager;
    private Text main;
    private Text shadow;
    private Color mainColor;
    private Color shadowColor;

    //=============================================================
    private void Init () {
        CRef();
    }

    //=============================================================
    private void CRef () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        main = transform.Find("Main").GetComponent<Text>();
        shadow = transform.Find("Shadow").GetComponent<Text>();

        mainColor = main.color;
        shadowColor = shadow.color;
    }

    //=============================================================
    private void Awake () {
        Init();
    }

    private void Update () {
        speed += grav * gameManager.TimeForGame();
        alpha -= gameManager.TimeForGame() * alphaSpeed;

        if(gameManager.TimeForGame() != 0) GetComponent<RectTransform>().localPosition += new Vector3(0,speed,0);
        GetComponent<RectTransform>().localEulerAngles += new Vector3(0,0,rotateSpeed);
        main.color = new Color(mainColor.r,mainColor.g,mainColor.b,alpha);
        shadow.color = new Color(shadowColor.r,shadowColor.g,shadowColor.b,alpha);

        if(alpha <= 0) {
            Destroy(this.gameObject);
        }

        main.text = HitNum + "HIT!";
        shadow.text = HitNum + "HIT!";
    }
}