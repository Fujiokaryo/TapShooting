using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "BulletDataSO", menuName = "Create BulletDataSO")]
public class BulletDataSO : ScriptableObject
{
    public List<BulletData> bulletDataList = new List<BulletData>();

    /// <summary>
    /// バレットの種類
    /// </summary>
    [Serializable]
    public enum BulletType
    {
        A,
        B,
        C,
        Player_Normal,
        Player_Blaze,
        Player_3ways_Piercing,
        Player_5ways_Normal,
        None,
    }

    [Serializable]
    public enum LiberalType
    {
        Player,
        Enemy,
        Boss
    }

    [Serializable]
    public class BulletData
    {
        public int no; //バレットの通し番号
        public float bulletSpeed;  //速度
        public int bulletPower; //攻撃力
        public float loadingTime;  //発射までの待機時間
        public BulletType bulletType; //バレットの種類
        public Sprite btnSprite;  //バレット選択用のボタンの画像
        public LiberalType liberalType; //バレットの利用者の種類
        public int openExp; //バレットを利用するために必要なExp
        public float launchTime; //バレットを使用できる時間
        public Sprite bulletSprite; //発射するバレットの画像
        public string discription; //バレットの説明文
    }


}
