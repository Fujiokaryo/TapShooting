using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UIManager : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup canvasGroupGameClear;

    [SerializeField]
    private CanvasGroup canvasGroupGameOver;

    [SerializeField]
    private Text txtGameOver;

    [SerializeField]
    private Text txtDurability;

    [SerializeField]
    private Slider slider;

    [SerializeField]
    private Text txtTotalExp;

    /// <summary>
    /// ゲームクリア表示を見えなくする
    /// </summary>
    public void HideGameClearSet()
    {
        //GameClearSetの透明度を0にしてみえなくする
        canvasGroupGameClear.alpha = 0;    
    }

    /// <summary>
    /// ゲームクリア表示を行う
    /// </summary>
    public void DisplayGameClaerSet()
    {
        //GameClaerSetの透明度を徐々に１にしてゲームクリア表示
        canvasGroupGameClear.DOFade(1.0f, 0.25f);
    }

    /// <summary>
    /// ゲームオーバー表示を隠す
    /// </summary>
    public void HideGameOverSet()
    {
        canvasGroupGameOver.alpha = 0;
    }

    /// <summary>
    /// ゲームオーバー表示を行う
    /// </summary>
    public void DisplayGameOverSet()
    {
        //ゲームオーバー画面を徐々に表示
        canvasGroupGameOver.DOFade(1.0f, 1.0f);

        //ゲーム画面に表示する文字列を用意して代入
        string txt = "Game Over";

        //DOTextメソッドを利用して文字列を1文字ずつ順番に同じ表示時間で表示
        txtGameOver.DOText(txt, 1.5f).SetEase(Ease.Linear);

    }
    /// <summary>
    /// 耐久度の表示更新
    /// </summary>
    /// <param name="durability"></param>
    /// <param name="maxDurability"></param>
    public void DisplayDurability(int durability, int maxDurability)
    {
        txtDurability.text = durability + " / " + maxDurability;

        slider.DOValue((float)durability / maxDurability, 0.25f);
    }

    /// <summary>
    /// TotalExpの表示更新
    /// </summary>
    /// <param name="totalExp"></param>
    public void UpdateDisplayTotalExp(int totalExp)
    {
        txtTotalExp.text = totalExp.ToString();
    }
}
