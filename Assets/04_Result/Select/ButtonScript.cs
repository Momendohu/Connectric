using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 親のボタンスクリプト
public class ButtonScript : MonoBehaviour {

    //-----------------------------------------
    // どの曲が選ばれているか
    //-----------------------------------------
    private static int song_num;
    public int Song_num
    {
        get { return song_num; }
        set { song_num = value; }
    }
	
    //-------------------------------------------------
    // 各ボタンの処理
    //-------------------------------------------------
    public void OnClick(int num)
    {
        switch(num)
        {
            case 0:
                {
                    song_num = 0;
                    Debug.Log("一曲目！");
                    break;
                }
            case 1:
                {
                    song_num = 1;
                    Debug.Log("二曲目！");
                    break;
                }
            case 2:
                {
                    song_num = 2;
                    Debug.Log("三曲目！");
                    break;
                }
            default:
                {
                    song_num = -1;
                    break;
                }


        }
    }

}
