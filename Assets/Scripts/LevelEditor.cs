using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditor : MonoBehaviour
{
    private static LevelEditor instance;
    public static LevelEditor Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LevelEditor>();
            }
            return instance;
        }
    }

    [Header("Level Details")]

    [HideInInspector] public int currentPartIndex;
    [HideInInspector] public int currentLevelIndex = 0;
    [HideInInspector] public Vector3 middlePos;
    public Texture2D[] levelMaps; // An array of levels to generate the level by using level editor.
    public BoxCollider randomCubeSpawnArea;

    [Header("Cube Details")]
    public CubeManager[] cubePool; // A pool of cubes that is limited by 450 in order to prevent a garbage allocation that will be caused by unnecessary instantiating. Instead only the necessary amount will be activated, the rest will remain inactive.
    private int nextAvailableCubeIndex = 0;

    [Header("Cube Bases")]
    public CubeBase [] cubeBases; //0 Base for player, 1 Base for AI

    private void Awake()
    {
        middlePos = new Vector3(randomCubeSpawnArea.bounds.center.x, randomCubeSpawnArea.bounds.center.y, randomCubeSpawnArea.bounds.center.z);
    }

    public void CreateLevel(int partIndex)
    {
        currentPartIndex = partIndex;
        //Before generating the cubes, let's make sure that all the elements of our pool are deactivated.
        for (int i = 0; i < cubePool.Length; i++)
        {
            if (!cubePool[i].gameObject.activeSelf)
            {
                //if the element is already inactive, we can break the loop in order not to continue the loop unnecessarily, because there can't be an active one after inactive.
                break;
            }
            cubePool[i].ResetCubeValues();
            cubePool[i].gameObject.SetActive(false);
        }

        nextAvailableCubeIndex = 0; //This is here to keep the track of which cube will be activated. It will be adjusted to '0' in the beginning of each level.
       

        //Only for Part 1.
        if(partIndex == 0)
        {
            //Looping through all the pixels within the image that will create the level design.
            for (int x = 0; x < levelMaps[currentLevelIndex].width; x++)
            {
                for (int y = 0; y < levelMaps[currentLevelIndex].height; y++)
                {
                    GenerateCubes(x, y);
                }
            }
        }
        //Part 2 & 3
        else if(partIndex == 1 || partIndex == 2)
        {
            for(int i = 0; i < cubePool.Length; i++)
            {
                cubePool[i].transform.position = GetRandomTransformPoint(randomCubeSpawnArea);
                cubePool[i].boxCollider.transform.localRotation = Quaternion.Euler(0, 0, 0);
                cubePool[i].boxCollider.transform.localPosition = new Vector3(0, 0, 0);
                cubePool[i].meshRenderer.material = GameManager.Instance.so_gameValues.cubeColors[0];
                cubePool[i].gameObject.SetActive(true);
            }
        }

        for (int i = 0; i < cubeBases.Length; i++)
        {
            cubeBases[i].InitiateCubesAroundBase();
            cubeBases[i].collectedCubes.Clear();
        }
    }


    /// <summary>
    /// This generates cubes from image pixels for Part I.
    /// </summary>
    private void GenerateCubes(int x, int y)
    {
        Color pixelColor = levelMaps[currentLevelIndex].GetPixel(x, y);

        //If the alpha of the pixels is not zero, there is a color information for a cube.
        if (pixelColor.a != 0)
        {
            Vector3 position = new Vector3(x, 0.5f, y);
            cubePool[nextAvailableCubeIndex].transform.localPosition = position;
            cubePool[nextAvailableCubeIndex].meshRenderer.material = CubeColorArranger(pixelColor);
            cubePool[nextAvailableCubeIndex].boxCollider.transform.localRotation =  Quaternion.Euler(0,0,0);
            cubePool[nextAvailableCubeIndex].gameObject.SetActive(true);
            cubePool[nextAvailableCubeIndex].boxCollider.transform.localPosition = new Vector3(0,0,0);
            nextAvailableCubeIndex++;
        }
    }


    /// <summary>
    /// This spawns cubes randomly within the bounds of the spawn area collider for Part II.
    /// </summary>
    public Vector3 GetRandomTransformPoint(Collider collider)
    {
        return new Vector3(Random.Range(collider.bounds.min.x, collider.bounds.max.x), 0.5f, Random.Range(collider.bounds.min.z, collider.bounds.max.z));
    }


    /// <summary>
    /// This gets the color information from Image pixels and sets the correct color for the cube.
    /// </summary>
    public Material CubeColorArranger(Color color)
    {
        //Now let's find out what color is our cube.
        //Cube Color Material Array elements: 0 white, 1 black, 2 red, 3 yellow, 4 green
        if (color == Color.white)
        {
            return GameManager.Instance.so_gameValues.cubeColors[0];
        }
        else if (color == Color.black)
        {
            return GameManager.Instance.so_gameValues.cubeColors[1];
        }
        else if (color == Color.red)
        {
            return GameManager.Instance.so_gameValues.cubeColors[2];
        }
        else if (color == Color.yellow)
        {
            return GameManager.Instance.so_gameValues.cubeColors[3];
        }
        else if(color == Color.green)
        {
            return GameManager.Instance.so_gameValues.cubeColors[4];
        }
        else
        {
            return GameManager.Instance.so_gameValues.cubeColors[0]; // default is white.
        }
    }
}
