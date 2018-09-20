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
    //キャラクターデータ(構造体)
    public struct CharacterData {
        public int Id;
        public string Name;
        public string ActiveSkill;
        public string PassiveSkill;
        public INSTRUMENT_TYPE InstrumentType;
    }

    //キャラクターのステータス(構造体)
    public struct CharacterState {
        public int Id;
        public int Level;
        public float MaxHitPoint;
        public float HitPoint;
        public float AttackPower;
    }

    //キャラクターのステータス
    public CharacterState[] CharacterStatus = {
        new CharacterState{Id=0},
        new CharacterState{Id=1},
        new CharacterState{Id=2},
    };

    //エネミーのステータス
    public CharacterState[] EnemyStatus = {
        new CharacterState{Id=1000,}
    };


    public CharacterData[] CharacterDatas = {
        new CharacterData{Id=0, Name="kanade",ActiveSkill="Pitch Shift",PassiveSkill="Power Code",InstrumentType=INSTRUMENT_TYPE.GUITAR},
        new CharacterData{Id=1, Name="seira",ActiveSkill="Abandonne",PassiveSkill="Con Anima",InstrumentType=INSTRUMENT_TYPE.DJ}, //abandonne(感情のままに) con anima(魂をこめて)
        new CharacterData{Id=2, Name="???",ActiveSkill="Poly Rhythm",PassiveSkill="Ghost Note",InstrumentType=INSTRUMENT_TYPE.DRUM},
    };

    //=============================================================
    private SoundManager soundManager;

    //=============================================================
    [System.NonSerialized]
    public float BGMBPM = 128f; //tst001->171 bgm001->130 bgm002->128 bgm003->146
    [System.NonSerialized]
    public string BGMName = "bgm002";
    [System.NonSerialized]
    public int BeatInterbal = 8;
    [System.NonSerialized]
    public int FocusCharacter = 0; //フォーカスするキャラクター
    [System.NonSerialized]
    public int FocusEnemy = 0; //フォーカスするエネミー

    //=============================================================
    //一時停止しているかどうか
    private bool isPause;
    public bool IsPause {
        get { return isPause; }
        set { isPause = value; }
    }

    //=============================================================
    //ゲームオーバー状態かどうか
    private bool isGameOver;
    public bool IsGameOver {
        get { return isGameOver; }
        set { isGameOver = value; }
    }

    //=============================================================
    //ビートが変わったかどうか(タイミングバーが到達したかどうか)
    private bool isBeatChange;
    public bool IsBeatChange {
        get { return isBeatChange; }
        set { isBeatChange = value; }
    }

    //=============================================================
    private string beforeFrameSceneName; //前フレームのシーン
    private bool sceneJumpFlag; //シーンジャンプしたフラグ

    //=============================================================
    private bool Init () {
        if(this != Instance) {
            Destroy(this.gameObject);
            return false;
        }

        DontDestroyOnLoad(this.gameObject);

        CRef();
        return true;
    }

    //=============================================================
    private void CRef () {
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    //=============================================================
    //体力を計算する
    private int CalculateHitPoint (int level) {
        return 25 + (level - 1) * 5;
    }

    //攻撃力を計算する
    private int CalculateAttackPoint (int level) {
        return 5 * level;
    }

    //=============================================================
    //キャラクターステータスの初期化
    private void InitCharacterStatus () {
        for(int i = 0;i < CharacterStatus.Length;i++) {
            CharacterStatus[i].Level = 1;
            CharacterStatus[i].MaxHitPoint = 1;
            //CharacterStatus[i].MaxHitPoint = CalculateHitPoint(CharacterStatus[i].Level);
            CharacterStatus[i].HitPoint = CharacterStatus[i].MaxHitPoint;
            CharacterStatus[i].AttackPower = CalculateAttackPoint(CharacterStatus[i].Level);
        }
    }

    //=============================================================
    //エネミーステータスの初期化
    private void InitEnemyStatus () {
        for(int i = 0;i < EnemyStatus.Length;i++) {
            EnemyStatus[i].Level = 1;
            EnemyStatus[i].MaxHitPoint = CalculateHitPoint(CharacterStatus[i].Level);
            EnemyStatus[i].HitPoint = CharacterStatus[i].MaxHitPoint;
            EnemyStatus[i].AttackPower = CalculateAttackPoint(CharacterStatus[i].Level);
        }
    }

    //=============================================================
    private void Awake () {
        if(!Init()) return;

        beforeFrameSceneName = SceneManager.GetActiveScene().name; //シーン名前保存
        InitCharacterStatus(); //キャラクターステータスの初期化
        InitEnemyStatus(); //エネミーステータスの初期化

        switch(SceneManager.GetActiveScene().name) {
            case "CharacterSelect":
            break;

            case "Game":
            InitGame();
            break;

            case "Game_copy":
            InitGame();
            break;

            default:
            Debug.Log("謎のシーンだよ");
            break;
        }
    }

    private void Update () {
        //シーンが変化したらもう一回Awakeを呼びだす
        if(!SceneManager.GetActiveScene().name.Equals(beforeFrameSceneName)) {
            sceneJumpFlag = true;
        }

        if(sceneJumpFlag) {
            Awake();
            sceneJumpFlag = false;
        }

        switch(SceneManager.GetActiveScene().name) {
            case "CharacterSelect":
            break;

            case "Game":
            RoutineGame();
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
    //シーン遷移(キャラクターセレクトからゲーム)
    public void JumpSceneCharacterSelectToGame () {
        SceneManager.LoadScene("Game_copy");
    }

    //==============================================================================================================================================
    //Gameシーン
    //==============================================================================================================================================
    private List<GameObject> timingBars = new List<GameObject>();
    private int notesWaveForTimingBar;
    private bool onceFlagGamePause; //ゲームポーズ時に一回だけ使いたい処理を挟むときのためのフラグ

    private BoardManager boardManager; //ボードマネージャー
    private int beforeCombo; //コンボ数

    //=============================================================
    private void CRefGame () {
        boardManager = GameObject.Find("BoardManager").GetComponent<BoardManager>();
    }

    //=============================================================
    private void InitGame () {
        CRefGame();

        soundManager.TriggerBGM(BGMName,false);
        //タイミングバー用のウェーブ指定
        notesWaveForTimingBar = GetBeatWaveNum(soundManager.GetBGMTime(BGMName),BeatInterbal,BGMBPM);
    }

    //=============================================================
    private void RoutineGame () {
        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        //なんでか参照が切れるからこの処理をいれた(時間があったら探したい)
        if(boardManager==null) {
            boardManager= GameObject.Find("BoardManager").GetComponent<BoardManager>();
        }
        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||

        //ゲームオーバーなら処理をスキップ
        if(isGameOver) {
            return;
        }

        //ポーズ状態でないなら
        if(!isPause) {
            //BGMのポーズ状態を解除する
            if(onceFlagGamePause) {
                soundManager.UnPauseBGM(BGMName);
                onceFlagGamePause = false;
            }

            //Debug.Log("ボードマネージャー:"+boardManager);
            //Debug.Log(boardManager.Combo);

            //タイミングバー生成
            if(notesWaveForTimingBar != GetBeatWaveNum(soundManager.GetBGMTime(BGMName),BeatInterbal,BGMBPM)) {
                notesWaveForTimingBar = GetBeatWaveNum(soundManager.GetBGMTime(BGMName),BeatInterbal,BGMBPM);

                timingBars.Add(CreateTimingBar());

                //登録してあるtimingbarがnullになったら除外
                for(int i = timingBars.Count - 1;i >= 0;i--) {
                    if(timingBars[i].GetComponent<TimingBar>().DestroyFlag) {
                        isBeatChange = true;
                        timingBars.RemoveAt(i);

                        //コンボ数が0だったらダメージ
                        if(beforeCombo == 0) {
                            Debug.LogError("ダメージ:" + beforeCombo + ":" + boardManager.Combo);
                            ApplyToHitPoint(FocusCharacter,-EnemyStatus[FocusEnemy].AttackPower);
                        }
                    }
                }
            }

            CheckGameOver();
            beforeCombo = boardManager.Combo; //コンボ数を保存

        } else { //ポーズ状態なら
            //BGMをポーズ状態にする
            if(!onceFlagGamePause) {
                soundManager.PauseBGM(BGMName);
                onceFlagGamePause = true;
            }
        }
    }

    //=============================================================
    //体力値に数値を適用する
    private void ApplyToHitPoint (int id,float num) {
        CharacterStatus[id].HitPoint += num;
    }

    //=============================================================
    //ゲームオーバーかどうかを判別する
    private void CheckGameOver () {
        //キャラクターのHPが0以下だったらゲームオーバー
        if(CharacterStatus[FocusCharacter].HitPoint <= 0) {
            isGameOver = true;
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
            if(timingBars[0] != null) {
                return timingBars[0].transform.Find("PieceLink_UpScreen").GetComponent<PieceLink_UpScreen>().PieceLink;
            } else {
                //Debug.Log("ピースリンクがうまく取得できてないよ");
                int[,] tmp = { { -1,-1 },{ -1,-1 } };
                return tmp;
            }
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

    //=============================================================
    //シーン遷移(ゲームからゲームへ)
    public void JumpSceneGameToGame () {
        InitCharacterStatus(); //キャラクターステータスの初期化
        InitEnemyStatus(); //エネミーステータスの初期化
        isGameOver = false; //ゲームオーバーフラグの初期化
        isPause = false; //ポーズフラグの初期化
        timingBars.Clear(); //タイミングバーの参照の初期化
        sceneJumpFlag = true; //明示的にシーン遷移フラグを立たせる

        soundManager.StopBGM(BGMName);
        SceneManager.LoadScene("Game_copy");
    }
}