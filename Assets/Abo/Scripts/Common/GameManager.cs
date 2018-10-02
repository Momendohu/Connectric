using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public Sprite[] CharacterImageDamage; //キャラクターイメージ(ダメージ)
    public Sprite[] CharacterImageSmile; //キャラクターイメージ(スキル)

    //=============================================================
    //キャラクターデータ(構造体)
    public struct CharacterData {
        public int Id;
        public string Name;
        public int SkillId;
        public INSTRUMENT_TYPE InstrumentType;
    }

    //キャラクターのステータス(構造体)
    public struct CharacterState {
        public int Id; //ID
        public int Level; //レベル

        public float MaxHitPoint; //最大体力
        public float HitPoint; //体力
        public float AttackPower; //攻撃力

        public float MaxVoltage; //最大ボルテージ(スキル)
        public float Voltage; //ボルテージ(スキル)
        public float Tension; //テンション(ボルテージ上昇値)
    }

    //ゲーム中の記録系ステータス(構造体)
    public struct GameRecordState {
        public int Combo; //コンボ数
        public int MaxCombo; //最大コンボ数
        public int MaxHit; //最大ヒット数
        public int Score; //スコア
        public int DefeatEnemyNum; //撃破した敵の数

        public int SeparateCombo; //区切りコンボ数
        public int SeparateComboSeparateNum; //区切りコンボ数の区切り
    }

    //スキルデータ(構造体)
    public struct SkillData {
        public int Id; //id
        public string Name; //名前
        public string Description; //説明
    }

    //=============================================================
    //キャラクターのステータス
    public CharacterState[] CharacterStatus = {
        new CharacterState{Id=0},
        new CharacterState{Id=1},
        new CharacterState{Id=2},
    };

    //エネミーのステータス
    public CharacterState[] EnemyStatus = {
        new CharacterState{Id=0,}
    };

    //ゲームレコード
    public GameRecordState GameRecordStatus = new GameRecordState { };

    //スキルデータ
    public SkillData[] SkillDatas = {
        new SkillData{ Id=0,Name="ピッチシフト",Description="敵に与えるダメージを1.5倍にして、曲の速さを1.1倍にする(デメリット)"},
        new SkillData{ Id=1,Name="アバンドーネ",Description="レインボーピースを大量発生させる"},
        new SkillData{ Id=2,Name="ゴーストノート",Description="攻撃を2重ヒットさせる"}
    };

    //キャラクターデータ
    public CharacterData[] CharacterDatas = {
        new CharacterData{Id=0, Name="カナデ",SkillId=0,InstrumentType=INSTRUMENT_TYPE.GUITAR},
        new CharacterData{Id=1, Name="セイラ",SkillId=1,InstrumentType=INSTRUMENT_TYPE.DJ}, //abandonne(感情のままに) con anima(魂をこめて)
        new CharacterData{Id=2, Name="ヒビカ",SkillId=2,InstrumentType=INSTRUMENT_TYPE.DRUM},
    };

    //エネミーデータ
    public CharacterData[] EnemyDatas = {
        new CharacterData{Id=0, Name="enemy"},
    };

    //=============================================================
    private SoundManager soundManager;

    //=============================================================
    [System.NonSerialized]
    public float BGMBPM = 128f; //tst001->171 bgm001->126 bgm002->146
    [System.NonSerialized]
    public string BGMName = "bgm001";
    [System.NonSerialized]
    public int BeatInterbal = 8;
    [System.NonSerialized]
    public int FocusCharacter = 0; //フォーカスするキャラクター
    [System.NonSerialized]
    public int FocusEnemy = 0; //フォーカスするエネミー
    [System.NonSerialized]
    public int FocusBGM = 0;

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
    //ゲームクリア状態かどうか
    private bool isGameClear;
    public bool IsGameClear {
        get { return isGameClear; }
        set { isGameClear = value; }
    }

    //=============================================================
    //スキルモードかどうか
    private bool isSkillMode;
    public bool IsSkillMode {
        get { return isSkillMode; }
        set { isSkillMode = value; }
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
        return 50 + (level - 1) * 5;
    }

    //攻撃力を計算する
    private int CalculateAttackPoint (int level) {
        return 5 * level;
    }

    //=============================================================
    //取得スコアを計算する
    private int CalculateGetScore (int hit,int combo) {
        return hit * 300 + combo * 100;
    }

    //=============================================================
    //BGMの再生初期化
    private void InitMusicPlay () {
        for(int i = 0;i < soundManager.BGMNum;i++) {
            soundManager.StopBGM(soundManager.BGMDatas[i].Name);
        }
    }

    //=============================================================
    private void Awake () {
        if(!Init()) return;

        beforeFrameSceneName = SceneManager.GetActiveScene().name; //シーン名前保存
        InitCharacterStatus(); //キャラクターステータスの初期化
        InitEnemyStatus(); //エネミーステータスの初期化

        switch(SceneManager.GetActiveScene().name) {
            case "Title":
            InitTitle();
            break;

            case "Home":
            break;

            case "SelectSound":
            InitSelectSound();
            break;

            case "CharacterSelect":
            InitCharacterSelect();
            break;

            case "Game":
            InitGame();
            break;

            case "Game_copy2":
            InitGame();
            break;

            case "Game_copy3":
            InitGame();
            break;

            case "Result":
            break;

            default:
            Debug.Log("謎のシーンだよ");
            break;
        }
    }

    private void Update () {
        TapEffect();

        //シーンが変化したらもう一回Awakeを呼びだす
        if(!SceneManager.GetActiveScene().name.Equals(beforeFrameSceneName)) {
            sceneJumpFlag = true;
        }

        if(sceneJumpFlag) {
            Awake();
            sceneJumpFlag = false;
        }

        switch(SceneManager.GetActiveScene().name) {
            case "Title":
            RoutineTitle();
            break;

            case "Home":
            break;

            case "SelectSound":
            break;

            case "CharacterSelect":
            break;

            case "Game":
            RoutineGame();
            break;

            case "Game_copy2":
            RoutineGame();
            break;

            case "Game_copy3":
            RoutineGame();
            break;

            case "Result":
            break;

            default:
            Debug.Log("謎のシーンだよ");
            break;
        }
    }

    //==============================================================================================================================================
    //Titleシーン
    //==============================================================================================================================================
    private GameObject text_tapToStart;
    private GameObject text_tokyoBTeam;

    //=============================================================
    private bool isTitleAppeared; //タイトルが出現済かどうか
    public bool IsTitleAppeared {
        get { return isTitleAppeared; }
        set { isTitleAppeared = value; }
    }

    //=============================================================
    private void CRefTitle () {
        text_tapToStart = GameObject.Find("Canvas/TapToStart").gameObject;
        text_tokyoBTeam = GameObject.Find("Canvas/TokyoBTeam").gameObject;
    }

    //=============================================================
    private void InitTitle () {
        CRefTitle();

        text_tapToStart.SetActive(false);
        text_tokyoBTeam.SetActive(false);
    }

    //=============================================================
    private void RoutineTitle () {
        if(TouchUtil.GetTouch() == TouchUtil.TouchInfo.Began) {
            JumpSceneTitleToHome();
        }

        //タイトルが出現したら
        if(isTitleAppeared) {
            text_tapToStart.SetActive(true);
            text_tokyoBTeam.SetActive(true);
        }
    }

    //=============================================================
    //シーン遷移(タイトルからホームへ)
    private void JumpSceneTitleToHome () {
        if(isTitleAppeared) {
            SceneManager.LoadScene("Home");
        }
    }

    //==============================================================================================================================================
    //Homeシーン
    //==============================================================================================================================================
    public void JumpSceneHomeToSelectSound () {
        SceneManager.LoadScene("SelectSound");
    }

    //==============================================================================================================================================
    //SelectSoundシーン
    //==============================================================================================================================================
    private void InitSelectSound () {
        InitMusicPlay();
    }

    //=============================================================
    //シーン遷移(曲セレクトからキャラクターセレクト)
    public void JumpSceneSelectSoundToCharacterSelect () {
        SceneManager.LoadScene("CharacterSelect");
    }

    //=============================================================
    //BGMを選択適用する
    public void ApplyToBGMData (int num) {
        if(num >= 0 && num <= soundManager.BGMNum - 1) {
            FocusBGM = num;
        } else {
            FocusBGM = 0;
        }

        BGMBPM = soundManager.BGMDatas[FocusBGM].BPM;
        BGMName = soundManager.BGMDatas[FocusBGM].Name;
    }

    //==============================================================================================================================================
    //CharacterSelectシーン
    //==============================================================================================================================================
    private void InitCharacterSelect () {
        InitMusicPlay();
    }

    //=============================================================
    //シーン遷移(キャラクターセレクトからゲーム)
    public void JumpSceneCharacterSelectToGame () {
        SceneManager.LoadScene("Game_copy2");
    }

    //==============================================================================================================================================
    //Gameシーン
    //==============================================================================================================================================
    private List<GameObject> timingBars = new List<GameObject>();
    private int notesWaveForTimingBar;
    private bool onceFlagGamePause; //ゲームポーズ時に一回だけ使いたい処理を挟むときのためのフラグ

    private BoardManager boardManager; //ボードマネージャー
    private int beforeCombo; //前フレームのコンボ数(タイミングバー消滅感知から消すまでの流れで1Fかかる可能性を考慮)

    private bool onceFlagSkillActivate; //スキルが発動したときに一回だけ動作させる処理のためのフラグ

    //=============================================================
    private void CRefGame () {
        //boardManager_copy = GameObject.Find("BoardManager_copy").GetComponent<BoardManager_copy>();
        boardManager = GameObject.Find("BoardManager").GetComponent<BoardManager>();
    }

    //=============================================================
    private void InitGame () {
        CRefGame();

        soundManager.TriggerBGM(BGMName,false);
        //タイミングバー用のウェーブ指定
        notesWaveForTimingBar = GetBeatWaveNum(soundManager.GetBGMTime(BGMName),BeatInterbal,BGMBPM);

        InitializeGameRecordStatus();
        InitializeGameStatus();
    }

    //=============================================================
    private void RoutineGame () {
        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        //ゲームシーンリロード時になんでか参照が2フレーム以降に切れるからこの処理をいれた(時間があったら原因探したい)
        if(boardManager == null) {
            boardManager = GameObject.Find("BoardManager").GetComponent<BoardManager>();
        }

        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||

        //ゲームオーバー、ゲームクリアなら処理をスキップ
        if(isGameOver || isGameClear) {
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

            //タイミングバー生成
            if(notesWaveForTimingBar != GetBeatWaveNum(soundManager.GetBGMTime(BGMName),BeatInterbal,BGMBPM)) {
                notesWaveForTimingBar = GetBeatWaveNum(soundManager.GetBGMTime(BGMName),BeatInterbal,BGMBPM);

                timingBars.Add(CreateTimingBar());

                //登録してあるtimingbarのDestroyフラグがたったら除外
                for(int i = timingBars.Count - 1;i >= 0;i--) {
                    if(timingBars[i].GetComponent<TimingBar>().DestroyFlag) {
                        isBeatChange = true;
                        timingBars.RemoveAt(i);


                        if(beforeCombo == 0) {
                            Damage();
                        } else {
                            Attack(beforeCombo);
                            StartCoroutine(SkillEffectForAttack(CharacterDatas[FocusCharacter].SkillId,beforeCombo)); //スキルに応じて処理
                        }
                    }
                }
            }

            CheckGameOver();
            CheckGameClear();
            CheckSkillMode();

            if(isSkillMode) {
                //一回だけカットインを作成する
                if(!onceFlagSkillActivate) {
                    StartCoroutine(SkillEffect(CharacterDatas[FocusCharacter].SkillId)); //スキルの発動
                    onceFlagSkillActivate = true;
                }
            }

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
    //ダメージ処理
    private void Damage () {
        //Debug.Log(EnemyDatas[FocusEnemy].Name + "の攻撃! " + CharacterDatas[FocusCharacter].Name + "に" + EnemyStatus[FocusEnemy].AttackPower + "のダメージ!");
        //ダメージを受ける
        ApplyToCharacterHitPoint(FocusCharacter,-EnemyStatus[FocusEnemy].AttackPower);

        //ボルテージの上昇
        ApplyToEnemyVoltage(FocusEnemy);

        //コンボが0に
        GameRecordStatus.Combo = 0;
        GameRecordStatus.SeparateCombo = 0;
    }

    //=============================================================
    //攻撃
    private void Attack (int hit) {
        //SEを鳴らす
        soundManager.TriggerSE("puzzledelete");

        //ダメージを与える
        //Debug.Log(CharacterDatas[FocusCharacter].Name + "の攻撃! " + EnemyDatas[FocusEnemy].Name + "に" + CharacterStatus[FocusCharacter].AttackPower + "のダメージ!");
        ApplyToEnemyHitPoint(FocusEnemy,-CharacterStatus[FocusCharacter].AttackPower);

        //ボルテージの上昇
        ApplyToCharacterVoltage(FocusCharacter);

        //スコアの計算
        ApplyToScore(CalculateGetScore(hit,GameRecordStatus.Combo));

        //ヒット数の表示 + コンボ数の加算
        CreateHitDisplayer(hit);
        GameRecordStatus.Combo += hit;

        //区切りコンボ数の計算、下部にコンボの表示
        GameRecordStatus.SeparateCombo += hit;
        if(GameRecordStatus.SeparateCombo >= GameRecordStatus.SeparateComboSeparateNum) {
            GameRecordStatus.SeparateCombo -= GameRecordStatus.SeparateComboSeparateNum;
            CreateComboUnder((GameRecordStatus.Combo - GameRecordStatus.SeparateCombo));
        }

        GameRecordStatus.MaxCombo = (GameRecordStatus.MaxCombo < GameRecordStatus.Combo) ? GameRecordStatus.Combo : GameRecordStatus.MaxCombo;
        GameRecordStatus.MaxHit = (GameRecordStatus.MaxHit < hit) ? hit : GameRecordStatus.MaxHit;
    }

    //=============================================================
    //特定のスキルが発動しているかどうか
    public bool IsUsingSkill (int num) {
        return IsSkillMode && (CharacterDatas[FocusCharacter].SkillId == num);
    }

    //=============================================================
    //スキルの処理(攻撃時)
    private IEnumerator SkillEffectForAttack (int num,int hit) {
        //スキルモードなら
        if(IsSkillMode) {
            switch(num) {
                case 0:
                break;

                case 1:
                break;

                case 2:
                yield return MyWaitForSeconds(0.3f);
                Attack(hit);
                break;

                default:
                Debug.Log("謎のスキルだよ");
                break;
            }
        }

        yield return null;
    }

    //=============================================================
    //スキルの処理
    private IEnumerator SkillEffect (int num) {
        CreateCutIn();

        switch(num) {
            case 0:
            CharacterStatus[FocusCharacter].AttackPower *= 1.2f; //攻撃力を1.2倍に
            soundManager.SetBGMPitch(BGMName,1.1f); //速度(ピッチ)を1.1倍に
            //カウントダウンない
            yield return MyWaitForSeconds(15); //15秒待つ

            //効果を元にもどす
            CharacterStatus[FocusCharacter].AttackPower /= 1.2f;
            soundManager.SetBGMPitch(BGMName,1f);
            break;

            case 1:
            yield return MyWaitForSeconds(20); //20秒待つ
            break;

            case 2:
            yield return MyWaitForSeconds(15); //15秒待つ
            break;

            default:
            Debug.Log("謎のスキルだよ");
            break;
        }

        onceFlagSkillActivate = false;
        isSkillMode = false;
        CharacterStatus[FocusCharacter].Voltage = 0;
    }

    //=============================================================
    //指定した秒数待つ(ゲームフラグでの管理用に作成)
    private IEnumerator MyWaitForSeconds (float waitTime) {
        float time = 0;
        while(true) {
            time += TimeForGame();
            if(time >= waitTime) {
                break;
            }

            yield return null;
        }
    }

    //=============================================================
    //ゲームシーン管理下のTime
    public float TimeForGame () {
        if(IsGameClear || IsGameOver || IsPause) {
            return 0;
        } else {
            return Time.fixedDeltaTime;
        }
    }

    //=============================================================
    //プレイヤー体力値に数値を適用する
    private void ApplyToCharacterHitPoint (int id,float num) {
        CharacterStatus[id].HitPoint += num;
    }

    //=============================================================
    //エネミー体力値に数値を適用する
    private void ApplyToEnemyHitPoint (int id,float num) {
        EnemyStatus[id].HitPoint += num;
    }

    //=============================================================
    //プレイヤーのスキルボルテージに数値を適用する
    private void ApplyToCharacterVoltage (int id) {
        CharacterStatus[id].Voltage += CharacterStatus[id].Tension;
    }

    //=============================================================
    //エネミーのスキルボルテージに数値を適用する
    private void ApplyToEnemyVoltage (int id) {
        EnemyStatus[id].Voltage += EnemyStatus[id].Tension;
    }

    //=============================================================
    //スコアに数値を適用する
    private void ApplyToScore (int num) {
        GameRecordStatus.Score += num;
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
    //ゲームクリアかどうかを判別する
    private void CheckGameClear () {
        //曲が終わりに限りなく近づいたらゲームクリア(雑な実装)
        //Debug.Log(soundManager.GetBGMTimeLength(BGMName) - soundManager.GetBGMTime(BGMName));
        if(soundManager.GetBGMTimeLength(BGMName) - soundManager.GetBGMTime(BGMName) <= 0.1f) {
            isGameClear = true;
        }
    }

    //=============================================================
    //スキルモードかどうかを判断する
    private void CheckSkillMode () {
        //ボルテージが最大に達したら
        if(CharacterStatus[FocusCharacter].Voltage >= CharacterStatus[FocusCharacter].MaxVoltage) {
            isSkillMode = true;
        }
    }

    //=============================================================
    //タイミングバーの作成
    private GameObject CreateTimingBar () {
        GameObject obj = Instantiate(Resources.Load("Prefabs/UI/TimingBar")) as GameObject;
        obj.transform.SetParent(GameObject.Find("Canvas/UpScreen").transform,false);
        obj.transform.SetSiblingIndex(GameObject.Find("Canvas/UpScreen/TimingBars/TimingBarGoal").transform.GetSiblingIndex() + 1);
        return obj;
    }

    //=============================================================
    //ヒット数表示オブジェクトの生成
    private void CreateHitDisplayer (int num) {
        GameObject obj = Instantiate(Resources.Load("Prefabs/UI/HitDisplayer")) as GameObject;
        obj.transform.SetParent(GameObject.Find("Canvas/UpScreen").transform,false);
        obj.transform.SetAsLastSibling();

        obj.GetComponent<HitDisplayer>().HitNum = num;
    }

    //=============================================================
    //下部のコンボ数表示の生成
    private void CreateComboUnder (int num) {
        GameObject obj = Instantiate(Resources.Load("Prefabs/UI/ComboUnder")) as GameObject;
        obj.transform.SetParent(GameObject.Find("Canvas/UpScreen").transform,false);
        obj.transform.SetAsLastSibling();

        obj.GetComponent<ComboUnder>().Combo = num;
    }

    //=============================================================
    //カットインの作成
    private void CreateCutIn () {
        GameObject obj = Instantiate(Resources.Load("Prefabs/UI/CutIn")) as GameObject;
        obj.transform.SetParent(GameObject.Find("Canvas").transform,false);
        obj.transform.SetAsLastSibling();
        obj.GetComponent<CutIn>().DisplayText = SkillDatas[CharacterDatas[FocusCharacter].SkillId].Name; //スキル名の取得
        obj.GetComponent<CutIn>().Id = CharacterDatas[FocusCharacter].Id; //キャラクターIDの指定
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
            if(timingBars[1] != null) {
                return timingBars[1].transform.Find("PieceLink_UpScreen").GetComponent<PieceLink_UpScreen>().PieceLink;
            } else {
                //Debug.Log("ピースリンクがうまく取得できてないよ");
                int[,] tmp = { { -1,-1 },{ -1,-1 } };
                return tmp;
            }
        } else {
            //Debug.Log("ピースリンクが1個以下だよ");
            int[,] tmp = { { -1,-1 },{ -1,-1 } };
            return tmp;
        }
    }

    //=============================================================
    //一番新しく生成されたピースリンクを取得
    public int[,] GetNewestPieceLink () {
        if(timingBars.Count >= 1) {
            if(timingBars[timingBars.Count - 1] != null) {
                return timingBars[timingBars.Count - 1].transform.Find("PieceLink_UpScreen").GetComponent<PieceLink_UpScreen>().PieceLink;
            } else {
                //Debug.Log("ピースリンクがうまく取得できてないよ");
                int[,] tmp = { { -1,-1 },{ -1,-1 } };
                return tmp;
            }
        } else {
            //Debug.Log("ピースリンクが1個以下だよ");
            int[,] tmp = { { -1,-1 },{ -1,-1 } };
            return tmp;
        }
    }

    //=============================================================
    //シーン遷移(ゲームからゲームへ)
    public void JumpSceneGameToGame () {
        InitializeGameStatus();
        InitializeGameRecordStatus();

        SceneManager.LoadScene("Game_copy2");
    }

    //=============================================================
    //シーン遷移(ゲームからリザルトへ)
    public void JumpSceneGameToResult () {
        InitializeGameStatus();

        SceneManager.LoadScene("Result");
    }

    //=============================================================
    //シーン遷移(ゲームから曲セレクトへ)
    public void JumpSceneGameToMusicSelect () {
        InitializeGameStatus();
        InitializeGameRecordStatus();

        SceneManager.LoadScene("SelectSound");
    }

    //==============================================================================================================================================
    //Resultシーン
    //==============================================================================================================================================
    //シーン遷移(リザルトから曲セレクトへ)
    public void JumpSceneResultToMusicSelect () {
        InitializeGameRecordStatus();
        SceneManager.LoadScene("SelectSound");
    }

    //==============================================================================================================================================
    //初期化系
    //==============================================================================================================================================
    //キャラクターステータスの初期化
    private void InitCharacterStatus () {
        for(int i = 0;i < CharacterStatus.Length;i++) {
            CharacterStatus[i].Level = 1;
            //CharacterStatus[i].MaxHitPoint = 1;
            CharacterStatus[i].MaxHitPoint = CalculateHitPoint(CharacterStatus[i].Level);
            CharacterStatus[i].HitPoint = CharacterStatus[i].MaxHitPoint;
            CharacterStatus[i].AttackPower = CalculateAttackPoint(CharacterStatus[i].Level);
            CharacterStatus[i].MaxVoltage = 100;
            CharacterStatus[i].Voltage = 0;
            CharacterStatus[i].Tension = 100;
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
            EnemyStatus[i].MaxVoltage = 100;
            EnemyStatus[i].Voltage = 0;
            EnemyStatus[i].Tension = 10;
        }
    }

    //=============================================================
    //ゲームプレイ系のステータス初期化
    private void InitializeGameStatus () {
        InitCharacterStatus(); //キャラクターステータスの初期化
        InitEnemyStatus(); //エネミーステータスの初期化
        isGameOver = false; //ゲームオーバーフラグの初期化
        isGameClear = false; //ゲームクリアフラグの初期化
        isPause = false; //ポーズフラグの初期化
        timingBars.Clear(); //タイミングバーの参照の初期化
        sceneJumpFlag = true; //明示的にシーン遷移フラグを立たせる
    }

    //=============================================================
    //ゲームレコード系のステータス初期化
    private void InitializeGameRecordStatus () {
        GameRecordStatus.Combo = 0;
        GameRecordStatus.MaxCombo = 0;
        GameRecordStatus.MaxHit = 0;
        GameRecordStatus.Score = 0;

        GameRecordStatus.SeparateCombo = 0;
        GameRecordStatus.SeparateComboSeparateNum = 10;
    }

    //==============================================================================================================================================
    //タップエフェクト
    //==============================================================================================================================================
    //タップした位置にエフェクトを生成する
    private void TapEffect () {
        if(TouchUtil.GetTouch() == TouchUtil.TouchInfo.Began) {
            Camera cam = GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<Camera>();
            if(cam != null) {
                Vector3 touchPosition = TouchUtil.GetTouchWorldPosition(cam);
                CreateTapEffect(new Vector3(touchPosition.x,touchPosition.y,0));
                //Debug.Log(TouchUtil.GetTouchWorldPosition(cam));
            }
        }
    }

    //=============================================================
    //タップエフェクトの生成
    private void CreateTapEffect (Vector3 pos) {
        GameObject obj = Instantiate(Resources.Load("Prefabs/Effects/TapEffect")) as GameObject;
        obj.transform.SetParent(GameObject.Find("Canvas").transform,false);
        obj.transform.position = pos;
        obj.GetComponent<ParticleSystem>().Play();
    }
}