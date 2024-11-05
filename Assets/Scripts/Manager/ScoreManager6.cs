using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;    //TextMeshProに使用

public class ScoreManager6 : Singleton<ScoreManager6>
{
    ///手っ取り早くデータを保存するには
    ///PlayerPrefsという関数を使います。

    ///Getでロード、Setで保存
    ///保存したデータを呼び出す時には PlayerPrefs.Get〇〇
    ///データを保存したい時には       PlayerPrefs.Set〇〇
    ///と記述します。

    ///〇〇の部分は保存したいデータの型によって異なり、
    ///int型（整数） float（浮動小数点数） string（文字列） の6種類の型を使用できます。
    ///フラグなどを保存したくてもbool型の関数ないので、
    ///int型の0か1かで管理するなどして代用しましょう。

    ///保存したデータはPC内部に保存されます。
    ///スクリプトでデータを削除することが出来る他、
    ///Unityエディターの　[Edit]→[Clear All Player]でも
    /// 保存データを削除できます。

    ///実際の参考例は以下に記述しているので参考にしてください。


    //スコアとハイスコアを格納する変数
    [SerializeField] public int score6, hiScore6;

    //スコアとハイスコアを表示するテキスト
    [SerializeField] TextMeshProUGUI scoreText6, hiScoreText6;

    //処理状況を表示するテキスト
    [SerializeField] TextMeshProUGUI messageText6;

    private int time6, scoretime6;

    void Start()
    {
        scoreText6.enabled = false;
        hiScoreText6.enabled = false;
        messageText6.enabled = false;

        //スコアの初期化
        ScoreReset6();

        //保存データを呼び出す時はPlayerPrefs.GetIntを使う
        //（）の中は、第1引数…"保存データの名前"、第2引数…保存データがなかった場合の値
        hiScore6 = PlayerPrefs.GetInt("HiScore6", 0);

        //ハイスコアのテキストを更新
        hiScoreText6.text = hiScore6.ToString();

        //メッセージテキストを更新
        messageText6.text = "Update";
    }

    void Update()
    {
        Score6();

    }

    //スコアをリセット
    public void ScoreReset6()
    {
        score6 = 0;
        scoreText6.text = score6.ToString();

        //メッセージテキストを更新
        messageText6.text = "Reset";
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

    void Score6()
    {
        if (GameManager.Instance.gameClear == true)
        {
            Debug.Log("スコア");
            scoreText6.enabled = true;
            hiScoreText6.enabled = true;
            //messageText6.enabled = true;
            time6 = (int)GameManager.Instance.timeCount;
            scoretime6 = 1000 - time6;
            score6 = GameManager.Instance.itemCountCurrent*100 + scoretime6 + GameManager.Instance.HPCurrent*10;

            if (score6 > hiScore6)
            {
                hiScore6 = score6;
                hiScoreText6.text = hiScore6.ToString();
            }

            scoreText6.text = score6.ToString();

            messageText6.text = "Update";
            HiScoreSave6();

        }
    }

    //ハイスコアが保存してあるスコアより高ければ保存する処理
    public void HiScoreSave6()
    {
        //現在のハイスコアが保存時のハイスコアより高かったら
        if (hiScore6 > PlayerPrefs.GetInt("HiScore6", 0))
        {
            //ハイスコアを保存
            PlayerPrefs.SetInt("HiScore6", hiScore6);

            //保存時にコレを書かないとちゃんと保存されない
            PlayerPrefs.Save();


            messageText6.text = "Keep your score。";
        }
        else
        {
            //メッセージテキストを更新
            messageText6.text = "Dont keep score";
        }
    }

    //ハイスコアの保存データを削除
    public void HiScoreDelete6()
    {
        //指定したキーだけを削除
        PlayerPrefs.DeleteKey("HiScore6");

        //全ての保存データを削除したい場合はコレ
        //PlayerPrefs.DeleteAll();

        //スコアをリセット
        ScoreReset6();

        //ハイスコアの変数もリセット
        hiScore6 = 0;

        //ハイスコアのテキストを更新
        hiScoreText6.text = hiScore6.ToString();

        //メッセージテキストを更新
        messageText6.text = "Delete";

    }
}
