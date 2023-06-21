using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }
            return instance;
        }
    }

    [Header("Main Canvas")]
    public CanvasGroup preGameCv;
    public CanvasGroup inGameCv;
    [SerializeField] private TextMeshProUGUI mainTitleText;

    [Header("Parts Information")]
    //Part1
    [SerializeField] private CanvasGroup part1Cv;

    //Part2
    [SerializeField] private CanvasGroup part2Cv;
    [SerializeField] private TextMeshProUGUI part2TimerText;
    [SerializeField] private TextMeshProUGUI part2ScoreBoardText;

    //Part3
    [SerializeField] private CanvasGroup part3Cv;
    [SerializeField] private TextMeshProUGUI part3TimerText;
    [SerializeField] private TextMeshProUGUI part3ScoreBoardText;
    [SerializeField] private TextMeshProUGUI part3AiScoreBoardText;

    /// <summary>
    /// Used for manipulating alpha of the UI objects.
    /// </summary>
    public IEnumerator ChangingAlpha(CanvasGroup cvGroup, int targetAlpha)
    {
        while (!Mathf.Approximately(cvGroup.alpha, targetAlpha))
        {
            //... move the alpha towards its target alpha.
            cvGroup.alpha = Mathf.MoveTowards(cvGroup.alpha, targetAlpha, 1f * Time.unscaledDeltaTime);

            //Wait for a frame then continue.
            yield return null;
        }
        cvGroup.alpha = targetAlpha;
        if (targetAlpha == 1)
        {
            cvGroup.blocksRaycasts = true;
        }
        else
        {
            cvGroup.blocksRaycasts = false;
        }
    }
    public void ScoreTextChange(int aiOrPlayer, int scoreAmount)
    {
        if(aiOrPlayer == 0)
        {
            if (LevelEditor.Instance.currentPartIndex == 1)
            {
                part2ScoreBoardText.text = "Score: " + scoreAmount;
            }
            else if (LevelEditor.Instance.currentPartIndex == 2)
            {
                part3ScoreBoardText.text = "Score: " + scoreAmount;
            }
        }
        else
        {
            if (LevelEditor.Instance.currentPartIndex == 2)
            {
                part3AiScoreBoardText.text = "Score: " + scoreAmount;
            }
        }
    }
    public void SetTimerText(int timeLeft)
    {
        if (LevelEditor.Instance.currentPartIndex == 1)
        {
            part2TimerText.text = "Time: " + timeLeft;
        }
        else if (LevelEditor.Instance.currentPartIndex == 2)
        {
            part3TimerText.text = "Time: " + timeLeft;
        }
    }
    public void SetTitleText(string text)
    {
        mainTitleText.text = text;
    }
    public void WhichPartCanvasGroupToIncrease(int whichPart)
    {
        switch (whichPart)
        {
            case 0:
                part1Cv.alpha = 1;
                part1Cv.blocksRaycasts = true;
                break;
            case 1:
                part2Cv.alpha = 1;
                part2Cv.blocksRaycasts = true;
                break;
            case 2:
                part3Cv.alpha = 1;
                part3Cv.blocksRaycasts = true;
                break;
        }
    }
    public void ArrangeScoreAndTimerText(int whichPart, int timeLeft, int playerScore)
    {
        switch (whichPart)
        {
            case 1:
                part2TimerText.text = "Time: " + timeLeft;
                part2ScoreBoardText.text = "Score: " + playerScore;
                break;
            case 2:
                part3TimerText.text = "Time: " + timeLeft;
                part3ScoreBoardText.text = "Score: " + playerScore;
                break;
        }
    }
    public void AllPartsCanvasGroupsAlphaDown()
    {
        //Resetting UI.
        part1Cv.alpha = 0;
        part1Cv.blocksRaycasts = false;

        part2Cv.alpha = 0;
        part2Cv.blocksRaycasts = false;

        part3Cv.alpha = 0;
        part3Cv.blocksRaycasts = false;
    }
}
