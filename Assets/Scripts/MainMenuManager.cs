using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // 追加

public class MainMenuManager : MonoBehaviour
{
    [Header("各Canvas")]
    [SerializeField] GameObject[] canvas;

    [Header("各メニューの初期カーソルの対象")]
    [SerializeField] GameObject[] focusObject;

    [Header("各キャンバスのキャンバスグループ")]
    [SerializeField] CanvasGroup[] canvasGroup;

    // フェードにかかる時間
    [SerializeField] float fadeTime = 1;

    // フォーカスが外れてしまわないようにする処理
    // 正確に言えば外れた後に元に戻す処理
    GameObject currentFocus;  // 現在
    GameObject previousFocus; // 前フレーム


    void Start()
    {
        // 初期化
        CanvasInit();

        // メインメニューだけアクティブ
        canvas[1].SetActive(true);

        // 初期カーソルを設定
        EventSystem.current.SetSelectedGameObject(focusObject[1]);

    }

    
    void Update()
    {
        FocusCheak(); // フォーカスが外れていないかチェック

    }
    
    // すべてのキャンバスを非表示に
    void CanvasInit()
    {
        for(int i = 0; i < canvas.Length; i++)
        {
            canvas[i].SetActive(false);
        }
    }

    // キャンバスの切り替え
    public void Transition(int nextCanvas)
    {
        // 一旦すべてのキャンバスを非表示
        CanvasInit();

        // 次のキャンバスを表示
        canvas[nextCanvas].SetActive(true);

        // フェードインの処理
        StartCoroutine(FadeIn(nextCanvas));

        // 次のキャンバスの初期カーソル位置を設定
        EventSystem.current.SetSelectedGameObject(focusObject[nextCanvas]);

    }

    // フォーカスが外れていないかチェック
    void FocusCheak()
    {
        // 現在のフォーカスを格納
        currentFocus = EventSystem.current.currentSelectedGameObject;

        // もし前回までのフォーカスと同じなら即終了
        if (currentFocus == previousFocus) return;

        // もしフォーカスが外れていたら
        // 前フレームのフォーカスに戻す
        if (currentFocus == null)
        {
            EventSystem.current.SetSelectedGameObject(previousFocus);
            return;
        }

        // 残された条件から、フォーカスが存在するのは確定
        // 前フレームのフォーカスを更新
        previousFocus = EventSystem.current.currentSelectedGameObject;

    }

    // Canvasを表示する時に徐々にフェードインさせる
    IEnumerator FadeIn(int number)
    {
        // フェードが終わるまでは操作不可にする
        canvasGroup[number].interactable = false;
        float time = 0;

        // 条件を満たすまで処理を繰り返す
        while(time <= fadeTime)
        {
            //                                     出発点, 到着点, 経過時間
            canvasGroup[number].alpha = Mathf.Lerp(0f, 1f, time / fadeTime);
            time += Time.deltaTime;
            // これがないとCoroutineは使えない
            yield return null; // 1フレーム処理を待つ
            // yield return new WaitForSeconds(秒数); // 特定の秒数処理を待つ(メインゲームを作るときは適さない、バグをおこしやすい)

        }

        canvasGroup[number].alpha = 1;
        // フェードが終わったら操作可能になる
        canvasGroup[number].interactable = true;
        
    }


}
