using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyObjPrefab;

    [SerializeField]
    public float preparateTime;

    private int generateCount;

    private float timer;

    private Gamemanager gamemanager;

    public int maxGenerate;

    public bool isGenerateEnd;

    public bool isBossDestroyed;

    public void SetUpEnemyGenerator(Gamemanager gamemanager)
    {
        this.gamemanager = gamemanager;
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

    private void GenerateEnemy(bool isBoss = false)
    {
       GameObject enemySetObj = Instantiate(enemyObjPrefab, transform, false);

       EnemyController enemyController = enemySetObj.GetComponent<EnemyController>();

       enemyController.SetUpEnemy(isBoss);

        if(isBoss)
        {
            enemyController.AdditionalSetUPEnemy(this);
        }
    }

    private IEnumerator GenerateBoss()
    {
        // TODO ボス出現の警告演出

        yield return new WaitForSeconds(1.0f);

        // TODO ボス生成
        GenerateEnemy(true);


    }

    public void SwitchBossDestroyed(bool switchBossDestroyed)
    {
        isBossDestroyed = switchBossDestroyed;

        gamemanager.SwitchGameUp(isBossDestroyed);

        gamemanager.PreparateGameClear();
    }
}
