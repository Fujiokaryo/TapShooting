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

    [SerializeField]
    private BulletDataSO bulletDataSO;

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
            bulletSelectDetail.SetUpBulletSelectDetail(this, bulletDataSO.bulletDataList[i]);

            //リストに追加
            bulletSelectDetailList.Add(bulletSelectDetail);

            //0.25秒だけ処理を中断（順番にボタンが生成されるように演出）
            yield return new WaitForSeconds(0.25f);
        }

        // TODO 使用するバレットの情報を初期設定。後ほど、引数を変更する
        GameData.instance.SetBulletData(bulletDataSO.bulletDataList[0]);
    }

    public void ChangeLoadingBulletSettings(int bulletNo)
    {
        //バレット選択ボタンのListの要素をすべて取り出し、１つずつ順番に処理する
        for(int i = 0; i < bulletSelectDetailList.Count; i++)
        {
            //この要素のバレット選択ボタンのNoの値がbulletNo（選択されたバレット）と同じであるなら
            if(bulletSelectDetailList[i].bulletData.no == bulletNo)
            {
                //装填中状態にする
                bulletSelectDetailList[i].ChangeLoadingBullet(true);

                Debug.Log("装填中のバレットのNo" + bulletNo);
            }
            else
            {
                //未装填状態にする
                bulletSelectDetailList[i].ChangeLoadingBullet(false);

                //
                Debug.Log("未装填のバレットのNo" + bulletNo);
            }
        }
    }

    public void ActivateDefaultBullet()
    {
        //バレット選択ボタンのListから要素を一つずつ取り出す
        foreach(BulletSelectDetail bulletSelectDetail in bulletSelectDetailList)
        {
            //初期バレットの設定があるバレットデータの場合
            if(bulletSelectDetail.isDefaultBullet)
            {
                //そのバレットを装填中の状態にする
                bulletSelectDetail.OnClickBulletSelect();

                Debug.Log("初期バレットを装填中のバレットとして設定");

                return;
            }
        }
    }
}
