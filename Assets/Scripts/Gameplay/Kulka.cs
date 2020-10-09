using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



public class Kulka : MonoBehaviour
{
    private bool isSelected;
    [SerializeField]
    private Animator anim;
    public List<Sprite> Numbers;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer NumberSprite;
    public Color color;
    public int index;
    public float movementSpeed = 5.0f;
    public bool isPerformingStartingCheck = true;


    // Start is called before the first frame update
    private void Start()
    {

        
    }
    public void Deselect()
    {
        isSelected = false;
        anim.SetBool("isSelected", isSelected);
    }
    // Update is called once per frame
   public void SetColor(Color col, int i)
    {
        color = col;
        spriteRenderer.color = col;
        NumberSprite.sprite = Numbers[i];
        index = i;
    }
    private void OnMouseDown()
    {
        if (!GameController.instance.isMoving)
        {
            GameController.instance.UpdateGrid();
            isSelected = true;
            anim.SetBool("isSelected", isSelected);
            GameController.instance.SetSelectedBall(this);
        }

    }
    public void Born()
    {       
        GameController.instance.UpdateGrid(); 
        if(!CheckIfCanDisappear())
            isPerformingStartingCheck = false;
    }
    private bool CheckIfCanDisappear()
    {
        bool disappearedH = CheckHorizontal();
        bool disappearedV = CheckVertical();
        bool disappearedL = CheckLoweringDiagnal();
        bool disappearedU = CheckUppeningDiagnal();
        return disappearedH|| disappearedH || disappearedH || disappearedH;
    }
    public void SetPath(List<Node> path)
    {
        StartCoroutine(MoveByPath(path));
    }
    public void Move(Vector3 direction)
    {
        StartCoroutine(MoveByUnit(direction));
    }
    public bool CheckHorizontal()
    {
        //left side
        List<Kulka> Left = new List<Kulka>();
        List<Kulka> Right = new List<Kulka>();
        int MatchingColourBallsLeft = 0;
        int MatchingColourBallsRight = 0;
 
        for (int i = (int)transform.position.x-1; i>=0;i--)
        {
            Kulka temp = CheckPosition(i, (int)transform.position.y);
            if ( temp != null)
            {
                MatchingColourBallsLeft++;
                Left.Add(temp);
            }
            else
                break;
        }
        for (int i = (int)(transform.position.x + 1) ; i <9; i++)
        {
            Kulka temp = CheckPosition(i, (int)transform.position.y);
            if (temp != null)
            {
                MatchingColourBallsRight++;
                Right.Add(temp);
            }
            else
                break;
        }
   
        
        if (MatchingColourBallsLeft+MatchingColourBallsRight+1>=5)
        {
            GameController.instance.IncreaseScore(MatchingColourBallsLeft + MatchingColourBallsRight + 1);
            foreach (Kulka k in Left)
            {
                k.Dissolve();
            }
            foreach (Kulka k in Right)
            {
                k.Dissolve();
            }
            Dissolve();
       
            return true;
        }
        return false;
    }
    public bool CheckVertical()
    {
        //left side
        List<Kulka> Up = new List<Kulka>();
        List<Kulka> Down = new List<Kulka>();
        int MatchingColourBallsUp = 0;
        int MatchingColourBallsDown = 0;
        for (int i = (int)(transform.position.y - 1); i > -9; i--)
        {
            Kulka temp = CheckPosition((int)(transform.position.x),i);
            if (temp != null)
            {
                MatchingColourBallsDown++;
                Down.Add(temp);
            }
            else
                break;
        }
        for (int i = (int)(transform.position.y + 1); i < 9; i++)
        {
            Kulka temp = CheckPosition((int)(transform.position.x), i);
            if (temp != null)
            {
                MatchingColourBallsUp++;
                Up.Add(temp);
            }
            else
                break;
        }

        if (MatchingColourBallsUp + MatchingColourBallsDown + 1 >= 5)
        {
            GameController.instance.IncreaseScore(MatchingColourBallsUp + MatchingColourBallsDown + 1);
            foreach (Kulka k in Up)
            {
                k.Dissolve();
            }
            foreach (Kulka k in Down)
            {
                k.Dissolve();
            }
            Dissolve();
            return true;
        }
        return false;
    }
    public bool CheckLoweringDiagnal()
    {
        //left side
        List<Kulka> Left = new List<Kulka>();
        List<Kulka> Right = new List<Kulka>();
        int MatchingColourBallsLeft = 0;
        int MatchingColourBallsRight = 0;


        ///LEWA STRONA W GORE
        int differenceY = 0;
        for (int i = (int)transform.position.x - 1; i >= 0; i--)
        {
            differenceY++;
            Kulka temp = CheckPosition(i, (int)transform.position.y+differenceY);
            if (temp != null)
            {
                MatchingColourBallsLeft++;
                Left.Add(temp);
            }
            else
                break;
        }
        ///LEWA STRONA W DOL
        differenceY = 0;
        for (int i = (int)(transform.position.x + 1); i < 9; i++)
        {
            differenceY--;
            Kulka temp = CheckPosition(i, (int)transform.position.y+differenceY);
            if (temp != null)
            {
                MatchingColourBallsRight++;
                Right.Add(temp);
            }
            else
                break;
        }
        if (MatchingColourBallsLeft + MatchingColourBallsRight + 1 >= 5)
        {
            GameController.instance.IncreaseScore(MatchingColourBallsLeft + MatchingColourBallsRight + 1);
            foreach (Kulka k in Left)
            {
                k.Dissolve();
            }
            foreach (Kulka k in Right)
            {
                k.Dissolve();
            }
            Dissolve();     
            return true;
        }
        return false;
    }
    public bool CheckUppeningDiagnal()
    {
        //left side
        List<Kulka> Left = new List<Kulka>();
        List<Kulka> Right = new List<Kulka>();
        int MatchingColourBallsLeft = 0;
        int MatchingColourBallsRight = 0;


        ///LEWA STRONA W DOL
        int differenceY = 0;
        for (int i = (int)transform.position.x - 1; i >= 0; i--)
        {
            differenceY--;
            Kulka temp = CheckPosition(i, (int)transform.position.y + differenceY);
            if (temp != null)
            {
                MatchingColourBallsLeft++;
                Left.Add(temp);
            }
            else
                break;
        }
        ///LEWA STRONA W GORE
        differenceY = 0;
        for (int i = (int)(transform.position.x + 1); i < 9; i++)
        {
            differenceY++;
            Kulka temp = CheckPosition(i, (int)transform.position.y + differenceY);
            if (temp != null)
            {
                MatchingColourBallsRight++;
                Right.Add(temp);
            }
            else
                break;
        }
        if (MatchingColourBallsLeft + MatchingColourBallsRight + 1 >= 5)
        {
            GameController.instance.IncreaseScore(MatchingColourBallsLeft + MatchingColourBallsRight + 1);
            foreach (Kulka k in Left)
            {
                k.Dissolve();
            }
            foreach (Kulka k in Right)
            {
                k.Dissolve();
            }
            Dissolve();
         
            return true;
        }
        return false;
    }
  
    public Kulka CheckPosition(int PosX, int PosY)
    {
        Vector2 position = new Vector2(PosX, PosY);
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.left, 0.01f);
        if(hit.collider != null)
        {
            if(index == hit.transform.gameObject.GetComponent<Kulka>().index)
            {
                return hit.collider.gameObject.GetComponent<Kulka>();
            }
            return null;
        }   
        return null;
        
    }
    private void Dissolve()
    {
        GameController.instance.blockMovement();
        GetComponent<BoxCollider2D>().enabled = false;
        anim.SetTrigger("Vanish");
    }
    private IEnumerator WaitAndUpdate(float time)
    {
        yield return new WaitForSeconds(time);
        GameController.instance.UpdateGrid();
    }
    public void Die()
    {
        GameController.instance.UpdateGrid();
        GameController.instance.AllBalls.Remove(this);
        Destroy(gameObject);
    }
    private IEnumerator MoveByPath(List<Node> path)
    {
        
        foreach(Node curent in path)
        {
            yield return StartCoroutine(MoveByUnit(new Vector2(curent.gridX, -curent.gridY)));
             
        }
        Deselect();
        GameController.instance.DeselectCurrentBall();
        bool disappearedH= CheckHorizontal();
        bool disappearedV = CheckVertical();
        bool disappearedL = CheckLoweringDiagnal();
        bool disappearedU = CheckUppeningDiagnal();
        if (!disappearedH && !disappearedV && !disappearedL && !disappearedU)
        {
            GameController.instance.SpawnBalls(3);
        }
        else
        {
            GameController.instance.UpdateGrid();
        }
        
       


    }

    private IEnumerator MoveByUnit(Vector3 destination)
    {
       
        float timer = 0;
        while (timer < 1)
        {
            transform.position = Vector2.MoveTowards(transform.position, destination, Time.deltaTime * movementSpeed);
            timer += Time.deltaTime * movementSpeed;         
            yield return null;
        }

    }
   
}
