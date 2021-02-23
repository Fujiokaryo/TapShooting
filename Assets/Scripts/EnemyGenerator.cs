using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyObjPrefab; //エネミーのプレファブ

    [SerializeField]
    public float preparateTime; //エネミー生成までの時間

    private int generateCount; //生成下エネミーの数をカウント

    private float timer;

    private Gamemanager gamemanager;

    public int maxGenerate;

    public bool isGenerateEnd;

    public bool isBossDestroyed;

    public EnemyDataSO enemyDataSO;

    public List<EnemyDataSO.EnemyData> normalEnemyDatas = new List<EnemyDataSO.EnemyData>();

    public List<EnemyDataSO.EnemyData> bossEnemyDatas = new List<EnemyDataSO.EnemyData>();

    /// <summary>
    /// EnemyGanaratorの設定
    /// </summary>
    /// <param name="gamemanager"></param>
    public void SetUpEnemyGenerator(Gamemanager gamemanager)
    {
        this.gamemanager = gamemanager;

        normalEnemyDatas = GetEnemyTypeList(EnemyType.Normal);

        bossEnemyDatas = GetEnemyTypeList(EnemyType.Boss);
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


       GameObject enemySetObj = Instantiate(enemyObjPrefab, transform, false);

       EnemyController enemyController = enemySetObj.GetComponent<EnemyController>();

       enemyController.SetUpEnemy(enemyData);
       
       enemyController.AdditionalSetUPEnemy(this);
        
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
}
