using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public MyGUI gui;
    public Popup popup;
    public Kulka CurrentlySelectedBall;
    public Vector3 CurrentDestination;
    public Text placementText;
    public InputField PlayerName;
    private float BallReactionDelay = 0.1f;
    private float BallReactionDelayTimer = 0.0f;
    Grid grid;
    public bool isMoving = false;
    public List<Node> path = new List<Node>();
    Kulka lastBallAdded;
    public List<Color> BallColours = new List<Color>();
    public List<int> NextColors = new List<int>();
    public Kulka PrefabKulki;
    public int BallsToSpawn = 3;
    private int ColoursAvailable=3;
    public int difficulty;
    public bool isSaving = false;
    public Animator LoadingScreen;

    public List<Kulka> AllBalls = new List<Kulka>();

    public int Score;

    private void Awake()
    {
        instance = this;
        grid = GetComponent<Grid>();
    }
    public void IncreaseScore(int x)
    {
        Score += x;
        gui.UpdateScore();
    } 
    public void ReloadGame()
    {
        
        GameSaveManager.instance.shouldBeLoadedFromFile = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
    }
   
    private void Start()
    {
        

        if (!GameSaveManager.instance.shouldBeLoadedFromFile)
        {
            if (PlayerPrefs.HasKey("Difficulty"))
            {
                difficulty = PlayerPrefs.GetInt("Difficulty");
                switch (difficulty)
                {
                    case 0:
                        ColoursAvailable = 3;
                        break;
                    case 1:
                        ColoursAvailable = 5;
                        break;
                    case 2:
                        ColoursAvailable = 7;
                        break;
                    case 3:
                        ColoursAvailable = 9;
                        break;
                }
                gui.SetDifficulty(difficulty);
            }

            List<Vector2> takenPositions = new List<Vector2>();
            while (BallsToSpawn > 0)
            {
                int randomX = Random.Range(0, grid.GridSizeX);
                int randomY = -Random.Range(0, grid.GridSizeY);
                int colorIndex = Random.Range(0, ColoursAvailable);
                if (!takenPositions.Contains(new Vector2(randomX, randomY)))
                {
                    BallsToSpawn--;
                    var kulka = Instantiate(PrefabKulki, new Vector2(randomX, randomY), Quaternion.identity);
                    kulka.SetColor(BallColours[colorIndex], colorIndex);
                    lastBallAdded = kulka;
                    AllBalls.Add(kulka);
                    takenPositions.Add(new Vector2(randomX, randomY));
                }
            }
            NextColors.Clear();
            NextColors.Add(Random.Range(0, ColoursAvailable));
            NextColors.Add(Random.Range(0, ColoursAvailable));
            NextColors.Add(Random.Range(0, ColoursAvailable));
            gui.UpdateNextColors();
            StartCoroutine(WaitForCheck());
        }
        else
        {
            GameSaveManager.instance.LoadGame();
            gui.UpdateScore();
            NextColors.Clear();
            NextColors.Add(Random.Range(0, ColoursAvailable));
            NextColors.Add(Random.Range(0, ColoursAvailable));
            NextColors.Add(Random.Range(0, ColoursAvailable));
            gui.UpdateNextColors();
        }
       
    }
    public void SetDifficulty(int _difficulty)
    {
        difficulty = _difficulty;
        switch (difficulty)
        {
            case 0:
                ColoursAvailable = 3;
                break;
            case 1:
                ColoursAvailable = 5;
                break;
            case 2:
                ColoursAvailable = 7;
                break;
            case 3:
                ColoursAvailable = 9;
                break;
        }
        gui.SetDifficulty(difficulty);
    }
    public void SaveGame()
    {
        if (!isSaving)
        {
            LoadingScreen.SetTrigger("Show");
            isSaving = true;
            GameSaveManager.instance.SaveGame();
        }
    }
    public void ShowResetPopup()
    {
        popup.ShowReset();
    }
    public void GoBackToMenu()
    {
        SceneManager.LoadScene(0);
    }
    public Color GetNextColor(int i)
    {
        return BallColours[NextColors[i]];
    }
    // Start is called before the first frame update
    public void SetSelectedBall(Kulka x)
    {
        ///When selecting a ball deselect old ball
        if (CurrentlySelectedBall != null)
            CurrentlySelectedBall.Deselect();


        //if you press on the same ball twice, you deselect it and current is null again;
        if (CurrentlySelectedBall == x)
            CurrentlySelectedBall = null;
        else // if you press on a different ball then the other one is selected
            CurrentlySelectedBall = x;
        BallReactionDelayTimer = BallReactionDelay;
    }
    public void blockMovement()
    {
        isMoving = true;
    }
    public void unblockMovement()
    {
        isMoving = false;
    }
    public void SpawnBalls(int num)
    {
        int CurrentNumberOfBalls = GameObject.FindGameObjectsWithTag("Kulka").Length;
        num = Mathf.Clamp(num, 0, 81 - CurrentNumberOfBalls);
        int i = 0;
        while (num > 0)
        {
            int randomX = Random.Range(0, grid.GridSizeX);
            int randomY = -Random.Range(0, grid.GridSizeY);
            if (IsPositionFree(randomX, randomY))
            {
                num--;
                var kulka = Instantiate(PrefabKulki, new Vector2(randomX, randomY), Quaternion.identity);
                kulka.SetColor(BallColours[NextColors[i]], NextColors[i]);
                AllBalls.Add(kulka);
                lastBallAdded = kulka;
                i++;
            }
        }
        NextColors.Clear();
        NextColors.Add(Random.Range(0, ColoursAvailable));
        NextColors.Add(Random.Range(0, ColoursAvailable));
        NextColors.Add(Random.Range(0, ColoursAvailable));
        gui.UpdateNextColors();
        StartCoroutine(WaitForCheck());

    }
    int highScoreIndex = -1;
    IEnumerator WaitForCheck()
    {
        yield return new WaitForSeconds(0.8f);
      //  while (lastBallAdded.isPerformingStartingCheck || isMoving)
      //      yield return null;
        if (AllBalls.Count >= 81)
        {
            if (CheckIfHighScore())
            {
                placementText.text = (highScoreIndex + 1).ToString();
                if (highScoreIndex == 0) placementText.text += "st";
                else if (highScoreIndex == 1) placementText.text += "nd";
                else if (highScoreIndex == 2) placementText.text += "rd";
                else placementText.text += "th";
                popup.SetHighScore();
            }

            popup.Show();
            SaveSystem.EraseGame();

            if (!PlayerPrefs.HasKey("NoAds"))
            {
               
               AdsController.instance.ShowAd();                                      
                   
            }
            else
            {
                Debug.Log("NoAds Key found!");
            }
            
            
            
        }
       
       
           
        
       


    }
    public void AddNewName()
    {
        string name = PlayerName.text;
        if (name.Length < 1) name = "NO NAME";
        ScoreData.instance.InsertNewScore(highScoreIndex, PlayerName.text, Score, difficulty);
        ReloadGame();
    }
    private bool CheckIfHighScore()
    {
       highScoreIndex = ScoreData.instance.PlaceIndex(Score, difficulty);
       return highScoreIndex != -1;
    }
    public void DeselectCurrentBall()
    {
        CurrentlySelectedBall = null;
    }
    // Update is called once per frame
    void Update()
    {

        
        if (CurrentlySelectedBall != null && !isMoving)
        {
            
            BallReactionDelayTimer -= Time.deltaTime;
            if (Input.GetMouseButtonDown(0) && BallReactionDelayTimer <= 0)
            {
               
                Vector2 adjustedMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                adjustedMousePosition.x = Mathf.RoundToInt(adjustedMousePosition.x);
                adjustedMousePosition.y = Mathf.RoundToInt(adjustedMousePosition.y);

                bool doesPathExist =Pathfinding.instance.FindPath(CurrentlySelectedBall.transform.position, adjustedMousePosition);
                if (path != null && doesPathExist)
                {
                    CurrentlySelectedBall.SetPath(path);
                    isMoving = true;
                }

            }
        }
    }
    public void UpdateGridSafe()
    {
        grid.UpdateWalkableInfo();
    }
    public void UpdateGrid()
    {
        isMoving = false;
        grid.UpdateWalkableInfo();
    }
    public bool IsPositionFree(int PosX, int PosY)
    {
        Vector2 position = new Vector2(PosX, PosY);
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.left, 0.01f);
        if (hit.collider != null)
        {
            return false;
              
        }
        return true;

    }
}
