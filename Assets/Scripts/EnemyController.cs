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
            //エネミーのX軸の位置を、ゲーム画面に収まる葉にｄふぇランダムな位置に変更
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
            
            if (collision.gameObject.TryGetComponent(out Bullet bullet))
            {
                UpdateHP(bullet);

                SoundManager.instance.PlaySE(SoundDataSO.SeType.BulletDamage_1);

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
        //ダメージ確定用
        int bulletPower = 0;

        //弱点判定用。弱点ｎの場合には、trueに切り替える
        bool isWeekness = false;

        //バレットの属性とエネミーの属性の情報を利用して相性を判定
        if(ElementCompatibilityHelper.GetElementCompatibility(bullet.bulletData.elementType, enemyData.elementType) == true)
        {
            Debug.Log("Element　相性　良");

            //バレットの攻撃力をダメージ倍率分掛けた値にして、計算結果を切り上げてint型にする
            bulletPower = Mathf.FloorToInt(bullet.bulletData.bulletPower * GameData.instance.DamageRatio);

            isWeekness = true;
        }
        else
        {
            //バレットの攻撃力をそのまま利用する
            bulletPower = bullet.bulletData.bulletPower;
        }



        //バレットの攻撃力値用のフロート表示の生成
        CreateFloatingMessageToBulletPower(bulletPower, isWeekness);

        //hpの減算処理
        hp -= bulletPower;

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

                SoundManager.instance.PlaySE(SoundDataSO.SeType.GameClear);
                SoundManager.instance.PlayVoice(SoundDataSO.VoiceType.Win);
            }

            //ExpをTotaExpに加算
            GameData.instance.UpDateTotalExp(enemyData.exp);

            //最新のTotalExpを利用して表示更新
            enemyGenerator.PreparateDisplayTotalExp(enemyData.exp);


            Destroy(gameObject);
        }

        //ノーマル弾の場合
        if(bullet.bulletData.bulletType == BulletDataSO.BulletType.Player_Normal || bullet.bulletData.bulletType == BulletDataSO.BulletType.Player_5ways_Normal)
        {
            Destroy(bullet.gameObject);
        }
    }

    /// <summary>
    ///Hpゲージの表示更新 
    /// </summary>
    private void DisplayHpGauge()
    {
        slider.DOValue((float)hp / maxHp, 0.25f);
    }

    /// <summary>
    /// 被弾時のヒット演出用のエフェクト作成
    /// </summary>
    /// <param name="bulletTran"></param>
    private void GenerateBulletEffect(Transform bulletTran)
    {
        //ヒット演出用のエフェクトをバレットのぶつかった位置で作成
        GameObject effect = Instantiate(bulletEffect, bulletTran, false);

        //エフェクトをエネミーの子オブジェクトにする
        effect.transform.SetParent(transform);

        Destroy(effect, 3f);
    }

    /// <summary>
    /// エネミーの追加設定
    /// </summary>
    /// <param name="enemyGenerator"></param>
    /// <param name="bulletData"></param>
    public void AdditionalSetUPEnemy(EnemyGenerator enemyGenerator, BulletDataSO.BulletData bulletData)
    {
        //引数で届いた情報を変数に代入してスクリプト内で利用可能にする
        this.enemyGenerator = enemyGenerator;

        //MoveEventSOスクリプタブルオブジェクトのGetMoveEventメソッドを実行し、戻り値で移動用のメソッドを受け取る
        moveEvent = this.enemyGenerator.moveEventSO.GetMoveEvent(enemyData.moveType);

        moveEvent.Invoke(transform, enemyData.moveDuration);


        if(bulletData != null && bulletData.bulletType != BulletDataSO.BulletType.None)
        {
            StartCoroutine(EnemyShot(bulletData));
        }

    }

    /// <summary>
    /// バレットの自動生成
    /// </summary>
    /// <param name="bulletData"></param>
    /// <returns></returns>
    private IEnumerator EnemyShot(BulletDataSO.BulletData bulletData)
    {
        while(true)
        {
           //エネミーのバレットのクローンを生成し、戻り値をbulletObjに代入
           GameObject bulletObj = Instantiate(enemyBulletPrefab, transform);

           //クローンのゲームオブジェクトからBulletスクリプトを取得してShotBulletメソッドを実行する
           bulletObj.GetComponent<Bullet>().ShotBullet(enemyGenerator.PreparateGetPlayerDirection(transform.position), bulletData);


            if(enemyData.moveType == MoveType.Boss_Horizontal)
            {
                //バレットとエネミーの親子関係を解消し、バレットの親をTemporaryObjectContainerオブジェクトに変更する
                bulletObj.transform.SetParent(TransformHelper.GetTemporaryObjectContainerTran());
            }

            yield return new WaitForSeconds(bulletData.loadingTime);
        }

    }
    /// <summary>
    /// バレットの攻撃力値分のフロート表示の生成
    /// </summary>
    /// <param name="bullletPower"></param>
    private void CreateFloatingMessageToBulletPower(int bullletPower, bool isWeekness)
    {
        //フロート表示の生成。生成位置はEnemySetオブジェクト内のFloatingMessageTranオブジェクトの位置
        FloatingMessage floatingMessage = Instantiate(floatingMessagePrefab, floatingDamageTran, false);

        //生成下フロート表示の設定用メソッドを実行。引数としてバレットの攻撃値とフロート表示の種類を指定して渡す
        floatingMessage.DisplayFloatingMessage(bullletPower, FloatingMessage.FloatingMessageType.EnemyDamage, isWeekness);
    }


}
