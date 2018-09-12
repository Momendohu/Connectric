//==============================================
// シーン切り替え
//
//
//　　　2018/05/09(水)　　　　　ryoya nagata
//==============================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;      // シーン管理




//========================================================
// シーン切り替えクラス
//========================================================
public class ChangeScene : MonoBehaviour {

    // シーン番号
    public enum SCENENUM
    {
        TITLE　= 0,
        CHARA_SELECT,
        GAME,
        RESULT,
        SCENE_MAX,
    };

    // シーン名
    private string[] sceneData = 
    {
        "Title",
        "CharaSelect",
        "Game",
        "Result"
    };

	// 値の保持
    [SerializeField]private static int currentScene;        // 現在のシーン  
    [SerializeField]private static int nextScene;           // 次のシーン

	// Use this for initialization
	void Start () {

        for(int i=0; i < (int)SCENENUM.SCENE_MAX; i++)
        {
            if(sceneData[i] == SceneManager.GetActiveScene().name)
            {
				currentScene = i;
				nextScene = currentScene + 1;
                // マックスの場合はタイトルへ
                if (nextScene == (int)SCENENUM.SCENE_MAX)
                {
                    nextScene = (int)SCENENUM.TITLE;
                }
            }
        }
        
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Delete))
        {
            // 次シーンのロード
            SceneManager.LoadScene( sceneData[nextScene] );
        }
	}

	//==========================================================================
	// シーン再読み込み
	//==========================================================================
	public void SceneRestart()
	{
		SceneManager.LoadScene( sceneData[currentScene] );
	}

	//==========================================================================
	// シーンのチェンジをする関数 
	//==========================================================================
	public void SceneChange(int SceneNum)
    {
		// 次シーンのロード
		SceneManager.LoadScene( sceneData[SceneNum] );
	}

}
