using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    int goal;
    int counter;
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        goal = GameObject.FindGameObjectsWithTag("Target").Length;
        Debug.Log("goal = " + goal);
        counter = 0;
        text.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void targetHit()
    {
        counter++;
        if (counter >= goal)
            endLevel();
        Debug.Log("counter = " + counter);


    }
    public void endLevel()
    {
        text.enabled = true;
        Debug.Log("Got Eem");
    }
}
