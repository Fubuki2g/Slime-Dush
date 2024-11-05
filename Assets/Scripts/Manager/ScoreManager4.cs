using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;    //TextMeshPro�Ɏg�p

public class ScoreManager4 : Singleton<ScoreManager4>
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
    [SerializeField] public int score4, hiScore4;

    //�X�R�A�ƃn�C�X�R�A��\������e�L�X�g
    [SerializeField] TextMeshProUGUI scoreText4, hiScoreText4;

    //�����󋵂�\������e�L�X�g
    [SerializeField] TextMeshProUGUI messageText4;

    private int time4, scoretime4;

    void Start()
    {
        scoreText4.enabled = false;
        hiScoreText4.enabled = false;
        messageText4.enabled = false;

        //�X�R�A�̏�����
        ScoreReset4();

        //�ۑ��f�[�^���Ăяo������PlayerPrefs.GetInt���g��
        //�i�j�̒��́A��1�����c"�ۑ��f�[�^�̖��O"�A��2�����c�ۑ��f�[�^���Ȃ������ꍇ�̒l
        hiScore4 = PlayerPrefs.GetInt("HiScore4", 0);

        //�n�C�X�R�A�̃e�L�X�g���X�V
        hiScoreText4.text = hiScore4.ToString();

        //���b�Z�[�W�e�L�X�g���X�V
        messageText4.text = "Update";
    }

    void Update()
    {
        Score4();

    }

    //�X�R�A�����Z�b�g
    public void ScoreReset4()
    {
        score4 = 0;
        scoreText4.text = score4.ToString();

        //���b�Z�[�W�e�L�X�g���X�V
        messageText4.text = "Reset";
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
        messageText.text = "Update";
    }*/

    void Score4()
    {
        if (GameManager.Instance.gameClear == true)
        {
            Debug.Log("�X�R�A");
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

    //�n�C�X�R�A���ۑ����Ă���X�R�A��荂����Εۑ����鏈��
    public void HiScoreSave4()
    {
        //���݂̃n�C�X�R�A���ۑ����̃n�C�X�R�A��荂��������
        if (hiScore4 > PlayerPrefs.GetInt("HiScore4", 0))
        {
            //�n�C�X�R�A��ۑ�
            PlayerPrefs.SetInt("HiScore4", hiScore4);

            //�ۑ����ɃR���������Ȃ��Ƃ����ƕۑ�����Ȃ�
            PlayerPrefs.Save();


            messageText4.text = "Keep your score�B";
        }
        else
        {
            //���b�Z�[�W�e�L�X�g���X�V
            messageText4.text = "Dont keep score";
        }
    }

    //�n�C�X�R�A�̕ۑ��f�[�^���폜
    public void HiScoreDelete4()
    {
        //�w�肵���L�[�������폜
        PlayerPrefs.DeleteKey("HiScore4");

        //�S�Ă̕ۑ��f�[�^���폜�������ꍇ�̓R��
        //PlayerPrefs.DeleteAll();

        //�X�R�A�����Z�b�g
        ScoreReset4();

        //�n�C�X�R�A�̕ϐ������Z�b�g
        hiScore4 = 0;

        //�n�C�X�R�A�̃e�L�X�g���X�V
        hiScoreText4.text = hiScore4.ToString();

        //���b�Z�[�W�e�L�X�g���X�V
        messageText4.text = "Delete";

    }
}
