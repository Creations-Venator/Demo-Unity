using UnityEngine;
using UnityEngine.UI;

public class Ladder : MonoBehaviour
{   
	public BoxCollider2D topCollider;
	private bool isInRange;
	private PlayerMovement playerMovement;
	
	private Text interactUI;
	

    void Awake()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
		interactUI = GameObject.FindGameObjectWithTag("InteractUITag").GetComponent<Text>();
    }
    
    void Update()
    {
		if(isInRange && playerMovement.isClimbing && Input.GetKeyDown(KeyCode.E))
		{
			// descendre de l'echelle
			playerMovement.isClimbing = false;
			topCollider.isTrigger = false;
			//Debug.Log("test");
			// stop ici la lecture de cette fonction pour cette frame
			return;
		}
		
        if(isInRange && Input.GetKeyDown(KeyCode.E))
		{
			// la variable isClimbing est un bool PUBLIC du script PlayerMovement
			playerMovement.isClimbing = true;
			topCollider.isTrigger = true;
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
		// mettre aussi la condition ? if(collision.CompareTag("Player"))
		isInRange = false;
		playerMovement.isClimbing = false;
		topCollider.isTrigger = false;
		interactUI.enabled = false;
	}
}
