using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;    //TextMeshPro�Ɏg�p

public class ScoreManager3 : Singleton<ScoreManager3>
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
    [SerializeField] public int score3, hiScore3;

    //�X�R�A�ƃn�C�X�R�A��\������e�L�X�g
    [SerializeField] TextMeshProUGUI scoreText3, hiScoreText3;

    //�����󋵂�\������e�L�X�g
    [SerializeField] TextMeshProUGUI messageText3;

    private int time3, scoretime3;

    void Start()
    {
        scoreText3.enabled = false;
        hiScoreText3.enabled = false;
        messageText3.enabled = false;

        //�X�R�A�̏�����
        ScoreReset3();

        //�ۑ��f�[�^���Ăяo������PlayerPrefs.GetInt���g��
        //�i�j�̒��́A��1�����c"�ۑ��f�[�^�̖��O"�A��2�����c�ۑ��f�[�^���Ȃ������ꍇ�̒l
        hiScore3 = PlayerPrefs.GetInt("HiScore", 0);

        //�n�C�X�R�A�̃e�L�X�g���X�V
        hiScoreText3.text = hiScore3.ToString();

        //���b�Z�[�W�e�L�X�g���X�V
        messageText3.text = "Update";
    }

    void Update()
    {
        Score3();

    }

    //�X�R�A�����Z�b�g
    public void ScoreReset3()
    {
        score3 = 0;
        scoreText3.text = score3.ToString();

        //���b�Z�[�W�e�L�X�g���X�V
        messageText3.text = "Reset";
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

    void Score3()
    {
        if (GameManager.Instance.gameClear == true)
        {
            Debug.Log("�X�R�A");
            scoreText3.enabled = true;
            hiScoreText3.enabled = true;
            //messageText3.enabled = true;
            time3 = (int)GameManager.Instance.timeCount;
            scoretime3 = 1000 - time3;
            score3 = GameManager.Instance.itemCountCurrent*100 + scoretime3 + GameManager.Instance.HPCurrent*10;

            if (score3 > hiScore3)
            {
                hiScore3 = score3;
                hiScoreText3.text = hiScore3.ToString();
            }

            scoreText3.text = score3.ToString();

            messageText3.text = "Update";
            HiScoreSave3();

        }
    }

    //�n�C�X�R�A���ۑ����Ă���X�R�A��荂����Εۑ����鏈��
    public void HiScoreSave3()
    {
        //���݂̃n�C�X�R�A���ۑ����̃n�C�X�R�A��荂��������
        if (hiScore3 > PlayerPrefs.GetInt("HiScore3", 0))
        {
            //�n�C�X�R�A��ۑ�
            PlayerPrefs.SetInt("HiScore3", hiScore3);

            //�ۑ����ɃR���������Ȃ��Ƃ����ƕۑ�����Ȃ�
            PlayerPrefs.Save();


            messageText3.text = "Keep your score�B";
        }
        else
        {
            //���b�Z�[�W�e�L�X�g���X�V
            messageText3.text = "Dont keep score";
        }
    }

    //�n�C�X�R�A�̕ۑ��f�[�^���폜
    public void HiScoreDelete3()
    {
        //�w�肵���L�[�������폜
        PlayerPrefs.DeleteKey("HiScore3");

        //�S�Ă̕ۑ��f�[�^���폜�������ꍇ�̓R��
        //PlayerPrefs.DeleteAll();

        //�X�R�A�����Z�b�g
        ScoreReset3();

        //�n�C�X�R�A�̕ϐ������Z�b�g
        hiScore3 = 0;

        //�n�C�X�R�A�̃e�L�X�g���X�V
        hiScoreText3.text = hiScore3.ToString();

        //���b�Z�[�W�e�L�X�g���X�V
        messageText3.text = "Delete";

    }
}
