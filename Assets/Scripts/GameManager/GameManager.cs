using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    

    [Header("-----Player-----")]
    public GameObject player;

    [Header("-----UI-----")]
    public GameObject activeMenu;
    public GameObject pauseMenu;

    public bool isPaused;
    float timeScaleOrig;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        timeScaleOrig = Time.timeScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            if(activeMenu == null)
            {
                isPaused = !isPaused;
                activeMenu = pauseMenu;
                activeMenu.SetActive(isPaused);
                pauseState();
            }
            else if(activeMenu != null && activeMenu == pauseMenu)
            {
                unPauseState();
            }
        }
    }

    public void pauseState()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void unPauseState()
    {
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = !isPaused;
        activeMenu.SetActive(false);
        activeMenu = null;
    }
}
