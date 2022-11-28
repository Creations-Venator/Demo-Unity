using UnityEngine;
using System.Collections;

// =========================================
//
// Script commun aux mobs permettant de gérer les HP et les dégâts qu'ils subissent
// 
// =========================================


public class LosingHP : MonoBehaviour
{
		
	public bool isActivate = false;
	public float currentHP;	
	public Animator animator;
	public SpriteRenderer graphics;
	
	// pour éviter que certain mobs ne se blessent eux-même
	public BoxCollider2D selfCollision = null;
		
	public GameObject deathExplosion;	
	public GameObject parentToDestroy;
	
	public AudioClip dieSound;
	public AudioClip hurtedSound;
	
    // en cas de contact avec un projectil :
	private void OnTriggerEnter2D(Collider2D collision)
	{			
		// toujours détruit en cas de collision
		if(isActivate &&  collision.CompareTag("BulletTag"))
		{
			currentHP -= PlayerStrikes.instance.damageShoot;
			StartCoroutine(VisualTakeDamage());
		}
		if (isActivate &&  collision.CompareTag("BeamTag"))
		{
			currentHP -= PlayerStrikes.instance.damageBeam;
			StartCoroutine(VisualTakeDamage());
		}
		// firendly fire between mobs
		if (isActivate &&  collision.CompareTag("BulletBeamMobTag") && collision != selfCollision)
		{		
			currentHP -= 50;
			StartCoroutine(VisualTakeDamage());
		}
		if (isActivate && PlayerStrikes.instance.isDashing &&  collision.CompareTag("Player") && collision != selfCollision)
		{		
			currentHP -= PlayerStrikes.instance.dashDamage;
			StartCoroutine(VisualTakeDamage());
		}
	}
	
	private IEnumerator VisualTakeDamage()
	{	
		if (currentHP <= 0 )
			{
				isActivate = false;
				animator.SetTrigger("Die");				
				AudioScript.instance.PlayClipAt(dieSound, transform.position);
				yield return new WaitForSeconds(0.18f);
				Instantiate(deathExplosion, transform.position, Quaternion.identity);
				//yield return new WaitForSeconds(0.10f);					
				Destroy(parentToDestroy, 0.35f);
			}
		else
		{				
			AudioScript.instance.PlayClipAt(hurtedSound, transform.position);
			graphics.color = new Color(2f,0.01f,0.01f,0.9f);
			yield return new WaitForSeconds(0.2f);
			graphics.color = new Color(1f,1f,1f,1f);
		}
		
		
	}	
	
}
