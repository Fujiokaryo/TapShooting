﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
[RequireComponent(typeof(CapsuleCollider2D))]
public class EnemyController : MonoBehaviour
{
    [Header("エネミーのデータ情報")]
    public EnemyDataSO.EnemyData enemyData;

    [SerializeField]
    private Image imgEnemy; //エネミーの画像設定用

    [SerializeField]
    private Slider slider;

    [SerializeField]
    private GameObject bulletEffect;

    private int maxHp;

    private int hp;

    private EnemyGenerator enemyGenerator;

    private UnityAction<Transform, float> moveEvent;
    /// <summary>
    /// エネミーの設定
    /// </summary>
    /// <param name="enemyData"></param>
    public void SetUpEnemy(EnemyDataSO.EnemyData enemyData)
    {
        this.enemyData = enemyData;

        if(this.enemyData.enemyType != EnemyType.Boss)
        {
            transform.localPosition = new Vector3(transform.localPosition.x + Random.Range(-650, 650), transform.localPosition.y, 0);
        }
        else
        {
            //ボスの場合、サイズを大きくする
            transform.localScale = Vector3.one * 2.0f;

            //HPゲージの位置を高い位置にする
            slider.transform.localPosition = new Vector3(0, 150, 0);

        }
        //画像をEnemyDataの画像にする
        imgEnemy.sprite = this.enemyData.enemySprite;

        maxHp = this.enemyData.hp;

        hp = maxHp;

        DisplayHpGauge();

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {

            DestroyBullet(collision);
            

            if (collision.gameObject.TryGetComponent(out Bullet bullet))
            {
                UpdateHP(bullet);

                GenerateBulletEffect(collision.gameObject.transform);
            }

        }

    }

    /// <summary>
    /// バレットの破壊処理
    /// </summary>
    /// <param name="col"></param>
    private void DestroyBullet(Collider2D col)
    {
        Destroy(col.gameObject);      
    }

    /// <summary>
    /// Hpの更新処理とエネミーーの破壊確認処理
    /// </summary>
    /// <param name="bullet"></param>
    private void UpdateHP(Bullet bullet)
    {
        //HPを15減らす
        hp -= bullet.bulletPower;

        //HPの上下限の設定
        maxHp = Mathf.Clamp(maxHp, 0, maxHp);

        DisplayHpGauge();

        //hpが0以下になったら
        if (hp <= 0)
        {
            hp = 0;

            //ボスの場合
            if(enemyData.enemyType == EnemyType.Boss)
            {
                //ボス討伐済みのフラグをたてる
                enemyGenerator.SwitchBossDestroyed(true);
            }

            //ExpをTotaExpに加算
            GameData.instance.UpDateTotalExp(enemyData.exp);

            //最新のTotalExpを利用して表示更新
            enemyGenerator.PreparateDisplayTotalExp(enemyData.exp);

            Destroy(gameObject);
        }
    }

    private void DisplayHpGauge()
    {
        slider.DOValue((float)hp / maxHp, 0.25f);
    }

    private void GenerateBulletEffect(Transform bulletTran)
    {
        GameObject effect = Instantiate(bulletEffect, bulletTran, false);

        effect.transform.SetParent(transform);

        Destroy(effect, 3f);
    }

    public void AdditionalSetUPEnemy(EnemyGenerator enemyGenerator)
    {
        //引数で届いた情報を変数に代入してスクリプト内で利用可能にする
        this.enemyGenerator = enemyGenerator;

        moveEvent = this.enemyGenerator.moveEventSO.GetMoveEvent(enemyData.moveType);

        moveEvent.Invoke(transform, enemyData.moveDuration);

    }
    
}
