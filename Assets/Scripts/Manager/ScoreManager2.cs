using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;    //TextMeshProに使用

public class ScoreManager2 : Singleton<ScoreManager2>
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
    [SerializeField] public int score2, hiScore2;

    //スコアとハイスコアを表示するテキスト
    [SerializeField] TextMeshProUGUI scoreText2, hiScoreText2;

    //処理状況を表示するテキスト
    [SerializeField] TextMeshProUGUI messageText2;

    private int time2, scoretime2;

    void Start()
    {
        scoreText2.enabled = false;
        hiScoreText2.enabled = false;
        messageText2.enabled = false;

        //スコアの初期化
        ScoreReset2();

        //保存データを呼び出す時はPlayerPrefs.GetIntを使う
        //（）の中は、第1引数…"保存データの名前"、第2引数…保存データがなかった場合の値
        hiScore2 = PlayerPrefs.GetInt("HiScore", 0);

        //ハイスコアのテキストを更新
        hiScoreText2.text = hiScore2.ToString();

        //メッセージテキストを更新
        messageText2.text = "Update";
    }

    void Update()
    {
        Score2();

    }

    //スコアをリセット
    public void ScoreReset2()
    {
        score2 = 0;
        scoreText2.text = score2.ToString();

        //メッセージテキストを更新
        messageText2.text = "Reset";
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

    void Score2()
    {
        if (GameManager.Instance.gameClear == true)
        {
            Debug.Log("スコア");
            scoreText2.enabled = true;
            hiScoreText2.enabled = true;
            //messageText2.enabled = true;
            time2 = (int)GameManager.Instance.timeCount;
            scoretime2 = 1000 - time2;
            score2 = GameManager.Instance.itemCountCurrent*100 + scoretime2 + GameManager.Instance.HPCurrent*10;

            if (score2 > hiScore2)
            {
                hiScore2 = score2;
                hiScoreText2.text = hiScore2.ToString();
            }

            scoreText2.text = score2.ToString();

            messageText2.text = "Update";
            HiScoreSave2();

        }
    }

    //ハイスコアが保存してあるスコアより高ければ保存する処理
    public void HiScoreSave2()
    {
        //現在のハイスコアが保存時のハイスコアより高かったら
        if (hiScore2 > PlayerPrefs.GetInt("HiScore2", 0))
        {
            //ハイスコアを保存
            PlayerPrefs.SetInt("HiScore2", hiScore2);

            //保存時にコレを書かないとちゃんと保存されない
            PlayerPrefs.Save();


            messageText2.text = "Keep your score。";
        }
        else
        {
            //メッセージテキストを更新
            messageText2.text = "Dont keep score";
        }
    }

    //ハイスコアの保存データを削除
    public void HiScoreDelete2()
    {
        //指定したキーだけを削除
        PlayerPrefs.DeleteKey("HiScore2");

        //全ての保存データを削除したい場合はコレ
        //PlayerPrefs.DeleteAll();

        //スコアをリセット
        ScoreReset2();

        //ハイスコアの変数もリセット
        hiScore2 = 0;

        //ハイスコアのテキストを更新
        hiScoreText2.text = hiScore2.ToString();

        //メッセージテキストを更新
        messageText2.text = "Delete";

    }
}
