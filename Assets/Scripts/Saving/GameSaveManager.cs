using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameSaveManager : MonoBehaviour
{
    public static GameSaveManager instance;
    public Kulka PrefabKulki;
    GameData data;
    public bool shouldBeLoadedFromFile = false;
    public bool isSaveFilePresent = false;
    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        instance = this;
        DontDestroyOnLoad(this);
        data = SaveSystem.LoadGame();
        if (data != null)
        {
            isSaveFilePresent = true;
        }
    }
    // Start is called before the first frame update
    
    public void SetLoad()
    {
        shouldBeLoadedFromFile = true;
        SceneManager.LoadScene(1);
    }
    public void SaveGame()
    {
        List<Ball> temp = new List<Ball>();
        foreach(Kulka k in GameController.instance.AllBalls)
        {
            temp.Add(new Ball(k.index,(int)k.transform.position.x, (int)k.transform.position.y));
        }
        SaveSystem.SaveGame(GameController.instance.difficulty, GameController.instance.Score, temp);
        GameController.instance.LoadingScreen.SetTrigger("Hide");
        GameController.instance.isSaving = false;
    }
    public void LoadGame()
    {
        GameData data = SaveSystem.LoadGame();
        GameController.instance.SetDifficulty(data.difficulty);
        GameController.instance.Score = data.score;
        foreach (Ball b in data.board)
        {
            var kulka = Instantiate(PrefabKulki, new Vector2(b.x, b.y), Quaternion.identity);
            kulka.SetColor(GameController.instance.BallColours[b.index], b.index);
            GameController.instance.AllBalls.Add(kulka);
        }  
            
        
    }
}
