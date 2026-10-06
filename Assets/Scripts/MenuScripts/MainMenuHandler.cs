//Name: David Sargent
//Date: 10-06-2026
//Desc: This is MainMenu Handler for using its buttons and such
//Attach: MainMenu.GUIButtonHandler

using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuHandler : MonoBehaviour
{
    //Reference itself
    public GameObject menu;
    //No pause in this script
    // Update is called once per frame
    void Update()
    {
        
    }
    public void loadGame()
    {
        //Switch our scenes to main game
        DontDestroyOnLoad(this.gameObject);
        //Scene is loaded
        menu.SetActive(false);
        //Load the leve
        SceneManager.LoadScene("MainLevel");
    }
    public void exitGame()
    {
        //Works on a full build
        Application.Quit();
        //For now, log it
        Debug.Log("Exiting Application. . .");
    }
    public void loadHowTo()
    {
        //Switch the scene to HowTo
        DontDestroyOnLoad (this.gameObject);
        //sceneLoaded = true;
        menu.SetActive(false);
        SceneManager.LoadScene("HowTo");
    }
}
