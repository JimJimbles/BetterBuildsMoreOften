using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private GameObject player;
    private float speed = 10f;
    public void SetTarget(GameObject go)
    {
        player = go;
    }

    void TrackToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position,
            player.transform.position,
            speed * Time.deltaTime);
    }

    private void Update()
    {
        if (player)
        {
            TrackToTarget();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Hit");
        PlayerController pl;
        if (other.gameObject.TryGetComponent<PlayerController>(out pl))
        {
            if (pl.health.invincible)
            {
                Die();
            }
        }
        else
        {
            pl.health.TakeDamage(1f);
            //TODO: Show damage visibly
            Die();
        }
    }

    void Die()
    {
        GameManager.Instance.enemiesManager.UnregisterEnemy(this);
        Destroy(gameObject, 0f);
    }
}
