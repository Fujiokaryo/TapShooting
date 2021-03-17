using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName ="ElementDataSO", menuName ="Create ElementDataSO")]
public class ElementDataSO : ScriptableObject
{
    public List<ElementData> elementDataList = new List<ElementData>();

    /// <summary>
    /// 属性のデータ
    /// </summary>
    [Serializable]
    public class ElementData
    {
        public int no; //通し番号
        public Sprite elementSprite; //属性の画像
        public ElementType elementType; //属性の情報
    }
}
