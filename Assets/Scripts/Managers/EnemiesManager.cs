using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    
    public List<EnemyController> Enemies { get; private set; }
    public int NumberOfEnemiesTotal { get; private set; }
    public int NumberOfEnemiesRemaining => Enemies.Count;
    
    void Awake()
    {
        Enemies = new List<EnemyController>();
    }

    public void RegisterEnemy(EnemyController enemy)
    {
        Enemies.Add(enemy);

        NumberOfEnemiesTotal++;
    }

    public void UnregisterEnemy(EnemyController enemyKilled)
    {
        int enemiesRemainingNotification = NumberOfEnemiesRemaining - 1;

        EnemyKillEvent evt = Events.EnemyKillEvent;
        evt.Enemy = enemyKilled.gameObject;
        EventManager.Broadcast(evt);

        // removes the enemy from the list, so that we can keep track of how many are left
        Enemies.Remove(enemyKilled);
    }
    public void SpawnEnemy()
    {
        if (NumberOfEnemiesRemaining < 200)
        {
            //Spawn around the player a random distance away
            Vector2 randomDir = Random.insideUnitCircle.normalized;
            randomDir *= Random.Range(5f, 15f);
            GameObject go = GameObject.Instantiate(enemyPrefab, new Vector3(
                GameManager.Instance.player.transform.position.x + randomDir.x,
                GameManager.Instance.player.transform.position.y,
                GameManager.Instance.player.transform.position.z + randomDir.y), Quaternion.identity);
            go.GetComponent<EnemyController>().SetTarget(GameManager.Instance.player.gameObject);
            RegisterEnemy(go.GetComponent<EnemyController>());
        }
    }
}
