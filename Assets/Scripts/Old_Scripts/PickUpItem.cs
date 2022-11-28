using UnityEngine;
using UnityEngine.UI;

public class PickUpItem : MonoBehaviour
{
	private Text interactUI;
	private bool isInRange;
	
	//public Animator animator;
	public AudioClip soundToPLay;
	
	public ConsommableItems item;
    
    void Awake()
    {
        interactUI = GameObject.FindGameObjectWithTag("InteractUITag").GetComponent<Text>();
    
    }

    void Update()
    {
        if(isInRange && Input.GetKeyDown(KeyCode.E))
		{
			TakeItem();
		}
    }
	
	void TakeItem()
	{
		// .Add utilisée par Unity pour les listes /
		Inventory.instance.content.Add(item);
		Inventory.instance.UpdateInventoryUI();
		AudioScript.instance.PlayClipAt(soundToPLay, transform.position);
		interactUI.enabled = false;
		Destroy(gameObject);
		
		
	}
	
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.CompareTag("Player"))
		{
			interactUI.enabled = true;
			isInRange = true;
		}
	}
	
	private void OnTriggerExit2D(Collider2D collision)
	{	
		if(collision.CompareTag("Player"))
			{
			interactUI.enabled = false;
			isInRange = false;
			}
	}	
	
	
}


