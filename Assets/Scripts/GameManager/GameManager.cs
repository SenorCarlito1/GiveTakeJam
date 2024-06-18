using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    [Header("----Camera----")]
    public Camera firstPersonCamera;
    public Camera thirdPersonCamera;
    public Camera currCamera;
    private Camera[] allCameras;

    [Header("-----Player-----")]
    public GameObject player;


    [Header("-----UI-----")]
    public GameObject activeMenu;
    public GameObject pauseMenu;
    public Button resumeButton;
    public TextMeshProUGUI resumeText;

    public bool isPaused;
    float timeScaleOrig;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        timeScaleOrig = Time.timeScale;
        thirdPersonCamera = Camera.main;
        allCameras = Camera.allCameras;
        for (int i = 0; i < allCameras.Length; i++)
        {
            if (allCameras[i].CompareTag("FirstPersonCamera"))
            {
                firstPersonCamera = allCameras[i];
            }
        }
    }

    private void Start()
    {
        currCamera = thirdPersonCamera;
        firstPersonCamera.gameObject.SetActive(false);
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
        
        ChangePOV();
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

    private void ChangePOV()
    {
        if (Input.GetButtonDown("TogglePOV"))
        {
            //currCamera = firstPersonCamera;
            if (currCamera == thirdPersonCamera)
            {
                thirdPersonCamera.gameObject.SetActive(false);
                firstPersonCamera.gameObject.SetActive(true);
                currCamera = firstPersonCamera;
                Camera.SetupCurrent(currCamera);
            }
            else if (currCamera == firstPersonCamera)
            {
                thirdPersonCamera.gameObject.SetActive(true);
                firstPersonCamera.gameObject.SetActive(false);
                currCamera = thirdPersonCamera;
                Camera.SetupCurrent(currCamera);
            }
        }
    }
}
