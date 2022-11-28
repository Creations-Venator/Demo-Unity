using System.Collections;
using UnityEngine;

// =========================================
//
// Script permettant au joueur d'utiliser la touche BAS pour passer au travers des plateformes dite "fine" 
// Le collider du joueur sera alors désactivé pendant 0.4 seconde
// =========================================

public class FallThrough : MonoBehaviour
{
    // platformCollider correspond à une hitbox/collider invisible et intangible au dessus de ces plateformes fines
	public Collider2D platformCollider;
	private bool playerOnPlatform = false;
	public Collider2D playerCollider = null;
	
    void Update()
    {
        if(Input.GetAxis("Vertical") < 0  && playerOnPlatform )
		{					
			playerOnPlatform = false;
			StartCoroutine(PassThrough());	
		}
    }
	
	private void OnTriggerEnter2D(Collider2D collision)
	{	
		if(collision.CompareTag("Player"))
		{
				playerOnPlatform = true;
				playerCollider = collision;
		}
	}
		
	public IEnumerator PassThrough()
	{		
		platformCollider.enabled = false;
		playerCollider.enabled = false;
		yield return new WaitForSeconds(0.4f);
		playerCollider.enabled = true;
		platformCollider.enabled = true;
	}
	
}
