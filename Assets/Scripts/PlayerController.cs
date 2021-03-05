using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private GameObject bulletPrefab;

    private Gamemanager gamemanager;

    void Start()
    {
        
    }

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
            GenerateBullet(direction);        
        }
    }

    /// <summary>
    /// バレットの生成
    /// </summary>
    /// <param name="direction"></param>
    private void GenerateBullet(Vector3 direction)
    {
        //bulletPrefabのクローンを生成、生成位置はPlayerSetオブジェクトの子オブジェクトを指定
        GameObject bulletObj =  Instantiate(bulletPrefab, transform);

        //
        bulletObj.GetComponent<Bullet>().ShotBullet(direction, GameData.instance.GetCullentBulletData());
    }

    public void SetUpPlayer(Gamemanager gamemanager)
    {
        this.gamemanager = gamemanager;
    }
}
