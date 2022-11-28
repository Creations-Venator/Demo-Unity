using UnityEngine;

// =========================================
//
// Script des projectiles du joueur
// (les dégâts effectués sur LosingHP)
// =========================================

public class GunShoot1 : MonoBehaviour
{
	public float speedProjectil;
	public float timeProjectil;
	public Animator animator;
	public SpriteRenderer spriteRenderer;
	public float damageAmount = 20;
	
	//  shootDirection = 1 lors d'un tir à gauche ou -1 à droite
	private int shootDirection = 1;
	
	void Start()
    {        
		// déstruction du projectile après la durée: timeProjectil //
        Destroy(gameObject,timeProjectil);
		
		if (PlayerMovement.instance.lookAtRight == true)
		{
			shootDirection = -1;
			spriteRenderer.flipX = true;
		}
    }
	
    void Update()
    {
		// déplacement du projectile //
		transform.Translate( new Vector3(shootDirection,0,0) * speedProjectil * Time.deltaTime, Space.World );
		
		//dir.normalized
    }
    
	private void OnTriggerEnter2D(Collider2D collision)
	{		
				
		// Gestion de l'impact //
		if( collision.CompareTag("MobTag") || collision.CompareTag("GroundTag")  )
		{
			//Debug.Log(collision);
			//Debug.Log(PlayerMovement.instance.playerCirclePosition.position);
			Destroy(gameObject,0.18f);			
			animator.SetTrigger("Impact");
			speedProjectil = 0;
		}
	}
}
