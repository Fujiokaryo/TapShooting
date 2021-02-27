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

        durability = GameData.instance.GetDurability();

        maxDurability = durability;

        gameManager.uiManager.DisplayDurability(durability, maxDurability);

        // TODO ゲージの表示を耐久力の値に合わせて更新
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            //ダメージの設定用
            int damage = 0;

            //侵入してきたコライダをオフにする（重複判定の防止）
            collision.GetComponent<CapsuleCollider2D>().enabled = false;
            
            if(collision.gameObject.TryGetComponent(out Bullet bullet))
            {
                damage = bullet.bulletPower;
            }
            else if(collision.gameObject.TryGetComponent(out EnemyController enemy))
            {
                damage = enemy.enemyData.power;
            }
            UpdateDurability(damage);

            GenerateEnemyAttackEffect(collision.gameObject.transform);

            Destroy(collision.gameObject);
        }
    }

    void UpdateDurability(int damage)
    {
        durability -= damage;

        Debug.Log("エネミーからのダメージ :" + damage);

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
