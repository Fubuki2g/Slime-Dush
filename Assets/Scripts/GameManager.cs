using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [Header("ゲームの進行を示すグラフ")]
    public bool gameStart = false;    // ゲーム開始前
    public bool mainGame = false;     // ゲーム中
    public bool clearble = false;     // クリア可能状態
    public bool gameClear = false;    // ゲームクリア
    public bool gameOver = false;     // ゲームオーバー
    public bool state_damage = false; // ダメージ中

    [Header("デモ演出")]
    [SerializeField] PlayableDirector pd_gameStart; // ゲームスタート
    [SerializeField] PlayableDirector pd_gameClear; // ゲームクリア
    [SerializeField] PlayableDirector pd_gameOver;  // ゲームオーバー

    // クリア条件のアイテムの数を把握するための変数
    GameObject[] itemObject;
    public int itemCountCurrent, itemCountMax, shootitemCount;

    [Header("Itemのテキスト")]
    public TextMeshProUGUI currentText;
    public TextMeshProUGUI maxText;

    [Header("PlayerのHP")]
    public int HPCurrent = 10;
    public int HPMax = 10;

    [Header("ゲージのUI")]
    public Image hpGauge;
    float hpValue;

    [Header("時間計測")]
    public float timeCount = 60;
    public TextMeshProUGUI timeCountText;

    [Header("スキップ時に必要な設定")]
    [SerializeField] GameObject canvasMainGame;
    [SerializeField] GameObject canvasStartDemo;
    [SerializeField] GameObject pd_parent;
    [SerializeField] GameObject[] mainCamera;

    // [SerializeField] GameObject[] mainCamera; 変数利用でも可

    [Header("クリア表示用")]
    [SerializeField] GameObject canvasClearDemo;
    [SerializeField] GameObject gd_parent;

    [Header("ゲームオーバー表示用")]
    [SerializeField] GameObject canvasOverDemo;
    [SerializeField] GameObject ov_parent;

    // ポーズ中フラグ
    public bool pauseFLG;   // ポーズ中
    public bool hitStopFLG; // ヒットストップ中

    public bool clearFLG;
    public bool overFLG;


    [Header("キャンバス")]
    [SerializeField] GameObject[] canvas;

    [Header("カーソル初期位置")]
    [SerializeField] GameObject[] focus;


    GameObject currentFocus;  // 現在
    GameObject previousFocus; // 前フレーム


    void Start()
    {
        // クリアとゲームオーバーの非表示
        canvasClearDemo.SetActive(false);
        canvasOverDemo.SetActive(false);
        gd_parent.SetActive(false);
        ov_parent.SetActive(false);

        // スタートデモを再生
        pd_gameStart.Play();

        // Tagを検索、Itemタグのオブジェクト数を把握
        itemObject = GameObject.FindGameObjectsWithTag("Item");

        // アイテムの最大数 = オブジェクトの最大数
        itemCountMax = itemObject.Length;

        // アイテムの数をテキストで表示
        currentText.text = itemCountCurrent.ToString("000");
        maxText.text = itemCountMax.ToString("000");

        Default(0);

        mainCamera[1].SetActive(false);
        mainCamera[2].SetActive(false);
        mainCamera[3].SetActive(false);
        mainCamera[4].SetActive(false);
        mainCamera[5].SetActive(false);
        mainCamera[6].SetActive(false);
        mainCamera[7].SetActive(false);
        mainCamera[8].SetActive(false);
        mainCamera[9].SetActive(false);
        mainCamera[10].SetActive(false);
        mainCamera[11].SetActive(false);
        mainCamera[12].SetActive(false);
        mainCamera[13].SetActive(false);
        mainCamera[14].SetActive(false);
        mainCamera[15].SetActive(false);
        mainCamera[16].SetActive(false);
        mainCamera[17].SetActive(false);
        mainCamera[18].SetActive(false);
        mainCamera[19].SetActive(false);

        shootitemCount = 0;

        //初期化
        CanvasInit();

        //メインゲーム中のキャンバスだけアクティブ
        canvas[0].SetActive(true);

        clearFLG = false;
        overFLG = false;

    }


    void Update()
    {
        // HPが0にならないように処理
        //                Clamp(現在値,最小値,最大値)
        HPCurrent = Mathf.Clamp(HPCurrent, 0, HPMax);

        // HPが0になったらゲームオーバー
        if (HPCurrent <= 0 && !gameOver)
        {
            GameOver();
        }

        // スタートデモ中にキーを押したら演出スキップ
        if (pd_gameStart.state == PlayState.Playing && Input.GetKeyDown(KeyCode.Return))
        {
            DemoSkip(); // スキップ処理
        }


        if (gameClear == true)
        {
            ClearDemo();
        }

        if (gameOver == true)
        {
            OverDemo();
        }

        if(mainGame)
        {
            TimeCount();
        }

        if (!pauseFLG)
        {
            // escapeを押したら時間停止
            if (Input.GetKeyDown(KeyCode.Escape) && mainGame)
            {
                ChangePause(true);
                return;
            }

        }

        if (pauseFLG || clearFLG || overFLG)
        {
            FocusCheck();
        }

    }

    // アイテムを取得した時に呼び出すメソッド
    public void ItemGet()
    {
        itemCountCurrent++; // アイテムの現在値増える
        shootitemCount++;
        currentText.text = itemCountCurrent.ToString("000");
        
        

        // アイテムが最大値に到達したら
        if (itemCountCurrent >= itemCountMax)
        {
            clearble = true;
            Debug.Log("アイテムが最大になりました");
        }

    }

    // 外部からメインゲームのフラグを操作
    public void MainGameFLG(bool flg)
    {
        mainGame = flg;
    }

    // スタート演出のスキップ
    void DemoSkip()
    {
        pd_gameStart.Stop();

        canvasMainGame.SetActive(true);  // メインUI
        canvasStartDemo.SetActive(false); // デモ中UI
        pd_parent.SetActive(false);      // デモ中カメラ
        mainCamera[0].SetActive(true);
        mainCamera[1].SetActive(false);
        mainCamera[2].SetActive(false);
        mainCamera[3].SetActive(false);
        mainCamera[4].SetActive(false);
        mainCamera[5].SetActive(false);
        mainCamera[6].SetActive(false);
        mainCamera[7].SetActive(false);
        mainCamera[8].SetActive(false);
        mainCamera[9].SetActive(false);
        mainCamera[10].SetActive(false);
        mainCamera[11].SetActive(false);
        mainCamera[12].SetActive(false);
        mainCamera[13].SetActive(false);
        mainCamera[14].SetActive(false);
        mainCamera[15].SetActive(false);
        mainCamera[16].SetActive(false);
        mainCamera[17].SetActive(false);
        mainCamera[18].SetActive(false);
        mainCamera[19].SetActive(false);

        SoundManager.Instance.PlayBGM(0);
        mainGame = true;

    }

    // クリア時の演出表示
    void ClearDemo()
    {
        clearFLG = true;
        SoundManager.Instance.StopBGM();
        pd_gameClear.Play();

        canvasMainGame.SetActive(false);
        canvasClearDemo.SetActive(true);
        gd_parent.SetActive(true);
        EventSystem.current.SetSelectedGameObject(focus[1]);
        mainCamera[0].SetActive(false);
        mainCamera[1].SetActive(false);
        mainCamera[2].SetActive(false);
        mainCamera[3].SetActive(false);
        mainCamera[4].SetActive(false);
        mainCamera[5].SetActive(false);
        mainCamera[6].SetActive(false);
        mainCamera[7].SetActive(false);
        mainCamera[8].SetActive(false);
        mainCamera[9].SetActive(false);
        mainCamera[10].SetActive(false);
        mainCamera[11].SetActive(false);
        mainCamera[12].SetActive(false);
        mainCamera[13].SetActive(false);
        mainCamera[14].SetActive(false);
        mainCamera[15].SetActive(false);
        mainCamera[16].SetActive(false);
        mainCamera[17].SetActive(false);
        mainCamera[18].SetActive(false);
        mainCamera[19].SetActive(false);

        mainGame = false;
        
    }

    // ゲームオーバー時の演出表示
    void OverDemo()
    {
        overFLG = true;
        SoundManager.Instance.StopBGM();
        pd_gameOver.Play();

        canvasMainGame.SetActive(false);
        canvasOverDemo.SetActive(true);
        ov_parent.SetActive(true);
        EventSystem.current.SetSelectedGameObject(focus[2]);
        mainCamera[0].SetActive(false);
        mainCamera[1].SetActive(false);
        mainCamera[2].SetActive(false);
        mainCamera[3].SetActive(false);
        mainCamera[4].SetActive(false);
        mainCamera[5].SetActive(false);
        mainCamera[6].SetActive(false);
        mainCamera[7].SetActive(false);
        mainCamera[8].SetActive(false);
        mainCamera[9].SetActive(false);
        mainCamera[10].SetActive(false);
        mainCamera[11].SetActive(false);
        mainCamera[12].SetActive(false);
        mainCamera[13].SetActive(false);
        mainCamera[14].SetActive(false);
        mainCamera[15].SetActive(false);
        mainCamera[16].SetActive(false);
        mainCamera[17].SetActive(false);
        mainCamera[18].SetActive(false);
        mainCamera[19].SetActive(false);

        mainGame = false;
        
    }

    // ゲームクリアした時の処理
    public void GameClear()
    {
        clearFLG = true;
        gameClear = true;
        Debug.Log("ゲームクリア");

    }

    // 死んだときの処理
    void GameOver()
    {
        overFLG = true;
        gameOver = true;
        Debug.Log("ゲームオーバー");
    }

    // ダメージを受けた時に呼び出すメソッド
    public void Damage(float damage)
    {
        // HPを更新   int型に置き換える
        hpValue = (HPCurrent - damage) / HPMax;
        HPCurrent -= (int)damage;

        // HPバーを更新
        hpGauge.fillAmount = hpValue;

        // ダメージ中であることを示すフラグ
        state_damage = true;

        Debug.Log("HP = " + HPCurrent);


    }

    // ダメージを受けた時に呼び出すメソッド
    public void Shoot(float shoot)
    {
        // HPを更新   int型に置き換える
        hpValue = (HPCurrent - shoot) / HPMax;
        HPCurrent -= (int)shoot;

        // HPバーを更新
        hpGauge.fillAmount = hpValue;

    }

    // 回復するメソッド
    public void Heal(float heal)
    {
        hpValue = (HPCurrent + heal) / HPMax;
        HPCurrent += (int)heal;

        hpGauge.fillAmount = hpValue;


        Debug.Log("HPCurrent = " + HPCurrent);

    }

    // デフォルトHP
    public void Default(float def)
    {
        hpValue = (HPCurrent - def) / HPMax;
        HPCurrent -= (int)def;

        hpGauge.fillAmount = hpValue;


        Debug.Log("HPCurrent = " + HPCurrent);

    }

    // 時間を増やすメソッド
    void TimeCount()
    {
        timeCount += Time.deltaTime;

        timeCountText.text = timeCount.ToString("00.00");

    }

    // ポーズ処理
    public void ChangePause(bool flg)
    {
        CanvasInit(); // キャンバス全部消す
        pauseFLG = flg;

        // ポーズ中だったら時間停止
        if (flg)
        {
            Time.timeScale = 0;
            canvas[1].SetActive(true);

            // 初期カーソル位置設定
            EventSystem.current.SetSelectedGameObject(focus[0]);
        }
        else
        {
            Time.timeScale = 1;
            canvas[0].SetActive(true);
        }

    }



    //全てのキャンバスを非表示に
    void CanvasInit()
    {
        for (int i = 0; i < canvas.Length; i++)
        {
            canvas[i].SetActive(false);
        }
    }

    //フォーカスが外れていないかチェック
    void FocusCheck()
    {
        //現在のフォーカスを格納
        currentFocus = EventSystem.current.currentSelectedGameObject;

        //もし前回までのフォーカスと同じなら即終了
        if (currentFocus == previousFocus) return;

        //もしフォーカスが外れていたら
        //前フレームのフォーカスに戻す
        if (currentFocus == null)
        {
            EventSystem.current.SetSelectedGameObject(previousFocus);
            return;
        }

        //残された条件から、フォーカスが存在するのは確定
        //前フレームのフォーカスを更新
        previousFocus = EventSystem.current.currentSelectedGameObject;
    }

    //
    public void ReStart()
    {
        // timeScaleを元に戻してから
        ChangePause(false);

        FadeManager.Instance.LoadScene(SceneManager.GetActiveScene().name, 1);
    }

    public void Scene0()
    {
        ChangePause(false);
        SceneManager.LoadScene(0);
    }
}



