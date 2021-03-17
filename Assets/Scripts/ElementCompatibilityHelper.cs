using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ElementCompatibilityHelper
{
    /// <summary>
    /// 攻撃側と防御側の属性の種類によって弱点かどうかを判定
    /// </summary>
    /// <param name="attackElementType"></param>
    /// <param name="defenceElementType"></param>
    /// <returns></returns>
    public static bool GetElementCompatibility(ElementType attackElementType, ElementType defenceElementType)
    {
        //攻撃側の属性情報を確認し、その後防御側の属性情報を確認して判定する
        if(attackElementType == ElementType.Black)
        {
            if(defenceElementType == ElementType.White)
            {
                return true;
            }
        }
        else if(attackElementType == ElementType.Blue)
        {
            if(defenceElementType == ElementType.Red)
            {
                return true;
            }
        }
        else if(attackElementType == ElementType.Green)
        {
            if(defenceElementType == ElementType.Blue)
            {
                return true;
            }
        }
        else if(attackElementType == ElementType.Red)
        {
            if(defenceElementType == ElementType.Green)
            {
                return true;
            }
        }
        else if(attackElementType == ElementType.White)
        {
            if(defenceElementType == ElementType.Black)
            {
                return true;
            }
        }

        //上記以外では弱点ではない
        return false;
    }
}
