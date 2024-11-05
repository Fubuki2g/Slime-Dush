using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // �ǉ�

public class MainMenuManager : MonoBehaviour
{
    [Header("�eCanvas")]
    [SerializeField] GameObject[] canvas;

    [Header("�e���j���[�̏����J�[�\���̑Ώ�")]
    [SerializeField] GameObject[] focusObject;

    [Header("�e�L�����o�X�̃L�����o�X�O���[�v")]
    [SerializeField] CanvasGroup[] canvasGroup;

    // �t�F�[�h�ɂ����鎞��
    [SerializeField] float fadeTime = 1;

    // �t�H�[�J�X���O��Ă��܂�Ȃ��悤�ɂ��鏈��
    // ���m�Ɍ����ΊO�ꂽ��Ɍ��ɖ߂�����
    GameObject currentFocus;  // ����
    GameObject previousFocus; // �O�t���[��


    void Start()
    {
        // ������
        CanvasInit();

        // ���C�����j���[�����A�N�e�B�u
        canvas[1].SetActive(true);

        // �����J�[�\����ݒ�
        EventSystem.current.SetSelectedGameObject(focusObject[1]);

    }

    
    void Update()
    {
        FocusCheak(); // �t�H�[�J�X���O��Ă��Ȃ����`�F�b�N

    }
    
    // ���ׂẴL�����o�X���\����
    void CanvasInit()
    {
        for(int i = 0; i < canvas.Length; i++)
        {
            canvas[i].SetActive(false);
        }
    }

    // �L�����o�X�̐؂�ւ�
    public void Transition(int nextCanvas)
    {
        // ��U���ׂẴL�����o�X���\��
        CanvasInit();

        // ���̃L�����o�X��\��
        canvas[nextCanvas].SetActive(true);

        // �t�F�[�h�C���̏���
        StartCoroutine(FadeIn(nextCanvas));

        // ���̃L�����o�X�̏����J�[�\���ʒu��ݒ�
        EventSystem.current.SetSelectedGameObject(focusObject[nextCanvas]);

    }

    // �t�H�[�J�X���O��Ă��Ȃ����`�F�b�N
    void FocusCheak()
    {
        // ���݂̃t�H�[�J�X���i�[
        currentFocus = EventSystem.current.currentSelectedGameObject;

        // �����O��܂ł̃t�H�[�J�X�Ɠ����Ȃ瑦�I��
        if (currentFocus == previousFocus) return;

        // �����t�H�[�J�X���O��Ă�����
        // �O�t���[���̃t�H�[�J�X�ɖ߂�
        if (currentFocus == null)
        {
            EventSystem.current.SetSelectedGameObject(previousFocus);
            return;
        }

        // �c���ꂽ��������A�t�H�[�J�X�����݂���̂͊m��
        // �O�t���[���̃t�H�[�J�X���X�V
        previousFocus = EventSystem.current.currentSelectedGameObject;

    }

    // Canvas��\�����鎞�ɏ��X�Ƀt�F�[�h�C��������
    IEnumerator FadeIn(int number)
    {
        // �t�F�[�h���I���܂ł͑���s�ɂ���
        canvasGroup[number].interactable = false;
        float time = 0;

        // �����𖞂����܂ŏ������J��Ԃ�
        while(time <= fadeTime)
        {
            //                                     �o���_, �����_, �o�ߎ���
            canvasGroup[number].alpha = Mathf.Lerp(0f, 1f, time / fadeTime);
            time += Time.deltaTime;
            // ���ꂪ�Ȃ���Coroutine�͎g���Ȃ�
            yield return null; // 1�t���[��������҂�
            // yield return new WaitForSeconds(�b��); // ����̕b��������҂�(���C���Q�[�������Ƃ��͓K���Ȃ��A�o�O���������₷��)

        }

        canvasGroup[number].alpha = 1;
        // �t�F�[�h���I������瑀��\�ɂȂ�
        canvasGroup[number].interactable = true;
        
    }


}
