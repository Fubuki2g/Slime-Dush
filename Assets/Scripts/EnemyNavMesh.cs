using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMesh : Singleton<EnemyNavMesh>
{
    Animator animator;

    [Header("索敵時のスピード")]
    public int nsp = 10;
    public int nac = 10;
    public int nan = 10;

    [Header("巡回時のスピード")]
    public int nspe = 10;
    public int nacc = 10;
    public int nang = 10;


    // 移動の方法
    enum MoveType
    {
        なし,
        プレイヤーを追跡,
        ルートを巡回,
        巡回しつつ追跡,
        逃げる,
        巡回しつつ逃げる
    }

    // 移動方法をエディターに表示
    // 表示したいが、他のところからアクセスさせたくない
    [SerializeField] private MoveType type;
    // public MoveType type;
    // 他からアクセスしたいが、表示はしたくない
    // [System.NonSerialized] public MoveType type;
    //                        ↳ 無くてもいい

    // 追跡したい対象
    GameObject target;

    // ルート移動用のポイント設定
    [SerializeField] GameObject[] routePoint;
    int nextPoint;

    // NavMeshAgentコンポーネント用
    NavMeshAgent nav;

    // 索敵をしている子オブジェクト
    // [SerializeField] GameObject searchObject;
    EnemySearch search;

    
    void Start()
    {
        animator = GetComponent<Animator>();
            animator.SetTrigger("Hit");

        // 追跡したい対象をTagから検索
        target = GameObject.FindGameObjectWithTag("Player");

        // コンポーネントを取得
        nav = GetComponent<NavMeshAgent>();
        
        // 自分の子を探す処理
        // search = transform.GetChild(1).GetComponent<EnemySearch>();
        // Transform mySon = transform.GetChild(1);
        // search = searchObject.GetComponent<EnemySearch>();
        // 探す処理があるので何も処理が無くても子を作らなければならない
    }


    void Update()
    {
        // ゲームごとの移動方法を実行
        switch (type)
        {
            case MoveType.なし:
                break;

            case MoveType.プレイヤーを追跡:
                PlayerChase();
                break;

            case MoveType.ルートを巡回:
                routePatrol();
                break;

            case MoveType.巡回しつつ追跡:
                ChaseAndPatrol();
                break;

            case MoveType.逃げる:
                PlayerRun();
                break;

            case MoveType.巡回しつつ逃げる:
                SearchAndRun();
                break;

        }

    }

    void PlayerChase()
    {
        // ターゲットが存在している時のみ
        if (target != null)
        {
            if (search.playerOn)
            {
                // プレイヤーがダメージ中じゃなければ
                if (!GameManager.Instance.state_damage)
                {
                    // 行き先を設定(行き先のポジション)
                    nav.SetDestination(target.transform.position);

                }

            }
            else
            {
                nav.SetDestination(transform.position);
            }
            return;

        }
        Debug.LogWarning("ターゲットが存在していない");  

    }

    // ルート移動の処理
    void routePatrol()
    {
            // 目標地点に近づいたら次の拠点を設定
            if (nav.pathPending == false && nav.remainingDistance <= 0.1f)
        {
            // 次の地点のポイントを目標に
            nav.destination = routePoint[nextPoint].transform.position;

            // 配列の次の値を設定、次がなければ０に戻る
            nextPoint = (nextPoint + 1) % routePoint.Length;
        }
    }

    // 巡回しつつ追跡もする
    void ChaseAndPatrol()
    {
        //  索敵範囲に侵入したら追跡
        if (search.playerOn)
        {
            PlayerChase();
            nav.speed = nsp;
            nav.acceleration = nac;
            nav.angularSpeed = nan;
        }
        else
        {
            // それ以外の時は巡回
            routePatrol();
            nav.speed = nspe;
            nav.acceleration = nacc;
            nav.angularSpeed = nang;
        }

    }

    // プレイヤーから逃げる
    void PlayerRun()
    {
        if (target != null)
        {
            if (search.playerOn)
            {
                // 逃げ先のポジションを決める
                Vector3 dir = transform.position - target.transform.position;

                // 行き先を設定
                nav.SetDestination(transform.position + dir * 0.3f);

            }
            else
            {
                nav.SetDestination(transform.position);
            }
        }
    }

    void SearchAndRun()
    {
        if (target != null)
        {
            if (search.playerOn)
            {
                PlayerRun();
            }
            else
            {
                routePatrol();
            }
        }
    }
    
}
