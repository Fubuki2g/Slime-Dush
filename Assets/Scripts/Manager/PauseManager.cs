using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;    // 追加
using UnityEngine.SceneManagement; // 追加

public class PauseManager : Singleton<PauseManager>
{
    // ポーズ中フラグ
    public bool pauseFLG;   // ポーズ中
    public bool hitStopFLG; // ヒットストップ中


    [Header("キャンバス")]
    [SerializeField] GameObject[] canvas;

    [Header("ポーズメニューのカーソル初期位置")]
    [SerializeField] GameObject focusPauseMenu;

    [Header("ヒットストップ")]
    [SerializeField] float timeScalse = 0.1f;
    [SerializeField] float slowTime = 1f;
    float currentTime;

    // フォーカスが外れないようにする処理用
    GameObject currentFocus;  // 現在
    GameObject previousFocus; // 前フレーム


    void Start()
    {
        //初期化
        CanvasInit();

        //メインゲーム中のキャンバスだけアクティブ
        canvas[0].SetActive(true);

    }

    
    void Update()
    {
        // 処理が止まっているわけではないのでデバッグログは流れ続ける
        // Debug.Log("処理をしている");

        // ポーズ中じゃない時のみボタンを受け付ける
        if (!pauseFLG)
        {
            // Pを押したら時間停止
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ChangePause(true);
                return;
            }

            /* Oを押したらヒットストップ
            if (Input.GetKeyDown(KeyCode.O))
            {
                HitStopStart();
                return;
            }

            // ヒットストップ中の時間計測
            HitStopTime();*/

        }


        //ポーズ中のみ
        //フォーカスが外れていないかチェック
        if (pauseFLG || GameManager.Instance.gameClear || GameManager.Instance.gameOver)
        {
            FocusCheck();
            Debug.Log("pause");

        }

    }

    // FixedUpdateは止まる処理
    /*void FixedUpdate()
    {
        Debug.Log("Fix処理をしている");
    }*/

    // ヒットストップ開始
    void HitStopStart()
    {
        currentTime = 0f;
        Time.timeScale = timeScalse;
        hitStopFLG = true;
    }

    // ヒットストップ時間計測
    void HitStopTime()
    {
        if (hitStopFLG)
        {
            currentTime += Time.unscaledDeltaTime;

            // 時間経過で元の早さに
            if (currentTime >= slowTime)
            {
                Time.timeScale = 1f;
                hitStopFLG = false;
            }

        }
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
            EventSystem.current.SetSelectedGameObject(focusPauseMenu);
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
