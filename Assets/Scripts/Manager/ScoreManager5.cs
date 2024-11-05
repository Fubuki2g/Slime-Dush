using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;    //TextMeshProに使用

public class ScoreManager5 : Singleton<ScoreManager5>
{
    ///手っ取り早くデータを保存するには
    ///PlayerPrefsという関数を使います。

    ///Getでロード、Setで保存
    ///保存したデータを呼び出す時には PlayerPrefs.Get〇〇
    ///データを保存したい時には       PlayerPrefs.Set〇〇
    ///と記述します。

    ///〇〇の部分は保存したいデータの型によって異なり、
    ///int型（整数） float（浮動小数点数） string（文字列） の5種類の型を使用できます。
    ///フラグなどを保存したくてもbool型の関数ないので、
    ///int型の0か1かで管理するなどして代用しましょう。

    ///保存したデータはPC内部に保存されます。
    ///スクリプトでデータを削除することが出来る他、
    ///Unityエディターの　[Edit]→[Clear All Player]でも
    /// 保存データを削除できます。

    ///実際の参考例は以下に記述しているので参考にしてください。


    //スコアとハイスコアを格納する変数
    [SerializeField] public int score5, hiScore5;

    //スコアとハイスコアを表示するテキスト
    [SerializeField] TextMeshProUGUI scoreText5, hiScoreText5;

    //処理状況を表示するテキスト
    [SerializeField] TextMeshProUGUI messageText5;

    private int time5, scoretime5;

    void Start()
    {
        scoreText5.enabled = false;
        hiScoreText5.enabled = false;
        messageText5.enabled = false;

        //スコアの初期化
        ScoreReset5();

        //保存データを呼び出す時はPlayerPrefs.GetIntを使う
        //（）の中は、第1引数…"保存データの名前"、第2引数…保存データがなかった場合の値
        hiScore5 = PlayerPrefs.GetInt("HiScore5", 0);

        //ハイスコアのテキストを更新
        hiScoreText5.text = hiScore5.ToString();

        //メッセージテキストを更新
        messageText5.text = "Update";
    }

    void Update()
    {
        Score5();

    }

    //スコアをリセット
    public void ScoreReset5()
    {
        score5 = 0;
        scoreText5.text = score5.ToString();

        //メッセージテキストを更新
        messageText5.text = "Reset";
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

    void Score5()
    {
        if (GameManager.Instance.gameClear == true)
        {
            Debug.Log("スコア");
            scoreText5.enabled = true;
            hiScoreText5.enabled = true;
            //messageText5.enabled = true;
            time5 = (int)GameManager.Instance.timeCount;
            scoretime5 = 1000 - time5;
            score5 = GameManager.Instance.itemCountCurrent*100 + scoretime5 + GameManager.Instance.HPCurrent*10;

            if (score5 > hiScore5)
            {
                hiScore5 = score5;
                hiScoreText5.text = hiScore5.ToString();
            }

            scoreText5.text = score5.ToString();

            messageText5.text = "Update";
            HiScoreSave5();

        }
    }

    //ハイスコアが保存してあるスコアより高ければ保存する処理
    public void HiScoreSave5()
    {
        //現在のハイスコアが保存時のハイスコアより高かったら
        if (hiScore5 > PlayerPrefs.GetInt("HiScore5", 0))
        {
            //ハイスコアを保存
            PlayerPrefs.SetInt("HiScore5", hiScore5);

            //保存時にコレを書かないとちゃんと保存されない
            PlayerPrefs.Save();


            messageText5.text = "Keep your score。";
        }
        else
        {
            //メッセージテキストを更新
            messageText5.text = "Dont keep score";
        }
    }

    //ハイスコアの保存データを削除
    public void HiScoreDelete5()
    {
        //指定したキーだけを削除
        PlayerPrefs.DeleteKey("HiScore5");

        //全ての保存データを削除したい場合はコレ
        //PlayerPrefs.DeleteAll();

        //スコアをリセット
        ScoreReset5();

        //ハイスコアの変数もリセット
        hiScore5 = 0;

        //ハイスコアのテキストを更新
        hiScoreText5.text = hiScore5.ToString();

        //メッセージテキストを更新
        messageText5.text = "Delete";

    }
}
