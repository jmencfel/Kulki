using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour
{
    public Animator anim;
    private bool isResetShowing = false;
    // Start is called before the first frame update
    void Start()
    {
        anim.SetBool("NewHighScore", false);
    }

    public void SetHighScore()
    {
        anim.SetBool("NewHighScore", true);
    }
    public void Show()
    {
        anim.SetTrigger("Show");
    }
    public void Hide()
    {
        anim.SetTrigger("Hide");
        isResetShowing = false;
    }
    public void ShowReset()
    {
        if (!isResetShowing)
        {
            isResetShowing = true;
            anim.SetTrigger("ShowReset");
        }
    }

}
