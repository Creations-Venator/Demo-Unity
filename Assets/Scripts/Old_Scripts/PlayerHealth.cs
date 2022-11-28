using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    //public int maxHealth = 100;
	public int maxHealth;
	public int currentHealth;
	
	public HealthBar healthBar;
	
	public bool isInvincible = false;
	
	public SpriteRenderer graphics;
	public float invicibilityFlashTime;
	public float invicibilityTime;
	
	public Animator spellSystem;
	
	public AudioClip hitSound;
	
	public GameObject blueExplosion;
	
	public static PlayerHealth instance;
		
	// se joue AVANT Start, optimisé : évite les findGameObject
	private void Awake()
	{
		if(instance != null)
		{
			Debug.LogWarning("Il y a plus d'une instance de PlayerHealth dans la scène");
			return;
		}
		instance = this;
	}
	
    void Start()
    {
		// vraie valeur de PV :
        currentHealth = maxHealth;
		// afficher la valeur de PV :
		// la variable healthBar fait appel à la méthode SetMaxHealth du fichier HealthBar
		healthBar.SetMaxHealth(maxHealth);
		
		// animation test sur la touche H : FindGameObjectWithTag à chaque utilisation, c'est pas opti
		spellSystem = GameObject.FindGameObjectWithTag("SpellTag").GetComponent<Animator>();
    }

    
    void Update()
    {
		// utiliser la touche H pour perdre 20 HP :
        if(Input.GetKeyDown(KeyCode.H))
		{			
			//Vector3 impact = transform.position;
			Instantiate(blueExplosion, transform.position, Quaternion.identity);
			//spellSystem.SetTrigger("SpellTrigger");
			//TakeDamage(50);			
		}
    }
	
	public void TakeDamage(int damage)
	{
		if(!isInvincible)
			// alias : if(isInvincible == false)
		{
			
		currentHealth -= damage;
		// alias :
		// currentHealth = currentHealth - damage;
		
		// met à jour le visuel de la healthBar
		healthBar.SetHealth(currentHealth);
		
		AudioScript.instance.PlayClipAt(hitSound, transform.position);
		
		// on test si le J a encore des HP 
		if( currentHealth <= 0 )
		{
			Die();
			return;
		}
		
		isInvincible = true;
		StartCoroutine(InvincibilityFlash());
		StartCoroutine(InvincibilityDelay());
		
		}
	}
	
	public IEnumerator InvincibilityFlash()
	{
		while(isInvincible)
		{
			// RGB et Canal alpha pour la transparence
			graphics.color = new Color(1f,0.1f,0.1f,0.8f);
			yield return new WaitForSeconds(invicibilityFlashTime);
			graphics.color = new Color(1f,1f,1f,1f);
			yield return new WaitForSeconds(invicibilityFlashTime);
		}
	}
	
	public IEnumerator InvincibilityDelay()
	{
		yield return new WaitForSeconds(invicibilityTime);
		isInvincible = false;
	}
	
	public void HealDamage(int amount)
	{
		if( (currentHealth+amount) > maxHealth )
		{
			currentHealth = maxHealth;			
		}
		else
		{
			currentHealth += amount;
		}				
		// met à jour le visuel de la healthBar
		healthBar.SetHealth(currentHealth);
				
	}
	
	public void Die()
	{
		Debug.Log("Le joueur est MORT");
		// désactiver le script PlayerMovement:
		PlayerMovement.instance.enabled = false;
		PlayerMovement.instance.animator.SetTrigger("Death");
		// pour désactiver la physique du joueur :
		PlayerMovement.instance.rb.bodyType = RigidbodyType2D.Kinematic;
		PlayerMovement.instance.rb.velocity = Vector3.zero;
		PlayerMovement.instance.playerCollider.enabled = false;
		
		
		GameOverScript.instance.OnPLayerDeath();
		
	}
	
	public void Respawn()
	{
		// quasi l'inverse de Die
		PlayerMovement.instance.enabled = true;
		PlayerMovement.instance.animator.SetTrigger("Respawn");
		PlayerMovement.instance.rb.bodyType = RigidbodyType2D.Dynamic;
		PlayerMovement.instance.playerCollider.enabled = true;
		
		currentHealth = maxHealth;
		healthBar.SetHealth(currentHealth);
		
	}
	
}
