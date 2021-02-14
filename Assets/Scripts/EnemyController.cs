using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class EnemyController : MonoBehaviour
{

    public int enemyHp = 100;
    public int attackPower;
    public float enemySpeed;
    void Start()
    {
        
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

            if(collision.gameObject.TryGetComponent(out Bullet bullet))
            {
                DestroyBullet(collision);
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
        enemyHp -= bullet.bulletPower;

        //hpが0以下になったら
        if (enemyHp <= 0)
        {
            enemyHp = 0;
            Destroy(gameObject);
        }
    }

}
