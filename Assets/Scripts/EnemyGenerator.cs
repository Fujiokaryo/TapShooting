using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField]
    private EnemyController enemyObjPrefab; //エネミーのプレファブ

    [SerializeField]
    public float preparateTime; //エネミー生成までの時間

    private int generateCount; //生成下エネミーの数をカウント

    private float timer;

    private Gamemanager gamemanager;

    public int maxGenerate;

    public bool isGenerateEnd;

    public bool isBossDestroyed;

    public EnemyDataSO enemyDataSO;

    private List<EnemyDataSO.EnemyData> normalEnemyDatas = new List<EnemyDataSO.EnemyData>();

    private List<EnemyDataSO.EnemyData> bossEnemyDatas = new List<EnemyDataSO.EnemyData>();

    public MoveEventSO moveEventSO; //エネミー移動用のスクリプタブルオブジェクト

    [SerializeField]
    private List<EnemyController> enemiesList = new List<EnemyController>();

    /// <summary>
    /// EnemyGanaratorの設定
    /// </summary>
    /// <param name="gamemanager"></param>
    public void SetUpEnemyGenerator(Gamemanager gamemanager)
    {
        this.gamemanager = gamemanager;

        normalEnemyDatas = GetEnemyTypeList(EnemyType.Normal);

        bossEnemyDatas = GetEnemyTypeList(EnemyType.Boss);

        maxGenerate = GameData.instance.GetMaxGenerateCount();
    }
    void Update()
    {
        if(isGenerateEnd)
        {
            return;
        }

        if(!gamemanager.isGameUp)
        {           
                PreparateGenerateEnemy();
        }

        
    }

    private void PreparateGenerateEnemy()
    {
        timer += Time.deltaTime;

        if(timer >= preparateTime)
        {
            timer = 0;

            GenerateEnemy();

            generateCount++;

            if(generateCount >= maxGenerate)
            {
                isGenerateEnd = true;

                Debug.Log("生成完了+ボス出現演出");

                StartCoroutine(GenerateBoss());
            }

        }
    }

    private void GenerateEnemy(EnemyType enemyType = EnemyType.Normal)
    {
       //ランダムな値を代入するための変数を宣言
       int randomEnemyNo;

       //EnemyDataを代入するための変数を宣言
       EnemyDataSO.EnemyData enemyData = null;

        //EnemyTypeに合わせて生成するエネミーの種類を決定しそのエネミーの種類毎のリストからランダムなEnemyDataを取得
        switch (enemyType)
        {
            case EnemyType.Normal:
                randomEnemyNo = Random.Range(0, normalEnemyDatas.Count);
                enemyData = normalEnemyDatas[randomEnemyNo];
                break;

            case EnemyType.Boss:
                randomEnemyNo = Random.Range(0, bossEnemyDatas.Count);
                enemyData = bossEnemyDatas[randomEnemyNo];
                break;

        }
       //プレファブからエネミーのクローンを生成する。生成位置はEnemyGeneratorの位置。戻り値の値はEnemyController型になる
       EnemyController enemyController = Instantiate(enemyObjPrefab, transform, false);
       
       //EnemyControllerスクリプトのSetUpEnemyメソッドを実行する（Startメソッドの変わりの処理）
       enemyController.SetUpEnemy(enemyData);
       
       //Boss以外でも追加設定を行う
       enemyController.AdditionalSetUPEnemy(this);

        //Listに生成したエネミーの情報を追加
        enemiesList.Add(enemyController);
        
    }

    private IEnumerator GenerateBoss()
    {
        // TODO ボス出現の警告演出

        yield return new WaitForSeconds(1.0f);

        // TODO ボス生成
        GenerateEnemy(EnemyType.Boss);


    }

    public void SwitchBossDestroyed(bool switchBossDestroyed)
    {
        isBossDestroyed = switchBossDestroyed;

        gamemanager.SwitchGameUp(isBossDestroyed);

        gamemanager.PreparateGameClear();
    }

    /// <summary>
    /// 引数で指定されたエネミーの種類のListを作成し、作成した値を戻り値で返す
    /// </summary>
    /// <param name="enemyType"></param>
    /// <returns></returns>
    private List<EnemyDataSO.EnemyData> GetEnemyTypeList(EnemyType enemyType)
    {
        //引数で受け取ったEnemyTypeのEnemyDataだけが代入されるListを用意する
        List<EnemyDataSO.EnemyData> enemyDatas = new List<EnemyDataSO.EnemyData>();

        //引数で受け取ったEnemyTypeのエネミーのデータだけをスクリプタブル・オブジェクトより抽出してリストに追加する
        for(int i = 0; i < enemyDataSO.enemyDataList.Count; i++)
        {
            //1つずつEnemyData内の情報にあるEnemyTypeを確認し、それが引数で受け取ったEnemyTypeと同じTypeの情報の場合
            if(enemyDataSO.enemyDataList[i].enemyType == enemyType)
            {
                //用意しておいたListにEnemyDataを追加する。これによりListにはEnemyTypeが引数で届いたTypeのEnemyDataのみが抽出される
                enemyDatas.Add(enemyDataSO.enemyDataList[i]);
            }
        }

        //抽出結果が代入されているListを処理結果として戻り値として返す
        return enemyDatas;
    }

    /// <summary>
    /// TotalExpの表示更新準備
    /// </summary>
    /// <param name="exp"></param>
    public void PreparateDisplayTotalExp(int exp)
    {
        //GameManagerスクリプトからUIManagerスクリプトのUpdateDisplayTptalExpメソッドを実行する
        gamemanager.uiManager.UpdateDisplayTotalExp(GameData.instance.GetTotalExp());

        //UIManagerスクリプトのCreateFloatingMessageToExpメソッドを実行する
        gamemanager.uiManager.CreateFloatingMessageToExp(exp, FloatingMessage.FloatingMessageType.GetExp);

        //使用可能バレット選択ボタンの確認と更新
        gamemanager.bulletSelectManager.JugdeOpenBullets();
    }

    /// <summary>
    /// プレイヤーとエネミーの位置から方向を判定する準備
    /// </summary>
    /// <param name="enemyPos"></param>
    /// <returns></returns>
    public Vector3 PreparateGetPlayerDirection(Vector3 enemyPos)
    {
        return gamemanager.GetPlayerDirection(enemyPos);
    }

    /// <summary>
    /// enemiesListに登録されているエネミーの打ち、リストに残っているエネミーのゲームオブジェクトを破壊し、リストをクリアする
    /// </summary>
    public void ClearEnemiesList()
    {
        //enemiesListの要素（中身）を１つずつ順番に、要素の最大値になるまで判定していく
        for(int i = 0; i < enemiesList.Count; i++)
        {
            //要素が空でない（プレイヤーに破壊されずにゲーム画面に残っているなら）
            if(enemiesList[i] != null)
            {
                //そのエネミーのゲームオブジェクトを破壊する
                Destroy(enemiesList[i].gameObject);
            }
        }
        //リストをクリア（要素が何もない状態）にする
        enemiesList.Clear();
    }

    public void DestroyTemporaryObjectContainer()
    {
        //TemporaryObjectContainerゲームオブジェクトを破壊する
        Destroy(TransformHelper.TemporaryObjectContainerTran.gameObject);
    }
}
