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

    public BulletDataSO.BulletData bulletData;

    public void SetUpBulletSelectDetail(BulletSelectManager bulletSelectManager, BulletDataSO.BulletData bulletData)
    {
        this.bulletSelectManager = bulletSelectManager;

        this.bulletData = bulletData;

        //画像変更
        imgBulletBtn.sprite = this.bulletData.btnSprite;

        //ボタンにメソッド登録
        btnBulletSelect.onClick.AddListener(OnClickBulletSelect);
    }

    public void OnClickBulletSelect()
    {
        //このバレット選択ボタンにお設定されているBulletDataをGameDataスクリプトのcullentBulletData変数を登録し、現在使用中のバレットとする
        GameData.instance.SetBulletData(bulletData);
        Debug.Log("バレット選択");
    }
}
