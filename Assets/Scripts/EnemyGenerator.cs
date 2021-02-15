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

    public void SetUpEnemyGenerator(Gamemanager gamemanager)
    {
        this.gamemanager = gamemanager;
    }
    void Update()
    {
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

        }
    }

    private void GenerateEnemy()
    {
       GameObject enemySetObj = Instantiate(enemyObjPrefab, transform, false);

       EnemyController enemyController = enemySetObj.GetComponent<EnemyController>();

       enemyController.SetUpEnemy();
    }

    
}
