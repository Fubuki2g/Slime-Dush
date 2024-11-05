using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;    //TextMeshProに使用

public class ScoreManager : Singleton<ScoreManager>
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
    [SerializeField] public int score, hiScore;

    //スコアとハイスコアを表示するテキスト
    [SerializeField] public TextMeshProUGUI scoreText, hiScoreText;

    //処理状況を表示するテキスト
    [SerializeField] TextMeshProUGUI messageText;

    private int time,scoretime;

    void Start()
    {
        scoreText.enabled = false;
        hiScoreText.enabled = false;
        messageText.enabled = false;

        //スコアの初期化
        ScoreReset();

        //保存データを呼び出す時はPlayerPrefs.GetIntを使う
        //（）の中は、第1引数…"保存データの名前"、第2引数…保存データがなかった場合の値
        hiScore = PlayerPrefs.GetInt("HiScore", 0);

        //ハイスコアのテキストを更新
        hiScoreText.text = hiScore.ToString();

        //メッセージテキストを更新
        messageText.text = "Update";
    }

    void Update()
    {
        Score();
        
    }

    //スコアをリセット
    public void ScoreReset()
    {
        score = 0;
        scoreText.text = score.ToString();

        //メッセージテキストを更新
        messageText.text = "Reset";
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
        messageText.text = "スコアが増えました";
    }*/

    void Score()
    {
        if (GameManager.Instance.gameClear == true)
        {
            Debug.Log("スコア");
            scoreText.enabled = true;
            hiScoreText.enabled = true;
            //messageText.enabled = true;
            time = (int)GameManager.Instance.timeCount;
            scoretime = 1000 - time;
            score = GameManager.Instance.itemCountCurrent*100 + scoretime + GameManager.Instance.HPCurrent*10;

            if (score > hiScore)
            {
                hiScore = score;
                hiScoreText.text = hiScore.ToString();
            }

            scoreText.text = score.ToString();

            messageText.text = "Update";
            HiScoreSave();

        }
    }

    //ハイスコアが保存してあるスコアより高ければ保存する処理
    public void HiScoreSave()
    {
        //現在のハイスコアが保存時のハイスコアより高かったら
        if (hiScore > PlayerPrefs.GetInt("HiScore", 0))
        {
            //ハイスコアを保存
            PlayerPrefs.SetInt("HiScore", hiScore);

            //保存時にコレを書かないとちゃんと保存されない
            PlayerPrefs.Save();


            messageText.text = "Keep your Score";
        }
        else
        {
            //メッセージテキストを更新
            messageText.text = "Dont keep Score";
        }
    }

    //ハイスコアの保存データを削除
    public void HiScoreDelete()
    {
        //指定したキーだけを削除
        PlayerPrefs.DeleteKey("HiScore");

        //全ての保存データを削除したい場合はコレ
        //PlayerPrefs.DeleteAll();

        //スコアをリセット
        ScoreReset();

        //ハイスコアの変数もリセット
        hiScore = 0;

        //ハイスコアのテキストを更新 Score
        hiScoreText.text = hiScore.ToString();

        //メッセージテキストを更新
        messageText.text = "Delete";

    }
}
