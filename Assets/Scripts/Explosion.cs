using UnityEngine;
using System.Collections;

// =========================================
//
// Script utilisé par les Explosions infligant des dégâts
// 
// =========================================

public class Explosion : MonoBehaviour
{
	public int damageExplosion = 20;	
	public AudioClip attackSound;
	public float timeExplosion = 0.35f;
    
    
    void Start()
    {
        Destroy(gameObject,timeExplosion);
		
		//  + faire que l'explosion inflige le double à courte distance
    }
	
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.CompareTag("Player"))
		{
			PlayerHealth playerHealth = collision.transform.GetComponent<PlayerHealth>();			
			playerHealth.TakeDamage(damageExplosion);
			AudioScript.instance.PlayClipAt(attackSound, transform.position);
		}
	}
	
}
