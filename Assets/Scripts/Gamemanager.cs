using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    public bool isGameUp;

    [SerializeField]
    private DefenceBase defenceBase;

    [SerializeField]
    private EnemyGenerator enemyGenerator;

    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private Transform temporaryObjectContainerTran;

    public UIManager uiManager;

    public BulletSelectManager bulletSelectManager;

    IEnumerator Start()
    {
        SwitchGameUp(false);

        defenceBase.SetUpDefenceBase(this);

        playerController.SetUpPlayer(this);

        enemyGenerator.SetUpEnemyGenerator(this);

        TransformHelper.TemporaryObjectContainerTran = temporaryObjectContainerTran;

        uiManager.HideGameClearSet();

        uiManager.HideGameOverSet();

        yield return StartCoroutine(bulletSelectManager.GenerateBulletSelectDetail(this));

        bulletSelectManager.JugdeOpenBullets();
    }

    // Update is called once per frame
    
    public void SwitchGameUp(bool isSwitch)
    {
        isGameUp = isSwitch;

        if(isGameUp)
        {
            //画面に残っているエネミーをすべて破壊する
            enemyGenerator.ClearEnemiesList();

            //一時オブジェクトを破壊する（子オブジェクトである、ボスのエネミーのバレットやエフェクト等も一緒に破壊される
            enemyGenerator.DestroyTemporaryObjectContainer();
        }
    }

    public void PreparateGameClear()
    {
        //ゲームクリアの表示を行う
        uiManager.DisplayGameClaerSet();
    }

    public void PreparateGameOver()
    {
        uiManager.DisplayGameOverSet();
    }

    public Vector3 GetPlayerDirection(Vector3 enemyPos)
    {
        return (playerController.transform.position - enemyPos).normalized;
    }
}
