using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverText;
    public GameObject winText;
    public GameObject restartButton;
    public GameObject mainMenuButton;

    public bool gameOver;

    private SoundManager soundManagerScript;
    private TomatoController tomatoControllerScript;

    // Start is called before the first frame update
    void Start()
    {
        // find scripts
        soundManagerScript = GameObject.Find("Tomato Controller").GetComponent<SoundManager>();
        tomatoControllerScript = GameObject.Find("Tomato Controller").GetComponent<TomatoController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Display Game Over screen
    public void GameOver()
    {
        gameOver = true;
        soundManagerScript.PlaySound(soundManagerScript.scream);
        tomatoControllerScript.playerControl.PlayerInputs.Disable();
        gameOverText.SetActive(true);
        restartButton.SetActive(true);
        mainMenuButton.SetActive(true);
    }

    // Display Game Won screen 
    public void GameWon()
    {
        gameOver = true;
        soundManagerScript.PlaySound(soundManagerScript.trumpet);
        tomatoControllerScript.playerControl.PlayerInputs.Disable();
        winText.SetActive(true);
        restartButton.SetActive(true);
        mainMenuButton.SetActive(true);
    }
}
