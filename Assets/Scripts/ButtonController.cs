using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    public Button startGameButton;

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
}
