using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BulletSelectDetail : MonoBehaviour
{
    [SerializeField]
    private Button btnBulletSelect;

    private BulletSelectManager bulletSelectManager;

    [SerializeField]
    private Image imgBulletBtn;

    [SerializeField]
    private Image imgLaunchTimeGauge; //バレットを発射できる時間のゲージ

    [SerializeField]
    private Text txtOpenExpValue;

    public BulletDataSO.BulletData bulletData;

    private float launchTime; //バレットを発射できる残り時間

    private float initialLaunchTime; //バレットを発射できる時間の初期値

    private bool isLoading; //選択したバレットを発射できる状態かどうか。trueなら装填中で発射でき、launchTimeも減る。

    public bool isDefaultBullet; //初期バレットかどうか。初期バレットはバレットの発射時間の残り時間に関係なく発射できる

    private bool isCostPayment; //現在コストを支払って開放済かどうか。　trueならコスト支払い完了。

    public void SetUpBulletSelectDetail(BulletSelectManager bulletSelectManager, BulletDataSO.BulletData bulletData)
    {
        this.bulletSelectManager = bulletSelectManager;

        this.bulletData = bulletData;

        //画像変更
        imgBulletBtn.sprite = this.bulletData.btnSprite;

        //ボタンにメソッド登録
        btnBulletSelect.onClick.AddListener(OnClickBulletSelect);

        //バレットを選択できない状態にきりかえる
        SwitchAcriveBulletBtn(false);

        //バレットを発射できる時間の設定をし、初期値とする
        initialLaunchTime = this.bulletData.launchTime;

        //バレットの発射できる時間を初期値から設定する
        launchTime = initialLaunchTime;

        //バレットの発射できる残り時間のゲージ表示を０にして非表示にする。
        imgLaunchTimeGauge.fillAmount = 0;

        //バレット選択ボタンを推せるようにするために必要なExpを表示
        txtOpenExpValue.text = this.bulletData.openExp.ToString();

        //初期バレット確認
        if(this.bulletData.openExp == 0)
        {
            //初期バレット用の設定
            isDefaultBullet = true;

            //装填中の状態に切り替える
            ChangeLoadingBullet(true);

            TurnActiveOpenExpValue(false);

            SwitchAcriveBulletBtn(true);

            // TODO そのほかに設定する処理を追加

        }

    }

    public void OnClickBulletSelect()
    {
        //このバレット選択ボタンにお設定されているBulletDataをGameDataスクリプトのcullentBulletData変数を登録し、現在使用中のバレットとする
        GameData.instance.SetBulletData(bulletData);

        // TODO 装填中のバレットを切り替え。後ほど処理を変更
        bulletSelectManager.ChangeLoadingBulletSettings(bulletData.no);

        //重複タップ防止
        if (!isDefaultBullet && imgLaunchTimeGauge.fillAmount == 0)
        {
            //バレットの発射できる残り時間のゲージを最大値にして表示する
            imgLaunchTimeGauge.fillAmount = 1.0f;

            //コストExpを非表示
            TurnActiveOpenExpValue(false);

            //選択したバレットのコストの支払いとそれに関連する処理（コストのマイナス補正はメソッド側で行う
            bulletSelectManager.SelectedBulletCostPayment(bulletData.openExp);

            //コスト支払い済状態にする = 開放条件にてEXPが足りなくても推せない状態にならないようにする
            txtOpenExpValue.gameObject.SetActive(false);
            
        }

        Debug.Log("バレット選択");
    }

    /// <summary>
    /// 装填中のバレットを切り替え
    /// </summary>
    /// <param name="isSwitch"></param>
    public void ChangeLoadingBullet(bool isSwitch)
    {
        isLoading = isSwitch;
    }

    private void Update()
    {
        //初期バレットは発射できる時間制限なし
        if(isDefaultBullet)
        {
            return;
        }

        //装填しているバレットでなければ何もしない
        if(!isLoading)
        {
            return;
        }

        //発射できる残り時間を減らす
        launchTime -= Time.deltaTime;

        //残り時間とゲージを合わせる = 残り時間が無くなったらゲージも見えなくなる
        imgLaunchTimeGauge.DOFillAmount(launchTime / initialLaunchTime, 0.25f);

        //発射できる残り時間が無くなったら
        if(launchTime <= 0)
        {
            launchTime = 0;

            //初期バレット以外のバレットを初期状態に戻す
            initializeBulletState();
        }
    }

    private void initializeBulletState()
    {
        //装填中の状態を未装填の状態にする
        isLoading = false;

        //初期バレットを装填中のバレットとして設定 => このバレットの発射できる時間が終了したため
        bulletSelectManager.ActivateDefaultBullet();

        //発射時間の初期化
        launchTime = initialLaunchTime;

        //開放に必要なExpを表示
        TurnActiveOpenExpValue(true);

        //コスト未払いの状態に戻す
        SetStateBulletCostPayment(false);

        //選択可能なバレットの確認
        bulletSelectManager.JugdeOpenBullets();
        
    }

    private void TurnActiveOpenExpValue(bool isTurnActive)
    {
        txtOpenExpValue.gameObject.SetActive(isTurnActive);
    }

    public void SwitchAcriveBulletBtn(bool isSwitch)
    {
        btnBulletSelect.interactable = isSwitch;
    }

    /// <summary>
    /// コスト支払い状態の確認
    /// </summary>
    /// <returns></returns>
    public bool GetStateBulletCostPayment()
    {
        return isCostPayment;
    }

    /// <summary>
    /// コスト支払い状態の更新
    /// </summary>
    /// <param name="isSet"></param>
    public void SetStateBulletCostPayment(bool isSet)
    {
        isCostPayment = isSet;
    }
}
