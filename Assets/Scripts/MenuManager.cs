//Name: David Sargent
//Date: 10-17-2026
//Desc: Presents menus attached to it for Handing over scenes between the build
//Attach: All Scenes GUIButtonHandler/GameManager
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    public GameObject pausePanel;
    private bool isPaused = false;
    private void Start()
    {
        //Make PausePanel invisible at start
        if(pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
    }
    private void Update()
    {
        //Check if P was pressed
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePausePanel();
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            QuitGame();
        }
    }
    public void PlayGame()
    {
        //Starting the MainLevel Scene
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainLevel");
    }
    public void LoadMainMenu()
    {
        //Returning to MainMenu Scene
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    public void LoadEndState()
    {
        //Calling the Endstate Scene
        Time.timeScale = 1f;
        SceneManager.LoadScene("EndState");
    }
    public void QuitGame()
    {
        //Quitting the Application
        Application.Quit();
        Debug.Log("Quitting Application");
    }
    public void TogglePausePanel()
    {
        //Bool checking and toggling the Pause Panel
        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);

        if (isPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
    public void HowToPlay()
    {
        //Switch to HowTo Scene
        SceneManager.LoadScene("HowTo");
    }
}
