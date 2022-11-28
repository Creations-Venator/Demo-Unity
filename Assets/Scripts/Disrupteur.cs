using UnityEngine;
using System.Collections;

// =========================================
//
// Script principal du Disrupteur : gère la boucle de déplacement et le trigger des attaques
//
// =========================================

public class Disrupteur : MonoBehaviour
{
	// mettre en private
	public float speed;
	// array des zones où se déplacer :
	public Transform[] waypoints;	
	// zone cible actuelle du mob :
	private Transform target;
	// index du array :
	private int destPoints = 0;
	
	public SpriteRenderer graphics;
	
	//public int damageOnCollision = 20;	
	public AudioClip attackSound;
	public Animator attackAnimator;
	
	private bool inverse = false;
	private bool cooldown = true;
	
	// blueExplosion est le projectile utilisé par ce mob	
	public GameObject blueExplosion;
	public GameObject deathExplosion;
	
	public Animator animator;
		
    void Start()
    {
        target = waypoints[0];
    }

    void Update()
    {
       Vector3 dir = target.position - transform.position;
	   // normalized => vecteur de valeur 1
	   // Time.deltaTime => au fil du temps // Space.World : relatif au monde // Space.Self : relatif au parent
	   transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World );
	   
	   // distance à sa zone ciblée < 0.2 pour une meuilleur flexilibité 
	   if(Vector3.Distance(transform.position, target.position) < 0.2f && inverse == false)
	   {		   	   
		   if(destPoints +2 > waypoints.Length)
		   {
			   inverse = true;			   
			   destPoints = waypoints.Length-1;
		   }
		   else
		   {
			   destPoints += 1;  
		   }
		   target = waypoints[destPoints];
		   
	   }
	   // Changement de sens :
	   if(Vector3.Distance(transform.position, target.position) < 0.2f && inverse == true)
	   {  
		   
		   if(destPoints -1 < 0 )
		   {
			   inverse = false;			   
			   destPoints = 1;
		   }
		   else
		   {			   
				destPoints -= 1; 
		   }
		   target = waypoints[destPoints];
	    }	  

		// Attaque les joueurs assez proche :
		if(Vector3.Distance(transform.position + new Vector3(-0.4f,-0.3f,0f), PlayerMovement.instance.playerCirclePosition.position) < 1.2f && cooldown == true)
	   {
		   StartCoroutine(BlastAttack());
	   }
	   
    }
	
	private IEnumerator BlastAttack()
	{	
		cooldown = false;		
		attackAnimator.SetBool("isAttacking",true);
		yield return new WaitForSeconds(0.20f);
		Vector3 impact = transform.position + new Vector3(-0.4f,-1f,0f);
		// ajoute la prefab blueExplosion
		Instantiate(blueExplosion, impact, Quaternion.identity);
		yield return new WaitForSeconds(0.15f);
		attackAnimator.SetBool("isAttacking",false);
		yield return new WaitForSeconds(2.00f);
		cooldown = true;
	}
			
	
	// Dégats Avec collision trigger
	/*
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.CompareTag("Player"))
		{
			// reférence temporaire au script PlayerHealth
			//PlayerHealth playerHealth = collision.transform.GetComponent<PlayerHealth>();			
			//playerHealth.TakeDamage(damageOnCollision);
			//AudioScript.instance.PlayClipAt(attackSound, transform.position);
		}
	}*/
}