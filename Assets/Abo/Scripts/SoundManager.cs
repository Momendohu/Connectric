using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[DefaultExecutionOrder(-110)]
/// <summary>
/// BGM、SEを管理
/// </summary>
public class SoundManager : SingletonMonoBehaviour<SoundManager> {
    public AudioClip[] BGMList; //bgm
    public AudioClip[] SEList; //se

    private List<GameObject> BGMObject = new List<GameObject>();
    private List<GameObject> SEObject = new List<GameObject>();

    //===============================================================================
    private bool Init () {
        if(this != Instance) {
            Destroy(this.gameObject);
            return false;
        }

        DontDestroyOnLoad(this.gameObject);

        return true;
    }

    //===============================================================================
    private void Awake () {
        if(!Init()) return;
    }

    //===============================================================================
    //リスト内から特定の名前があるかどうか照合する
    private int CheckMatchNameInList (string name,List<GameObject> list) {
        for(int i = 0;i < list.Count;i++) {
            if(list[i].name.Equals(name)) {
                return i;
            }
        }

        return -1;
    }

    private int CheckMatchNameInList (string name,AudioClip[] list) {
        for(int i = 0;i < list.Length;i++) {
            if(list[i].name.Equals(name)) {
                return i;
            }
        }

        return -1;
    }

    //===============================================================================
    //オーディオを鳴らす
    public void TriggerBGM (string name,bool isUseLoop) {
        Debug.Log(name);
        //SoundManagerにアタッチしてあるものと照合
        //指定したものがなければ再生しない
        int bgmListNum = CheckMatchNameInList(name,BGMList);
        if(bgmListNum != -1) {

            //すでに生成してあるオブジェクトと照合
            //すでにあるならそれを再生
            //ないならオブジェクト生成して再生
            int bgmObjNum = CheckMatchNameInList(name,BGMObject);
            if(bgmObjNum != -1) {
                BGMObject[bgmObjNum].GetComponent<AudioSource>().Play();
            } else {

                //オーディオ再生用の子オブジェクトを作成
                GameObject obj = Instantiate(Resources.Load("Prefabs/SoundManagerAudio")) as GameObject;
                obj.name = name;
                BGMObject.Add(obj);
                obj.transform.SetParent(this.transform);

                //AudioSourceにAudioClipをアタッチ
                obj.GetComponent<AudioSource>().clip = BGMList[bgmListNum];

                //再生
                obj.GetComponent<AudioSource>().Play();
                obj.GetComponent<AudioSource>().loop = isUseLoop;
            }
        } else {
            Debug.Log("指定したAudioClipが無いよ");
        }
    }

    //===============================================================================
    //オーディオを鳴らす(独立して鳴らす)
    public void TriggerSE (string name) {

        //SoundManagerにアタッチしてあるものと照合
        //指定したものがなければ再生しない
        int seListNum = CheckMatchNameInList(name,SEList);
        if(seListNum != -1) {

            //すでに生成してあるオブジェクトと照合
            //すでにあるならそれを再生
            //ないならオブジェクト生成して再生
            int seObjNum = CheckMatchNameInList(name,SEObject);
            if(seObjNum != -1) {
                AudioSource audioSource = SEObject[seObjNum].GetComponent<AudioSource>();
                audioSource.PlayOneShot(audioSource.clip);
            } else {

                //オーディオ再生用の子オブジェクトを作成
                GameObject obj = Instantiate(Resources.Load("Prefabs/SoundManagerAudio")) as GameObject;
                obj.name = name;
                SEObject.Add(obj);
                obj.transform.SetParent(this.transform);

                //AudioSourceにAudioClipをアタッチ
                obj.GetComponent<AudioSource>().clip = SEList[seListNum];

                //再生
                obj.GetComponent<AudioSource>().PlayOneShot(SEList[seListNum]);
            }
        } else {
            Debug.Log("指定したAudioClipが無いよ");
        }
    }

    //===============================================================================
    //BGMを止める
    public void StopBGM (string name) {

        //すでに生成してあるオブジェクトと照合
        //あるならそれを停止
        int bgmObjNum = CheckMatchNameInList(name,BGMObject);
        if(bgmObjNum != -1) {
            BGMObject[bgmObjNum].GetComponent<AudioSource>().Stop();
        } else {
            Debug.Log("指定したBGMが無いよ");
        }
    }

    //===============================================================================
    //BGMのピッチを変更する
    public void SetBGMPitch (string name,float pitch) {

        //すでに生成してあるオブジェクトと照合
        //あるならそれを停止
        int bgmObjNum = CheckMatchNameInList(name,BGMObject);
        if(bgmObjNum != -1) {
            BGMObject[bgmObjNum].GetComponent<AudioSource>().pitch = pitch;
        } else {
            Debug.Log("指定したBGMが無いよ");
        }
    }

    //===============================================================================
    //BGMのボリュームを変更する
    public void SetBGMVolume (string name,float volume) {

        //すでに生成してあるオブジェクトと照合
        //あるならそれを停止
        int bgmObjNum = CheckMatchNameInList(name,BGMObject);
        if(bgmObjNum != -1) {
            BGMObject[bgmObjNum].GetComponent<AudioSource>().volume = volume;
        } else {
            Debug.Log("指定したBGMが無いよ");
        }
    }

    //===============================================================================
    //BGMの現在の再生時間を取得する
    public float GetBGMTime (string name) {

        //すでに生成してあるオブジェクトと照合
        //あるならそれを停止
        int bgmObjNum = CheckMatchNameInList(name,BGMObject);
        if(bgmObjNum != -1) {
            return BGMObject[bgmObjNum].GetComponent<AudioSource>().time;
        } else {
            Debug.Log("指定したBGMが無いよ");
            return -1;
        }
    }

    //===============================================================================
    //BGMの再生時間の長さを取得する
    public float GetBGMTimeLength (string name) {

        //すでに生成してあるオブジェクトと照合
        //あるならそれを停止
        int bgmObjNum = CheckMatchNameInList(name,BGMObject);
        if(bgmObjNum != -1) {
            return BGMObject[bgmObjNum].GetComponent<AudioSource>().clip.length;
        } else {
            Debug.Log("指定したBGMが無いよ");
            return -1;
        }
    }
}