using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour
{
	private Text interactUI;
	private bool isInRange;
	
	public Animator animator;
	public int coinsToAdd;
	public AudioClip openChestSound;
    
    void Awake()
    {
        interactUI = GameObject.FindGameObjectWithTag("InteractUITag").GetComponent<Text>();
    
    }

    void Update()
    {
		// old : Input.GetKeyDown(KeyCode.I)
        if(isInRange && Input.GetButtonDown("Interact"))
		{
			OpenChest();
		}
    }
	
	void OpenChest()
	{
		animator.SetTrigger("OpenChest");
		Inventory.instance.AddCoins(coinsToAdd);
		AudioScript.instance.PlayClipAt(openChestSound, transform.position);
		// on désactive la hitbox du coffre : 
		GetComponent<BoxCollider2D>().enabled = false;
		interactUI.enabled = false;
		
		
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
