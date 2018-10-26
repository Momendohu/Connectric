using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStartText : MonoBehaviour {
    //=============================================================
    private Text mainText;
    private Text shadowText;

    //=============================================================
    private void Init () {
        CRef();
    }

    //=============================================================
    private void CRef () {
        mainText = transform.Find("Main").GetComponent<Text>();
        shadowText = transform.Find("Shadow").GetComponent<Text>();
    }

    //=============================================================
    private void Awake () {
        Init();
    }

    private void Start () {
        StartCoroutine(Disappear(2,2));
    }

    //=============================================================
    //消滅
    private IEnumerator Disappear (float waitTime,float speed) {
        float time = 0;
        while((waitTime - time) >= 0) {
            time += Time.fixedDeltaTime * speed;

            mainText.color = new Color(1,1,1,waitTime - time);
            shadowText.color = new Color(0,0,0,waitTime - time);

            yield return null;
        }

        mainText.color = new Color(1,1,1,0);
        shadowText.color = new Color(0,0,0,0);

        Destroy(this.gameObject);
    }
}