using UnityEngine;

// =========================================
//
// Ce script est utilisé par les 2 lasers de scan des BeamMobs
// Il alerte de la présence d'un joueur
// =========================================


public class TargetingBeam : MonoBehaviour
{
	public bool targetFind = false;
	
    // utilisé par les rayons de ciblage
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if( collision.CompareTag("Player"))
		{
			targetFind = true;
		}
	}
	
	// faire une méthode en Stay2D ?
}
