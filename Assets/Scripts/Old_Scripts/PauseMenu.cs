//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
	
	public GameObject pauseMenuUI;
	
	public GameObject settingsWindow;
	
    void Update()
    {
		// Input.GetKeyDown(KeyCode.Escape)
        if(Input.GetButtonDown("Cancel") )
		{
			if(gameIsPaused)
			{
				Resume();
			}
			else
			{
				Paused();
			}
		}
    }
	
	void Paused()
	{
		// active la pause : stope le temps
		pauseMenuUI.SetActive(true);
		// Time.timeScale = 0.5;  pour ralentir le temps par 2 // ou 1.5  pour accelerer
		Time.timeScale = 0; 
		gameIsPaused = true;
		PlayerMovement.instance.enabled = false;
	}
	
	public void Resume()
	{
		pauseMenuUI.SetActive(false);
		Time.timeScale = 1; 
		gameIsPaused = false;	
		PlayerMovement.instance.enabled = true;		
	}
	
	public void LoadMainMenu()
	{
		
		Resume();
		SceneManager.LoadScene("MainMenu");
	}
	
	public void OpenSettingsWindow()
	{
		settingsWindow.SetActive(true);		
	}
	public void CloseSettingsWindow()
	{
		settingsWindow.SetActive(false);		
	}
	
}
