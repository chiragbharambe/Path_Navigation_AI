using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void PlayGameNOW()
    {
    	SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
    	Debug.Log("QUIT!!!");
    	Application.Quit();
    }

     public void Back_b()
    {
    	SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

     public void Back_bl2()
    {
        SceneManager.LoadScene("LEVEL_SELECT");
    }
     public void Main_MenuGameOverScreen()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
