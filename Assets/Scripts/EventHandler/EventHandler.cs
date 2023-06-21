using System;
using System.Collections.Generic;
using UnityEngine;

public static class EventHandler
{
    public static event Action SpawnCubeAtMiddleEvent;

    public static void CallSpawnCubeAtMiddleEvent()
    {
        if (SpawnCubeAtMiddleEvent != null)
        {
            SpawnCubeAtMiddleEvent();
        }
    }


    public static event Action SpawnObstacleRandomlyEvent;

    public static void CallSpawnObstacleRandomlyEvent()
    {
        if (SpawnObstacleRandomlyEvent != null)
        {
            SpawnObstacleRandomlyEvent();
        }
    }
}