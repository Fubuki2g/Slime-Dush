using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;    //TextMeshProに使用

public class ScoreManager4 : Singleton<ScoreManager4>
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
    [SerializeField] public int score4, hiScore4;

    //スコアとハイスコアを表示するテキスト
    [SerializeField] TextMeshProUGUI scoreText4, hiScoreText4;

    //処理状況を表示するテキスト
    [SerializeField] TextMeshProUGUI messageText4;

    private int time4, scoretime4;

    void Start()
    {
        scoreText4.enabled = false;
        hiScoreText4.enabled = false;
        messageText4.enabled = false;

        //スコアの初期化
        ScoreReset4();

        //保存データを呼び出す時はPlayerPrefs.GetIntを使う
        //（）の中は、第1引数…"保存データの名前"、第2引数…保存データがなかった場合の値
        hiScore4 = PlayerPrefs.GetInt("HiScore4", 0);

        //ハイスコアのテキストを更新
        hiScoreText4.text = hiScore4.ToString();

        //メッセージテキストを更新
        messageText4.text = "Update";
    }

    void Update()
    {
        Score4();

    }

    //スコアをリセット
    public void ScoreReset4()
    {
        score4 = 0;
        scoreText4.text = score4.ToString();

        //メッセージテキストを更新
        messageText4.text = "Reset";
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

    void Score4()
    {
        if (GameManager.Instance.gameClear == true)
        {
            Debug.Log("スコア");
            scoreText4.enabled = true;
            hiScoreText4.enabled = true;
            //messageText4.enabled = true;
            time4 = (int)GameManager.Instance.timeCount;
            scoretime4 = 1000 - time4;
            score4 = GameManager.Instance.itemCountCurrent*100 + scoretime4 + GameManager.Instance.HPCurrent*10;

            if (score4 > hiScore4)
            {
                hiScore4 = score4;
                hiScoreText4.text = hiScore4.ToString();
            }

            scoreText4.text = score4.ToString();

            messageText4.text = "Update";
            HiScoreSave4();

        }
    }

    //ハイスコアが保存してあるスコアより高ければ保存する処理
    public void HiScoreSave4()
    {
        //現在のハイスコアが保存時のハイスコアより高かったら
        if (hiScore4 > PlayerPrefs.GetInt("HiScore4", 0))
        {
            //ハイスコアを保存
            PlayerPrefs.SetInt("HiScore4", hiScore4);

            //保存時にコレを書かないとちゃんと保存されない
            PlayerPrefs.Save();


            messageText4.text = "Keep your score。";
        }
        else
        {
            //メッセージテキストを更新
            messageText4.text = "Dont keep score";
        }
    }

    //ハイスコアの保存データを削除
    public void HiScoreDelete4()
    {
        //指定したキーだけを削除
        PlayerPrefs.DeleteKey("HiScore4");

        //全ての保存データを削除したい場合はコレ
        //PlayerPrefs.DeleteAll();

        //スコアをリセット
        ScoreReset4();

        //ハイスコアの変数もリセット
        hiScore4 = 0;

        //ハイスコアのテキストを更新
        hiScoreText4.text = hiScore4.ToString();

        //メッセージテキストを更新
        messageText4.text = "Delete";

    }
}
