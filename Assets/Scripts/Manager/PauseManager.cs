using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;    // �ǉ�
using UnityEngine.SceneManagement; // �ǉ�

public class PauseManager : Singleton<PauseManager>
{
    // �|�[�Y���t���O
    public bool pauseFLG;   // �|�[�Y��
    public bool hitStopFLG; // �q�b�g�X�g�b�v��


    [Header("�L�����o�X")]
    [SerializeField] GameObject[] canvas;

    [Header("�|�[�Y���j���[�̃J�[�\�������ʒu")]
    [SerializeField] GameObject focusPauseMenu;

    [Header("�q�b�g�X�g�b�v")]
    [SerializeField] float timeScalse = 0.1f;
    [SerializeField] float slowTime = 1f;
    float currentTime;

    // �t�H�[�J�X���O��Ȃ��悤�ɂ��鏈���p
    GameObject currentFocus;  // ����
    GameObject previousFocus; // �O�t���[��


    void Start()
    {
        //������
        CanvasInit();

        //���C���Q�[�����̃L�����o�X�����A�N�e�B�u
        canvas[0].SetActive(true);

    }

    
    void Update()
    {
        // �������~�܂��Ă���킯�ł͂Ȃ��̂Ńf�o�b�O���O�͗��ꑱ����
        // Debug.Log("���������Ă���");

        // �|�[�Y������Ȃ����̂݃{�^�����󂯕t����
        if (!pauseFLG)
        {
            // P���������玞�Ԓ�~
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ChangePause(true);
                return;
            }

            /* O����������q�b�g�X�g�b�v
            if (Input.GetKeyDown(KeyCode.O))
            {
                HitStopStart();
                return;
            }

            // �q�b�g�X�g�b�v���̎��Ԍv��
            HitStopTime();*/

        }


        //�|�[�Y���̂�
        //�t�H�[�J�X���O��Ă��Ȃ����`�F�b�N
        if (pauseFLG || GameManager.Instance.gameClear || GameManager.Instance.gameOver)
        {
            FocusCheck();
            Debug.Log("pause");

        }

    }

    // FixedUpdate�͎~�܂鏈��
    /*void FixedUpdate()
    {
        Debug.Log("Fix���������Ă���");
    }*/

    // �q�b�g�X�g�b�v�J�n
    void HitStopStart()
    {
        currentTime = 0f;
        Time.timeScale = timeScalse;
        hitStopFLG = true;
    }

    // �q�b�g�X�g�b�v���Ԍv��
    void HitStopTime()
    {
        if (hitStopFLG)
        {
            currentTime += Time.unscaledDeltaTime;

            // ���Ԍo�߂Ō��̑�����
            if (currentTime >= slowTime)
            {
                Time.timeScale = 1f;
                hitStopFLG = false;
            }

        }
    }

    // �|�[�Y����
    public void ChangePause(bool flg)
    {
        CanvasInit(); // �L�����o�X�S������
        pauseFLG = flg;

        // �|�[�Y���������玞�Ԓ�~
        if (flg)
        {
            Time.timeScale = 0;
            canvas[1].SetActive(true);

            // �����J�[�\���ʒu�ݒ�
            EventSystem.current.SetSelectedGameObject(focusPauseMenu);
        }
        else
        {
            Time.timeScale = 1;
            canvas[0].SetActive(true);
        }

    }



    //�S�ẴL�����o�X���\����
    void CanvasInit()
    {
        for (int i = 0; i < canvas.Length; i++)
        {
            canvas[i].SetActive(false);
        }
    }

    //�t�H�[�J�X���O��Ă��Ȃ����`�F�b�N
    void FocusCheck()
    {
        //���݂̃t�H�[�J�X���i�[
        currentFocus = EventSystem.current.currentSelectedGameObject;

        //�����O��܂ł̃t�H�[�J�X�Ɠ����Ȃ瑦�I��
        if (currentFocus == previousFocus) return;

        //�����t�H�[�J�X���O��Ă�����
        //�O�t���[���̃t�H�[�J�X�ɖ߂�
        if (currentFocus == null)
        {
            EventSystem.current.SetSelectedGameObject(previousFocus);
            return;
        }

        //�c���ꂽ��������A�t�H�[�J�X�����݂���̂͊m��
        //�O�t���[���̃t�H�[�J�X���X�V
        previousFocus = EventSystem.current.currentSelectedGameObject;
    }

    //
    public void ReStart()
    {
        // timeScale�����ɖ߂��Ă���
        ChangePause(false);

        FadeManager.Instance.LoadScene(SceneManager.GetActiveScene().name, 1);
    }

    public void Scene0()
    {
        ChangePause(false);
        SceneManager.LoadScene(0);
    }
}