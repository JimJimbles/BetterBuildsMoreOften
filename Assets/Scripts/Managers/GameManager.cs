using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public PlayerController player;
    public EnemiesManager enemiesManager;
    public PowerupManager powerupManager;
    private void Awake()
    {
        EventManager.AddListener<EnemyKillEvent>(OnEnemyKilled);
    }

    private void Start()
    {
        enemiesManager.SpawnEnemy();
    }

    void OnEnemyKilled(EnemyKillEvent killEvent){
        //Spawn another one, maybe even TWO!!
        enemiesManager.SpawnEnemy();
        enemiesManager.SpawnEnemy();
    }
}
