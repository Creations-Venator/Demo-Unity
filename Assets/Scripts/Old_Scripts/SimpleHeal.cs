using UnityEngine;

public class SimpleHeal : MonoBehaviour
{
	public int healValue = 10;
	public AudioClip healSound;
	    
    private void OnTriggerEnter2D(Collider2D collision)
	{
			if (collision.CompareTag("Player"))
			{
				
				if(PlayerHealth.instance.currentHealth < PlayerHealth.instance.maxHealth)
				{	
					//public AudioClip healSound;
					AudioScript.instance.PlayClipAt(healSound, transform.position);
					
					
					PlayerHealth.instance.HealDamage(healValue);
					gameObject.SetActive(false);
					// Destroy(gameObject);
				}
		
				
			}
	}

    
}
