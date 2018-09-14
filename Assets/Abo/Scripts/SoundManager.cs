using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// BGM、SEを管理
/// </summary>
public class SoundManager : SingletonMonoBehaviour<SoundManager> {
    public AudioClip[] BGMList; //bgm
    public AudioClip[] SEList; //se

    private List<GameObject> BGMObject = new List<GameObject>();
    private List<GameObject> SEObject = new List<GameObject>();

    //===============================================================================
    private void Init () {
        if(this != Instance) {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    //===============================================================================
    private void Awake () {
        Init();

        StartCoroutine(tst());
    }

    IEnumerator tst () {
        while(true) {
            TriggerBGM("nv_tt",false);
            TriggerSE("ズドーン1");
            yield return new WaitForSeconds(1);
            StopBGM("nv_tt");
            break;
        }
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
    //オーディオのボリュームを変更する
    /*public void SetVolume (float volume,int num) {
        audioList[num].volume = volume;
    }

    //1===============================================================================
    //オーディオの現在の再生時間を取得する
    public float GetTime (int num) {
        return audioList[num].time;
    }

    //===============================================================================
    //オーディオの再生時間の長さを取得する
    public float GetTimeLength (int num) {
        return audioList[num].clip.length;
    }

    //===============================================================================
    //再生しているかどうか
    public bool IsPlaying (int num) {
        return audioList[num].isPlaying;
    }

    //===============================================================================
    //データを参照して追加する
    public void AddMusic (string name) {
        audioList.Add(transform.Find(name).GetComponent<AudioSource>());
    }*/
}