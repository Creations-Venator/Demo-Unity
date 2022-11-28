using System.Collections;
using UnityEngine;

// =========================================
//
// Ce Script gère les attaques possibles du joueur : Tirs, Rayon laser, Dash 
//  Il rectifie aussi des situations de bug si jamais le joueur traverse le sol ou les murs
// =========================================


public class PlayerStrikes : MonoBehaviour
{
	public Animator animator;	
	public Animator channelAnimator;
	
	public GameObject playerShoot1;
	public GameObject playerBeam1;
	public GameObject vfxChannel;
	public GameObject vfxDashChannel;
	private bool isChanneling = false;
	public bool usingBeam = false;
	
	public bool gunOnCooldown = false;
	public float gunCooldown = 0.5f;
	
	public AudioClip shoot1Sound;
	public AudioClip beam1Sound;
	
	public float damageShoot = 20f;
	public float damageBeam = 70f;
	
	private float laserPlacement;	
	private int beamChannelTicks = 0;
	public AudioClip beamReadySound;
	
	public float dashCooldown = 0;
	public bool dashOnCooldown = false;
	public float dashForce;
	public float dashDamage = 35f;
	public float dashDuration = 0.33f;	
	public bool isWallBlock = false;
	public bool isWallStuck = false;
	public bool isDashing = false;
	// -1 ou 1
	private int dashDir = 1 ; 
	// [SerializedField] c'est comme public mais pas utilisable par les autres scripts
	public TrailRenderer trail;	
	public LayerMask stuckLayers;
	private int dashChannelTicks = 0;
	public AudioClip dashSound;
	
	private Vector3 targetVelocity;
	
	//private Vector3 dashVelocity = Vector3.zero;
	
	//public PlayerEffects playerEffects;
	
	public static PlayerStrikes instance;
		
	// Singleton lié au joueur
	private void Awake()
	{
		if(instance != null)
		{
			Debug.LogWarning("Il y a plus d'une instance de PlayerStrikes dans la scène");
			return;
		}
		instance = this;
	}
	
	/*private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(PlayerMovement.instance.playerCirclePosition.position + new Vector3(0f,0.25f,0f), 0.35f);
		//Gizmos.DrawWireSphere(PlayerMovement.instance.playerCirclePosition.position + new Vector3(0f,0.06f,0f), 0.2f);
	}*/
    
    // Update is called once per frame
    void Update()
    {
		isWallBlock = Physics2D.OverlapCircle(PlayerMovement.instance.playerCirclePosition.position + new Vector3(0f,0.25f,0f), 0.35f, PlayerMovement.instance.collisionLayers);
		isWallStuck = Physics2D.OverlapCircle(PlayerMovement.instance.playerCirclePosition.position + new Vector3(0f,0.06f,0f), 0.2f, stuckLayers);
		
		if(isWallBlock && isDashing)
		{
			StopDashing();
			//Debug.Log("WALL");
			// stop l'élan du joueur
			PlayerMovement.instance.rb.velocity = new Vector2(0f,0f);			
		}
		if(isWallStuck)
		{			
			//Debug.Log("WasStuck");
			transform.Translate(new Vector2(0f,0.8f), Space.World );
		}
		
		// E pour Dash, à changer plus tard   .LeftShift  ?
		if(Input.GetKeyUp(KeyCode.E) && !dashOnCooldown && !isWallBlock )
		{			
			if (PlayerMovement.instance.lookAtRight)
			{
				dashDir = -1;
			}
			else
			{
				dashDir = 1;
			}			
			
			if(dashChannelTicks > 200)
			{
				dashDir = 2*dashDir;
			}
			
			//Vector3 targetVelocity = new Vector2(dashForce*dashDir, PlayerMovement.instance.rb.velocity.y);			
			//targetVelocity = new Vector2(dashForce*dashDir, 0f);
			
			// utiliser une max speed ?
			PlayerMovement.instance.rb.AddForce(new Vector2(dashForce*dashDir,0f));
						
			//PlayerMovement.instance.rb.velocity = Vector3.SmoothDamp(PlayerMovement.instance.rb.velocity,targetVelocity, ref PlayerMovement.instance.velocity, .001f);
			StartCoroutine(dashInvincibility());
		}
		if(Input.GetButton("Fire2") && !dashOnCooldown)
		{
			dashChannelTicks += 1;
			if( dashChannelTicks == 50 )
			{
				vfxDashChannel.SetActive(true);
			}
			if(dashChannelTicks == 200)
			{
				AudioScript.instance.PlayClipAt(beamReadySound, transform.position);
			}
		}
		
        // R pour attaquer : peut changer plus tard
		if(Input.GetKeyUp(KeyCode.R) && beamChannelTicks < 200 && gunOnCooldown == false )
		{	
			if (beamChannelTicks < 50)
			{
			StartCoroutine(GunAttack());
			animator.SetTrigger("UseShoot1");	
			Instantiate(playerShoot1, transform.position + new Vector3(0f,-0.1f,0f), Quaternion.identity);
			AudioScript.instance.PlayClipAt(shoot1Sound, transform.position);
			}			
			beamChannelTicks = 0;			
			vfxChannel.SetActive(false);
			isChanneling = false;
			animator.SetBool("IsChannel",isChanneling);
			
		}
		if(Input.GetButton("Fire1") && gunOnCooldown == false)
		{
			beamChannelTicks += 1;					
		}
		
		if (isChanneling == false && beamChannelTicks > 50 )
		{
			vfxChannel.SetActive(true);
			//vfxChannel.enabled = true;
			isChanneling = true;
			//animator.SetBool("IsChannel",isChanneling);
			
		}
		
		if( beamChannelTicks == 200)
		{
			channelAnimator.SetTrigger("Ready");
			AudioScript.instance.PlayClipAt(beamReadySound, transform.position);
		}
				
		if ( beamChannelTicks > 199 && Input.GetKeyUp(KeyCode.R))
			{	
				PlayerMovement.instance.rb.bodyType = RigidbodyType2D.Static;
				//PlayerMovement.instance.rb.gravityScale = 0.2f ;
				//GetComponent<BoxCollider2D>().enabled = false;
				
				animator.SetBool("IsChannel",isChanneling);
				vfxChannel.SetActive(false);
				isChanneling = false;
				if( PlayerMovement.instance.lookAtRight == false )
				{
					laserPlacement = 8.05f;
				}
				else
				{
					laserPlacement = -8.05f;
				}
				//playerEffects.AddSpeed(-230,0.5f);
				StartCoroutine(BeamAttack());
				animator.SetTrigger("UseShoot1");
				AudioScript.instance.PlayClipAt(beam1Sound, transform.position);
				//new GameObject beam = 
				Instantiate(playerBeam1, transform.position + new Vector3(laserPlacement,-0.2f,0f), Quaternion.identity);
				beamChannelTicks = 0;
				
			}
    }
// lien vers mon blog perso : https://creation-venator.blogspot.com/	
	private IEnumerator GunAttack()
	{	
		gunOnCooldown = true;
		yield return new WaitForSeconds(gunCooldown);
		gunOnCooldown = false;		
	}
	
	private IEnumerator BeamAttack()
	{	
		gunOnCooldown = true;
		yield return new WaitForSeconds(0.1f);
		// ne peut plus se déplacer pendant 0.4 sec
		usingBeam = true;
		yield return new WaitForSeconds(0.3f);
		gunOnCooldown = false;
		animator.SetBool("IsChannel",isChanneling);
		PlayerMovement.instance.rb.bodyType = RigidbodyType2D.Dynamic;
		usingBeam = false;
	}
	
	private IEnumerator dashInvincibility()
	{
		vfxDashChannel.SetActive(false);
		dashChannelTicks = 0;
		PlayerMovement.instance.playerCollider.isTrigger = true;
		PlayerMovement.instance.rb.gravityScale = 0.1f ;
		trail.emitting = true;
		isDashing = true;
		dashOnCooldown = true;
		animator.SetBool("IsDashing",true);
		AudioScript.instance.PlayClipAt(dashSound, transform.position);
		PlayerHealth.instance.isInvincible = true;
		//PlayerHealth.instance.graphics.color = new Color(0.1f,1.0f,0.5f,0.9f);
		yield return new WaitForSeconds(dashDuration);
		StopDashing();		
		
		yield return new WaitForSeconds(dashCooldown);
		dashOnCooldown = false;			
	}
	public void StopDashing()
	{
		//PlayerHealth.instance.graphics.color = new Color(1.0f,1.0f,1.0f,1.0f);
		PlayerMovement.instance.rb.gravityScale = 1f ;
		PlayerHealth.instance.isInvincible = false;
		animator.SetBool("IsDashing",false);
		trail.emitting = false;
		isDashing = false;
		PlayerMovement.instance.playerCollider.isTrigger = false;		
	}
	
}
