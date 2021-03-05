using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{

    public int bulletPower;

    public BulletDataSO.BulletData bulletData;

    [SerializeField]
    private Image imgBullet;

    /// <summary>
    /// バレットの制御
    /// </summary>
    /// <param name="direction"></param>
    public void ShotBullet(Vector3 direction, BulletDataSO.BulletData bulletData = null)
    {
        //引数よりバレットの情報を取得し、どのようなバレットであるのか振る舞いを決定
        this.bulletData = bulletData;

        //バレットのデータがない場合
        if(this.bulletData == null)
        {
            //ここで処理を終了する
            return;
        }

        //バレットの画像を決定
        imgBullet.sprite = this.bulletData.bulletSprite;

        //バレットの移動処理
        GetComponent<Rigidbody2D>().AddForce(direction * this.bulletData.bulletSpeed);
        //5秒後にバレットを破壊する
        Destroy(gameObject, 5f);
    }
}
