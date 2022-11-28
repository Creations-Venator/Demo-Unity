using UnityEngine;
using UnityEngine.UI;

public class SellButton : MonoBehaviour
{
    public Text itemName;
	public Image itemImage;
	public Text itemPrice;
	
	public ConsommableItems item;
	
	public void BuyItem()
	{
		// plutôt que mettre 'Inventory.instance. " partout, on fait une variable temporaire
		// c'est plus optimisé
		Inventory inventory = Inventory.instance;
		
		if(inventory.coinsCount >= item.price)
		{
			inventory.content.Add(item);
			inventory.UpdateInventoryUI();
			inventory.coinsCount  -= item.price;
			inventory.UpdateTextUI();
		}
	}
}
