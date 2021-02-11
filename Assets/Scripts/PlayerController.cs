using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private GameObject bulletPrefab;

    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 tapPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = tapPos - transform.position;

            direction = Vector3.Scale(direction, new Vector3(1, 1, 0));
            direction = direction.normalized;
           

            GenerateBullet(direction);        
        }
    }

    private void GenerateBullet(Vector3 direction)
    {
        GameObject bulletObj =  Instantiate(bulletPrefab, transform);
        bulletObj.GetComponent<Bullet>().ShotBullet(direction);
    }
}
