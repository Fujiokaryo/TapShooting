using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{

    
    public float bulletSpeed;
    
   
    public void ShotBullet(Vector3 direction)
    {
        //バレットの移動処理
        GetComponent<Rigidbody2D>().AddForce(direction * bulletSpeed);

        //5秒後にバレットを破壊する
        Destroy(gameObject, 5f);
    }
}
