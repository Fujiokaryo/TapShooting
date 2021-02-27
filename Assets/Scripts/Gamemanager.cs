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


    void Start()
    {
        SwitchGameUp(false);

        defenceBase.SetUpDefenceBase(this);

        playerController.SetUpPlayer(this);

        enemyGenerator.SetUpEnemyGenerator(this);

        TransformHelper.TemporaryObjectContainerTran = temporaryObjectContainerTran;

        uiManager.HideGameClearSet();

        uiManager.HideGameOverSet();
    }

    // Update is called once per frame
    
    public void SwitchGameUp(bool isSwitch)
    {
        isGameUp = isSwitch;
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
}
