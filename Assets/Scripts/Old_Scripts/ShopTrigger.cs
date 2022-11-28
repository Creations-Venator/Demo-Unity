using UnityEngine;
using UnityEngine.UI;

public class ShopTrigger : MonoBehaviour
{
    private Text interactUI;
	private bool isInRange;
	
	public ConsommableItems[] itemsToSell;
	public string pnjName;
	
	void Awake()
    {
        interactUI = GameObject.FindGameObjectWithTag("InteractUITag").GetComponent<Text>();    
    }
	
    void Update()
    {
        if(isInRange && Input.GetKeyDown(KeyCode.E))
		{
			ShopScript.instance.OpenShop(itemsToSell,pnjName);
		}
    }
	
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.CompareTag("Player"))
		{
			isInRange = true;
			interactUI.enabled = true;
			
		}
	}
	
	private void OnTriggerExit2D(Collider2D collision)
	{	
		if(collision.CompareTag("Player"))
			{
			interactUI.enabled = false;
			isInRange = false;
			ShopScript.instance.CloseShop();
			}
	}
}
