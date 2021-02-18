﻿using System.Collections;
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
}