using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    

    public void resetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.instance.unPauseState();
    }

    
}
