using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public　static class TransformHelper 
{
    private static Transform temporaryObjectContainerTran;

    /// <summary>
    /// TemporaryObjectContainerTran変数のプロパティ
    /// </summary>
    public static Transform TemporaryObjectContainerTran
    {
        set
        {
            temporaryObjectContainerTran = value;
        }

        get 
        {
            return temporaryObjectContainerTran;    
        }
    }


    /// <summary>
    /// temporaryObjectContainerTranに情報をセット
    /// </summary>
    /// <param name="newTran"></param>
    public static void SetTemporaryObjectContainerTran(Transform newTran)
    {
        temporaryObjectContainerTran = newTran;

        Debug.Log("temporaryObjectContainerTran 変数に位置情報をセット完了");
    }

    /// <summary>
    /// temporaryObjectContainerTranの情報を取得
    /// </summary>
    /// <returns></returns>
    public static Transform GetTemporaryObjectContainerTran()
    {
        return temporaryObjectContainerTran;
    }
}
