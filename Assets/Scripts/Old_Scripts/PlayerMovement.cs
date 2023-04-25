using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
	public float moveSpeed = 100;
	// le corps qui va subir des forces
	public Rigidbody2D rb;	
	// Vector3 pour 3D meme pour ce jeu 2d
	public Vector3 velocity = Vector3.zero;
	
	public bool isJumping = false;
	public float jumpForce;
	
	// variable pour savoir sur le joueur touche le sol
	public bool isGrounded = false;
	
	public Animator animator;
	public SpriteRenderer spriteRenderer;
	
		
	public float horizontalMovement;
	public Transform playerCirclePosition;
	public float groundCheckRadius;
	public LayerMask collisionLayers;
	//public LayerMask fallThroughLayers;
	
	public bool isClimbing;	
	private float verticalMovement;	
	public float climbSpeed = 70;	
	
	// utile pour donner le sens des tirs du joueurs
	public bool lookAtRight = true;
	
	//public float normalGravity = 1f;
	
	// utilisé dans PlayerHealth
	public CapsuleCollider2D playerCollider;
	
	// nombre actuel de jump sans toucher le sol private
	public int jumpNumber = 1;
	// nombre max de jump sans toucher le sol
	public int maxJump = 1;
	private bool tryToGrounded = true;
		
	
	public static PlayerMovement instance;
		
	// se joue AVANT Start, optimisé : évite les findGameObject
	private void Awake()
	{
		if(instance != null)
		{
			Debug.LogWarning("Il y a plus d'une instance de PlayerMovement dans la scène");
			return;
		}
		instance = this;
		
		//rb.gravityScale = normalGravity;
	}
	
    

    // Update is called once per frame
	void Update()
	{	
		if (rb.bodyType == RigidbodyType2D.Dynamic)
		{
		//new à mettre dans fixedupdate ?
		isGrounded = Physics2D.OverlapCircle(playerCirclePosition.position, groundCheckRadius, collisionLayers);		
		//animator.SetBool("IsGround",isGrounded);
		
		
		if(isGrounded && tryToGrounded && jumpNumber != maxJump )
		{		
			tryToGrounded = false;
			StartCoroutine(WaitToJump());
		}
		
		if(Input.GetButtonDown("Jump") && !isGrounded && jumpNumber > 0 )
		{			
			isJumping = true;
			jumpNumber -= 1;		
			animator.SetTrigger("UseJump");
		}		
		
		// jump est reconnu par unity sur la touche espace          // && !isClimbing  pour bloquer l'action quand il grimpe à une echelle 
		else if(Input.GetButtonDown("Jump") && isGrounded )
		{			
			isJumping = true;
			jumpNumber -= 1;	
			animator.SetTrigger("UseJump");
		}
		// pourquoi il y a 2 fois le même morceau de code avec isGrounded et pas uniquement jumpNumber > 0 ? pour pouvoir toujours jump après avoir touché le sol => meuilleure fluidité
		
		
			MovePlayer(horizontalMovement,verticalMovement);
		}
		
		
		
				
		//if(isWallStuck)
			//PlayerStrikes.instance.StopDashing();
			//rb.AddForce(new Vector2(0f,50));		
		
		// bool qui permet de savoir si le joueur peut passer à travers le sol
		//canFallThrough = Physics2D.OverlapCircle(playerCirclePosition.position, groundCheckRadius, fallThroughLayers);
				
		
	}
	
    void FixedUpdate()
    {	
				
		// calculer la direction et la vitesse du Joueur    // on peut aussi utiliser : Time.deltaTime   mais c'est moins bien  // Time.fixedDeltaTime = 0.02 pour moi
        horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed * Time.fixedDeltaTime;
		verticalMovement = Input.GetAxis("Vertical") * climbSpeed * Time.fixedDeltaTime;
		
		// Appliquer une force au joeur pour son déplacement		
			
		Flip(rb.velocity.x);
		
		// variable temporaire permettant d'avoir la valeur absolue de la vitesse du perso
		float characterVelocity = Mathf.Abs(rb.velocity.x);
		// Axe horizontal, soit l'axe X
		animator.SetFloat("Speed",characterVelocity);
		// Speed est compris par unity, on envoit cette donnée 
		
		// "IsClimbing" = nom de la condition dans l'animator, et isClimbing bool déjà défini
		animator.SetBool("IsClimbing",isClimbing);
		
    }
	
	// le '_' indique ici un paramètre à la fonction
	void MovePlayer(float _horizontalMovement, float _verticalMovement)
	{	
		if(isClimbing == false )
		{			
			
			// la valeur en Y n'est pas modifiée ou impactée par le déplacement horizontal du joueur		
			Vector3 targetVelocity = new Vector2(_horizontalMovement, rb.velocity.y);
			// Attribution du mouvement, SmoothDamp permet un 'fondu' , un aspect progressif du mouvement
			// transition entre la velocity du rb actuel vers la velocity visée
			rb.velocity = Vector3.SmoothDamp(rb.velocity,targetVelocity, ref velocity, .05f);
			// le 0.05f correspond au temps de transition en sec
		
			if(isJumping == true )
			{
				rb.AddForce(new Vector2(0f,jumpForce));
				isJumping = false;				
				
			}
			
			
		}
		// déplacement vertical lors d'un échelle
		else
		{			
			Vector3 targetVelocity = new Vector2(0,_verticalMovement);
			rb.velocity = Vector3.SmoothDamp(rb.velocity,targetVelocity, ref velocity, .05f);
			
		}
		
	}
		
	void Flip(float _velocity)
	{
		if ( _velocity > 0.1f ) 
		{
			spriteRenderer.flipX = false;
			lookAtRight = false;
		}else if( _velocity < -0.1f)
		{
			// flipX est l'option flip axe X d'unity
			spriteRenderer.flipX = true;
			lookAtRight = true;
		}
	}	
	
	// info visuel visible sur Unity
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(playerCirclePosition.position, groundCheckRadius);
	}
	
	private IEnumerator WaitToJump()
	{	
		yield return new WaitForSeconds(0.1f);
		if(isGrounded)
		{			
			// reset le nombre max de jump si le joueur touche le sol
			jumpNumber = maxJump;
			//Debug.Log("au sol");
		}
		
		tryToGrounded = true;
	}
	
}
