using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aimor : MonoBehaviour
{
    public float bulletSpeed = 10;

    public float destroyTime = 1;

    public GameObject effect;

    Rigidbody rig; //�ϐ�Rigidbody�^

    void Start()
    {

        // �I�u�W�F�N�g�̃R���|�[�l���g���擾
        rig = GetComponent<Rigidbody>();

        // ���W�b�h�{�f�B�ɗ͂�������
        // ��1�����c�͂̋����A��2�����c�͂̎��
        rig.AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);

        // ��1�����c���������́H�A��2�����c�������s������
        // this�͏ȗ��ł���
        Destroy(this.gameObject, destroyTime);

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(effect, transform.position, transform.rotation);
        // �G�t�F�N�g�𐶐�������Ɏ������g������
        Destroy(gameObject);
    }

}
