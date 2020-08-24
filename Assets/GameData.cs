using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Ball
{
    public int index;
    public int x;
    public int y;
   
    public Ball(int i, int _x, int _y)
    {
        index = i;
        x = _x;
        y = _y;
    }
}
[System.Serializable]
public class GameData
{
    // Start is called before the first frame update

    public int difficulty=0;
    public int score = 0;
    public List<Ball> board = new List<Ball>();

  
    public GameData(int dif, int sc, List<Ball> b)
    {
        difficulty = dif;
        score = sc;
        board = b;
    }
}
