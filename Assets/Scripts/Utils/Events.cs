using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Events
{
    public static EnemyKillEvent EnemyKillEvent = new EnemyKillEvent();
    public static PickupEvent PickupEvent = new PickupEvent();
}

public class EnemyKillEvent : GameEvent
{
    public GameObject Enemy;
}

public class PickupEvent : GameEvent
{
    public PlayerController player;
    public int powerID;
}