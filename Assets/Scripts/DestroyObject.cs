using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            Death();
        }
    }

    void Death()
    {
        SoundManager.Instance.PlaySE_Game(1);
        Destroy(gameObject);
        Instantiate(EffectManager.Instance.StageFX[0], transform.position, Quaternion.identity);
        Debug.Log("“GŽ€–S");
    }
}
