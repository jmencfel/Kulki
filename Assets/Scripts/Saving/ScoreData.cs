using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ScoreBoard
{
    public string[] names = new string[10];
    public int[] scores = new int[10];
    public ScoreBoard()
    {
        names = new string[10];
        scores = new int[10];
    }

}
public class ScoreData : MonoBehaviour
{
    public static ScoreData instance;
    private ScoreBoard easy;
    private ScoreBoard medium;
    private ScoreBoard hard;
    private ScoreBoard insane;
    public Dictionary<string, ScoreBoard> Boards = new Dictionary<string, ScoreBoard>();
    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        instance = this;
        DontDestroyOnLoad(this);
        LoadFromFiles();
    }
    public void LoadFromFiles()
    {
        easy = SaveSystem.Load(0);
        medium = SaveSystem.Load(1);
        hard = SaveSystem.Load(2);
        insane = SaveSystem.Load(3);
        Boards.Clear();
        Boards.Add(((diff)0).ToString(), easy);
        Boards.Add(((diff)1).ToString(), medium);
        Boards.Add(((diff)2).ToString(), hard);
        Boards.Add(((diff)3).ToString(), insane);
    }
  
    public string GetScoreText(int difficulty)
    {
        string temp = "High Scores:\n\n";
        
        for(int i=0;i<10;i++)
        {
            if (Boards[((diff)difficulty).ToString()].scores[i] > 0)
            {
                temp += (i + 1) + ". " + Boards[((diff)difficulty).ToString()].names[i] + "  " + Boards[((diff)difficulty).ToString()].scores[i] + "\n";
            }
            else
                temp += (i+1)+".\n";
        }
        return temp;
    }
    
    public int PlaceIndex(int score, int difficulty)
    {
        for (int i = 0; i < 10; i++)
        {
            if (score > Boards[((diff)difficulty).ToString()].scores[i])
            {
                return i;
            }
        }
        return -1;
    }
    
    public void InsertNewScore(int index, string name, int score, int difficulty)
    {
        for (int i = 8; i >= index; i--)
        {
            Boards[((diff)difficulty).ToString()].scores[i + 1] = Boards[((diff)difficulty).ToString()].scores[i];
            Boards[((diff)difficulty).ToString()].names[i + 1] = Boards[((diff)difficulty).ToString()].names[i];
        }
        Boards[((diff)difficulty).ToString()].scores[index] = score;
        Boards[((diff)difficulty).ToString()].names[index] = name;
        SaveSystem.Save(Boards[((diff)difficulty).ToString()], difficulty);
        
    }
    
}
