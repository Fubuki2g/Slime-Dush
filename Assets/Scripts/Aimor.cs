using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aimor : MonoBehaviour
{
    public float bulletSpeed = 10;

    public float destroyTime = 1;

    public GameObject effect;

    Rigidbody rig; //変数Rigidbody型

    void Start()
    {

        // オブジェクトのコンポーネントを取得
        rig = GetComponent<Rigidbody>();

        // リジッドボディに力を加える
        // 第1引数…力の強さ、第2引数…力の種類
        rig.AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);

        // 第1引数…何を消すの？、第2引数…処理を行う時間
        // thisは省略できる
        Destroy(this.gameObject, destroyTime);

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(effect, transform.position, transform.rotation);
        // エフェクトを生成した後に自分自身を消す
        Destroy(gameObject);
    }

}
