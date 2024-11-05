using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;    //TextMeshPro�Ɏg�p

public class ScoreManager5 : Singleton<ScoreManager5>
{
    ///�����葁���f�[�^��ۑ�����ɂ�
    ///PlayerPrefs�Ƃ����֐����g���܂��B

    ///Get�Ń��[�h�ASet�ŕۑ�
    ///�ۑ������f�[�^���Ăяo�����ɂ� PlayerPrefs.Get�Z�Z
    ///�f�[�^��ۑ����������ɂ�       PlayerPrefs.Set�Z�Z
    ///�ƋL�q���܂��B

    ///�Z�Z�̕����͕ۑ��������f�[�^�̌^�ɂ���ĈقȂ�A
    ///int�^�i�����j float�i���������_���j string�i������j ��5��ނ̌^���g�p�ł��܂��B
    ///�t���O�Ȃǂ�ۑ��������Ă�bool�^�̊֐��Ȃ��̂ŁA
    ///int�^��0��1���ŊǗ�����Ȃǂ��đ�p���܂��傤�B

    ///�ۑ������f�[�^��PC�����ɕۑ�����܂��B
    ///�X�N���v�g�Ńf�[�^���폜���邱�Ƃ��o���鑼�A
    ///Unity�G�f�B�^�[�́@[Edit]��[Clear All Player]�ł�
    /// �ۑ��f�[�^���폜�ł��܂��B

    ///���ۂ̎Q�l��͈ȉ��ɋL�q���Ă���̂ŎQ�l�ɂ��Ă��������B


    //�X�R�A�ƃn�C�X�R�A���i�[����ϐ�
    [SerializeField] public int score5, hiScore5;

    //�X�R�A�ƃn�C�X�R�A��\������e�L�X�g
    [SerializeField] TextMeshProUGUI scoreText5, hiScoreText5;

    //�����󋵂�\������e�L�X�g
    [SerializeField] TextMeshProUGUI messageText5;

    private int time5, scoretime5;

    void Start()
    {
        scoreText5.enabled = false;
        hiScoreText5.enabled = false;
        messageText5.enabled = false;

        //�X�R�A�̏�����
        ScoreReset5();

        //�ۑ��f�[�^���Ăяo������PlayerPrefs.GetInt���g��
        //�i�j�̒��́A��1�����c"�ۑ��f�[�^�̖��O"�A��2�����c�ۑ��f�[�^���Ȃ������ꍇ�̒l
        hiScore5 = PlayerPrefs.GetInt("HiScore5", 0);

        //�n�C�X�R�A�̃e�L�X�g���X�V
        hiScoreText5.text = hiScore5.ToString();

        //���b�Z�[�W�e�L�X�g���X�V
        messageText5.text = "Update";
    }

    void Update()
    {
        Score5();

    }

    //�X�R�A�����Z�b�g
    public void ScoreReset5()
    {
        score5 = 0;
        scoreText5.text = score5.ToString();

        //���b�Z�[�W�e�L�X�g���X�V
        messageText5.text = "Reset";
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

    void Score5()
    {
        if (GameManager.Instance.gameClear == true)
        {
            Debug.Log("�X�R�A");
            scoreText5.enabled = true;
            hiScoreText5.enabled = true;
            //messageText5.enabled = true;
            time5 = (int)GameManager.Instance.timeCount;
            scoretime5 = 1000 - time5;
            score5 = GameManager.Instance.itemCountCurrent*100 + scoretime5 + GameManager.Instance.HPCurrent*10;

            if (score5 > hiScore5)
            {
                hiScore5 = score5;
                hiScoreText5.text = hiScore5.ToString();
            }

            scoreText5.text = score5.ToString();

            messageText5.text = "Update";
            HiScoreSave5();

        }
    }

    //�n�C�X�R�A���ۑ����Ă���X�R�A��荂����Εۑ����鏈��
    public void HiScoreSave5()
    {
        //���݂̃n�C�X�R�A���ۑ����̃n�C�X�R�A��荂��������
        if (hiScore5 > PlayerPrefs.GetInt("HiScore5", 0))
        {
            //�n�C�X�R�A��ۑ�
            PlayerPrefs.SetInt("HiScore5", hiScore5);

            //�ۑ����ɃR���������Ȃ��Ƃ����ƕۑ�����Ȃ�
            PlayerPrefs.Save();


            messageText5.text = "Keep your score�B";
        }
        else
        {
            //���b�Z�[�W�e�L�X�g���X�V
            messageText5.text = "Dont keep score";
        }
    }

    //�n�C�X�R�A�̕ۑ��f�[�^���폜
    public void HiScoreDelete5()
    {
        //�w�肵���L�[�������폜
        PlayerPrefs.DeleteKey("HiScore5");

        //�S�Ă̕ۑ��f�[�^���폜�������ꍇ�̓R��
        //PlayerPrefs.DeleteAll();

        //�X�R�A�����Z�b�g
        ScoreReset5();

        //�n�C�X�R�A�̕ϐ������Z�b�g
        hiScore5 = 0;

        //�n�C�X�R�A�̃e�L�X�g���X�V
        hiScoreText5.text = hiScore5.ToString();

        //���b�Z�[�W�e�L�X�g���X�V
        messageText5.text = "Delete";

    }
}
