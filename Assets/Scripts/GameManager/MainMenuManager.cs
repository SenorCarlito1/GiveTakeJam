using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;

    public GameObject activeMenu;
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject creditsMenu;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        SceneManager.LoadScene("Jymeer");
    }

    public void SettingsMenu()
    {
        MainMenuManager.instance.activeMenu = settingsMenu;
        MainMenuManager.instance.activeMenu.SetActive(true);
        MainMenuManager.instance.mainMenu.SetActive(false);
       
    }
    

    public void Quit()
    {
        Application.Quit();
    }
}
