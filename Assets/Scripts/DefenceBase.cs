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

    [SerializeField]
    private FloatingMessage floatingMessagePrefab;

    [SerializeField]
    private Transform floatingDamageTran;

    private Gamemanager gameManager;

    private int maxDurability;

    public void SetUpDefenceBase(Gamemanager gameManager)
    {
        this.gameManager = gameManager;

        //ゲームデータより耐久力を取得
        durability = GameData.instance.GetDurability();

        //ゲーム開始時点の耐久力の値を最大値として代入
        maxDurability = durability;

        //耐久力の表示更新
        gameManager.uiManager.DisplayDurability(durability, maxDurability);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            //ダメージの設定用
            int damage = 0;

            //侵入してきたコライダをオフにする（重複判定の防止）
            collision.GetComponent<CapsuleCollider2D>().enabled = false;
            
            //ダメージ値の取得
            if(collision.gameObject.TryGetComponent(out Bullet bullet))
            {
                damage = bullet.bulletPower;
            }
            else if(collision.gameObject.TryGetComponent(out EnemyController enemy))
            {
                damage = enemy.enemyData.power;
            }

            //耐久力の更新とゲームオーバーの確認
            UpdateDurability(damage);

            //エネミーからのダメージ値用のフロート表示の生成
            CreateFloatingMessageToDamage(damage);

            //エネミーの攻撃演出用のエフェクト生成
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

   private void CreateFloatingMessageToDamage(int damage)
    {
        //フロート表示の生成。生成位置はDefenceBaseオブジェクトないのtxtDutabilityオブジェクトの位置
        FloatingMessage floatingMessage = Instantiate(floatingMessagePrefab, floatingDamageTran, false);

        //生成したフロート表示のせっいぇい」用のメソッドを実行。引数として、エネミーからのダメージ値とフロート表示の種類を指定して渡す
        floatingMessage.DisplayFloatingMessage(damage, FloatingMessage.FloatingMessageType.PlayerDamage);
    }
}
