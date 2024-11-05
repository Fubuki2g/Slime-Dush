using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;    //TextMeshPro�Ɏg�p

public class ScoreManager2 : Singleton<ScoreManager2>
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
    [SerializeField] public int score2, hiScore2;

    //�X�R�A�ƃn�C�X�R�A��\������e�L�X�g
    [SerializeField] TextMeshProUGUI scoreText2, hiScoreText2;

    //�����󋵂�\������e�L�X�g
    [SerializeField] TextMeshProUGUI messageText2;

    private int time2, scoretime2;

    void Start()
    {
        scoreText2.enabled = false;
        hiScoreText2.enabled = false;
        messageText2.enabled = false;

        //�X�R�A�̏�����
        ScoreReset2();

        //�ۑ��f�[�^���Ăяo������PlayerPrefs.GetInt���g��
        //�i�j�̒��́A��1�����c"�ۑ��f�[�^�̖��O"�A��2�����c�ۑ��f�[�^���Ȃ������ꍇ�̒l
        hiScore2 = PlayerPrefs.GetInt("HiScore", 0);

        //�n�C�X�R�A�̃e�L�X�g���X�V
        hiScoreText2.text = hiScore2.ToString();

        //���b�Z�[�W�e�L�X�g���X�V
        messageText2.text = "Update";
    }

    void Update()
    {
        Score2();

    }

    //�X�R�A�����Z�b�g
    public void ScoreReset2()
    {
        score2 = 0;
        scoreText2.text = score2.ToString();

        //���b�Z�[�W�e�L�X�g���X�V
        messageText2.text = "Reset";
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

    void Score2()
    {
        if (GameManager.Instance.gameClear == true)
        {
            Debug.Log("�X�R�A");
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

    //�n�C�X�R�A���ۑ����Ă���X�R�A��荂����Εۑ����鏈��
    public void HiScoreSave2()
    {
        //���݂̃n�C�X�R�A���ۑ����̃n�C�X�R�A��荂��������
        if (hiScore2 > PlayerPrefs.GetInt("HiScore2", 0))
        {
            //�n�C�X�R�A��ۑ�
            PlayerPrefs.SetInt("HiScore2", hiScore2);

            //�ۑ����ɃR���������Ȃ��Ƃ����ƕۑ�����Ȃ�
            PlayerPrefs.Save();


            messageText2.text = "Keep your score�B";
        }
        else
        {
            //���b�Z�[�W�e�L�X�g���X�V
            messageText2.text = "Dont keep score";
        }
    }

    //�n�C�X�R�A�̕ۑ��f�[�^���폜
    public void HiScoreDelete2()
    {
        //�w�肵���L�[�������폜
        PlayerPrefs.DeleteKey("HiScore2");

        //�S�Ă̕ۑ��f�[�^���폜�������ꍇ�̓R��
        //PlayerPrefs.DeleteAll();

        //�X�R�A�����Z�b�g
        ScoreReset2();

        //�n�C�X�R�A�̕ϐ������Z�b�g
        hiScore2 = 0;

        //�n�C�X�R�A�̃e�L�X�g���X�V
        hiScoreText2.text = hiScore2.ToString();

        //���b�Z�[�W�e�L�X�g���X�V
        messageText2.text = "Delete";

    }
}
