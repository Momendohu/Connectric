using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoadUtil {
    public static AsyncOperation async;

    //=============================================================
    //シーンのローディング
    public static IEnumerator Load (string name,bool isWaitJumpScene) {
        DisplayLoadProgress();

        async = SceneManager.LoadSceneAsync(name);
        async.allowSceneActivation = false;

        while(async.progress < 0.9f) {
            yield return null;
        }

        if(!isWaitJumpScene) {
            AllowJumpScene();
        }

        while(!async.allowSceneActivation) {
            yield return null;
        }

        yield return async;
    }

    //=============================================================
    //シーン遷移の許可を与える
    public static void AllowJumpScene () {
        if(async != null) {
            async.allowSceneActivation = true;
        }
    }

    //=============================================================
    //ロードを表示する
    public static void DisplayLoadProgress () {
        GameObject obj = Object.Instantiate(Resources.Load("Prefabs/Loading/LoadingUI")) as GameObject;
        obj.transform.SetParent(GameObject.Find("Canvas").transform,false);
        obj.transform.SetAsLastSibling();
    }
}

