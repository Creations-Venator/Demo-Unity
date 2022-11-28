using UnityEngine;

public class ItemsDataBase : MonoBehaviour
{
	public ConsommableItems[] allItems;
	
	
	public static ItemsDataBase instance;
		
	// se joue AVANT Start
	private void Awake()
	{
		if(instance != null)
		{
			Debug.LogWarning("Il y a plus d'une instance de ItemsDataBase dans la scène");
			return;
		}
		instance = this;
	}
	
    
}
