using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    
    void Start()
    {
        
    }

    
    void Update()
    {

    }

    public void SceneRiset()
    {
        // FadeManagerからシーンロードを行う
        FadeManager.Instance.LoadScene(SceneManager.GetActiveScene().name, 1);

        // SoundManagerからSEを呼び出す
        SoundManager.Instance.PlaySE_Sys(0);
    }

    public void Scene0()
    {
        FadeManager.Instance.LoadSceneIndex(0, 1);
    }

    public void Scene1()
    {
        FadeManager.Instance.LoadSceneIndex(1, 1);
    }

    public void Scene2()
    {
        FadeManager.Instance.LoadSceneIndex(2, 1);
    }

    public void Scene3()
    {
        FadeManager.Instance.LoadSceneIndex(3, 1);
    }

    public void Scene4()
    {
        FadeManager.Instance.LoadSceneIndex(4, 1);
    }

    public void Scene5()
    {
        FadeManager.Instance.LoadSceneIndex(5, 1);
    }

    public void Scene6()
    {
        FadeManager.Instance.LoadSceneIndex(6, 1);
    }

    public void Scene7()
    {
        FadeManager.Instance.LoadSceneIndex(7, 1);
    }

    public void Scene8()
    {
        FadeManager.Instance.LoadSceneIndex(8, 1);
    }

    public void Scene9()
    {
        FadeManager.Instance.LoadSceneIndex(9, 1);
    }

    public void Scene10()
    {
        FadeManager.Instance.LoadSceneIndex(10, 1);
    }

    public void Scene11()
    {
        FadeManager.Instance.LoadSceneIndex(11, 1);
    }

    public void Scene12()
    {
        FadeManager.Instance.LoadSceneIndex(12, 1);
    }

    public void Scene13()
    {
        FadeManager.Instance.LoadSceneIndex(13, 1);
    }

    public void Scene14()
    {
        FadeManager.Instance.LoadSceneIndex(14, 1);
    }

    public void Scene15()
    {
        FadeManager.Instance.LoadSceneIndex(15, 1);
    }

    public void Scene16()
    {
        FadeManager.Instance.LoadSceneIndex(16, 1);
    }

    public void Scene17()
    {
        FadeManager.Instance.LoadSceneIndex(17, 1);
    }
}
