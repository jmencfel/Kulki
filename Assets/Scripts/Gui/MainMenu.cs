using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Animation anim;
    private bool isInfoVisible = false;
    public GameObject continueButton;
    private bool isNewGameButtonVisible = false;
    private bool isHighScoresVisible = false;
    public Text HighScoresText;
    public GameObject NoAdsBuyButton;
    public Text NextScoresButton;
    private int currentDifficultyIndex = 0;


    private void Awake()
    {
        Application.targetFrameRate = 120;

    }
    public void ContinueButton()
    {
        GameSaveManager.instance.SetLoad();
    }

    private void Start()
    {
#if !UNITY_WEBGL
        LoadHighScores();
        if(GameSaveManager.instance.isSaveFilePresent)
        {
            continueButton.SetActive(true);
        }
        else
            continueButton.SetActive(false);
    }


#endif
    }
        public void SetNextDifficultyScores()
    {
        if (currentDifficultyIndex == 0)
        {
            NextScoresButton.text = "Medium";
            currentDifficultyIndex = 1;
            LoadHighScores(currentDifficultyIndex);
           
        }
        else if (currentDifficultyIndex == 1)
        {
            NextScoresButton.text = "Hard";
            currentDifficultyIndex = 2;
            LoadHighScores(currentDifficultyIndex);
        }
        else if (currentDifficultyIndex == 2)
        {
            NextScoresButton.text = "Insane";
            currentDifficultyIndex = 3;
            LoadHighScores(currentDifficultyIndex);
        }
        else if (currentDifficultyIndex == 3)
        {
            NextScoresButton.text = "Easy";
            currentDifficultyIndex = 0;
            LoadHighScores(currentDifficultyIndex);
        }
    }
    private void LoadHighScores(int difficulty=0)
    {
        HighScoresText.text = ScoreData.instance.GetScoreText(difficulty);
    }
    public void ShowNewGameButtons()
    {
        if (!isNewGameButtonVisible)
        {
            anim.Play("SlideOut");
            isNewGameButtonVisible = true;
        }
        else
        {
            anim.Play("SlideIn");
            isNewGameButtonVisible = false;
        }
    }
    public void ShowHighScores()
    {
        if (!isHighScoresVisible)
        {
            anim.Play("ShowHighScores");
            isHighScoresVisible = true;
        }
        else
        {
            anim.Play("HideHighScores");
            isHighScoresVisible = false;
        }
    }
    public void StartGame()
    {
        anim.Play("SlideIn");
        SceneManager.LoadScene(1);
    }
    public void StartGame(int difficulty)
    {
        PlayerPrefs.SetInt("Difficulty", difficulty);
        SceneManager.LoadScene(1);
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void ShowInfo()
    {
        if (!isInfoVisible)
        {
            anim.Play("ShowInfo");
            isInfoVisible = true;
        }
        else
        {
            anim.Play("HideInfo");
            isInfoVisible = false;
        }
    }
    public void HideInfo()
    {
        isInfoVisible = false;
        anim.Play("HideInfo");
    }
}
