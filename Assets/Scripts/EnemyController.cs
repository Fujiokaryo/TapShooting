using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(CapsuleCollider2D))]
public class EnemyController : MonoBehaviour
{

    public int hp;
    public int attackPower;
    public float enemySpeed;

    [SerializeField]
    private Slider slider;

    [SerializeField]
    private GameObject bulletEffect;

    private int maxHp;
    
    public void SetUpEnemy()
    {
        transform.localPosition = new Vector3(transform.localPosition.x + Random.Range(-650, 650), transform.localPosition.y, 0);

        maxHp = hp;

        DisplayHpGauge();
    }

    // Update is called once per frame
    void Update()
    {
       transform.Translate(0, enemySpeed, 0);

        if(gameObject.transform.position.y < -1500)
        {
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {

            DestroyBullet(collision);
            GenerateBulletEffect(collision.gameObject.transform);

            if (collision.gameObject.TryGetComponent(out Bullet bullet))
            {
                UpdateHP(bullet);
            }

        }

    }

    /// <summary>
    /// バレットの破壊処理
    /// </summary>
    /// <param name="col"></param>
    private void DestroyBullet(Collider2D col)
    {
        Debug.Log("当たったオブジェクト" + col.gameObject.tag);

        Destroy(col.gameObject);      
    }


    private void UpdateHP(Bullet bullet)
    {
        //HPを15減らす
        hp -= bullet.bulletPower;

        maxHp = Mathf.Clamp(maxHp, 0, maxHp);

        DisplayHpGauge();

        //hpが0以下になったら
        if (hp <= 0)
        {
            hp = 0;
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

}
