using UnityEngine;
using System.Linq;

public class LoadAndSaveData : MonoBehaviour
{
	public static LoadAndSaveData instance;
		
	// se joue AVANT Start
	private void Awake()
	{
		if(instance != null)
		{
			Debug.LogWarning("Il y a plus d'une instance de LoadAndSaveData dans la scène");
			return;
		}
		instance = this;
	}
	
    // Start is called before the first frame update
    void Start()
    {
		//PlayerPrefs.GetInt("butin", 0)  : 0 est la valeur par defaut
        Inventory.instance.coinsCount = PlayerPrefs.GetInt("butin", 0);
		Inventory.instance.UpdateTextUI();
		
		// var temporaire :
		/*int savedHealth = PlayerPrefs.GetInt("saveHP", PlayerHealth.instance.maxHealth );
		PlayerHealth.instance.currentHealth = savedHealth;
		PlayerHealth.instance.healthBar.SetHealth(savedHealth);*/
		int currentHealth = PlayerPrefs.GetInt("saveHP", 50 );
		PlayerHealth.instance.currentHealth = currentHealth;
		PlayerHealth.instance.healthBar.SetHealth(currentHealth);
		
		// RECHARGER LES ITEMS
		// recompense le string en une liste de caractères
		string[] itemsSaved = PlayerPrefs.GetString("Mon Stuff","").Split(',');
		for (int i = 0; i < itemsSaved.Length ;i++)
		{
			// évite le cas où itemsSaved est vide (qui bug)
			if(itemsSaved[i] != "")
			{
				// redonner les items 1 par 1 
			// la classe générale  des entiers int.  permet de faire Parse qui converti les string en int
			int myID = int.Parse(itemsSaved[i]);
			// requete linq Single : renvoit un unique id égal myID pour chaque 'x'
			ConsommableItems currentItem = ItemsDataBase.instance.allItems.Single(x => x.id == myID);
			// ajouter note ConsommableItems à notre inventaire :   Rappel : .Add pour ajouter un objet à une liste
			Inventory.instance.content.Add(currentItem);
			}
								
		}
		// maj le UI
		Inventory.instance.UpdateInventoryUI();
    }
	
	public void SaveData()
	{
		// PlayerPrefs est un outil de sauvegrade de unity
		PlayerPrefs.SetInt("butin",Inventory.instance.coinsCount);
		// SetInt(key : on choisit , valeur corrspondante) , SetFloat, SetString 

		// pour les HP : 
		PlayerPrefs.SetInt("saveHP",PlayerHealth.instance.currentHealth);
		
		if(CurrentScene.instance.levelToUnlock > PlayerPrefs.GetInt("progression", 1) )
		{
			PlayerPrefs.SetInt("progression",CurrentScene.instance.levelToUnlock);
		}
		
		// SAVE des ITEMS
		// string.  => class string pour modifier des chaines de caractères
		// cette ligne fait une string composé des ID des objets espacées par des " , "
		string itemsInInventory = string.Join(",",Inventory.instance.content.Select(x => x.id));
		
		PlayerPrefs.SetString("Mon Stuff", itemsInInventory);
		
		
		
	}

}
