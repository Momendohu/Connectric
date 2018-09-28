using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleEffect : MonoBehaviour {
    //=============================================================
    private GameManager gameManager;

    private Image title;
    private Image red;
    private Image blue;
    private Image green;
    private Image yellow;

    //=============================================================
    public AnimationCurve EffectAlpha; //透明度の調整

    //=============================================================
    private void Init () {
        CRef();
    }

    //=============================================================
    private void CRef () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        title = transform.Find("Title").GetComponent<Image>();
        red = transform.Find("Red").GetComponent<Image>();
        blue = transform.Find("Blue").GetComponent<Image>();
        green = transform.Find("Green").GetComponent<Image>();
        yellow = transform.Find("Yellow").GetComponent<Image>();
    }

    //=============================================================
    private void Awake () {
        Init();
    }

    private void Start () {
        StartCoroutine(Flush(1600f,1f));
    }

    //=============================================================
    //エフェクトの点滅
    private IEnumerator Flush (float speed,float length) {
        float time = 0;
        while(true) {
            time += Time.deltaTime * speed / length;

            red.color = new Color(1,1,1,Mathf.Sin(time * Mathf.Deg2Rad) * EffectAlpha.Evaluate(time / speed));
            blue.color = new Color(1,1,1,Mathf.Sin(time * Mathf.Deg2Rad * 2) * EffectAlpha.Evaluate(time / speed));
            green.color = new Color(1,1,1,Mathf.Sin(time * Mathf.Deg2Rad * 3) * EffectAlpha.Evaluate(time / speed));
            yellow.color = new Color(1,1,1,Mathf.Sin(time * Mathf.Deg2Rad * 4) * EffectAlpha.Evaluate(time / speed));

            if(time >= speed) {
                break;
            }

            yield return null;
        }

        yield return AppearTitle();
    }

    //=============================================================
    //タイトルの出現
    private IEnumerator AppearTitle () {
        float time = 0;
        while(true) {
            time += Time.deltaTime;
            title.color = new Color(1,1,1,time);

            if(time >= 1) {
                break;
            }

            yield return null;
        }

        gameManager.IsTitleAppeared = true; //タイトル出現フラグを立たせる
    }
}