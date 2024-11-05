using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : Singleton<PlayerController>
{
    [Header("Playerの移動パラメータ")]
    public float moveSpeed = 10;   // 移動速度
    public float jumpPower = 10;   // ジャンプ力
    public float riseTime = 1;     // 上昇時間
    public float gravity = 10;   // 重力
    public float rotateSpeed = 10; // 回転速度
    public float knockBack = 10;   // ノックバック量

    // ジャンプに必要な変数
    float riseTimeTemp; // 一時的に数値を保持する変数
    bool jumpFLG;

    // プレイヤーの状態を表すグラフ
    bool death, clear;      // 死亡、クリア
    public bool invincible; // ダメージ中(無敵状態)
    bool inputPossible;     // 操作可能な状態

    [Header("カメラの切り替え")]
    public GameObject[] changeCamera; // カメラのオブジェクト
    int cameraNumber;                 // アクティブ

    CharacterController characon;
    Animator animator;

    // 移動に関する変数
    float hor, ver;           // 入力の値を代入する変数
    Vector3 moveDirection;    // 移動方向と移動量を代入するための変数
    Vector3 gravityDirection; // Y方向の移動量を代入するための変数

    // 回転に関する変数
    Vector3 cameraForward;    // カメラの方向
    Vector3 moveForward;      // 移動方向
    Quaternion rotation;      // 回転

    [Header("発射するスライム")]
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
        // メインゲーム中の時 または ダメージ中以外の時に操作可能
        if (GameManager.Instance.mainGame && !GameManager.Instance.state_damage)
        {
            inputPossible = true;
        }
        else
        {
            inputPossible = false;
        }
    }


    // 移動処理
    void Move()
    {
        // InputManagerで設定された入力方法から
        // 入力した値を変数に代入
        hor = Input.GetAxis("Horizontal"); // 水平(左右)
        ver = Input.GetAxis("Vertical");   // 垂直(上下)

        // Debug.Log("左右 = " + hor);
        // Debug.Log("上下 = " + ver);

        // 各ベクトルへ(移動方向)に入力の値を入れる
        // moveDirection.x = hor * moveSpeed;
        // moveDirection.z = ver * moveSpeed;

        // カメラが向いているほうが正面になるように移動
        moveDirection.x = moveForward.x;
        moveDirection.z = moveForward.z;

        // ※斜め移動の時は1.4倍の速度になってしまう
        // 同じ速度を保つのにnormalizedを使って正規化する
        moveDirection = new Vector3(moveDirection.normalized.x, 0, moveDirection.z) * moveSpeed;


        // CharacterControllerの「Move関数」を使って、
        // 移動の文だけ移動実行
        characon.Move(moveDirection * Time.deltaTime);

    }

    // 重力処理
    void Gravity()
    {
        // CharacterControllerのisGround関数で
        // 地面に立っているかどうかか判別してくれる

        gravityDirection.y -= gravity * Time.deltaTime;

        // Move関数を用いてY方向に落ちる
        characon.Move(gravityDirection * Time.deltaTime);

        // もしも地面に接触したら
        if (characon.isGrounded)
        {
            riseTimeTemp = 0;
            jumpFLG = false;
            gravityDirection.y = -0.1f;
            Debug.Log("地面に立っている");

        }
        else
        {
            Debug.Log("空中にいるよ");
        }

    }
    // ジャンプ処理
    void jump()
    {
        // 地面に設置しているときのみ入力可能
        if (characon.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                jumpFLG = true;
                gravityDirection.y = jumpPower;
                // Debug.Log("ジャンプしたよ");
            }
        }
        else
        {
            // 空中にいる時
            // ボタンを離すか、一定以上上昇を続けたら上昇をやめる
            //                                             計測          上昇
            if (jumpFLG && Input.GetButtonUp("Jump") || riseTimeTemp > riseTime)
            {
                jumpFLG = false;
            }

            // ボタンを押しっぱなしだったら、時間を計測して
            // 一定時間まで上昇を続ける
            if (jumpFLG && Input.GetButton("Jump") && riseTimeTemp <= riseTime)
            {
                riseTimeTemp += Time.deltaTime; // 時間を加算
                gravityDirection.y = jumpPower;
            }
        }
    }

    // キャラが回転する際、カメラの向いている方向が正面になるよう処理
    void CameraDirection()
    {
        // カメラの方向から、X-Z平面の単位ベクトルを取得
        cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        // 方向キーの入力値とカメラの向きから、移動方向を決定
        moveForward = cameraForward * ver + Camera.main.transform.right * hor;
    }

    // 回転
    void Rotate()
    {
        // 移動している時のみ回転してほしい
        if (hor != 0 || ver != 0)
        {
            // 回転方向は？
            rotation = Quaternion.LookRotation(moveForward);

            //                                                  第1引数     第2引数     第3引数
            // 回転処理                                        (今の角度, 向きたい角度, 向く速度)
            transform.localRotation = Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed * Time.deltaTime);
        }
    }

    // カメラをすべて非アクティブに
    void CameraInit()
    {
        for (int i = 0; i < changeCamera.Length; i++)
        {
            changeCamera[i].SetActive(false);
        }
    }

    // カメラ切り替え
    public void CameraChange()
    {
        cameraNumber++; // 次のカメラを指定

        if (cameraNumber >= changeCamera.Length)
        {
            cameraNumber = 0;
        }

        // 一旦全カメラを非アクティブにした後で
        // 次の番号のカメラをアクティブに
        CameraInit();
        changeCamera[cameraNumber].SetActive(true);
        Debug.Log(changeCamera[cameraNumber] + "に切り替わりました");
    }

    public void CameraReturn()
    {
        cameraNumber--; // 次のカメラを指定

        if (cameraNumber >= changeCamera.Length)
        {
            cameraNumber = 0;
        }

        // 一旦全カメラを非アクティブにした後で
        // 次の番号のカメラをアクティブに
        CameraInit();
        changeCamera[cameraNumber].SetActive(true);
        Debug.Log(changeCamera[cameraNumber] + "に切り替わりました");
    }

    public void KnockBack()
    {
        // アニメーション名が"Damage"の時
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Damage"))
        {
            // アニメーションの再生時間が0.8の時まで
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.8)
            {
                // 後方に吹き飛ぶ
                moveDirection = -transform.forward * knockBack;
                characon.Move(moveDirection * Time.deltaTime);
            }
            else
            {
                // ダメージ状態の終了
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
            // プレハブを生成する処理
            // 第1引数…オブジェクト、第二引数…生成するポジション、第3引数…生成した時の角度
            Instantiate(aimor, firePos.transform.position, transform.rotation);

            Debug.Log("弾を発射");

            GameManager.Instance.shootitemCount -= 1;
            PlayerHitCheck.Instance.Player.transform.localScale = new Vector3(PlayerHitCheck.Instance.X -= 0.2f, PlayerHitCheck.Instance.Y -= 0.2f, PlayerHitCheck.Instance.Z -= 0.2f);
            PlayerHitCheck.Instance.Speed = 2;
            moveSpeed -= PlayerHitCheck.Instance.Speed;
            GameManager.Instance.Shoot(10);
            PlayerController.Instance.CameraReturn();

        }
    }

}
