//Name: David Sargent
//Date: 10-06-2026
//Desc: HowTo Handler, only return to main menu
//Attach HowToHandler for single button usage
using UnityEngine;
using UnityEngine.SceneManagement;
public class HowToHandler : MonoBehaviour
{
    //back goes back to MainMenu scene
    public GameObject menu;
    // Update is called once per frame
    void Update()
    {
       
    }
    public void returntoMain()
    {
        //return to mainmenu from HowTo
        DontDestroyOnLoad(this.gameObject);
        menu.SetActive(false);
        SceneManager.LoadScene("MainMenu");
    }
}
