using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
	
	public GameObject gameOverUI;
	
	public static GameOverScript instance;
		
	// se joue AVANT Start, optimisé : évite les findGameObject
	private void Awake()
	{
		if(instance != null)
		{
			Debug.LogWarning("Il y a plus d'une instance de GameOverScript dans la scène");
			return;
		}
		instance = this;
	}
    
   public void OnPLayerDeath()
    {
		//affiche l'UI
        gameOverUI.SetActive(true);
    }
	
	public void RetryButton()
	{	
		Inventory.instance.RemoveCoins(CurrentScene.instance.coinsCollectedInThisSceneCount);
		// recharger le niveau :
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		PlayerHealth.instance.Respawn();
		gameOverUI.SetActive(false);
	}
	public void MainMenuButton()
	{
		
		// la scene étant le menu prinpipal s'appelle "MainMenu"
		SceneManager.LoadScene("MainMenu");
	}
	public void QuitButton()
	{
		Application.Quit();
	}
    
}
