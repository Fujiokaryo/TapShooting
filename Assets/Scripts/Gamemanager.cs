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

    void Start()
    {
        SwiychGameUp(false);

        defenceBase.SetUpDefenceBase(this);

        playerController.SetUpPlayer(this);

        enemyGenerator.SetUpEnemyGenerator(this);
    }

    // Update is called once per frame
    
    public void SwiychGameUp(bool isSwitch)
    {
        isGameUp = isSwitch;
    }
}
