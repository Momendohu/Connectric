using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.UI;

[DefaultExecutionOrder(-100)]
/// <summary>
/// ゲームマネージャー
/// </summary>
public class GameManager : SingletonMonoBehaviour<GameManager> {
    //=============================================================
    // ピースタイプ
    public enum INSTRUMENT_TYPE {
        GUITAR = 0,
        DRUM,
        VOCAL,
        DJ,
        MAX
    };

    //=============================================================
    //ピースリンクタイプ
    public enum PIECE_LINK_TYPE {
        V = 0,
        H = 1,
        O = 2,
    }

    //=============================================================
    public Sprite[] PieceLinkImage; //ピースのイメージ
    public Sprite[] CharacterImage; //キャラクターのイメージ

    //=============================================================
    //キャラクターデータ
    public struct CharacterData {
        public int Id;
        public string Name;
        public string ActiveSkill;
        public string PassiveSkill;
        public INSTRUMENT_TYPE InstrumentType;
    }

    //キャラクターのステータス
    public struct CharacterState {
        public int Id;
        public int Level;
        public int MaxHitPoint;
        public int HitPoint;
        public float AttackPower;
    }

    public CharacterState[] CharacterStatus = {
        new CharacterState{Id=0,Level=-1,MaxHitPoint=-1,HitPoint=-1,AttackPower=-1},
        new CharacterState{Id=1,Level=-1,MaxHitPoint=-1,HitPoint=-1,AttackPower=-1},
        new CharacterState{Id=1,Level=-1,MaxHitPoint=-1,HitPoint=-1,AttackPower=-1},
    };


    public CharacterData[] CharacterDatas = {
        new CharacterData{Id=0, Name="kanade",ActiveSkill="Pitch Shift",PassiveSkill="Power Code",InstrumentType=INSTRUMENT_TYPE.GUITAR},
        new CharacterData{Id=1, Name="seira",ActiveSkill="Abandonne",PassiveSkill="Con Anima",InstrumentType=INSTRUMENT_TYPE.DJ}, //abandonne(感情のままに) con anima(魂をこめて)
        new CharacterData{Id=1, Name="???",ActiveSkill="Poly Rhythm",PassiveSkill="Ghost Note",InstrumentType=INSTRUMENT_TYPE.DRUM},
    };

    //=============================================================
    private SoundManager soundManager;

    //=============================================================
    [System.NonSerialized]
    public float BGMBPM = 146f; //tst001->171 bgm001->130 bgm002->128 bgm003->146
    [System.NonSerialized]
    public string BGMName = "bgm003";
    [System.NonSerialized]
    public int BeatInterbal = 8;
    
    [System.NonSerialized]
    public int FocusCharacter = 0; //フォーカスするキャラクター

    //=============================================================
    //ビートが変わったかどうか(タイミングバーが到達したかどうか)
    private bool isBeatChange;
    public bool IsBeatChange {
        get { return isBeatChange; }
        set { isBeatChange = value; }
    }

    //=============================================================
    private List<GameObject> timingBars = new List<GameObject>();
    private int notesWaveForTimingBar;

    //=============================================================
    private void Init () {
        if(this != Instance) {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this.gameObject);

        CRef();
    }

    //=============================================================
    private void CRef () {
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    //=============================================================
    private void Awake () {
        Init();

        switch(SceneManager.GetActiveScene().name) {
            case "CharacterSelect":
            InitCharacterSelect();
            break;

            case "Game_copy":
            InitGame();
            break;

            default:
            Debug.Log("謎のシーンだよ");
            break;
        }
    }

    private void Start () {
        switch(SceneManager.GetActiveScene().name) {
            case "CharacterSelect":
            break;

            case "Game_copy":
            break;

            default:
            Debug.Log("謎のシーンだよ");
            break;
        }
    }

    private void Update () {
        switch(SceneManager.GetActiveScene().name) {
            case "CharacterSelect":
            break;

            case "Game_copy":
            RoutineGame();
            break;

            default:
            Debug.Log("謎のシーンだよ");
            break;
        }
    }

    //==============================================================================================================================================
    //CharacterSelectシーン
    //==============================================================================================================================================
    private void InitCharacterSelect () {
        InitCharacterStatus();
    }

    //体力を計算する
    private int CalculateHitPoint (int level) {
        return 30 + level * 3;
    }

    //攻撃力を計算する
    private int CalculateAttackPoint (int level) {
        return 5 * level;
    }

    //ステータスの初期化
    private void InitCharacterStatus () {
        for(int i = 0;i < CharacterStatus.Length;i++) {
            CharacterStatus[i].Level = 1;
            CharacterStatus[i].MaxHitPoint = CalculateHitPoint(CharacterStatus[i].Level);
            CharacterStatus[i].HitPoint = CharacterStatus[i].MaxHitPoint;
            CharacterStatus[i].AttackPower = CalculateAttackPoint(CharacterStatus[i].Level);
        }
    }

    //==============================================================================================================================================
    //Gameシーン
    //==============================================================================================================================================
    private void InitGame () {
        soundManager.TriggerBGM(BGMName,false);

        //タイミングバー用のウェーブ指定
        notesWaveForTimingBar = GetBeatWaveNum(soundManager.GetBGMTime(BGMName),BeatInterbal,BGMBPM);
    }

    //=============================================================
    private void RoutineGame () {
        //タイミングバー生成
        if(notesWaveForTimingBar != GetBeatWaveNum(soundManager.GetBGMTime(BGMName),BeatInterbal,BGMBPM)) {
            notesWaveForTimingBar = GetBeatWaveNum(soundManager.GetBGMTime(BGMName),BeatInterbal,BGMBPM);

            timingBars.Add(CreateTimingBar());

            //登録してあるtimingbarがnullになったら除外
            for(int i = timingBars.Count - 1;i >= 0;i--) {
                if(timingBars[i].GetComponent<TimingBar>().DestroyFlag) {
                    isBeatChange = true;
                    timingBars.RemoveAt(i);
                    for(int j = 0;j < timingBars.Count;j++) {
                    }
                }
            }
        }
    }

    //=============================================================
    //タイミングバーの作成
    private GameObject CreateTimingBar () {
        GameObject obj = Instantiate(Resources.Load("Prefabs/UI/TimingBar")) as GameObject;
        obj.transform.SetParent(GameObject.Find("Canvas/UpScreen/MainImage").transform,false);
        return obj;
    }

    //=============================================================
    //1ビート毎の秒数を取得
    public float GetTimePerBeats (float BPM) {
        return 60 / BPM;
    }

    //=============================================================
    //ビートウェイブを取得
    public float GetBeatWave (float time,int interval,float BPM) {
        return time / (GetTimePerBeats(BPM) * interval);
    }

    //=============================================================
    //ビートウェイブ(何Waveか)を取得
    public int GetBeatWaveNum (float time,int interval,float BPM) {
        return Mathf.FloorToInt(GetBeatWave(time,interval,BPM));
    }

    //=============================================================
    //ビートウェイブのタイミングを取得(0-1)
    public float GetBeatWaveTiming (float time,int interval,float BPM) {
        return GetBeatWave(time,interval,BPM) - GetBeatWaveNum(time,interval,BPM);
    }

    //=============================================================
    //直近のピースリンクを取得
    public int[,] GetLatestPieceLink () {
        if(timingBars.Count != 0) {
            return timingBars[0].transform.Find("PieceLink_UpScreen").GetComponent<PieceLink_UpScreen>().PieceLink;
        } else {
            //Debug.Log("ピースリンクがうまく取得できてないよ");
            int[,] tmp = { { -1,-1 },{ -1,-1 } };
            return tmp;
        }
    }

    //=============================================================
    //2番目のピースリンクを取得
    public int[,] GetNextPieceLink () {
        if(timingBars.Count >= 2) {
            return timingBars[1].transform.Find("PieceLink_UpScreen").GetComponent<PieceLink_UpScreen>().PieceLink;
        } else {
            //Debug.Log("ピースリンクが1個以下だよ");
            int[,] tmp = { { -1,-1 },{ -1,-1 } };
            return tmp;
        }
    }
}