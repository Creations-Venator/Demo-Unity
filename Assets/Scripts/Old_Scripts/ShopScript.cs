using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopScript : MonoBehaviour
{
	public Animator animator;
	public Text pnjNameText;
	
	public GameObject sellButtonPrefab;
	public Transform sellButtonsParent;
	
    public static ShopScript instance;
	private void Awake()
	{
		if(instance != null)
		{
			Debug.LogWarning("Il y a plus d'une instance de ShopScript dans la scène");
			return;
		}
		instance = this;
	}
	
	public void OpenShop(ConsommableItems[] items, string pnjName)
	{
		pnjNameText.text = pnjName;
		UpdateItemsToSell(items);
		animator.SetBool("IsOpen",true);
		
		/*if(items.Length <)
		{
			Debug.Log("test");
		}*/
		
	}
	
	void UpdateItemsToSell(ConsommableItems[] items)
	{	
		/*  génère un bug :
		// Supprime les anciens boutons présent dans le parent
		for(int j = 0; j < sellButtonsParent.childCount ; j++)
		{
			Destroy(sellButtonsParent.GetChild(j).gameObject);
		}*/
				
		for(int i =0; i< items.Length ; i++)
		{
			//Instantiate génère ce qu'on lui demande à l'endroit voulu
			GameObject button = Instantiate(sellButtonPrefab, sellButtonsParent);
			SellButton buttonScript = button.GetComponent<SellButton>();
			buttonScript.itemName.text = items[i].name;
			buttonScript.itemImage.sprite = items[i].image;
			buttonScript.itemPrice.text = items[i].price.ToString();
			
			buttonScript.item = items[i];
			
			// utilisation d'event unity pour UI dynamique
			button.GetComponent<Button>().onClick.AddListener(delegate{buttonScript.BuyItem(); } );
		}
		// supprimer le template ( l'exemple) ne marche pas, mais on peut le désacivter
		//Destroy(sellButtonsParent.GetChild(0).gameObject);
		//gameObject.SetActive(false);
		sellButtonsParent.GetChild(0).gameObject.SetActive(false);
		// Ok, mais pas opti car génère des boutons désacivtés à chaque fois qu'on parle au vendeur
	}
	
	
	public void CloseShop()
	{
		animator.SetBool("IsOpen",false);		
	}

        
}
