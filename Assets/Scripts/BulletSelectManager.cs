using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSelectManager : MonoBehaviour
{
    [SerializeField]
    private BulletSelectDetail bulletSelectDetailPrefab;

    [SerializeField]
    private Transform bulletTran;

    private const int maxBulletBtnNum = 4;

    public List<BulletSelectDetail> bulletSelectDetailList = new List<BulletSelectDetail>();

    private void Start()
    {
        //バレット選択ボタンの生成
        StartCoroutine(GenerateSelectDetail());
    }

    public IEnumerator GenerateSelectDetail()
    {
        for(int i = 0; i < maxBulletBtnNum; i++)
        {
            //バレットボタン生成
            BulletSelectDetail bulletSelectDetail = Instantiate(bulletSelectDetailPrefab, bulletTran, false);

            //バレットボタンの設定
            bulletSelectDetail.SetUpBulletSelectDetail(this);

            //リストに追加
            bulletSelectDetailList.Add(bulletSelectDetail);

            //0.25秒だけ処理を中断（順番にボタンが生成されるように演出）
            yield return new WaitForSeconds(0.25f);
        }
    }
}
