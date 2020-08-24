using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorRandomizer : MonoBehaviour
{
    // Start is called before the first frame update
    public Image ball;
    void Start()
    {
        float RandomR = Random.Range(0, 1.0f);
        float RandomG = Random.Range(0, 1.0f);
        float RandomB = Random.Range(0, 1.0f);
        ball.color = new Color(RandomR, RandomG, RandomB, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
