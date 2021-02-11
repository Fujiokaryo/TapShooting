using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class EnemyController : MonoBehaviour
{
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

            DestroyObj(collision);
       
        }

    }

    /// <summary>
    /// バレットとエネミーの破壊処理
    /// </summary>
    /// <param name="col"></param>
    private void DestroyObj(Collider2D col)
    {
        Debug.Log("当たったオブジェクト" + col.gameObject.tag);

        Destroy(col.gameObject);
        Destroy(gameObject);
    }
}
