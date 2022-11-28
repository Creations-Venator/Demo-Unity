using UnityEngine;
using System.Collections;

// =========================================
//
// Script principal du 'BeamMob' : gère l'activation ( passe d'un simple props à un mob hostile) , Les Scan vers le bas par la rotation des lasers,
// le changement de sens de façon à viser vers le joueur, le déclanchement d'un tir et le changement entre le mod Scan ou le tir de base.
// =========================================

public class BeamMob : MonoBehaviour
{
	public GameObject triggerZone;
	private bool isActivate = false;
	
	public float timeToStart;
	public float timeToSwitch;
	public Animator animator;
	public SpriteRenderer graphics;
	public SpriteRenderer graphicsBeamRight;
	public SpriteRenderer graphicsBeamLeft;
	public Rigidbody2D rb;
	private Vector3 velocity = Vector3.zero;
	
	public GameObject laserTargetLeft;
	public GameObject laserTargetRight;
	private int scanPhase = 7;
	public bool startScanAtLeft = true;
	
	public float scanSpeed = 0.1f;
	public float cooldownDuration = 1.6f;
	public Transform angleScanLeft;
	public int maxAngleScan = 60;
		
	public float moveSpeed = 2f;
	
	private bool channelAttack = false;
	// ne scan que si sur-élevé par rapport au joueur
	private bool targetHorizontal = false;
	
	private float horizontalFocus = 0f;
	
	public GameObject deadlyProjectil;
	
	public Vector3 targetAngle;
	
	//private bool cooldown = true;
		
	
	void Update()
    {
		laserTargetLeft.transform.position = gameObject.transform.position +new Vector3(0f,-0.1f,0f);
		laserTargetRight.transform.position = gameObject.transform.position +new Vector3(0f,-0.1f,0f);
		
		
		// variable temporaire permettant d'avoir la valeur absolue de la vitesse du mob
		float mobVelocity = Mathf.Abs(rb.velocity.x);
		animator.SetFloat("Speed",mobVelocity);
		
				// attaque horizontal
		if(isActivate && !channelAttack && targetHorizontal )
		{
			
			laserTargetRight.transform.Rotate(new Vector3(0f,0f, + laserTargetLeft.transform.rotation.eulerAngles.z), Space.Self);
			laserTargetLeft.transform.Rotate(new Vector3(0f,0f,- laserTargetLeft.transform.rotation.eulerAngles.z), Space.Self);
			
			// si le joueur est à gauche ou à droite
			if(gameObject.transform.position.x > PlayerMovement.instance.playerCirclePosition.position.x)
			{
				laserTargetLeft.SetActive(true);
				laserTargetRight.SetActive(false);				
				graphics.flipX = true;
			}
			else
			{
				laserTargetLeft.SetActive(false);
				laserTargetRight.SetActive(true);
				graphics.flipX = false;
			}
			
			SwitchScanMod();
		}
		
		// si le scan de droite ou gauche trouve sa cible : 
		if(laserTargetLeft.GetComponent<TargetingBeam>().targetFind)
		{
			if(!targetHorizontal)
			{
				laserTargetLeft.GetComponent<TargetingBeam>().targetFind = false;
				// true pour Left et false pour Right
				StartCoroutine(TimeToShoot(true));
			}
			// tirer à l'horizontal : laser qui devient de + en + visible
			else
			{
				
				horizontalFocus +=0.005f;
				graphicsBeamLeft.color = new Color(1f,1f,1f,horizontalFocus);
				if(horizontalFocus > 0.95f && !channelAttack )
				{
					horizontalFocus = 0f;
					laserTargetLeft.GetComponent<TargetingBeam>().targetFind = false;
					StartCoroutine(TimeToShoot(true));
					
				}
				
			}
			
		}
		
		if(laserTargetRight.GetComponent<TargetingBeam>().targetFind)
		{		
			if(!targetHorizontal)
			{
				laserTargetRight.GetComponent<TargetingBeam>().targetFind = false;
				StartCoroutine(TimeToShoot(false));
			}
			
			else
			{
				horizontalFocus +=0.005f;
				graphicsBeamRight.color = new Color(1f,1f,1f,horizontalFocus);
				if(horizontalFocus > 0.95f && !channelAttack)
				{
					horizontalFocus = 0f;
					laserTargetRight.GetComponent<TargetingBeam>().targetFind = false;
					StartCoroutine(TimeToShoot(false));					
				}
			}
			
		}
		
		// initie le mob 
		if(triggerZone.GetComponent<TriggerZone>().isTrigger && isActivate == false)
		{
			isActivate = true;
			//bool isActivate = triggerZone.GetComponent<TriggerZone>().isTrigger;
			StartCoroutine(InitiateTime());	
		}
		
		// SCAN des cibles 
		if(isActivate && !channelAttack && !targetHorizontal )
		{
			laserTargetLeft.transform.Rotate(new Vector3(0f,0f,scanSpeed), Space.Self);
			//print(angleScan.rotation.eulerAngles.z);
			laserTargetRight.transform.Rotate(new Vector3(0f,0f,-scanSpeed), Space.Self);
						
			if ( maxAngleScan < angleScanLeft.rotation.eulerAngles.z)
			{
				scanSpeed = -scanSpeed;
				scanPhase += 1;
				SwitchScanMod();				
			}
			
			if (scanPhase == 3 )
			{
				graphics.flipX = false;
				laserTargetLeft.SetActive(false);
				laserTargetRight.SetActive(true);
				scanPhase += 1;
			}
			if (scanPhase == 6 )
			{
				graphics.flipX = true;
				laserTargetLeft.SetActive(true);
				laserTargetRight.SetActive(false);
				scanPhase = 1;
			}			
		}
	}
	
	public IEnumerator TimeToShoot(bool targetAtTheLeft)
	{
		yield return new WaitForSeconds(0.05f);
		channelAttack = true;
		animator.SetBool("Fire",channelAttack);
		targetAngle = angleScanLeft.rotation.eulerAngles;
		deadlyProjectil.SetActive(true);
		//print("FIRE!");
		if(targetAtTheLeft)
		{
			deadlyProjectil.GetComponent<MobRay>().isFireLeft = true;
			yield return new WaitForSeconds(0.1f);
			laserTargetLeft.SetActive(false);
			Movement(moveSpeed,0);
			yield return new WaitForSeconds(cooldownDuration);			
			laserTargetLeft.SetActive(true);
			channelAttack = false;
			animator.SetBool("Fire",channelAttack);
			
		}
		else
		{
			deadlyProjectil.GetComponent<MobRay>().isFireRight = true;
			yield return new WaitForSeconds(0.1f);
			laserTargetRight.SetActive(false);			
			Movement(-1*moveSpeed,0);
			yield return new WaitForSeconds(cooldownDuration);			
			laserTargetRight.SetActive(true);
			channelAttack = false;	
			animator.SetBool("Fire",channelAttack);
		}		
		
	}
	
	public IEnumerator InitiateTime()
	{	
		rb.bodyType = RigidbodyType2D.Dynamic;
		GetComponent<CapsuleCollider2D>().enabled = true;
		
		GetComponent<LosingHP>().isActivate = true;
				
		triggerZone.SetActive(false);	
		
		animator.SetTrigger("Transformed");
		graphics.color = new Color(0.9f,0.9f,0.9f,0.9f);
		yield return new WaitForSeconds(timeToStart);
		graphics.color = new Color(1f,1f,1f,1f);
		yield return new WaitForSeconds(0.4f);
		
	// mettre à 3 pour avoir un 1er scan à droite et 6 pour gauche
		if (startScanAtLeft)
		{
			scanPhase = 6;
		}
		else
		{
			scanPhase = 3;
		}
		
	}
		
	private void SwitchScanMod()
	{
		if( gameObject.transform.position.y -1 < PlayerMovement.instance.playerCirclePosition.position.y )
		{
			targetHorizontal = true;
			graphicsBeamRight.color = new Color(1f,1f,1f,0.2f);
			graphicsBeamLeft.color = new Color(1f,1f,1f,0.2f);
		}
		else
		{
			targetHorizontal = false;
			graphicsBeamRight.color = new Color(1f,1f,1f,1f);
			graphicsBeamLeft.color = new Color(1f,1f,1f,1f);
		}
	}
	
	// entre 5 et -5  il sera plutôt lent, donc -2 et 2   moveSpeed
	void Movement(float _horizontalMovement, float _verticalMovement)
	{
				if(targetHorizontal)
				{
				
					Vector3 targetVelocity = new Vector2(_horizontalMovement, rb.velocity.y);
				// transition entre la velocity du rb actuel vers la velocity visée
					rb.velocity = Vector3.SmoothDamp(rb.velocity,targetVelocity, ref velocity, .01f);
				// le 0.1f correspond au temps de transition en sec
				}	
		
	}
		
}
	