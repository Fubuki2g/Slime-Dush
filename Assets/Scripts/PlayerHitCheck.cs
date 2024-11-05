using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHitCheck : Singleton<PlayerHitCheck>
{
    PlayerController controller;
    [Header("�X�e�[�^�X�̕ω�")]
    [SerializeField] public float Speed = 0;
    [SerializeField] public float X = 0.6f;
    [SerializeField] public float Y = 0.6f;
    [SerializeField] public float Z = 0.6f;

    public GameObject Player;

    void Start()
    {
        controller = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        controller = GetComponent<PlayerController>();
      
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Clear")
        {
            GameManager.Instance.GameClear(); // �N���A
            Debug.Log("�S�[��������");
            return;

        }

        if (other.transform.tag == "Item")
        {
            Debug.Log("�A�C�e�����������");
            Destroy(other.gameObject);
            SoundManager.Instance.PlaySE_Game(2);
            GameManager.Instance.ItemGet();
            GameManager.Instance.Heal(10);
            Speed = 2;
            controller.moveSpeed += Speed;
            Player.transform.localScale = new Vector3(X += 0.2f, Y += 0.2f, Z += 0.2f);
            PlayerController.Instance.CameraChange();

        }

        if (other.transform.tag == "Enemy" || other.transform.tag == "PEnemy" && !controller.invincible)
        {
            // �G�̕����Ɍ������鏈��
            Quaternion rotation = Quaternion.LookRotation(other.transform.position - transform.position);

            rotation = Quaternion.Euler(0f, rotation.eulerAngles.y, 0f);

            transform.localRotation = rotation;

            SoundManager.Instance.PlaySE_Game(3);
            GameManager.Instance.Damage(10);
            Speed = 2;
            controller.moveSpeed -= Speed;
            Player.transform.localScale = new Vector3(X -= 0.2f, Y -= 0.2f, Z -= 0.2f);
            PlayerController.Instance.CameraReturn();

        }

        if (other.transform.tag == "DeadArea")
        {
            // FadeManager����V�[�����[�h���s��
            FadeManager.Instance.LoadScene(SceneManager.GetActiveScene().name, 1);

            // SoundManager����SE���Ăяo��
            SoundManager.Instance.PlaySE_Sys(0);
        }

    }

}
