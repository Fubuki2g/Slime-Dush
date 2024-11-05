using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;    //TextMeshPro�Ɏg�p

public class ScoreManager : Singleton<ScoreManager>
{
    ///�����葁���f�[�^��ۑ�����ɂ�
    ///PlayerPrefs�Ƃ����֐����g���܂��B

    ///Get�Ń��[�h�ASet�ŕۑ�
    ///�ۑ������f�[�^���Ăяo�����ɂ� PlayerPrefs.Get�Z�Z
    ///�f�[�^��ۑ����������ɂ�       PlayerPrefs.Set�Z�Z
    ///�ƋL�q���܂��B

    ///�Z�Z�̕����͕ۑ��������f�[�^�̌^�ɂ���ĈقȂ�A
    ///int�^�i�����j float�i���������_���j string�i������j ��3��ނ̌^���g�p�ł��܂��B
    ///�t���O�Ȃǂ�ۑ��������Ă�bool�^�̊֐��Ȃ��̂ŁA
    ///int�^��0��1���ŊǗ�����Ȃǂ��đ�p���܂��傤�B

    ///�ۑ������f�[�^��PC�����ɕۑ�����܂��B
    ///�X�N���v�g�Ńf�[�^���폜���邱�Ƃ��o���鑼�A
    ///Unity�G�f�B�^�[�́@[Edit]��[Clear All Player]�ł�
    /// �ۑ��f�[�^���폜�ł��܂��B

    ///���ۂ̎Q�l��͈ȉ��ɋL�q���Ă���̂ŎQ�l�ɂ��Ă��������B


    //�X�R�A�ƃn�C�X�R�A���i�[����ϐ�
    [SerializeField] public int score, hiScore;

    //�X�R�A�ƃn�C�X�R�A��\������e�L�X�g
    [SerializeField] public TextMeshProUGUI scoreText, hiScoreText;

    //�����󋵂�\������e�L�X�g
    [SerializeField] TextMeshProUGUI messageText;

    private int time,scoretime;

    void Start()
    {
        scoreText.enabled = false;
        hiScoreText.enabled = false;
        messageText.enabled = false;

        //�X�R�A�̏�����
        ScoreReset();

        //�ۑ��f�[�^���Ăяo������PlayerPrefs.GetInt���g��
        //�i�j�̒��́A��1�����c"�ۑ��f�[�^�̖��O"�A��2�����c�ۑ��f�[�^���Ȃ������ꍇ�̒l
        hiScore = PlayerPrefs.GetInt("HiScore", 0);

        //�n�C�X�R�A�̃e�L�X�g���X�V
        hiScoreText.text = hiScore.ToString();

        //���b�Z�[�W�e�L�X�g���X�V
        messageText.text = "Update";
    }

    void Update()
    {
        Score();
        
    }

    //�X�R�A�����Z�b�g
    public void ScoreReset()
    {
        score = 0;
        scoreText.text = score.ToString();

        //���b�Z�[�W�e�L�X�g���X�V
        messageText.text = "Reset";
    }

    //�{�^�������������ɃX�R�A�����Z�E���Z���鏈��
    /*public void ScoreAdjustment(int value)
    {
        //�X�R�A�ɒl��������.�l�̓{�^����OnClick�ɐݒ�
        score += value;

        //�n�C�X�R�A�X�V
        if (score > hiScore)
        {
            hiScore = score;
            hiScoreText.text = hiScore.ToString();
        }

        //�X�R�A�X�V
        scoreText.text = score.ToString();

        //���b�Z�[�W�e�L�X�g���X�V
        messageText.text = "�X�R�A�������܂���";
    }*/

    void Score()
    {
        if (GameManager.Instance.gameClear == true)
        {
            Debug.Log("�X�R�A");
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

    //�n�C�X�R�A���ۑ����Ă���X�R�A��荂����Εۑ����鏈��
    public void HiScoreSave()
    {
        //���݂̃n�C�X�R�A���ۑ����̃n�C�X�R�A��荂��������
        if (hiScore > PlayerPrefs.GetInt("HiScore", 0))
        {
            //�n�C�X�R�A��ۑ�
            PlayerPrefs.SetInt("HiScore", hiScore);

            //�ۑ����ɃR���������Ȃ��Ƃ����ƕۑ�����Ȃ�
            PlayerPrefs.Save();


            messageText.text = "Keep your Score";
        }
        else
        {
            //���b�Z�[�W�e�L�X�g���X�V
            messageText.text = "Dont keep Score";
        }
    }

    //�n�C�X�R�A�̕ۑ��f�[�^���폜
    public void HiScoreDelete()
    {
        //�w�肵���L�[�������폜
        PlayerPrefs.DeleteKey("HiScore");

        //�S�Ă̕ۑ��f�[�^���폜�������ꍇ�̓R��
        //PlayerPrefs.DeleteAll();

        //�X�R�A�����Z�b�g
        ScoreReset();

        //�n�C�X�R�A�̕ϐ������Z�b�g
        hiScore = 0;

        //�n�C�X�R�A�̃e�L�X�g���X�V Score
        hiScoreText.text = hiScore.ToString();

        //���b�Z�[�W�e�L�X�g���X�V
        messageText.text = "Delete";

    }
}
