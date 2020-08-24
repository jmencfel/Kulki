using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyGUI : MonoBehaviour
{
    public Text score;
    public Text difficulty;
    public List<Image> nextBalls;
    // Start is called before the first frame update
    void Start()
    {
        UpdateScore();
    }

    // Update is called once per frame
    public void UpdateScore()
    {
        score.text = "Score:\n" + GameController.instance.Score;
    }
    public void SetDifficulty(int x)
    {
        switch (x)
        {
            case 0:
                difficulty.text = "Difficulty\nEasy\n3 Colours";
                break;
            case 1:
                difficulty.text = "Difficulty\nMedium\n5 Colours";
                break;
            case 2:
                difficulty.text = "Difficulty\nHard\n7 Colours";
                break;
            case 3:
                difficulty.text = "Difficulty\nInsane\n9 Colours";
                break;
        }
    }
    public void UpdateNextColors()
    {
        int i = 0;
        foreach(Image ball in nextBalls)
        {
            ball.color = GameController.instance.GetNextColor(i);
            i++;
        }
    }
}
