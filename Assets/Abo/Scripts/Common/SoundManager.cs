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
    [System.NonSerialized]
    public int BGMNum = 3; //bgmの数

    //BGMデータ(構造体)
    public struct BGMData {
        public string Name; //データ上の名前
        public string DisplayName; //表示する名前
        public string ArtistName; //アーティスト名
        public float BPM; //beat per minute
        public float Volume; //ボリューム
    }

    public BGMData[] BGMDatas ={
        new BGMData{ Name="bgm001",DisplayName="Broadcast",ArtistName="d-elf.com",BPM=128f,Volume=0.7f},
        new BGMData{ Name="bgm002",DisplayName="Experimental Model",ArtistName="d-elf.com",BPM=146f,Volume=0.5f},
        new BGMData{ Name="bgm003",DisplayName="Firmament",ArtistName="むつき醒",BPM=128f,Volume=1f},
        new BGMData{ Name="bgm004",DisplayName="???",ArtistName="???",BPM=202f,Volume=0.7f},
        new BGMData{ Name="Home",DisplayName="SystemBGM002",ArtistName="???",BPM=-1,Volume=0.5f}
    };

    //SEデータ(構造体)
    public struct SEData {
        public string Name;
        public float Volume;
    }

    public SEData[] SEDatas = {
        new SEData{ Name="puzzledelete",Volume=0.3f},
        new SEData{ Name="puzzlemove",Volume=0.3f},
        new SEData{ Name="EnemyAttack01",Volume=0.3f},
        new SEData{ Name="Cancel01",Volume=0.4f}
    };

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
    //特定のBGMの名前をデータと照合してボリューム設定を取得する
    private float GetVolumeInBGMData (string name) {
        for(int i = 0;i < BGMDatas.Length;i++) {
            if(BGMDatas[i].Name.Equals(name)) {
                return BGMDatas[i].Volume;
            }
        }

        return 0;
    }

    //===============================================================================
    //特定のSEの名前をデータと照合してボリューム設定を取得する
    private float GetVolumeInSEData (string name) {
        for(int i = 0;i < SEDatas.Length;i++) {
            if(SEDatas[i].Name.Equals(name)) {
                return SEDatas[i].Volume;
            }
        }

        return 0;
    }

    //=========================================================================================================================================================================
    //BGM関係
    //=========================================================================================================================================================================
    //オーディオを鳴らす
    public void TriggerBGM (string name,bool isUseLoop) {
        //Debug.Log(name);
        //SoundManagerにアタッチしてあるものと照合
        //指定したものがなければ再生しない
        int bgmListNum = CheckMatchNameInList(name,BGMList);
        if(bgmListNum != -1) {

            //すでに生成してあるオブジェクトと照合
            //すでにあるならそれを再生
            //ないならオブジェクト生成して再生
            int bgmObjNum = CheckMatchNameInList(name,BGMObject);
            if(bgmObjNum != -1) {
                //BGMDataからボリュームを設定
                SetBGMVolume(name,GetVolumeInBGMData(name));

                //再生
                BGMObject[bgmObjNum].GetComponent<AudioSource>().Play();
            } else {

                //オーディオ再生用の子オブジェクトを作成
                GameObject obj = Instantiate(Resources.Load("Prefabs/Other/SoundManagerAudio")) as GameObject;
                obj.name = name;
                BGMObject.Add(obj);
                obj.transform.SetParent(this.transform);

                //消えないように
                DontDestroyOnLoad(obj);

                //AudioSourceにAudioClipをアタッチ
                obj.GetComponent<AudioSource>().clip = BGMList[bgmListNum];

                //BGMDataからボリュームを設定
                SetBGMVolume(name,GetVolumeInBGMData(name));

                //再生
                obj.GetComponent<AudioSource>().Play();
                obj.GetComponent<AudioSource>().loop = isUseLoop;
            }
        } else {
            //Debug.Log("指定したAudioClipが無いよ");
        }
    }

    //===============================================================================
    //オーディオを鳴らす、ボリューム設定可能
    public void TriggerBGM (string name,bool isUseLoop,float volume) {
        //Debug.Log(name);
        //SoundManagerにアタッチしてあるものと照合
        //指定したものがなければ再生しない
        int bgmListNum = CheckMatchNameInList(name,BGMList);
        if(bgmListNum != -1) {

            //すでに生成してあるオブジェクトと照合
            //すでにあるならそれを再生
            //ないならオブジェクト生成して再生
            int bgmObjNum = CheckMatchNameInList(name,BGMObject);
            if(bgmObjNum != -1) {
                BGMObject[bgmObjNum].GetComponent<AudioSource>().volume = volume; //ボリュームの設定
                BGMObject[bgmObjNum].GetComponent<AudioSource>().Play();
            } else {

                //オーディオ再生用の子オブジェクトを作成
                GameObject obj = Instantiate(Resources.Load("Prefabs/Other/SoundManagerAudio")) as GameObject;
                obj.name = name;
                BGMObject.Add(obj);
                obj.transform.SetParent(this.transform);

                //消えないように
                DontDestroyOnLoad(obj);

                //AudioSourceにAudioClipをアタッチ
                obj.GetComponent<AudioSource>().clip = BGMList[bgmListNum];

                //ボリュームの設定
                obj.GetComponent<AudioSource>().volume = volume;

                //再生
                obj.GetComponent<AudioSource>().Play();
                obj.GetComponent<AudioSource>().loop = isUseLoop;
            }
        } else {
            //Debug.Log("指定したAudioClipが無いよ");
        }
    }

    //===============================================================================
    //BGMを一時停止する
    public void PauseBGM (string name) {

        //すでに生成してあるオブジェクトと照合
        int bgmObjNum = CheckMatchNameInList(name,BGMObject);
        if(bgmObjNum != -1) {
            BGMObject[bgmObjNum].GetComponent<AudioSource>().Pause();
        } else {
            //Debug.Log("指定したBGMが無いよ:" + name);
        }
    }

    //===============================================================================
    //BGMの一時停止を解除する
    public void UnPauseBGM (string name) {

        //すでに生成してあるオブジェクトと照合
        int bgmObjNum = CheckMatchNameInList(name,BGMObject);
        if(bgmObjNum != -1) {
            BGMObject[bgmObjNum].GetComponent<AudioSource>().UnPause();
        } else {
            //Debug.Log("指定したBGMが無いよ:" + name);
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
            //Debug.Log("指定したBGMが無いよ:" + name);
        }
    }

    //===============================================================================
    //BGMのピッチを変更する
    public void SetBGMPitch (string name,float pitch) {

        //すでに生成してあるオブジェクトと照合
        int bgmObjNum = CheckMatchNameInList(name,BGMObject);
        if(bgmObjNum != -1) {
            BGMObject[bgmObjNum].GetComponent<AudioSource>().pitch = pitch;
        } else {
            //Debug.Log("指定したBGMが無いよ:" + name);
        }
    }

    //===============================================================================
    //BGMのボリュームを変更する
    public void SetBGMVolume (string name,float volume) {

        //すでに生成してあるオブジェクトと照合
        int bgmObjNum = CheckMatchNameInList(name,BGMObject);
        if(bgmObjNum != -1) {
            BGMObject[bgmObjNum].GetComponent<AudioSource>().volume = volume;
        } else {
            //Debug.Log("指定したBGMが無いよ:" + name);
        }
    }

    //===============================================================================
    //BGMの現在の再生時間を取得する
    public float GetBGMTime (string name) {

        //すでに生成してあるオブジェクトと照合
        int bgmObjNum = CheckMatchNameInList(name,BGMObject);
        if(bgmObjNum != -1) {
            return BGMObject[bgmObjNum].GetComponent<AudioSource>().time;
        } else {
            //Debug.Log("指定したBGMが無いよ:" + name);
            return -1;
        }
    }

    //===============================================================================
    //BGMの再生時間の長さを取得する
    public float GetBGMTimeLength (string name) {

        //すでに生成してあるオブジェクトと照合
        int bgmObjNum = CheckMatchNameInList(name,BGMObject);
        if(bgmObjNum != -1) {
            return BGMObject[bgmObjNum].GetComponent<AudioSource>().clip.length;
        } else {
            //Debug.Log("指定したBGMが無いよ:" + name);
            return -1;
        }
    }

    //=========================================================================================================================================================================
    //SE関係
    //=========================================================================================================================================================================
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
                audioSource.volume = GetVolumeInSEData(name); //SEDataからボリュームを設定
                audioSource.PlayOneShot(audioSource.clip);
            } else {

                //オーディオ再生用の子オブジェクトを作成
                GameObject obj = Instantiate(Resources.Load("Prefabs/Other/SoundManagerAudio")) as GameObject;
                obj.name = name;
                SEObject.Add(obj);
                obj.transform.SetParent(this.transform);

                //消えないように
                DontDestroyOnLoad(obj);

                //AudioSourceにAudioClipをアタッチ
                obj.GetComponent<AudioSource>().clip = SEList[seListNum];

                //SEDataからボリュームを設定
                obj.GetComponent<AudioSource>().volume = GetVolumeInSEData(name);

                //再生
                obj.GetComponent<AudioSource>().PlayOneShot(SEList[seListNum]);
            }
        } else {
            //Debug.Log("指定したAudioClipが無いよ");
        }
    }

    //===============================================================================
    //オーディオを鳴らす(独立して鳴らす)、ボリューム設定可能
    public void TriggerSE (string name,float volume) {

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
                audioSource.volume = volume; //ボリュームの設定
                audioSource.PlayOneShot(audioSource.clip);
            } else {

                //オーディオ再生用の子オブジェクトを作成
                GameObject obj = Instantiate(Resources.Load("Prefabs/Other/SoundManagerAudio")) as GameObject;
                obj.name = name;
                SEObject.Add(obj);
                obj.transform.SetParent(this.transform);

                //消えないように
                DontDestroyOnLoad(obj);

                //AudioSourceにAudioClipをアタッチ
                obj.GetComponent<AudioSource>().clip = SEList[seListNum];

                //ボリュームの設定
                obj.GetComponent<AudioSource>().volume = volume;

                //再生
                obj.GetComponent<AudioSource>().PlayOneShot(SEList[seListNum]);
            }
        } else {
            //Debug.Log("指定したAudioClipが無いよ");
        }
    }

    //===============================================================================
    //SEが再生されているかどうか
    private bool IsPlayingSE (string name) {
        //すでに生成してあるオブジェクトと照合
        int seObjNum = CheckMatchNameInList(name,SEObject);
        if(seObjNum != -1) {
            if(SEObject[seObjNum].GetComponent<AudioSource>().isPlaying) {
                return true;
            } else {
                return false;
            }
        } else {
            //Debug.Log("指定したBGMが無いよ:" + name);
            return false;
        }
    }
}