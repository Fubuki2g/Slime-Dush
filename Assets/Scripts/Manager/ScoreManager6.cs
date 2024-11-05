using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;    //TextMeshPro�Ɏg�p

public class ScoreManager6 : Singleton<ScoreManager6>
{
    ///�����葁���f�[�^��ۑ�����ɂ�
    ///PlayerPrefs�Ƃ����֐����g���܂��B

    ///Get�Ń��[�h�ASet�ŕۑ�
    ///�ۑ������f�[�^���Ăяo�����ɂ� PlayerPrefs.Get�Z�Z
    ///�f�[�^��ۑ����������ɂ�       PlayerPrefs.Set�Z�Z
    ///�ƋL�q���܂��B

    ///�Z�Z�̕����͕ۑ��������f�[�^�̌^�ɂ���ĈقȂ�A
    ///int�^�i�����j float�i���������_���j string�i������j ��6��ނ̌^���g�p�ł��܂��B
    ///�t���O�Ȃǂ�ۑ��������Ă�bool�^�̊֐��Ȃ��̂ŁA
    ///int�^��0��1���ŊǗ�����Ȃǂ��đ�p���܂��傤�B

    ///�ۑ������f�[�^��PC�����ɕۑ�����܂��B
    ///�X�N���v�g�Ńf�[�^���폜���邱�Ƃ��o���鑼�A
    ///Unity�G�f�B�^�[�́@[Edit]��[Clear All Player]�ł�
    /// �ۑ��f�[�^���폜�ł��܂��B

    ///���ۂ̎Q�l��͈ȉ��ɋL�q���Ă���̂ŎQ�l�ɂ��Ă��������B


    //�X�R�A�ƃn�C�X�R�A���i�[����ϐ�
    [SerializeField] public int score6, hiScore6;

    //�X�R�A�ƃn�C�X�R�A��\������e�L�X�g
    [SerializeField] TextMeshProUGUI scoreText6, hiScoreText6;

    //�����󋵂�\������e�L�X�g
    [SerializeField] TextMeshProUGUI messageText6;

    private int time6, scoretime6;

    void Start()
    {
        scoreText6.enabled = false;
        hiScoreText6.enabled = false;
        messageText6.enabled = false;

        //�X�R�A�̏�����
        ScoreReset6();

        //�ۑ��f�[�^���Ăяo������PlayerPrefs.GetInt���g��
        //�i�j�̒��́A��1�����c"�ۑ��f�[�^�̖��O"�A��2�����c�ۑ��f�[�^���Ȃ������ꍇ�̒l
        hiScore6 = PlayerPrefs.GetInt("HiScore6", 0);

        //�n�C�X�R�A�̃e�L�X�g���X�V
        hiScoreText6.text = hiScore6.ToString();

        //���b�Z�[�W�e�L�X�g���X�V
        messageText6.text = "Update";
    }

    void Update()
    {
        Score6();

    }

    //�X�R�A�����Z�b�g
    public void ScoreReset6()
    {
        score6 = 0;
        scoreText6.text = score6.ToString();

        //���b�Z�[�W�e�L�X�g���X�V
        messageText6.text = "Reset";
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

    void Score6()
    {
        if (GameManager.Instance.gameClear == true)
        {
            Debug.Log("�X�R�A");
            scoreText6.enabled = true;
            hiScoreText6.enabled = true;
            //messageText6.enabled = true;
            time6 = (int)GameManager.Instance.timeCount;
            scoretime6 = 1000 - time6;
            score6 = GameManager.Instance.itemCountCurrent*100 + scoretime6 + GameManager.Instance.HPCurrent*10;

            if (score6 > hiScore6)
            {
                hiScore6 = score6;
                hiScoreText6.text = hiScore6.ToString();
            }

            scoreText6.text = score6.ToString();

            messageText6.text = "Update";
            HiScoreSave6();

        }
    }

    //�n�C�X�R�A���ۑ����Ă���X�R�A��荂����Εۑ����鏈��
    public void HiScoreSave6()
    {
        //���݂̃n�C�X�R�A���ۑ����̃n�C�X�R�A��荂��������
        if (hiScore6 > PlayerPrefs.GetInt("HiScore6", 0))
        {
            //�n�C�X�R�A��ۑ�
            PlayerPrefs.SetInt("HiScore6", hiScore6);

            //�ۑ����ɃR���������Ȃ��Ƃ����ƕۑ�����Ȃ�
            PlayerPrefs.Save();


            messageText6.text = "Keep your score�B";
        }
        else
        {
            //���b�Z�[�W�e�L�X�g���X�V
            messageText6.text = "Dont keep score";
        }
    }

    //�n�C�X�R�A�̕ۑ��f�[�^���폜
    public void HiScoreDelete6()
    {
        //�w�肵���L�[�������폜
        PlayerPrefs.DeleteKey("HiScore6");

        //�S�Ă̕ۑ��f�[�^���폜�������ꍇ�̓R��
        //PlayerPrefs.DeleteAll();

        //�X�R�A�����Z�b�g
        ScoreReset6();

        //�n�C�X�R�A�̕ϐ������Z�b�g
        hiScore6 = 0;

        //�n�C�X�R�A�̃e�L�X�g���X�V
        hiScoreText6.text = hiScore6.ToString();

        //���b�Z�[�W�e�L�X�g���X�V
        messageText6.text = "Delete";

    }
}
