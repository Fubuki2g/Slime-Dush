using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySearch : MonoBehaviour
{
    // プレイヤーが侵入しているフラグ
    public bool playerOn;

    // colliderコンポーネント
    SphereCollider col;
    
    void Start()
    {
        col = GetComponent<SphereCollider>();
    }

    
    void Update()
    {
        
    }

    // Colliderに接触している間
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerOn = true;
            // col.radius = 2;
        }
    }

    // Colliderから出たら
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerOn = false;
            // col.radius = 1;
        }
    }
}
