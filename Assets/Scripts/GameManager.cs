using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [Header("�Q�[���̐i�s�������O���t")]
    public bool gameStart = false;    // �Q�[���J�n�O
    public bool mainGame = false;     // �Q�[����
    public bool clearble = false;     // �N���A�\���
    public bool gameClear = false;    // �Q�[���N���A
    public bool gameOver = false;     // �Q�[���I�[�o�[
    public bool state_damage = false; // �_���[�W��

    [Header("�f�����o")]
    [SerializeField] PlayableDirector pd_gameStart; // �Q�[���X�^�[�g
    [SerializeField] PlayableDirector pd_gameClear; // �Q�[���N���A
    [SerializeField] PlayableDirector pd_gameOver;  // �Q�[���I�[�o�[

    // �N���A�����̃A�C�e���̐���c�����邽�߂̕ϐ�
    GameObject[] itemObject;
    public int itemCountCurrent, itemCountMax, shootitemCount;

    [Header("Item�̃e�L�X�g")]
    public TextMeshProUGUI currentText;
    public TextMeshProUGUI maxText;

    [Header("Player��HP")]
    public int HPCurrent = 10;
    public int HPMax = 10;

    [Header("�Q�[�W��UI")]
    public Image hpGauge;
    float hpValue;

    [Header("���Ԍv��")]
    public float timeCount = 60;
    public TextMeshProUGUI timeCountText;

    [Header("�X�L�b�v���ɕK�v�Ȑݒ�")]
    [SerializeField] GameObject canvasMainGame;
    [SerializeField] GameObject canvasStartDemo;
    [SerializeField] GameObject pd_parent;
    [SerializeField] GameObject[] mainCamera;

    // [SerializeField] GameObject[] mainCamera; �ϐ����p�ł���

    [Header("�N���A�\���p")]
    [SerializeField] GameObject canvasClearDemo;
    [SerializeField] GameObject gd_parent;

    [Header("�Q�[���I�[�o�[�\���p")]
    [SerializeField] GameObject canvasOverDemo;
    [SerializeField] GameObject ov_parent;

    // �|�[�Y���t���O
    public bool pauseFLG;   // �|�[�Y��
    public bool hitStopFLG; // �q�b�g�X�g�b�v��

    public bool clearFLG;
    public bool overFLG;


    [Header("�L�����o�X")]
    [SerializeField] GameObject[] canvas;

    [Header("�J�[�\�������ʒu")]
    [SerializeField] GameObject[] focus;


    GameObject currentFocus;  // ����
    GameObject previousFocus; // �O�t���[��


    void Start()
    {
        // �N���A�ƃQ�[���I�[�o�[�̔�\��
        canvasClearDemo.SetActive(false);
        canvasOverDemo.SetActive(false);
        gd_parent.SetActive(false);
        ov_parent.SetActive(false);

        // �X�^�[�g�f�����Đ�
        pd_gameStart.Play();

        // Tag�������AItem�^�O�̃I�u�W�F�N�g����c��
        itemObject = GameObject.FindGameObjectsWithTag("Item");

        // �A�C�e���̍ő吔 = �I�u�W�F�N�g�̍ő吔
        itemCountMax = itemObject.Length;

        // �A�C�e���̐����e�L�X�g�ŕ\��
        currentText.text = itemCountCurrent.ToString("000");
        maxText.text = itemCountMax.ToString("000");

        Default(0);

        mainCamera[1].SetActive(false);
        mainCamera[2].SetActive(false);
        mainCamera[3].SetActive(false);
        mainCamera[4].SetActive(false);
        mainCamera[5].SetActive(false);
        mainCamera[6].SetActive(false);
        mainCamera[7].SetActive(false);
        mainCamera[8].SetActive(false);
        mainCamera[9].SetActive(false);
        mainCamera[10].SetActive(false);
        mainCamera[11].SetActive(false);
        mainCamera[12].SetActive(false);
        mainCamera[13].SetActive(false);
        mainCamera[14].SetActive(false);
        mainCamera[15].SetActive(false);
        mainCamera[16].SetActive(false);
        mainCamera[17].SetActive(false);
        mainCamera[18].SetActive(false);
        mainCamera[19].SetActive(false);

        shootitemCount = 0;

        //������
        CanvasInit();

        //���C���Q�[�����̃L�����o�X�����A�N�e�B�u
        canvas[0].SetActive(true);

        clearFLG = false;
        overFLG = false;

    }


    void Update()
    {
        // HP��0�ɂȂ�Ȃ��悤�ɏ���
        //                Clamp(���ݒl,�ŏ��l,�ő�l)
        HPCurrent = Mathf.Clamp(HPCurrent, 0, HPMax);

        // HP��0�ɂȂ�����Q�[���I�[�o�[
        if (HPCurrent <= 0 && !gameOver)
        {
            GameOver();
        }

        // �X�^�[�g�f�����ɃL�[���������牉�o�X�L�b�v
        if (pd_gameStart.state == PlayState.Playing && Input.GetKeyDown(KeyCode.Return))
        {
            DemoSkip(); // �X�L�b�v����
        }


        if (gameClear == true)
        {
            ClearDemo();
        }

        if (gameOver == true)
        {
            OverDemo();
        }

        if(mainGame)
        {
            TimeCount();
        }

        if (!pauseFLG)
        {
            // escape���������玞�Ԓ�~
            if (Input.GetKeyDown(KeyCode.Escape) && mainGame)
            {
                ChangePause(true);
                return;
            }

        }

        if (pauseFLG || clearFLG || overFLG)
        {
            FocusCheck();
        }

    }

    // �A�C�e�����擾�������ɌĂяo�����\�b�h
    public void ItemGet()
    {
        itemCountCurrent++; // �A�C�e���̌��ݒl������
        shootitemCount++;
        currentText.text = itemCountCurrent.ToString("000");
        
        

        // �A�C�e�����ő�l�ɓ��B������
        if (itemCountCurrent >= itemCountMax)
        {
            clearble = true;
            Debug.Log("�A�C�e�����ő�ɂȂ�܂���");
        }

    }

    // �O�����烁�C���Q�[���̃t���O�𑀍�
    public void MainGameFLG(bool flg)
    {
        mainGame = flg;
    }

    // �X�^�[�g���o�̃X�L�b�v
    void DemoSkip()
    {
        pd_gameStart.Stop();

        canvasMainGame.SetActive(true);  // ���C��UI
        canvasStartDemo.SetActive(false); // �f����UI
        pd_parent.SetActive(false);      // �f�����J����
        mainCamera[0].SetActive(true);
        mainCamera[1].SetActive(false);
        mainCamera[2].SetActive(false);
        mainCamera[3].SetActive(false);
        mainCamera[4].SetActive(false);
        mainCamera[5].SetActive(false);
        mainCamera[6].SetActive(false);
        mainCamera[7].SetActive(false);
        mainCamera[8].SetActive(false);
        mainCamera[9].SetActive(false);
        mainCamera[10].SetActive(false);
        mainCamera[11].SetActive(false);
        mainCamera[12].SetActive(false);
        mainCamera[13].SetActive(false);
        mainCamera[14].SetActive(false);
        mainCamera[15].SetActive(false);
        mainCamera[16].SetActive(false);
        mainCamera[17].SetActive(false);
        mainCamera[18].SetActive(false);
        mainCamera[19].SetActive(false);

        SoundManager.Instance.PlayBGM(0);
        mainGame = true;

    }

    // �N���A���̉��o�\��
    void ClearDemo()
    {
        clearFLG = true;
        SoundManager.Instance.StopBGM();
        pd_gameClear.Play();

        canvasMainGame.SetActive(false);
        canvasClearDemo.SetActive(true);
        gd_parent.SetActive(true);
        EventSystem.current.SetSelectedGameObject(focus[1]);
        mainCamera[0].SetActive(false);
        mainCamera[1].SetActive(false);
        mainCamera[2].SetActive(false);
        mainCamera[3].SetActive(false);
        mainCamera[4].SetActive(false);
        mainCamera[5].SetActive(false);
        mainCamera[6].SetActive(false);
        mainCamera[7].SetActive(false);
        mainCamera[8].SetActive(false);
        mainCamera[9].SetActive(false);
        mainCamera[10].SetActive(false);
        mainCamera[11].SetActive(false);
        mainCamera[12].SetActive(false);
        mainCamera[13].SetActive(false);
        mainCamera[14].SetActive(false);
        mainCamera[15].SetActive(false);
        mainCamera[16].SetActive(false);
        mainCamera[17].SetActive(false);
        mainCamera[18].SetActive(false);
        mainCamera[19].SetActive(false);

        mainGame = false;
        
    }

    // �Q�[���I�[�o�[���̉��o�\��
    void OverDemo()
    {
        overFLG = true;
        SoundManager.Instance.StopBGM();
        pd_gameOver.Play();

        canvasMainGame.SetActive(false);
        canvasOverDemo.SetActive(true);
        ov_parent.SetActive(true);
        EventSystem.current.SetSelectedGameObject(focus[2]);
        mainCamera[0].SetActive(false);
        mainCamera[1].SetActive(false);
        mainCamera[2].SetActive(false);
        mainCamera[3].SetActive(false);
        mainCamera[4].SetActive(false);
        mainCamera[5].SetActive(false);
        mainCamera[6].SetActive(false);
        mainCamera[7].SetActive(false);
        mainCamera[8].SetActive(false);
        mainCamera[9].SetActive(false);
        mainCamera[10].SetActive(false);
        mainCamera[11].SetActive(false);
        mainCamera[12].SetActive(false);
        mainCamera[13].SetActive(false);
        mainCamera[14].SetActive(false);
        mainCamera[15].SetActive(false);
        mainCamera[16].SetActive(false);
        mainCamera[17].SetActive(false);
        mainCamera[18].SetActive(false);
        mainCamera[19].SetActive(false);

        mainGame = false;
        
    }

    // �Q�[���N���A�������̏���
    public void GameClear()
    {
        clearFLG = true;
        gameClear = true;
        Debug.Log("�Q�[���N���A");

    }

    // ���񂾂Ƃ��̏���
    void GameOver()
    {
        overFLG = true;
        gameOver = true;
        Debug.Log("�Q�[���I�[�o�[");
    }

    // �_���[�W���󂯂����ɌĂяo�����\�b�h
    public void Damage(float damage)
    {
        // HP���X�V   int�^�ɒu��������
        hpValue = (HPCurrent - damage) / HPMax;
        HPCurrent -= (int)damage;

        // HP�o�[���X�V
        hpGauge.fillAmount = hpValue;

        // �_���[�W���ł��邱�Ƃ������t���O
        state_damage = true;

        Debug.Log("HP = " + HPCurrent);


    }

    // �_���[�W���󂯂����ɌĂяo�����\�b�h
    public void Shoot(float shoot)
    {
        // HP���X�V   int�^�ɒu��������
        hpValue = (HPCurrent - shoot) / HPMax;
        HPCurrent -= (int)shoot;

        // HP�o�[���X�V
        hpGauge.fillAmount = hpValue;

    }

    // �񕜂��郁�\�b�h
    public void Heal(float heal)
    {
        hpValue = (HPCurrent + heal) / HPMax;
        HPCurrent += (int)heal;

        hpGauge.fillAmount = hpValue;


        Debug.Log("HPCurrent = " + HPCurrent);

    }

    // �f�t�H���gHP
    public void Default(float def)
    {
        hpValue = (HPCurrent - def) / HPMax;
        HPCurrent -= (int)def;

        hpGauge.fillAmount = hpValue;


        Debug.Log("HPCurrent = " + HPCurrent);

    }

    // ���Ԃ𑝂₷���\�b�h
    void TimeCount()
    {
        timeCount += Time.deltaTime;

        timeCountText.text = timeCount.ToString("00.00");

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
            EventSystem.current.SetSelectedGameObject(focus[0]);
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



