using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerManager : Singleton<FlickerManager>
{
    ///�I�u�W�F�N�g�𖾖ł�������@�̂P��
    ///�}�e���A����Emmision�̐F��ύX�������
    ///���O�Ƀ}�e���A����Emmision�̍��ڂ̓`�F�b�N�{�b�N�X�Ƀ`�F�b�N���Ă�������

    ///3D��`�悵�Ă���R���|�[�l���g��
    ///MeshRenderer�����łȂ��ASkinnedMeshRenderer ������
    ///�R���|�[�l���g���قȂ�̂ŁA�ǂ���ŕ`�悵�Ă��邩�m�F����
    ///�ϐ��̌^���`���邱��


    ///�V�F�[�_�[�̎�ނɂ���āAEmmision�̃v���p�e�B�iEnableKeyword���j��
    ///�قȂ�ꍇ������̂Ŏ��O�ɒ��ׂ�K�v������
    ///EnableKeyword�@�F�@Standard�c"_EMISSION"         UnityChan�V�F�[�_�["_EMISSIVE"
    ///EmissionColor�@�F�@Standard�c"_EmissionColor"    UnityChan�V�F�[�_�["_Emissive_Color"

    ///�v���p�e�B�̓C���X�y�N�^�[�r���[�̃V�F�[�_�[�̏��
    ///�E�N���b�N�����j���[���ڂ���Select Shader��I������ƕ\�������


    [Header("���ł��������I�u�W�F�N�g")]
    [SerializeField, Label("�Ώ�")] GameObject target;

    [Header("���ł̏���")]
    [SerializeField, Label("���ł̋���")] float intensity;
    [SerializeField, Label("���ł̑���")] float duration;
    [SerializeField, Label("���ł��鎞��")] float timer;

    //ColorUsage�͑�������True�ɂ����HDR�J���[�p�l��(Emmision�̃J���[)�ɂȂ�
    [SerializeField, Label("���ƂȂ�F")] [ColorUsage(false, true)] Color color1;
    [SerializeField, Label("���Ŏ��̐F")] [ColorUsage(false, true)] Color color2;

    //�Ώۂ��\������S�Ẵ}�e���A�������ł���悤��
    //�z���MeshRenderer�����I�u�W�F�N�g�S�Ă��i�[�ł���悤�ɂ���
    //��MeshRenderer�Ȃ̂�SkinnedMeshRenderer�Ȃ̂�����
    SkinnedMeshRenderer[] skinmeshs;
    MeshRenderer[] meshs;
    Material[] skinmats;
    Material[] mats;

    float currentTime;  //���݂̎��Ԃ��v������p
    bool flickerFLG;    //���Œ����𔻒肷��t���O


    void Start()
    {
        //GetComponentsInChildren�ŁA�ΏۃI�u�W�F�N�g�����łȂ����̎q�I�u�W�F�N�g�ȉ���
        //�I�u�W�F�N�g�S�Ă���R���|�[�l���g���擾�ł���
        skinmeshs = target.GetComponentsInChildren<SkinnedMeshRenderer>();
        meshs = target.GetComponentsInChildren<MeshRenderer>();

        //�Ώۂ̑�������material�̔z��̐���ݒ�
        skinmats = new Material[skinmeshs.Length];
        mats = new Material[meshs.Length];

        //�SSkinnedMeshRenderer���炻�ꂼ��̃}�e���A�����擾
        for (int i = 0; i < skinmeshs.Length; i++)
        {
            skinmats[i] = skinmeshs[i].material;
            skinmats[i].EnableKeyword("_EMISSION");   //�}�e���A����Emmision�ݒ�(Standard)
        }

        for (int i = 0; i < meshs.Length; i++)
        {
            mats[i] = meshs[i].material;
            mats[i].EnableKeyword("_EMISSION");   //�}�e���A����Emmision�ݒ�(Standard)
        }

    }

    void Update()
    {
        frash();

    }

    public void frash()
    {
        //���ł��Ă��Ȃ����ɍU����������Ɩ��ŊJ�n
        if ((ECharacter.Instance.Edamage == true) && !flickerFLG)
        {
            flickerFLG = true;
            Debug.Log("���ŊJ�n");
        }

        //�t���O�������Ă����疾�ł�������
        if (flickerFLG)
        {
            Fricker();
            Debug.Log("���Œ�");
        }
    }

    //���ł̏���
    public void Fricker()
    {
        //���Ԃ̌v��
        if (currentTime <= timer)
        {
            currentTime += Time.deltaTime;

            //skinnedMeshRenderer�̃}�e���A����
            //color1��color2�̐F��Mathf.PingPong�ŌJ��Ԃ�
            foreach (Material material in skinmats)
            {
                material.SetColor("_EmissionColor",
                    Color.Lerp(color2 * intensity, color1, Mathf.PingPong(currentTime * duration, 1f)));
            }

            foreach (Material material in mats)
            {
                material.SetColor("_EmissionColor",
                    Color.Lerp(color2 * intensity, color1, Mathf.PingPong(currentTime * duration, 1f)));
            }

        }
        else
        {
            //���Ԃ��߂����疾�ŏI��
            EndFlicker();
        }
    }

    //���ŏI������
    public void EndFlicker()
    {
        flickerFLG = false;
        currentTime = 0;

        //�S�Ă̐F�����ɖ߂�
        foreach (Material material in skinmats)
        {
            material.SetColor("_EmissionColor", color1);
        }
        Debug.Log("���ŏI��");

        foreach (Material material in mats)
        {
            material.SetColor("_EmissionColor", color1);
        }
        Debug.Log("���ŏI��");
    }

}