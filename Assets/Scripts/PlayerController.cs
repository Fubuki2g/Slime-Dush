using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : Singleton<PlayerController>
{
    [Header("Player�̈ړ��p�����[�^")]
    public float moveSpeed = 10;   // �ړ����x
    public float jumpPower = 10;   // �W�����v��
    public float riseTime = 1;     // �㏸����
    public float gravity = 10;   // �d��
    public float rotateSpeed = 10; // ��]���x
    public float knockBack = 10;   // �m�b�N�o�b�N��

    // �W�����v�ɕK�v�ȕϐ�
    float riseTimeTemp; // �ꎞ�I�ɐ��l��ێ�����ϐ�
    bool jumpFLG;

    // �v���C���[�̏�Ԃ�\���O���t
    bool death, clear;      // ���S�A�N���A
    public bool invincible; // �_���[�W��(���G���)
    bool inputPossible;     // ����\�ȏ��

    [Header("�J�����̐؂�ւ�")]
    public GameObject[] changeCamera; // �J�����̃I�u�W�F�N�g
    int cameraNumber;                 // �A�N�e�B�u

    CharacterController characon;
    Animator animator;

    // �ړ��Ɋւ���ϐ�
    float hor, ver;           // ���͂̒l��������ϐ�
    Vector3 moveDirection;    // �ړ������ƈړ��ʂ������邽�߂̕ϐ�
    Vector3 gravityDirection; // Y�����̈ړ��ʂ������邽�߂̕ϐ�

    // ��]�Ɋւ���ϐ�
    Vector3 cameraForward;    // �J�����̕���
    Vector3 moveForward;      // �ړ�����
    Quaternion rotation;      // ��]

    [Header("���˂���X���C��")]
    public GameObject firePos;
    public GameObject aimor;

    void Start()
    {
        characon = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

    }

    
    void Update()
    {
        InputCheck();

        CameraDirection();
        Rotate();
        KnockBack();
        Animation();
        Shoot();

        if(inputPossible)
        {
            Move();
            Gravity();
            jump();
            return;

        }

        Gravity();



    }

    void InputCheck()
    {
        // ���C���Q�[�����̎� �܂��� �_���[�W���ȊO�̎��ɑ���\
        if (GameManager.Instance.mainGame && !GameManager.Instance.state_damage)
        {
            inputPossible = true;
        }
        else
        {
            inputPossible = false;
        }
    }


    // �ړ�����
    void Move()
    {
        // InputManager�Őݒ肳�ꂽ���͕��@����
        // ���͂����l��ϐ��ɑ��
        hor = Input.GetAxis("Horizontal"); // ����(���E)
        ver = Input.GetAxis("Vertical");   // ����(�㉺)

        // Debug.Log("���E = " + hor);
        // Debug.Log("�㉺ = " + ver);

        // �e�x�N�g����(�ړ�����)�ɓ��͂̒l������
        // moveDirection.x = hor * moveSpeed;
        // moveDirection.z = ver * moveSpeed;

        // �J�����������Ă���ق������ʂɂȂ�悤�Ɉړ�
        moveDirection.x = moveForward.x;
        moveDirection.z = moveForward.z;

        // ���΂߈ړ��̎���1.4�{�̑��x�ɂȂ��Ă��܂�
        // �������x��ۂ̂�normalized���g���Đ��K������
        moveDirection = new Vector3(moveDirection.normalized.x, 0, moveDirection.z) * moveSpeed;


        // CharacterController�́uMove�֐��v���g���āA
        // �ړ��̕������ړ����s
        characon.Move(moveDirection * Time.deltaTime);

    }

    // �d�͏���
    void Gravity()
    {
        // CharacterController��isGround�֐���
        // �n�ʂɗ����Ă��邩�ǂ��������ʂ��Ă����

        gravityDirection.y -= gravity * Time.deltaTime;

        // Move�֐���p����Y�����ɗ�����
        characon.Move(gravityDirection * Time.deltaTime);

        // �������n�ʂɐڐG������
        if (characon.isGrounded)
        {
            riseTimeTemp = 0;
            jumpFLG = false;
            gravityDirection.y = -0.1f;
            Debug.Log("�n�ʂɗ����Ă���");

        }
        else
        {
            Debug.Log("�󒆂ɂ����");
        }

    }
    // �W�����v����
    void jump()
    {
        // �n�ʂɐݒu���Ă���Ƃ��̂ݓ��͉\
        if (characon.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                jumpFLG = true;
                gravityDirection.y = jumpPower;
                // Debug.Log("�W�����v������");
            }
        }
        else
        {
            // �󒆂ɂ��鎞
            // �{�^���𗣂����A���ȏ�㏸�𑱂�����㏸����߂�
            //                                             �v��          �㏸
            if (jumpFLG && Input.GetButtonUp("Jump") || riseTimeTemp > riseTime)
            {
                jumpFLG = false;
            }

            // �{�^�����������ςȂ���������A���Ԃ��v������
            // ��莞�Ԃ܂ŏ㏸�𑱂���
            if (jumpFLG && Input.GetButton("Jump") && riseTimeTemp <= riseTime)
            {
                riseTimeTemp += Time.deltaTime; // ���Ԃ����Z
                gravityDirection.y = jumpPower;
            }
        }
    }

    // �L��������]����ہA�J�����̌����Ă�����������ʂɂȂ�悤����
    void CameraDirection()
    {
        // �J�����̕�������AX-Z���ʂ̒P�ʃx�N�g�����擾
        cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        // �����L�[�̓��͒l�ƃJ�����̌�������A�ړ�����������
        moveForward = cameraForward * ver + Camera.main.transform.right * hor;
    }

    // ��]
    void Rotate()
    {
        // �ړ����Ă��鎞�̂݉�]���Ăق���
        if (hor != 0 || ver != 0)
        {
            // ��]�����́H
            rotation = Quaternion.LookRotation(moveForward);

            //                                                  ��1����     ��2����     ��3����
            // ��]����                                        (���̊p�x, ���������p�x, �������x)
            transform.localRotation = Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed * Time.deltaTime);
        }
    }

    // �J���������ׂĔ�A�N�e�B�u��
    void CameraInit()
    {
        for (int i = 0; i < changeCamera.Length; i++)
        {
            changeCamera[i].SetActive(false);
        }
    }

    // �J�����؂�ւ�
    public void CameraChange()
    {
        cameraNumber++; // ���̃J�������w��

        if (cameraNumber >= changeCamera.Length)
        {
            cameraNumber = 0;
        }

        // ��U�S�J�������A�N�e�B�u�ɂ������
        // ���̔ԍ��̃J�������A�N�e�B�u��
        CameraInit();
        changeCamera[cameraNumber].SetActive(true);
        Debug.Log(changeCamera[cameraNumber] + "�ɐ؂�ւ��܂���");
    }

    public void CameraReturn()
    {
        cameraNumber--; // ���̃J�������w��

        if (cameraNumber >= changeCamera.Length)
        {
            cameraNumber = 0;
        }

        // ��U�S�J�������A�N�e�B�u�ɂ������
        // ���̔ԍ��̃J�������A�N�e�B�u��
        CameraInit();
        changeCamera[cameraNumber].SetActive(true);
        Debug.Log(changeCamera[cameraNumber] + "�ɐ؂�ւ��܂���");
    }

    public void KnockBack()
    {
        // �A�j���[�V��������"Damage"�̎�
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Damage"))
        {
            // �A�j���[�V�����̍Đ����Ԃ�0.8�̎��܂�
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.8)
            {
                // ����ɐ������
                moveDirection = -transform.forward * knockBack;
                characon.Move(moveDirection * Time.deltaTime);
            }
            else
            {
                // �_���[�W��Ԃ̏I��
                GameManager.Instance.state_damage = false;
                invincible = false;
            }
        }
    }

    void Animation()
    {
        if (GameManager.Instance.state_damage && !invincible)
        {
            animator.SetTrigger("Damage");
            invincible = true;
        }

        if (GameManager.Instance.state_damage && !death && GameManager.Instance.HPCurrent == 0)
        {
            animator.SetTrigger("Death");
            death = true;
        }
    }

    void Shoot()
    {

        if (GameManager.Instance.shootitemCount >= 1 && Input.GetKeyDown(KeyCode.Return) && GameManager.Instance.mainGame == true)
        {
            // �v���n�u�𐶐����鏈��
            // ��1�����c�I�u�W�F�N�g�A�������c��������|�W�V�����A��3�����c�����������̊p�x
            Instantiate(aimor, firePos.transform.position, transform.rotation);

            Debug.Log("�e�𔭎�");

            GameManager.Instance.shootitemCount -= 1;
            PlayerHitCheck.Instance.Player.transform.localScale = new Vector3(PlayerHitCheck.Instance.X -= 0.2f, PlayerHitCheck.Instance.Y -= 0.2f, PlayerHitCheck.Instance.Z -= 0.2f);
            PlayerHitCheck.Instance.Speed = 2;
            moveSpeed -= PlayerHitCheck.Instance.Speed;
            GameManager.Instance.Shoot(10);
            PlayerController.Instance.CameraReturn();

        }
    }

}
