using System.Collections;
using UnityEngine;

// =========================================
//
// Gère l'attaque des BeamMobs. Doit être retravaillé.
// 
// =========================================


public class MobRay : MonoBehaviour
{
	public float speedProjectil;
	public float timeProjectil;
	public Animator animator;
	public SpriteRenderer spriteRenderer;
	public int damageAmount = 20;
	public float mobDamageAmount = 60;
	
	public GameObject gunParent;
	
	private int shootDirection = -1;
	private float currentSpeedProjectil = 0;
	
	public bool isFireLeft = false;
	public bool isFireRight = false;
	public Vector3 theAngle;
	public CapsuleCollider2D selfCollision;
	public GameObject impactExplosion;
		
	void Start()
    {
				
		//transform.Rotate(new Vector3(0f,0f,15f), Space.Self);
		/*
		if (PlayerMovement.instance.lookAtRight == true)
		{
			shootDirection = -1;
			spriteRenderer.flipX = true;
		}*/
    }
	
    void Update()
    {
		if(isFireLeft)
		{
			
			transform.position =  gunParent.transform.position + new Vector3(0f,-0.1f,0f);
			transform.Rotate(-theAngle, Space.Self);
			animator.SetBool("IsShooting",isFireLeft);
			currentSpeedProjectil = speedProjectil;
			theAngle = gunParent.GetComponent<BeamMob>().targetAngle;	
			transform.Rotate(theAngle, Space.Self);
			isFireLeft = false;
		}
		// certaines lignes sont ici dédoublée, à améliorer plus tard
		if(isFireRight)
		{			
			transform.position =  gunParent.transform.position + new Vector3(0f,-0.1f,0f);
			transform.Rotate(-theAngle, Space.Self);
			animator.SetBool("IsShooting",isFireRight);
			currentSpeedProjectil = speedProjectil;
			
			//Debug.Log(180f - gunParent.GetComponent<BeamMob>().targetAngle.z );			
			theAngle = new Vector3(0f,0f,180f - gunParent.GetComponent<BeamMob>().targetAngle.z );
			//theAngle = new Vector3(0f,0f,180f);
			transform.Rotate(theAngle, Space.Self);
			isFireRight = false;
		}
		
		transform.Translate( new Vector3(shootDirection,0,0) * currentSpeedProjectil * Time.deltaTime, Space.Self );
		
		
		//Vector3 dir = target.position - transform.position;
		//transform.Translate(dir.normalized * speedProjectil * Time.deltaTime, Space.World );
    }
    
	private void OnTriggerEnter2D(Collider2D collision)
	{
		//if( collision.CompareTag("MobTag") && collision != selfCollision ){Debug.Log("FriendlyFire");}					
			
		if( collision.CompareTag("Player"))
			{
				//Debug.Log("Pouf");
				PlayerHealth playerHealth = collision.transform.GetComponent<PlayerHealth>();			
				playerHealth.TakeDamage(damageAmount);
				//AudioScript.instance.PlayClipAt(attackSound, transform.position);
			}
				
		// toujours détruit en cas de collision
		if( collision.CompareTag("GroundTag")  )
		{
			StartCoroutine(HitTheGround());	
			
		}
	}
	
	public IEnumerator HitTheGround()
	{		
		yield return new WaitForSeconds(0.12f);
		Instantiate(impactExplosion, transform.position, Quaternion.identity);
			//animator.SetTrigger("Impact");   // + new Vector3(0f,-0.1f,0f)
		currentSpeedProjectil = 0;
		transform.position =  gunParent.transform.position ;
		animator.SetBool("IsShooting",false);
		gameObject.SetActive(false);
	}
}
