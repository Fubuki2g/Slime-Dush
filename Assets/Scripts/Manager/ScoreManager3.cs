using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;    //TextMeshProに使用

public class ScoreManager3 : Singleton<ScoreManager3>
{
    ///手っ取り早くデータを保存するには
    ///PlayerPrefsという関数を使います。

    ///Getでロード、Setで保存
    ///保存したデータを呼び出す時には PlayerPrefs.Get〇〇
    ///データを保存したい時には       PlayerPrefs.Set〇〇
    ///と記述します。

    ///〇〇の部分は保存したいデータの型によって異なり、
    ///int型（整数） float（浮動小数点数） string（文字列） の3種類の型を使用できます。
    ///フラグなどを保存したくてもbool型の関数ないので、
    ///int型の0か1かで管理するなどして代用しましょう。

    ///保存したデータはPC内部に保存されます。
    ///スクリプトでデータを削除することが出来る他、
    ///Unityエディターの　[Edit]→[Clear All Player]でも
    /// 保存データを削除できます。

    ///実際の参考例は以下に記述しているので参考にしてください。


    //スコアとハイスコアを格納する変数
    [SerializeField] public int score3, hiScore3;

    //スコアとハイスコアを表示するテキスト
    [SerializeField] TextMeshProUGUI scoreText3, hiScoreText3;

    //処理状況を表示するテキスト
    [SerializeField] TextMeshProUGUI messageText3;

    private int time3, scoretime3;

    void Start()
    {
        scoreText3.enabled = false;
        hiScoreText3.enabled = false;
        messageText3.enabled = false;

        //スコアの初期化
        ScoreReset3();

        //保存データを呼び出す時はPlayerPrefs.GetIntを使う
        //（）の中は、第1引数…"保存データの名前"、第2引数…保存データがなかった場合の値
        hiScore3 = PlayerPrefs.GetInt("HiScore", 0);

        //ハイスコアのテキストを更新
        hiScoreText3.text = hiScore3.ToString();

        //メッセージテキストを更新
        messageText3.text = "Update";
    }

    void Update()
    {
        Score3();

    }

    //スコアをリセット
    public void ScoreReset3()
    {
        score3 = 0;
        scoreText3.text = score3.ToString();

        //メッセージテキストを更新
        messageText3.text = "Reset";
    }

    //ボタンを押した時にスコアを加算・減算する処理
    /*public void ScoreAdjustment(int value)
    {
        //スコアに値を加える.値はボタンのOnClickに設定
        score += value;

        //ハイスコア更新
        if (score > hiScore)
        {
            hiScore = score;
            hiScoreText.text = hiScore.ToString();
        }

        //スコア更新
        scoreText.text = score.ToString();

        //メッセージテキストを更新
        messageText.text = "Update";
    }*/

    void Score3()
    {
        if (GameManager.Instance.gameClear == true)
        {
            Debug.Log("スコア");
            scoreText3.enabled = true;
            hiScoreText3.enabled = true;
            //messageText3.enabled = true;
            time3 = (int)GameManager.Instance.timeCount;
            scoretime3 = 1000 - time3;
            score3 = GameManager.Instance.itemCountCurrent*100 + scoretime3 + GameManager.Instance.HPCurrent*10;

            if (score3 > hiScore3)
            {
                hiScore3 = score3;
                hiScoreText3.text = hiScore3.ToString();
            }

            scoreText3.text = score3.ToString();

            messageText3.text = "Update";
            HiScoreSave3();

        }
    }

    //ハイスコアが保存してあるスコアより高ければ保存する処理
    public void HiScoreSave3()
    {
        //現在のハイスコアが保存時のハイスコアより高かったら
        if (hiScore3 > PlayerPrefs.GetInt("HiScore3", 0))
        {
            //ハイスコアを保存
            PlayerPrefs.SetInt("HiScore3", hiScore3);

            //保存時にコレを書かないとちゃんと保存されない
            PlayerPrefs.Save();


            messageText3.text = "Keep your score。";
        }
        else
        {
            //メッセージテキストを更新
            messageText3.text = "Dont keep score";
        }
    }

    //ハイスコアの保存データを削除
    public void HiScoreDelete3()
    {
        //指定したキーだけを削除
        PlayerPrefs.DeleteKey("HiScore3");

        //全ての保存データを削除したい場合はコレ
        //PlayerPrefs.DeleteAll();

        //スコアをリセット
        ScoreReset3();

        //ハイスコアの変数もリセット
        hiScore3 = 0;

        //ハイスコアのテキストを更新
        hiScoreText3.text = hiScore3.ToString();

        //メッセージテキストを更新
        messageText3.text = "Delete";

    }
}
