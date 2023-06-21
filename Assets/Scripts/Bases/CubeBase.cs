using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBase : MonoBehaviour
{
    public int cubeBaseIndex; // 0 Player Base, 1 AI Base
    public CubeAroundBase[] magnetedCubesAroundBase;
    public List<CubeManager> collectedCubes; //will be used in Part II and III.
    public float spawnTime;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            if (GameManager.Instance.levelStarted)
            {
                CubeManager cube = other.GetComponentInParent<CubeManager>();
                if (cube != null)
                {
                    if (cube.takenByWhichSide == 0 && cubeBaseIndex == 0) // taken by player and in player base
                    {
                        GameManager.Instance.ArrangeScore(0);
                    }
                    else if(cube.takenByWhichSide == 1 && cubeBaseIndex == 1) // taken by AI and in AI base
                    {
                        GameManager.Instance.ArrangeScore(1);
                    }

                    if (LevelEditor.Instance.currentPartIndex == 1 || LevelEditor.Instance.currentPartIndex == 2)
                    {
                        collectedCubes.Add(cube);
                    }
                    cube.placedInWhichBase = cubeBaseIndex;
                    cube.TakenCube();
                }
            }
        }
    }
    private void OnEnable()
    {
        EventHandler.SpawnCubeAtMiddleEvent += SpawnCube;
    }
    private void OnDisable()
    {
        EventHandler.SpawnCubeAtMiddleEvent -= SpawnCube;
    }

    public void SpawnCube()
    {
        if (GameManager.Instance.levelStarted)
        {
            if(collectedCubes.Count > 0)
            {
                spawnTime += Time.deltaTime;
                if(spawnTime >= GameManager.Instance.so_gameValues.cubeSpawnTime)
                {
                    spawnTime = 0f;

                    collectedCubes[0].CubeRandomSpawn();

                    if (collectedCubes.Count > 0)
                    {
                        collectedCubes.RemoveAt(0);
                    }
                }
            }
        }
    }
    

    public void InitiateCubesAroundBase()
    {
        for(int i = 0; i < magnetedCubesAroundBase.Length; i++)
        {
            magnetedCubesAroundBase[i].gameObject.SetActive(true);
        }
    }
    public void DeactivateCubesAroundBase()
    {
        for (int i = 0; i < magnetedCubesAroundBase.Length; i++)
        {
            magnetedCubesAroundBase[i].gameObject.SetActive(false);
        }
    }
}
