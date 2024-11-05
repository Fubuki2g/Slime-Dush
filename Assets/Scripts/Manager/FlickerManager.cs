using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerManager : Singleton<FlickerManager>
{
    ///オブジェクトを明滅させる方法の１つ
    ///マテリアルのEmmisionの色を変更するやり方
    ///事前にマテリアルのEmmisionの項目はチェックボックスにチェックしておくこと

    ///3Dを描画しているコンポーネントは
    ///MeshRendererだけでなく、SkinnedMeshRenderer もある
    ///コンポーネントが異なるので、どちらで描画しているか確認して
    ///変数の型を定義すること


    ///シェーダーの種類によって、Emmisionのプロパティ（EnableKeyword等）が
    ///異なる場合があるので事前に調べる必要がある
    ///EnableKeyword　：　Standard…"_EMISSION"         UnityChanシェーダー"_EMISSIVE"
    ///EmissionColor　：　Standard…"_EmissionColor"    UnityChanシェーダー"_Emissive_Color"

    ///プロパティはインスペクタービューのシェーダーの上で
    ///右クリックしメニュー項目からSelect Shaderを選択すると表示される


    [Header("明滅させたいオブジェクト")]
    [SerializeField, Label("対象")] GameObject target;

    [Header("明滅の処理")]
    [SerializeField, Label("明滅の強さ")] float intensity;
    [SerializeField, Label("明滅の早さ")] float duration;
    [SerializeField, Label("明滅する時間")] float timer;

    //ColorUsageは第二引数をTrueにするとHDRカラーパネル(Emmisionのカラー)になる
    [SerializeField, Label("元となる色")] [ColorUsage(false, true)] Color color1;
    [SerializeField, Label("明滅時の色")] [ColorUsage(false, true)] Color color2;

    //対象を構成する全てのマテリアルが明滅するように
    //配列でMeshRendererを持つオブジェクト全てを格納できるようにする
    //※MeshRendererなのかSkinnedMeshRendererなのか注意
    SkinnedMeshRenderer[] skinmeshs;
    MeshRenderer[] meshs;
    Material[] skinmats;
    Material[] mats;

    float currentTime;  //現在の時間を計測する用
    bool flickerFLG;    //明滅中かを判定するフラグ


    void Start()
    {
        //GetComponentsInChildrenで、対象オブジェクトだけでなくその子オブジェクト以下の
        //オブジェクト全てからコンポーネントを取得できる
        skinmeshs = target.GetComponentsInChildren<SkinnedMeshRenderer>();
        meshs = target.GetComponentsInChildren<MeshRenderer>();

        //対象の総数だけmaterialの配列の数を設定
        skinmats = new Material[skinmeshs.Length];
        mats = new Material[meshs.Length];

        //全SkinnedMeshRendererからそれぞれのマテリアルを取得
        for (int i = 0; i < skinmeshs.Length; i++)
        {
            skinmats[i] = skinmeshs[i].material;
            skinmats[i].EnableKeyword("_EMISSION");   //マテリアルのEmmision設定(Standard)
        }

        for (int i = 0; i < meshs.Length; i++)
        {
            mats[i] = meshs[i].material;
            mats[i].EnableKeyword("_EMISSION");   //マテリアルのEmmision設定(Standard)
        }

    }

    void Update()
    {
        frash();

    }

    public void frash()
    {
        //明滅していない時に攻撃が当たると明滅開始
        if ((ECharacter.Instance.Edamage == true) && !flickerFLG)
        {
            flickerFLG = true;
            Debug.Log("明滅開始");
        }

        //フラグが立っていたら明滅し続ける
        if (flickerFLG)
        {
            Fricker();
            Debug.Log("明滅中");
        }
    }

    //明滅の処理
    public void Fricker()
    {
        //時間の計測
        if (currentTime <= timer)
        {
            currentTime += Time.deltaTime;

            //skinnedMeshRendererのマテリアルが
            //color1とcolor2の色をMathf.PingPongで繰り返す
            foreach (Material material in skinmats)
            {
                material.SetColor("_EmissionColor",
                    Color.Lerp(color2 * intensity, color1, Mathf.PingPong(currentTime * duration, 1f)));
            }

            foreach (Material material in mats)
            {
                material.SetColor("_EmissionColor",
                    Color.Lerp(color2 * intensity, color1, Mathf.PingPong(currentTime * duration, 1f)));
            }

        }
        else
        {
            //時間が過ぎたら明滅終了
            EndFlicker();
        }
    }

    //明滅終了処理
    public void EndFlicker()
    {
        flickerFLG = false;
        currentTime = 0;

        //全ての色を元に戻す
        foreach (Material material in skinmats)
        {
            material.SetColor("_EmissionColor", color1);
        }
        Debug.Log("明滅終了");

        foreach (Material material in mats)
        {
            material.SetColor("_EmissionColor", color1);
        }
        Debug.Log("明滅終了");
    }

}
