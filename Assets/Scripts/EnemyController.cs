using System.Collections;
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

    [SerializeField]
    private GameObject enemyBulletPrefab;

    [SerializeField]
    private Transform floatingDamageTran; //フロート表示を行う位置表示

    [SerializeField]
    private FloatingMessage floatingMessagePrefab;

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

            //DestroyBullet(collision);
            

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
        //バレットの攻撃力値用のフロート表示の生成
        CreateFloatingMessageToBulletPower(bullet.bulletData.bulletPower);

        //HPを15減らす
        hp -= bullet.bulletData.bulletPower;

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

        //
        if(bullet.bulletData.bulletType == BulletDataSO.BulletType.Player_Normal || bullet.bulletData.bulletType == BulletDataSO.BulletType.Player_5ways_Normal)
        {
            Destroy(bullet.gameObject);
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

        //MoveEventSOスクリプタブルオブジェクトのGetMoveEventメソッドを実行し、戻り値で移動用のメソッドを受け取る
        moveEvent = this.enemyGenerator.moveEventSO.GetMoveEvent(enemyData.moveType);

        moveEvent.Invoke(transform, enemyData.moveDuration);

        Debug.Log("追加設定完了");

        if(enemyData.moveType == MoveType.Straight || enemyData.moveType == MoveType.Boss_Horizontal)
        {
            StartCoroutine(EnemyShot());
        }

    }

    private IEnumerator EnemyShot()
    {
        while(true)
        {
            //エネミーのバレットのクローンを生成し、戻り値をbulletObjに代入
           GameObject bulletObj = Instantiate(enemyBulletPrefab, transform);

            //クローンのゲームオブジェクトからBulletスクリプトを取得してShotBulletメソッドを実行する
            bulletObj.GetComponent<Bullet>().ShotBullet(enemyGenerator.PreparateGetPlayerDirection(transform.position));

            if(enemyData.moveType == MoveType.Boss_Horizontal)
            {
                //バレットとエネミーの親子関係を解消し、バレットの親をTemporaryObjectContainerオブジェクトに変更する
                bulletObj.transform.SetParent(TransformHelper.GetTemporaryObjectContainerTran());
            }

            yield return new WaitForSeconds(5f);
        }

    }
    /// <summary>
    /// バレットの攻撃力値分のフロート表示の生成
    /// </summary>
    /// <param name="bullletPower"></param>
    private void CreateFloatingMessageToBulletPower(int bullletPower)
    {
        //フロート表示の生成。生成位置はEnemySetオブジェクト内のFloatingMessageTranオブジェクトの位置
        FloatingMessage floatingMessage = Instantiate(floatingMessagePrefab, floatingDamageTran, false);

        //生成下フロート表示の設定用メソッドを実行。引数としてバレットの攻撃値とフロート表示の種類を指定して渡す
        floatingMessage.DisplayFloatingMessage(bullletPower, FloatingMessage.FloatingMessageType.EnemyDamage);
    }


}
