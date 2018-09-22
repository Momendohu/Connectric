using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public static class TouchUtil {
    private static Vector3 TouchPosition = Vector3.zero; //座標

    //=============================================================
    //タッチ情報
    public enum TouchInfo {
        None = 99, //タッチなし
        Began = 0, //移動
        Moved = 1, //移動
        Stationary = 2, //静止
        Ended = 3, //終了
        Canceled = 4, //キャンセル
    }

    //=============================================================
    //タッチの情報を取得する
    public static TouchInfo GetTouch () {
        if(Application.isEditor) {
            if(Input.GetMouseButtonDown(0)) { return TouchInfo.Began; }
            if(Input.GetMouseButton(0)) { return TouchInfo.Moved; }
            if(Input.GetMouseButtonUp(0)) { return TouchInfo.Ended; }
        } else {
            if(Input.touchCount > 0) {
                return (TouchInfo)((int)Input.GetTouch(0).phase);
            }
        }
        return TouchInfo.None;
    }

    //=============================================================
    //タッチの位置を取得する
    public static Vector3 GetTouchPosition () {
        if(Application.isEditor) {
            TouchInfo touch = TouchUtil.GetTouch();
            if(touch != TouchInfo.None) { return Input.mousePosition; }
        } else {
            if(Input.touchCount > 0) {
                Touch touch = Input.GetTouch(0);
                TouchPosition.x = touch.position.x;
                TouchPosition.y = touch.position.y;
                return TouchPosition;
            }
        }
        return Vector3.zero;
    }

    //=============================================================
    //タッチの位置を取得する(ワールド座標)
    public static Vector3 GetTouchWorldPosition (Camera camera) {
        return camera.ScreenToWorldPoint(GetTouchPosition());
    }
}
