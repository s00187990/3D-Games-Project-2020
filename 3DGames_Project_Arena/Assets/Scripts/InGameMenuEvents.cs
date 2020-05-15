using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenuEvents : MonoBehaviour
{
    
    
    public void Exit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
