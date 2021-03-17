using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    public bool isGameUp;

    [SerializeField]
    private DefenceBase defenceBase;

    [SerializeField]
    private EnemyGenerator enemyGenerator;

    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private Transform temporaryObjectContainerTran;

    [SerializeField]
    private GameObject fireworkPrefab;

    [SerializeField]
    private Transform canvasTran;

    public UIManager uiManager;

    public BulletSelectManager bulletSelectManager;

    public bool isSetUpEnd;

    IEnumerator Start()
    {
        isSetUpEnd = false;

        SwitchGameUp(false);

        defenceBase.SetUpDefenceBase(this);

        playerController.SetUpPlayer(this);

        enemyGenerator.SetUpEnemyGenerator(this);

        TransformHelper.TemporaryObjectContainerTran = temporaryObjectContainerTran;

        uiManager.HideGameClearSet();

        uiManager.HideGameOverSet();

        uiManager.HideBossAlertSet();

        yield return StartCoroutine(bulletSelectManager.GenerateBulletSelectDetail(this));

        yield return StartCoroutine(uiManager.PlayOpening());

        bulletSelectManager.JugdeOpenBullets();

        isSetUpEnd = true;
    }

    // Update is called once per frame
    
    public void SwitchGameUp(bool isSwitch)
    {
        isGameUp = isSwitch;

        if(isGameUp)
        {
            //画面に残っているエネミーをすべて破壊する
            enemyGenerator.ClearEnemiesList();

            //一時オブジェクトを破壊する（子オブジェクトである、ボスのエネミーのバレットやエフェクト等も一緒に破壊される
            enemyGenerator.DestroyTemporaryObjectContainer();
        }
    }

    public void PreparateGameClear()
    {
        //ゲームクリアの表示を行う
        uiManager.DisplayGameClaerSet();

        //花火の演出
        StartCoroutine(GenerateFireWorks());
    }

    public void PreparateGameOver()
    {
        uiManager.DisplayGameOverSet();
    }

    public Vector3 GetPlayerDirection(Vector3 enemyPos)
    {
        return (playerController.transform.position - enemyPos).normalized;
    }

    /// <summary>
    /// ランダムな数の花火の生成
    /// </summary>
    /// <returns></returns>
    private IEnumerator GenerateFireWorks()
    {
        yield return new WaitForSeconds(1.5f);

        //ランダムな値を取得し、その回数だけ処理を繰り返す
        for(int i = 0; i < Random.Range(5, 8); i++)
        {
            //花火のゲームオブジェクト生成
            GameObject fireworks = Instantiate(fireworkPrefab, canvasTran, false);

            //花火の色を変更するために、花火のゲームオブジェクトにアタッチされているパーティクルシステムのメイン情報を取得
            ParticleSystem.MainModule main = fireworks.GetComponent<ParticleSystem>().main;

            //パーティクルの色をランダムな2色に変更
            main.startColor = GetNewTwoRandomColors();

            //花火の位置をランダムな値を加えて変更
            fireworks.transform.localPosition = new Vector3(fireworks.transform.localPosition.x + Random.Range(-500, 500), fireworks.transform.localPosition.y + Random.Range(700, 1000));

            //指定した秒数後に花火のゲームオブジェクトを破壊
            Destroy(fireworks, 3f);

            //次の生成まで一次処理を中断（１つの花火のゲームオブジェクトが生成されてから1秒後に次の花火の生成）
            yield return new WaitForSeconds(1.0f);
        }
    }

    /// <summary>
    /// パーティクルの色をランダムで指定
    /// </summary>
    /// <returns></returns>
    private ParticleSystem.MinMaxGradient GetNewTwoRandomColors()
    {
        //
        return new ParticleSystem.MinMaxGradient(GetRandomColors(), GetRandomColors());
    }

    /// <summary>
    /// ランダムな色を取得
    /// </summary>
    /// <returns></returns>
    private Color GetRandomColors()
    {
        //Color32はbyte型で色の指定が可能なので、色の各成分用の値をint型でランダムに取得して、byte型にキャストして指定
        return new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 255);
    }
}
