using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FloatingMessage : MonoBehaviour
{
    [SerializeField]
    private Text txtFloatingMesssage;

    /// <summary>
    /// フロート表示の種類
    /// </summary>
    public enum FloatingMessageType
    {
        EnemyDamage, //エネミー被ダメージ
        PlayerDamage, //プレイヤー被ダメージ
        GetExp, //Exp獲得
        BulletCost, //バレットコスト支払い
    }

    /// <summary>
    /// フロート表示の制御
    /// </summary>
    /// <param name="floatingValue"></param>
    /// <param name="floatingMessageType"></param>
    public void DisplayFloatingMessage(int floatingValue, FloatingMessageType floatingMessageType, bool isWeekness = false)
    {
        //フロート表示の位置を毎回同じ位置にしないようにランダム要素を加える
        transform.localPosition = new Vector3(transform.localPosition.x + Random.Range(-20, 20), transform.localPosition.y + Random.Range(-10, 10), 0);

        //フロート表示の値を代入
        txtFloatingMesssage.text = floatingValue.ToString();

        //フロート表示の数字の色を指定
        txtFloatingMesssage.color = GetMessageColor(floatingMessageType);

        //弱点属性の場合
        if(isWeekness)
        {
            //フロート表示の数字を大きくする
            transform.localScale = Vector3.one * 2.0f;
        }

        //フロート表示を上方向に移動させて、移動し終わったら破壊
        transform.DOLocalMoveY(transform.localPosition.y + 50, 1.0f).OnComplete(() => { Destroy(gameObject); });
    }

    /// <summary>
    /// フロートの表示の色を設定
    /// </summary>
    /// <param name="floatingMessageType"></param>
    /// <returns></returns>
    private Color GetMessageColor(FloatingMessageType floatingMessageType)
    {
        switch (floatingMessageType)
        {
            case FloatingMessageType.EnemyDamage:

            case FloatingMessageType.PlayerDamage:
                return Color.red;

            case FloatingMessageType.GetExp:
                return Color.yellow;

            case FloatingMessageType.BulletCost:
                return Color.blue;

            default:
                return Color.white;
        }
    }
}
