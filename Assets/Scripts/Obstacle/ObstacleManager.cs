using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    private static ObstacleManager instance;
    public static ObstacleManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ObstacleManager>();
            }
            return instance;
        }
    }
    public Obstacle obstacle;
    public BoxCollider obstacleSpawnArea;
    public bool spawned;

    [Header("Explosion Particle FX")]
    public ParticleSystem explosionParticle;

    [Header("Time Values")]
    [SerializeField]private float timer;



    private void OnEnable()
    {
        EventHandler.SpawnObstacleRandomlyEvent += SpawnObstacle;
    }
    private void OnDisable()
    {
        EventHandler.SpawnObstacleRandomlyEvent -= SpawnObstacle;
    }

    
    public void SpawnObstacle()
    {
        if(GameManager.Instance.levelStarted && LevelEditor.Instance.currentPartIndex == 2)
        {
            timer += Time.deltaTime;
            if (spawned)
            {
                if (timer >= GameManager.Instance.so_gameValues.timeToDespawn)
                {
                    timer = 0;
                    obstacle.SetObstacleActive(false);
                    spawned = false;
                }
            }
            else
            {
                if (timer >= GameManager.Instance.so_gameValues.timeToSpawn)
                {
                    timer = 0;
                    obstacle.SetObstacleActive(true);
                    spawned = true;
                }
            }

        }
    }



    /// <summary>
    /// when the part first starts, reset the values.
    /// </summary>
    public void ResetObstacleSettings()
    {
        spawned = false;
        timer = 0;
    }
}
