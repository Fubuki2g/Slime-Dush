using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySearch : MonoBehaviour
{
    // �v���C���[���N�����Ă���t���O
    public bool playerOn;

    // collider�R���|�[�l���g
    SphereCollider col;
    
    void Start()
    {
        col = GetComponent<SphereCollider>();
    }

    
    void Update()
    {
        
    }

    // Collider�ɐڐG���Ă����
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerOn = true;
            // col.radius = 2;
        }
    }

    // Collider����o����
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerOn = false;
            // col.radius = 1;
        }
    }
}
