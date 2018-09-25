using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class TapEffect : MonoBehaviour {
    //=============================================================


    //=============================================================
    private void Init () {
        CRef();
    }

    //=============================================================
    private void CRef () {

    }

    //=============================================================
    private void Awake () {
        Init();
    }

    private void Start () {
        Destroy(this.gameObject,1);
    }

    private void Update () {

    }
}