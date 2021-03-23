using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーを制御するクラス
/// </summary>
public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private Bullet bulletPrefab;

    private Gamemanager gamemanager;



    // Update is called once per frame
    void Update()
    {
        if(gamemanager.isGameUp)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            //画面をタップした位置をカメラのスクリーン座標の情報を通じてワールド座標に変換
            Vector3 tapPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //方向を計算
            Vector3 direction = tapPos - transform.position;

            //方向の情報から、不要なZ軸情報ぞ除去を行う
            direction = Vector3.Scale(direction, new Vector3(1, 1, 0));

            //正規化処理を行い、単位ベクトルとする（方向の情報はもちつつ、距離による速度差をなくして一定値にする
            direction = direction.normalized;
           
            //バレット生成
            PreparateGenerateBullet(direction);        
        }
    }

    private void PreparateGenerateBullet(Vector3 direction)
    {
        //現在使用しているバレットの情報を取得
        BulletDataSO.BulletData bulletData = GameData.instance.GetCullentBulletData();

        //バレットの種類(BulletType)を元に決定
        switch(bulletData.bulletType)
        {
            //BulletTypeがPlayer_NormalまたはPlayer_Blazeの場合（処理が同じ場合にはこのように記述できる
            case BulletDataSO.BulletType.Player_Normal:
            case BulletDataSO.BulletType.Player_Blaze:

                GenerateBullet(direction, bulletData);
                break;

            //BulletTypeがPlayer_3ways_Piercingの場合
            case BulletDataSO.BulletType.Player_3ways_Piercing:

                for(int i = -1; i < 2; i++)
                {
                    //3方向に扇状に発射する
                    GenerateBullet(new Vector3(direction.x + (0.5f * i), direction.y, direction.z), bulletData);
                }
                break;

            //BulletTypeがPlayer_5ways_Normalの場合
            case BulletDataSO.BulletType.Player_5ways_Normal:

                for(int i = -2; i < 3; i++)
                {
                    //5方向に扇状に発射する
                    GenerateBullet(new Vector3(direction.x + (0.25f + i), direction.y, direction.z), bulletData);
                }
                break;
        }
    }

    /// <summary>
    /// バレットの生成
    /// </summary>
    /// <param name="direction">弾の方向</param>
    /// <param name="bulletData">弾の振る舞い</param>
    private void GenerateBullet(Vector3 direction, BulletDataSO.BulletData bulletData)
    {
        //Bulletスクリプトにてバレットを生成し、ShotBulletメソッドを実行する
        Instantiate(bulletPrefab, transform).ShotBullet(direction, bulletData);
        SoundManager.instance.PlayVoice(SoundDataSO.VoiceType.Attack);
 
    }

    public void SetUpPlayer(Gamemanager gamemanager)
    {
        this.gamemanager = gamemanager;
    }
}
