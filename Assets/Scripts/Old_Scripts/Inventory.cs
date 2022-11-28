using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    
    public int coinsCount;		
	
	public Text coinsCountText;
	
	// array mieux dans des cas de taille fixée et statique
	// Liste plus adapté à des cas dynamique et changeant
	public List<ConsommableItems> content = new List<ConsommableItems>();
	private int contentCurrentIndex = 0 ;
	public Image itemImageUI;
	public Sprite imageVide;
	public Text itemNameUI;
	
	public PlayerEffects playerEffects;
	
	
	public static Inventory instance;
		
	// se joue AVANT Start, permet de faire appel à inventory facilement et de n'importe où
	private void Awake()
	{
		if(instance != null)
		{
			Debug.LogWarning("Il y a plus d'une instance de Inventory dans la scène");
			return;
		}
		instance = this;
	}
	
	private void Start()
	{
		UpdateInventoryUI();
	}
	
	public void AddCoins(int count)
	{
		coinsCount += count;
		UpdateTextUI();
	}
	
	public void RemoveCoins(int count)
	{
		coinsCount -= count;		
		UpdateTextUI();
	}
	
	public void UpdateTextUI()
	{
		// maj l'interface : penser à passer le int en string
		coinsCountText.text = coinsCount.ToString();
	}
	
	public void ConsumeItem()
	{
		
		if(content.Count == 0 )
		{
			return;
		}
		ConsommableItems currentItem = content[contentCurrentIndex];
		PlayerHealth.instance.HealDamage(currentItem.hpGiven);
		//PlayerMovement.instance.moveSpeed += currentItem.speedGiven;
		playerEffects.AddSpeed(currentItem.speedGiven,currentItem.speedDuration);
		content.Remove(currentItem);
		GetNextItem();
		UpdateInventoryUI();
	}
	public void GetNextItem()
	{	
		if(content.Count == 0 )
		{
			return;
		}	
		contentCurrentIndex++;
		// retour au 1er element de la liste après le dernier :
		if(contentCurrentIndex > content.Count -1)
		{
			contentCurrentIndex = 0;
		}
		UpdateInventoryUI();
	}
	public void GetPreviousItem()
	{
		if(content.Count == 0 )
		{
			return;
		}
		contentCurrentIndex--;
		if(contentCurrentIndex < 0 )
		{
			contentCurrentIndex = content.Count -1;
		}
		UpdateInventoryUI();
	}
	public void UpdateInventoryUI()
	{	
		if(content.Count > 0 )
		{
			itemImageUI.sprite = content[contentCurrentIndex].image;
			itemNameUI.text = content[contentCurrentIndex].name;
		}
		else
		{
			// pas d'image si pas d'item dans l'inventory
			itemImageUI.sprite = imageVide;
			//Debug.Log("image vide");
			itemNameUI.text = "";
		}
	}

}
