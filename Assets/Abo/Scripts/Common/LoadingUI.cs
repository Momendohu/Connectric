using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour {
    //=============================================================
    private Vector3 rotateSpeed = new Vector3(0,0,10);

    //=============================================================
    private RectTransform circle;

    //=============================================================
    private void Init () {
        CRef();
    }

    //=============================================================
    private void CRef () {
        circle = transform.Find("Circle").GetComponent<RectTransform>();
    }

    //=============================================================
    private void Awake () {
        Init();
    }

    private void Update () {
        circle.eulerAngles += rotateSpeed;
    }
}
