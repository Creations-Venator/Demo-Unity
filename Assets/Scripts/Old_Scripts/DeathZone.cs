using UnityEngine;
// pour les coroutines
using System.Collections;

public class DeathZone : MonoBehaviour
{
	
	
	public int damageWhenFall = 20;
	
	//private Transform playerSpawn;
	
	private Animator fadeSystem;
	
	private void Awake()
	{
		// FindGameObject est mauvais au niveau performance, donc on le place 1 seule fois dans une variable 'playerSpawn'
		//playerSpawn = GameObject.FindGameObjectWithTag("PlayerSpawnTag").transform;
		fadeSystem = GameObject.FindGameObjectWithTag("FadeTag").GetComponent<Animator>();
	}
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
		
        if(collision.CompareTag("Player"))
		{
			StartCoroutine(ReplacePlayer(collision));
			
		}
    }
	
	// coroutine : 
	private IEnumerator ReplacePlayer(Collider2D collision)
	{
			fadeSystem.SetTrigger("FadeIn");
			yield return new WaitForSeconds(0.85f);
			
			// tp le joueur à son point de spawn
			collision.transform.position = CurrentScene.instance.respawnPoint;
			
			// le joueur perd de la vie en tombant
			PlayerHealth playerHealth = collision.transform.GetComponent<PlayerHealth>();			
			playerHealth.TakeDamage(damageWhenFall);
		
	}
}
