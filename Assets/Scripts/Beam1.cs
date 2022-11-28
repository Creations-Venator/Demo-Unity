using UnityEngine;

// =========================================
//
// Script du Rayon du joueur (qui fonctionne comme un projectile instantanné )
// Gère le sens et la durée du Rayon
// =========================================

public class Beam1 : MonoBehaviour
{
	public float BeamDuration = 0.2f;
	public SpriteRenderer spriteRenderer;
	
	void Start()
    {
        Destroy(gameObject,BeamDuration);
		
		if (PlayerMovement.instance.lookAtRight == true)
		{
			spriteRenderer.flipX = true;
		}
    }
	
}
