using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D))]

public class DefenceBase : MonoBehaviour
{
    public int durability;
 

    [SerializeField]
    private GameObject enemyAttackEffect;

    private Gamemanager gameManager;

    private int maxDurability;

    public void SetUpDefenceBase(Gamemanager gameManager)
    {
        this.gameManager = gameManager;

        maxDurability = durability;

        gameManager.uiManager.DisplayDurability(durability, maxDurability);

        // TODO ゲージの表示を耐久力の値に合わせて更新
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            GenerateEnemyAttackEffect(collision.gameObject.transform);

            if(collision.gameObject.TryGetComponent(out EnemyController enemy))
            {
                UpdateDurability(enemy);
            }

            
            Destroy(collision.gameObject);
        }
    }

    void UpdateDurability(EnemyController enemy)
    {
        durability -= enemy.attackPower;

        durability = Mathf.Clamp(durability, 0, maxDurability);

        gameManager.uiManager.DisplayDurability(durability, maxDurability);

        // TODO 耐久力が0以下になってないか確認
        if(durability <= 0 && gameManager.isGameUp == false)
        {
            // TODO 耐久力が0以下なら、ゲームオーバー判定を行う
            gameManager.SwitchGameUp(true);

            gameManager.PreparateGameOver();
        }

    }

  

    void GenerateEnemyAttackEffect(Transform enemyTransform)
    {
        GameObject damageEffect = Instantiate(enemyAttackEffect, enemyTransform, false);

        damageEffect.transform.SetParent(TransformHelper.TemporaryObjectContainerTran);

        Destroy(damageEffect, 1f);
    }
}
