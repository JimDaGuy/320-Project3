using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainSceneManager : MonoBehaviour
{
    public enum GameStates { Ingame, Paused, Ended, Won };
    public GameStates currentState;
    
    public float controlsFadeDelay;
    public float controlsFadeDuration;
    public GameObject HUDCanvas;
    public GameObject deathCanvas;
    public GameObject winCanvas;
    public GameObject pauseCanvas;
    public GameObject compassPointer;
    public Vector3 compassRot;

    public GameObject[] lightOrbs;
    public GameObject[] monsters;

    public GameObject player;
    public UnityStandardAssets.Characters.FirstPerson.FirstPersonController controller;
    public float playerHealth;

    // Use this for initialization
    void Start()
    {
        currentState = GameStates.Ingame;
        Time.timeScale = 1;
        monsters = GameObject.FindGameObjectsWithTag("Monster");
        compassPointer = GameObject.FindGameObjectWithTag("CompassPointer");
    }

    // Update is called once per frame
    void Update()
    {
        //find all light orbs in the scene to update the light orb array
        lightOrbs = GameObject.FindGameObjectsWithTag("LightOrb");

        playerHealth = player.GetComponent<PlayerController>().health;

        switch (currentState)
        {
            case GameStates.Ingame:
                // Update compass position
                float playerRotation = player.transform.eulerAngles.y + 210f;
                compassRot = new Vector3(0, 0, playerRotation);
                compassPointer.transform.eulerAngles = compassRot;

                if (playerHealth == 10000)
                {
                    HUDCanvas.SetActive(false);
                    pauseCanvas.SetActive(false);

                    winCanvas.SetActive(true);

                    controller.enabled = false;

                    currentState = GameStates.Won;
                    Time.timeScale = 0;

                    // Turn off sound
                    AudioListener.volume = 0f;
                }

                // End the game when health reaches 0
                if (playerHealth <= 0)
                {
                    HUDCanvas.SetActive(false);
                    pauseCanvas.SetActive(false);

                    deathCanvas.SetActive(true);

                    controller.enabled = false;

                    currentState = GameStates.Ended;
                    Time.timeScale = 0;

                    // Turn off sound
                    AudioListener.volume = 0f;
                }

                // Pause game on P pressed
                if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
                {
                    currentState = GameStates.Paused;

                    controller.enabled = false;
                    Time.timeScale = 0;

                    // Enable pause canvas
                    pauseCanvas.SetActive(true);

                    // Enable Cursor
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;

                    // Turn off sound
                    AudioListener.volume = 0f;
                }

                break;
            case GameStates.Paused:
                // Unpause game on P pressed
                if (Input.GetKeyDown(KeyCode.P))
                {
                    // Set game state to ingame
                    currentState = GameStates.Ingame;

                    //Disable pause menu
                    pauseCanvas.SetActive(false);

                    //Enable controls
                    controller.enabled = true;
                    Time.timeScale = 1;

                    //Disable cursor
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Confined;

                    //Turn on sound
                    AudioListener.volume = 1.0f;
                }

                // Return to menu
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Time.timeScale = 1;
                    //Turn on sound
                    AudioListener.volume = 1.0f;
                    SceneManager.LoadScene(0);
                }

                // Restart Game
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    Time.timeScale = 1;
                    //Turn on sound
                    AudioListener.volume = 1.0f;
                    SceneManager.LoadScene(1);
                }
                break;
            case GameStates.Ended:
                // Return to main menu on escape pressed
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Time.timeScale = 1;

                    // Enable Cursor
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;

                    // Turn on sound
                    AudioListener.volume = 1.0f;

                    SceneManager.LoadScene(0);
                }

                // Restart Game
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    // Turn on sound
                    AudioListener.volume = 1.0f;

                    Time.timeScale = 1;
                    SceneManager.LoadScene(1);
                }
                break;
            case GameStates.Won:
                // Return to main menu on escape pressed
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Time.timeScale = 1;

                    // Enable Cursor
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;

                    // Turn on sound
                    AudioListener.volume = 1.0f;

                    SceneManager.LoadScene(0);
                }

                // Restart Game
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    // Turn on sound
                    AudioListener.volume = 1.0f;

                    Time.timeScale = 1;
                    SceneManager.LoadScene(1);
                }
                break;
            default:
                break;
        }
    }
}
