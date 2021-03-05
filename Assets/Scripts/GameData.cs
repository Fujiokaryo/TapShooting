using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance;

    [SerializeField]
    private int totalExp; //獲得した合計Exp

    [SerializeField]
    private int durabilityBase; //拠点の耐久力

    [SerializeField]
    private int maxGenerateCountBase; //エネミーの最大生成数

    [SerializeField]
    private BulletDataSO.BulletData cullentbulletData;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// TotalExpの更新
    /// </summary>
    /// <param name="exp"></param>
    public void UpDateTotalExp(int exp)
    {
        totalExp += exp;
    }

    /// <summary>
    /// TotalExpの取得
    /// </summary>
    /// <returns></returns>
    public int GetTotalExp()
    {
        return totalExp;
    }

    /// <summary>
    /// 拠点の耐久力の値を取得
    /// </summary>
    /// <returns></returns>
    public int GetDurability()
    {
        return durabilityBase;
    }

    /// <summary>
    /// エネミーの最大生成数の値を取得
    /// </summary>
    /// <returns></returns>
    public int GetMaxGenerateCount()
    {
        return maxGenerateCountBase;
    }

    /// <summary>
    /// 使用するバレットのデータを設定
    /// </summary>
    /// <param name="bulletData"></param>
    public void SetBulletData(BulletDataSO.BulletData bulletData)
    {
        cullentbulletData = bulletData;
    }

    /// <summary>
    /// 使用しているバレットのデータを取得
    /// </summary>
    /// <returns></returns>
    public BulletDataSO.BulletData GetCullentBulletData()
    {
        return cullentbulletData;
    }
}
