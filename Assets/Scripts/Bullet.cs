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

    private bool isTarget;

    private Vector3 nearPos;

    /// <summary>
    /// バレットの制御
    /// EnemyController
    /// PlayerController
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="bulletData"></param>
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

        //追尾玉のみ処理する
        if(bulletData.bulletType == BulletDataSO.BulletType.Player_Blaze)
        {
            //発射時にゲーム内にいるエネミーの情報を取得
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            //エネミーが１体以上いるなら
            if(enemies.Length > 0)
            {
                //一番近いエネミーとして、配列の最初のエネミーの位置情報を仮に登録
                nearPos = enemies[0].transform.position;

                //すべてのエネミーの位置を一つずつ順番に確認
                for(int i = 0; i < enemies.Length; i++)
                {
                    //現在の順番のエネミーの位置の情報を代入
                    Vector3 pos = enemies[i].transform.position;

                    //現在のエネミーの位置とnearPosの比較をして、画面下（プレイヤー）に近いものを
                    if(nearPos.x > pos.x && nearPos.y > pos.y)
                    {
                        //nearPosとして更新する
                        nearPos = pos;
                    }
                }

                //追尾するターゲットありとし、Updateメソッドを動くようにする
                isTarget = true;
            }
        }

        //追尾弾以外
        if(bulletData.bulletType != BulletDataSO.BulletType.Player_Blaze)
        {
            //バレットのゲームオブジェクトにアタッチされているRigidbody2Dを取得してAddForceメソッドを実行する
            GetComponent<Rigidbody2D>().AddForce(direction * this.bulletData.bulletSpeed);
        }

        //バレットの移動処理
        //GetComponent<Rigidbody2D>().AddForce(direction * this.bulletData.bulletSpeed);

        //5秒後にバレットを破壊する
        Destroy(gameObject, 5f);
    }

    private void Update()
    {
        //追尾弾の対象がない場合には処理を行わない
        if(!isTarget)
        {
            return;
        }

        //追尾用（タップ位置に関係なく、プレイヤーに近いエネミーに発射する）
        //1フレーム毎に座標を更新
        Vector3 currentPos = transform.position;

        //バレットの移動
        transform.position = Vector3.MoveTowards(currentPos, nearPos, Time.deltaTime / 10 * bulletData.bulletSpeed);
    }
}
