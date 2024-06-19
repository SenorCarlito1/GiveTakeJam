using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public void resume()
    {
        GameManager.instance.unPauseState();
    }

    public void resetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
