using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D))]

public class DefenceBase : MonoBehaviour
{
    public int durability;
    // Start is called before the first frame update

    [SerializeField]
    private Text txtDurability;

    [SerializeField]
    private Slider slider;

    private Gamemanager gameManager;

    private int maxDurability;

    public void SetUpDefenceBase(Gamemanager gameManager)
    {
        this.gameManager = gameManager;

        maxDurability = durability;

        DisplayDurability();

        // TODO ゲージの表示を耐久力の値に合わせて更新
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            if(collision.gameObject.TryGetComponent(out EnemyController enemy))
            {
                UpdateDurability(enemy);
            }

            
            Destroy(collision.gameObject);
        }
    }

    void UpdateDurability(EnemyController enemy)
    {
        durability -= enemy.attackPower;

        durability = Mathf.Clamp(durability, 0, maxDurability);

        DisplayDurability();

        // TODO 耐久力が0以下になってないか確認
        if(durability <= 0 && gameManager.isGameUp == false)
        {
            // TODO 耐久力が0以下なら、ゲームオーバー判定を行う
            gameManager.SwiychGameUp(true);
        }

    }

    void DisplayDurability()
    {
        txtDurability.text = durability + " / " + maxDurability;

        slider.DOValue((float)durability / maxDurability, 0.25f);
    }
}
