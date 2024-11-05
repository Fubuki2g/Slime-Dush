using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class TitleManager : MonoBehaviour
{
    [Header("ポーズメニューのカーソル初期位置")]
    [SerializeField] GameObject focus;

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(focus);
    }
}
