using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_GameValues", menuName = "Scriptable Objects/GameValues/SO_GameValues")]
public class SO_GameValues : ScriptableObject
{
    [Header("Other Values")]
    public float cubeSpawnTime;
    public int remainingTime;
    [Header("Magnet Values")]
    public float magnetForceValue; // how strong is magnet?
    [Header("Obstacle Values")]
    public float timeToSpawn;
    public float timeToDespawn;
    [Header("Click Based Settings")]
    public float movementBoostMultiplier;
    public float clickSwipeSpeedBoostLimit; // after how much distance can the player go faster with swipe?
    public float clickSwipeMovementLimit; // after how much distance will the player move?
    [Header("Cube Colors")]
    public Material[] cubeColors; // Materials to adjust cube colors. Material Array elements: 0 white, 1 black, 2 red, 3 yellow, 4 green
    public Material playerColor;
    public Material aiColor;
}