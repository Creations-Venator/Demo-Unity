using System.Collections;
using UnityEngine;

// =========================================
//
// Ce script gère les mobs 'Platform' : Ses déplacements suivant 2 paterns ou 1 patern simple dans sa variante passive
//  Gère aussi le cilbage du joueur et le déclanchement de l'auto-destruction si le joueur reste dessus.
// =========================================


public class Mob_Plateform : MonoBehaviour
{
	public Animator animator;
    public GameObject triggerZone = null;
	public float WaitTime;
	public float speed;
	public Transform[] waypoints;	
	private Transform targetWaypoint;
	// index du array
	public int destPoints = 0;
	public Rigidbody2D rb;
	private Vector3 velocity = Vector3.zero;
	private Vector2 dir = Vector2.zero;
	private bool isWaiting = false;
	private bool isHostile = false;
	
	// si le mob n'a pas atteint sa destination avant cette durée :	
	public float toLate;
	private bool wallBlock = false;	
	public bool wallBlockDelay = false;
	
	public GameObject waypoint1;
	public GameObject waypoint2;
	public GameObject waypoint3;
	public GameObject waypoint4;	
	
	public SpriteRenderer reactorLeft;
	public SpriteRenderer reactorRight;
	private bool reactorIntensity = true;
	private bool canUpdateAltitude;
	
	public int patern = 1;
	
	public GameObject thePlatform;
	public GameObject theGlobalMobFolder;
	private bool isExplosing = false;
	public AudioClip bipSound;
	public AudioClip dieSound;
	public GameObject deathExplosion;
	
	// Ce Bool donne un patern simple à ce mob quand il restera toujours passif
	public bool stayPassif = false;
	
	public SpriteRenderer graphics;
	
	/*private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position + new Vector3(0f,-0.8f,0f), 0.4f);
	}*/
	
    void Start()
    {
        targetWaypoint = waypoints[0];
		//StartCoroutine(TestTimeAndTicks());
		VisualReactor();
				
		if(stayPassif)
		{
			
			reactorRight.color = new Color(1f,1f,1f,0f);			
			reactorLeft.color = new Color(1f,1f,1f,0f);
			StartCoroutine(VerticalMovements( ));
		}
    }

    // Update is called once per frame
    void Update()
    {
		if(thePlatform.GetComponent<AutoDestruction>().selfDestructionActivate == true && !isExplosing)
		{
			StartCoroutine(SelfDestruction(1.2f));
			isExplosing = true;
		}
		
		if(triggerZone.GetComponent<TriggerZone>().isTrigger && isHostile == false)
		{
			isHostile = true;
			TargetingPlayer();
			animator.SetTrigger("Hostile");
		}		
				
		//rb.velocity = Vector3.SmoothDamp(transform.position,new Vector2(0.1f,0.1f), ref velocity, 2.8f);
		if(!isWaiting && isHostile && !isExplosing)
		{
			dir = new Vector2(speed * (targetWaypoint.position.x - transform.position.x), speed * (targetWaypoint.position.y - transform.position.y) );
			rb.velocity = Vector3.SmoothDamp(rb.velocity,dir, ref velocity, 1.7f);
			wallBlock = Physics2D.OverlapCircle(transform.position + new Vector3(0f,-0.8f,0f), 0.4f, PlayerMovement.instance.collisionLayers);
		}			
		
		if(!wallBlockDelay && wallBlock && !isWaiting)
		{
			// replacer le prochain waypoint
			StartCoroutine(WaitDelay());
			NextAera();
			if(patern == 1 && canUpdateAltitude)
			{
			waypoint1.transform.position = transform.position + new Vector3(6f,0.1f,0f);
			waypoint2.transform.position = transform.position + new Vector3(-12f,0.1f,0f);
			waypoint3.transform.position = transform.position + new Vector3(9f,0.1f,0f);
			canUpdateAltitude = false;
			}
		}
		
		if(Vector3.Distance(transform.position, targetWaypoint.position) < 0.4f && !isWaiting)
	   {		   	   
		   NextAera();		   
	   }
    }
	
	private void NextAera()
	{
				
		//destPoints +2 > waypoints.Length
			if(destPoints == 3)
		   {
			   rb.velocity = new Vector2(0f,0f);
			   destPoints = 0;	
		       TargetingPlayer();
			   if(patern == 2)
				{
					canUpdateAltitude = true;
					rb.velocity = new Vector2(0f,0f);
				}
		   }		   
		   else
		   {
			   destPoints += 1; 
			   StartCoroutine(PauseAndWait());
			   if(patern == 2)
			   {
				   TargetingPlayer();
			   }
		   }
			   
		   targetWaypoint = waypoints[destPoints];
		   StartCoroutine(NextDestination(destPoints));
		   
			   graphics.flipX = !graphics.flipX;
			   VisualReactor();
	}
	
	
	private void TargetingPlayer()
	{
		if(patern == 1)
		{			
			waypoint1.transform.position = PlayerMovement.instance.playerCirclePosition.position + new Vector3(9f,-0.9f,0f);
			waypoint2.transform.position = PlayerMovement.instance.playerCirclePosition.position + new Vector3(-9f,-0.9f,0f);
			waypoint3.transform.position = PlayerMovement.instance.playerCirclePosition.position + new Vector3(12f,-0.9f,0f);
			canUpdateAltitude = true;
			waypoint4.transform.position = PlayerMovement.instance.playerCirclePosition.position + new Vector3(0,4,0);
			
		}
		if(patern == 2)
		{
			StartCoroutine(Parten2Delay());
		}
				
		
	}
	// parten 2 trop chaotique pour le moment
	private IEnumerator Parten2Delay()
	{
		wallBlockDelay = true;
		waypoint1.transform.position = PlayerMovement.instance.playerCirclePosition.position + new Vector3(0f,0f,0f);
		yield return new WaitForSeconds(1.5f);
		waypoint2.transform.position = PlayerMovement.instance.playerCirclePosition.position + new Vector3((PlayerMovement.instance.playerCirclePosition.position.x - transform.position.x)*1.3f,0.5f,0f);
		waypoint3.transform.position = PlayerMovement.instance.playerCirclePosition.position + new Vector3((PlayerMovement.instance.playerCirclePosition.position.x - transform.position.x)*1.3f ,4f,0f);
		waypoint4.transform.position = PlayerMovement.instance.playerCirclePosition.position + new Vector3(0,4,0);
		wallBlockDelay = false;
	}
	
	// Ce mob est bloqué par le sol, ça ne peut arriver qu'une fois toutes les 3 sec max
	private IEnumerator WaitDelay()
	{		
		wallBlockDelay = true;
		//rb.velocity = new Vector2(0f,0f);	
		yield return new WaitForSeconds(3);
		wallBlockDelay = false;
	}
	
	private IEnumerator PauseAndWait()
	{
		isWaiting = true;
		// stop l'élan
		rb.velocity = new Vector2(0f,0f);	
		yield return new WaitForSeconds(WaitTime);
		isWaiting = false;
	}
	private IEnumerator NextDestination(int oldDestPoints)
	{		
		yield return new WaitForSeconds(toLate);
		if(oldDestPoints == destPoints )
		{
			NextAera();
		}
	}
	
	private void VisualReactor()
	{
		if(reactorIntensity)
		{
			reactorLeft.color = new Color(1f,1f,1f,1f);
			reactorRight.color = new Color(1f,1f,1f,0f);
		}
		else
		{
			reactorLeft.color = new Color(1f,1f,1f, 0f);
			reactorRight.color = new Color(1f,1f,1f,1f);
		}
		reactorIntensity = !reactorIntensity;		
	}
	
	private IEnumerator SelfDestruction(float tictac)
	{			
		AudioScript.instance.PlayClipAt(bipSound, transform.position);
		graphics.color = new Color(2f,0.01f,0.01f,0.9f);
		yield return new WaitForSeconds(tictac/2);
		graphics.color = new Color(1f,1f,1f,1f);		
		if(tictac < 0.15f )
		{
			graphics.color = new Color(0.1f,0.1f,0.1f,1f);
			yield return new WaitForSeconds(0.06f);
			Instantiate(deathExplosion, transform.position, Quaternion.identity);
			AudioScript.instance.PlayClipAt(dieSound, transform.position);
			animator.SetTrigger("Die");
			yield return new WaitForSeconds(0.24f);
			Destroy(theGlobalMobFolder);
		}
		else
		{
		yield return new WaitForSeconds(tictac/2);
		StartCoroutine(SelfDestruction(tictac *0.7f ));	
		}
		
// lien vers mon blog : https://creation-venator.blogspot.com/
		
	}
	
	private IEnumerator VerticalMovements()
	{
		transform.Translate(new Vector2(0f,3f) * Time.deltaTime, Space.World );
		rb.velocity = new Vector2(0f,2f);		
		yield return new WaitForSeconds(3);
		rb.velocity = new Vector2(0f,0f);
		yield return new WaitForSeconds(4);
		rb.velocity = new Vector2(0f,-2f);
		yield return new WaitForSeconds(3);
		rb.velocity = new Vector2(0f,0f);
		yield return new WaitForSeconds(2);
		StartCoroutine(VerticalMovements( ));	
	}
	
		
}
