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
    private FloatingMessage floatingMessagePrefab;

    [SerializeField]
    private Text txtGameOver;

    [SerializeField]
    private Text txtDurability;

    [SerializeField]
    private Slider slider;

    [SerializeField]
    private Text txtTotalExp;

    [SerializeField]
    private Image imgGameClear;

    [SerializeField]
    private CanvasGroup canvasGroupRestartImage;

    [SerializeField]
    private CanvasGroup canvasGroupOpeningFilter;

    [SerializeField]
    private Image imgGameStart;

    [SerializeField]
    private CanvasGroup canvasGroupBossAlert;

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
        canvasGroupGameClear.DOFade(1.0f, 0.25f)
            .OnComplete(() =>
            {

                //ゲームクリアの画像をDOPunchScaleメソッドで伸縮させる
                imgGameClear.transform.DOPunchScale(Vector3.one * 2.5f, 0.5f)

                //DOPunchScaleメソッドの処理が終了したら
                .OnComplete(() =>
                {
                    //ゲームクリアの画像をDOShakeScaleメソッドで揺らす
                    imgGameClear.transform.DOShakeScale(0.5f);

                    //ゲームクリアの画像のサイズを1.5倍にする
                    imgGameClear.transform.localScale = Vector3.one * 1.5f;

                    //画像タップを許可
                    canvasGroupGameClear.blocksRaycasts = true;

                    //Restartの点滅表示
                    canvasGroupRestartImage.DOFade(1.0f, 1.5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
                });

            });
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

    public void CreateFloatingMessageToExp(int exp, FloatingMessage.FloatingMessageType floatingMessageType)
    {
        //フロート表示の生成。　生成位置はtxtTotalExpの位置
        FloatingMessage floatingMessage = Instantiate(floatingMessagePrefab, txtTotalExp.transform, false);

        //生成したフロート表示の設定用メソッドを実行。引数としてExp値とフロート表示の種類を指定して渡す
        floatingMessage.DisplayFloatingMessage(exp, floatingMessageType);
    }

    public IEnumerator PlayOpening()
    {
        //黒いフィルター画像を序所に非表示にして、ゲーム画面を見えるようにする
        canvasGroupOpeningFilter.DOFade(0.0f, 1.0f)

            //上記の処理が終了したら
            .OnComplete(() =>
            {
                //ゲームスタートのロゴ画像を跳ねさせながら、画面外の左端からゲーム画面の中央へ移動
                imgGameStart.transform.DOLocalJump(Vector3.zero, 300f, 3, 1.5f).SetEase(Ease.Linear);
            });

        //一時処理を中断（ゲームスタートのロゴが画面の中央で停止したままになる）
        yield return new WaitForSeconds(3.5f);

        //ゲームスタートのロゴ画像を跳ねさせながら、ゲーム画面の中央から画面の右端へ移動して画面外に
        imgGameStart.transform.DOLocalJump(new Vector3(1500, 0, 0), 200f, 3, 1.5f).SetEase(Ease.Linear);
    }

    public void HideBossAlertSet()
    {
        canvasGroupBossAlert.transform.parent.gameObject.SetActive(false);
    }

    public IEnumerator PlayBossAlert()
    {
        canvasGroupBossAlert.transform.parent.gameObject.SetActive(true);
        canvasGroupBossAlert.DOFade(1.0f, 0.5f).SetLoops(6, LoopType.Yoyo);
        yield return new WaitForSeconds(3f);
        canvasGroupBossAlert.DOFade(0f, 0.25f);
        yield return new WaitForSeconds(0.25f);
        canvasGroupBossAlert.transform.parent.gameObject.SetActive(false);
    }
}
