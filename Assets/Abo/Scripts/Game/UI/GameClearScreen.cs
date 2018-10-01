using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class GameClearScreen : MonoBehaviour {
    //=============================================================
    private GameManager gameManager;
    private GameObject waitVail;
    private GameObject gameClearImage;

    //=============================================================
    public AnimationCurve AnimScaleChange;

    //=============================================================
    private bool onceClear;
    private float clearWaitTime = 1; //クリア時の待機時間

    //=============================================================
    private void Init () {
        CRef();
    }

    //=============================================================
    private void CRef () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        waitVail = transform.Find("WaitVail").gameObject;
        gameClearImage = transform.Find("WaitVail/GameClear").gameObject;
    }

    //=============================================================
    private void Awake () {
        Init();
    }

    private void Update () {
        if(gameManager.IsGameClear) {
            waitVail.SetActive(true);
            if(!onceClear) {
                StartCoroutine(AnimGameClearImage());
                StartCoroutine(WaitAndDoSceneJump(clearWaitTime));
                onceClear = true;
            }
        } else {
            waitVail.SetActive(false);
        }
    }

    //=============================================================
    //ゲームクリアの画像を動かす
    private IEnumerator AnimGameClearImage () {
        float time = 0;
        while(true) {
            time += Time.deltaTime;
            if(time >= 1) {
                break;
            }

            gameClearImage.GetComponent<RectTransform>().localScale = Vector3.one * AnimScaleChange.Evaluate(time);

            yield return null;
        }
    }

    //=============================================================
    //シーン遷移を指定した時間だけ待ってその後遷移
    private IEnumerator WaitAndDoSceneJump (float waitTime) {
        yield return new WaitForSeconds(waitTime);
        gameManager.JumpSceneGameToResult();
    }
}