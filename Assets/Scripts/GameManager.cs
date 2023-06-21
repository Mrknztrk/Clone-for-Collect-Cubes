using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }

    public PlayerManager player;
    public AIManager aiOpponent;
    [SerializeField] private GameObject dynamicLevelObjects;

    [HideInInspector]public bool levelStarted;
    private int playerScore; //counts how many cubes player has taken.
    private int aiScore; //counts how many cubes AI has taken.
    private int timeLeft;

    [Header("Game Values & Tweaks")]
    public SO_GameValues so_gameValues;

    private void Update()
    {
        EventHandler.CallSpawnCubeAtMiddleEvent();
        EventHandler.CallSpawnObstacleRandomlyEvent();
    }


    public void CreateParticleEffect(ParticleSystem particle, Vector3 posToExplode)
    {
        particle.transform.position = posToExplode;
        particle.Play();
    }
    /// <summary>
    /// 0 for Player, 1 for AI.
    /// </summary>
    public void ArrangeScore(int aiOrPlayer)
    {
        if(aiOrPlayer == 0)
        {
            playerScore++;
            UIManager.Instance.ScoreTextChange(aiOrPlayer, playerScore);
        }
        else
        {
            aiScore++;
            UIManager.Instance.ScoreTextChange(aiOrPlayer, aiScore);
        }
    }
    public void ResetScore()
    {
        playerScore = 0;
        aiScore = 0;
    }

    public void StartGame(int partIndex)
    {
        switch (partIndex)
        {
            //PART 1
            case 0:
                LevelEditor.Instance.currentLevelIndex = 0; // You cannot switch levels in part 2 and 3, so it's just for this part.
                UIManager.Instance.SetTitleText("Part I - Current Level: " + (LevelEditor.Instance.currentLevelIndex + 1));
                break;

            //PART 2
            case 1:
                UIManager.Instance.SetTitleText("Part II - Collect as many cubes as possible before time runs out.");
                break;      
                
            //PART 3
            case 2:
                UIManager.Instance.SetTitleText("Part III - Get the highest score. Avoid hitting falling bombs.");
                break;
        }
        StartCoroutine(StartingChosenPart(partIndex));
    }

    public IEnumerator StartingChosenPart(int chosenPart)
    {
        UIManager.Instance.preGameCv.blocksRaycasts = false;

        yield return StartCoroutine(UIManager.Instance.ChangingAlpha(UIManager.Instance.preGameCv, 0));
        player.StopPlayer();
        player.RepositionPlayer();

        LevelEditor.Instance.CreateLevel(chosenPart);
        UIManager.Instance.AllPartsCanvasGroupsAlphaDown();
        aiOpponent.DeactivateAI();
        dynamicLevelObjects.SetActive(true);
        levelStarted = true;

        //part3Cv.alpha = 0;
        //part3Cv.blocksRaycasts = false;

        if (chosenPart == 0)
        {
            UIManager.Instance.WhichPartCanvasGroupToIncrease(chosenPart);
        }
        else if (chosenPart == 1)
        {
            timeLeft = so_gameValues.remainingTime;
            ResetScore();
            UIManager.Instance.WhichPartCanvasGroupToIncrease(chosenPart);
            UIManager.Instance.ArrangeScoreAndTimerText(chosenPart, timeLeft, playerScore);
            StartCoroutine(InitializingPart(1));
        }
        else if (chosenPart == 2)
        {
            timeLeft = so_gameValues.remainingTime;
            ResetScore();
            ObstacleManager.Instance.ResetObstacleSettings();
            UIManager.Instance.WhichPartCanvasGroupToIncrease(chosenPart);
            UIManager.Instance.ArrangeScoreAndTimerText(chosenPart, timeLeft, playerScore);
            aiOpponent.ActivateAI();
            StartCoroutine(InitializingPart(2));
        }

        StartCoroutine(UIManager.Instance.ChangingAlpha(UIManager.Instance.inGameCv, 1));

    }
    public void LeavePart()
    {
        UIManager.Instance.inGameCv.blocksRaycasts = false;
        dynamicLevelObjects.SetActive(false);

        //Obstacle exists only in the part 3.
        if(LevelEditor.Instance.currentPartIndex == 2)
        {
            ObstacleManager.Instance.obstacle.SetObstacleActive(false);
            ObstacleManager.Instance.ResetObstacleSettings();
        }

        for (int i = 0; i < LevelEditor.Instance.cubeBases.Length; i++)
        {
            LevelEditor.Instance.cubeBases[i].DeactivateCubesAroundBase();
        }
        UIManager.Instance.SetTitleText("Choose A Part To Try Out.");
        StartCoroutine(UIManager.Instance.ChangingAlpha(UIManager.Instance.inGameCv, 0));
        StartCoroutine(UIManager.Instance.ChangingAlpha(UIManager.Instance.preGameCv, 1));
        levelStarted = false;
    }

    //Part I
    public void NewLevelInPart1()
    {
        player.RepositionPlayer();

        LevelEditor.Instance.currentLevelIndex++;

        if (LevelEditor.Instance.currentLevelIndex > LevelEditor.Instance.levelMaps.Length - 1)
        {
            LevelEditor.Instance.currentLevelIndex = 0;
        }
        UIManager.Instance.SetTitleText("Part I - Current Level: " + (LevelEditor.Instance.currentLevelIndex + 1));

        LevelEditor.Instance.CreateLevel(0);
    }

    //Part II
    public IEnumerator InitializingPart(int whichPart)
    {
        while (timeLeft > 0)
        {
            if (!levelStarted)
            {
                yield break;
            }
            timeLeft -= 1;
            UIManager.Instance.SetTimerText(timeLeft);
            //part3TimerText.text = "Time: " + timeLeft;
            yield return new WaitForSecondsRealtime(1f);
        }
        StopPart();

        if(whichPart == 2)
        {
            DetermineWinner();
            aiOpponent.DeactivateAI();
            ObstacleManager.Instance.obstacle.SetObstacleActive(false);
            ObstacleManager.Instance.ResetObstacleSettings();
        }
    }

    public void StopPart()
    {
        levelStarted = false;
        player.RepositionPlayer();
        dynamicLevelObjects.SetActive(false);
        for (int i = 0; i < LevelEditor.Instance.cubeBases.Length; i++)
        {
            LevelEditor.Instance.cubeBases[i].DeactivateCubesAroundBase();
        }
    }

    public void DetermineWinner()
    {
        if(playerScore > aiScore)
        {
            UIManager.Instance.SetTitleText("Congratulations, you've won!");
        }
        else if(aiScore > playerScore)
        {
            UIManager.Instance.SetTitleText("You have lost.");
        }
        else
        {
            UIManager.Instance.SetTitleText("Well, it's pretty rare to get an even score but seems like there's a draw!");
        }
    }
}
