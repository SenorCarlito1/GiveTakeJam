using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject MMConfirmMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void resume()
    {
        GameManager.instance.unPauseState();
    }

    public void openSettings()
    {
        GameManager.instance.activeMenu.SetActive(false);
        GameManager.instance.activeMenu = null;
        GameManager.instance.activeMenu = GameManager.instance.settingsMenu;
        GameManager.instance.activeMenu.SetActive(true);

    }

    public void MMConfirmation()
    {
        GameManager.instance.activeMenu.SetActive(false);
        GameManager.instance.activeMenu = null;
        GameManager.instance.activeMenu = MMConfirmMenu;
        GameManager.instance.activeMenu.SetActive(true);
    }

    public void MMYes()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void MMNo()
    {
        GameManager.instance.activeMenu.SetActive(false);
        GameManager.instance.activeMenu = null;
        GameManager.instance.activeMenu = GameManager.instance.pauseMenu;
        GameManager.instance.activeMenu.SetActive(true);
    }
}
