using UnityEngine;

public class CurrentScene : MonoBehaviour
{	
	public int coinsCollectedInThisSceneCount;
	
	public Vector3 respawnPoint;
	public int levelToUnlock;
    
	public static CurrentScene instance;	
		
	// se joue AVANT Start, optimisé pour les scripts des objets présent en 1 seule fois.
	private void Awake()
	{
		if(instance != null)
		{
			Debug.LogWarning("Il y a plus d'une instance de CurrentScene dans la scène");
			return;
		}
		instance = this;
		
		
	respawnPoint = GameObject.FindGameObjectWithTag("Player").transform.position;
	}
    
	
	
}
