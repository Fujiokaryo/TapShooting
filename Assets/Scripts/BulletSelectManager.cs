using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    [SerializeField]
    private ElementDataSO elementDataSO;

    private Gamemanager gamemanager;

    /// <summary>
    /// バレット選択ボタンの生成
    /// [GameManager、Startメソッド]
    /// </summary>
    /// <param name="gamemanager"></param>
    /// <returns></returns>
    public IEnumerator GenerateBulletSelectDetail(Gamemanager gamemanager)
    {
        this.gamemanager = gamemanager;

        //BulletDataSOのデータから利用者の種類がPlayerのバレットの情報だけを代入するListを用意する
        List<BulletDataSO.BulletData> playerBulletDatas = new List<BulletDataSO.BulletData>();

        //バレットの利用者の種類がPlayerのバレットの情報だけを抽出してListを作成する
        playerBulletDatas = bulletDataSO.bulletDataList.Where(x => x.liberalType == BulletDataSO.LiberalType.Player).ToList();

        for (int i = 0; i < maxBulletBtnNum; i++)
        {
            //バレットボタン生成
            BulletSelectDetail bulletSelectDetail = Instantiate(bulletSelectDetailPrefab, bulletTran, false);

            //バレットボタンの設定
            bulletSelectDetail.SetUpBulletSelectDetail(this, playerBulletDatas[i]);

            //リストに追加
            bulletSelectDetailList.Add(bulletSelectDetail);

            //0.25秒だけ処理を中断（順番にボタンが生成されるように演出）
            yield return new WaitForSeconds(0.25f);
        }

        // TODO 使用するバレットの情報を初期設定。後ほど、引数を変更する
        GameData.instance.SetBulletData(playerBulletDatas[0]);

       
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

    /// <summary>
    /// 初期バレットを装填中のバレットとして設定
    /// </summary>
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

    public void JugdeOpenBullets()
    {
        //繰り返し利用する情報を変数にしておく
        int totalExp = GameData.instance.GetTotalExp();

        //バレット毎に使用可能なEXPを超えているか確認
        foreach(BulletSelectDetail bulletData in bulletSelectDetailList)
        {
            //Debug.Log(bulletData.bulletData.no);
            //ゲーム終了の状態になったら
            if (gamemanager.isGameUp)
            {
                //バレット選択ボタンを非活性化の状態にしてタップできない状態にする
                bulletData.SwitchAcriveBulletBtn(false);

                continue;
            }

            //
            if(bulletData.GetStateBulletCostPayment())
            {
                //
                bulletData.SwitchAcriveBulletBtn(true);

                continue;
            }

            //Expの現在値がバレットに設定されているコストの値以上であれば
            if (bulletData.bulletData.openExp <= totalExp)
            {
                //そのバレット選択ボタンを活性化の状態にしてタップできるようにする
                bulletData.SwitchAcriveBulletBtn(true);
            }
            else
            {
                //Expの現在値が超えていないバレット選択ボタンを非活性化状態にしてタップできなくする
                bulletData.SwitchAcriveBulletBtn(false);
            }
        }

     
       
    }

    /// <summary>
    /// 選択したバレットのコストの支払いとそれに関連する処理
    /// </summary>
    /// <param name="costExp"></param>
    public void SelectedBulletCostPayment(int costExp)
    {
        //TotalExp（Expの現在値)より、選択したバレットのコストを減算
        GameData.instance.UpDateTotalExp(-costExp);

        //画面のExpの表示減算後の値に更新
        gamemanager.uiManager.UpdateDisplayTotalExp(GameData.instance.GetTotalExp());

        //コストの値のフロート表示を生成
        gamemanager.uiManager.CreateFloatingMessageToExp(-costExp, FloatingMessage.FloatingMessageType.BulletCost);

        //使用可能バレットの確認と更新
        JugdeOpenBullets();

    }
    /// <summary>
    /// elementTypeからSprite取得用
    /// </summary>
    /// <param name="elementType"></param>
    /// <returns></returns>
    public Sprite GetElementTypeSprite(ElementType elementType)
    {
        //elementDataList変数の要素を１つずつ順番に取り出して、elementData変数へ代入
        foreach(ElementDataSO.ElementData elementData in elementDataSO.elementDataList)
        {
            //現在代入されているElementDataのElementTypeと引数のelementTypeが同じであるか照合
            if(elementData.elementType == elementType)
            {
                //照合の結果、合致したElementDataに登録されている画像情報を戻す
                return elementData.elementSprite;
            }
        }

        //どれも合致しない場合はnullを戻す
        return null;
    }
    /// <summary>
    /// BulletTypeよりBulletDataを検索して取得
    /// </summary>
    /// <param name="bulletType"></param>
    /// <returns></returns>
    public BulletDataSO.BulletData GetBulletData(BulletDataSO.BulletType bulletType)
    {
        //引数のBulletTypeと同じbulletTypeが登録されているbulletDataを探す
        foreach (BulletDataSO.BulletData bulletData in bulletDataSO.bulletDataList.Where(x => x.bulletType == bulletType))
        {

            Debug.Log("通過");
            //合致したbulletDataを戻す
            return bulletData;
            
        }

        Debug.Log("nullで通過");
        //どれも合致しない場合はnullを返す
        return null;
    }
}
