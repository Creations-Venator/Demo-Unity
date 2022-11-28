using UnityEngine;

// =========================================
//
// Permet de mieux synchroniser un joueur sur une plateforme en mouvement :
// Efficace pour les mouvements Verticaux uniquement (pour le moment) 
// =========================================


public class StayOnPlatform : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.transform.CompareTag("Player"))
		{
		collision.transform.SetParent(transform);
		//print("On");
		}
		
	}
	private void OnCollisionExit2D(Collision2D collision)
	{		
	if (collision.transform.CompareTag("Player"))
		{
		collision.transform.SetParent(null);
		//print("Off");
		}
		
	}
	
}
