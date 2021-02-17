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

    void Start()
    {
        SwitchGameUp(false);

        defenceBase.SetUpDefenceBase(this);

        playerController.SetUpPlayer(this);

        enemyGenerator.SetUpEnemyGenerator(this);

        //TransformHelper.SetTemporaryObjectContainerTran(temporaryObjectContainerTran);

        TransformHelper.TemporaryObjectContainerTran = temporaryObjectContainerTran;
    }

    // Update is called once per frame
    
    public void SwitchGameUp(bool isSwitch)
    {
        isGameUp = isSwitch;
    }
}
