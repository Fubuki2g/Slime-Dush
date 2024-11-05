using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AllScore : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI s1;
    [SerializeField] TextMeshProUGUI s2;
    [SerializeField] TextMeshProUGUI s3;
    [SerializeField] TextMeshProUGUI s4;
    [SerializeField] TextMeshProUGUI s5;
    [SerializeField] TextMeshProUGUI s6;

    int hs1;
    int hs2;
    int hs3;
    int hs4;
    int hs5;
    int hs6;

    // Start is called before the first frame update
    void Start()
    {
        hs1 = PlayerPrefs.GetInt("HiScore", 0);
        s1.text = hs1.ToString();

        hs2 = PlayerPrefs.GetInt("HiScore2", 0);
        s2.text = hs2.ToString();

        hs3 = PlayerPrefs.GetInt("HiScore3", 0);
        s3.text = hs3.ToString();

        hs4 = PlayerPrefs.GetInt("HiScore4", 0);
        s4.text = hs4.ToString();

        hs5 = PlayerPrefs.GetInt("HiScore5", 0);
        s5.text = hs5.ToString();

        hs6 = PlayerPrefs.GetInt("HiScore6", 0);
        s6.text = hs6.ToString();




    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ScoreResetAll()
    {
        PlayerPrefs.DeleteKey("HiScore");
        PlayerPrefs.DeleteKey("HiScore2");
        PlayerPrefs.DeleteKey("HiScore3");
        PlayerPrefs.DeleteKey("HiScore4");
        PlayerPrefs.DeleteKey("HiScore5");
        PlayerPrefs.DeleteKey("HiScore6");
    }
}
