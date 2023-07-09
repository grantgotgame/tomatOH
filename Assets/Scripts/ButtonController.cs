using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    public GameObject panel1;
    public GameObject panel2;
    public GameObject panel3;
    public GameObject panel4;
    public GameObject panel5;
    public GameObject panel6;
    public GameObject panel7;
    private int nextCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Start a new game on level 1
    public void StartGame()
    {
        SceneManager.LoadScene("Level1");
    }

    // Load the title screen
    public void MainMenu()
    {
        SceneManager.LoadScene("Title Screen");
    }

    // Load the intro/credits sequence
    public void StartIntro()
    {
        SceneManager.LoadScene("Intro");
    }

    // Progress through the intro sequence
    public void Next()
    {
        nextCounter += 1;
        if (nextCounter == 1)
        {
            panel1.SetActive(false);
            panel2.SetActive(true);
        }
        else if (nextCounter == 2)
        {
            panel2.SetActive(false);
            panel3.SetActive(true);
        }
        else if (nextCounter == 3)
        {
            panel3.SetActive(false);
            panel4.SetActive(true);
        }
        else if (nextCounter == 4)
        {
            panel4.SetActive(false);
            panel5.SetActive(true);
        }
        else if (nextCounter == 5)
        {
            panel5.SetActive(false);
            panel6.SetActive(true);
        }
        else if (nextCounter == 6)
        {
            panel6.SetActive(false);
            panel7.SetActive(true);
        }
        else
        {
            StartGame();
        }
    }
}
