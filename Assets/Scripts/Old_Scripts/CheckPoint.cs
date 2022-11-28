using UnityEngine;

public class CheckPoint : MonoBehaviour
{
	
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.CompareTag("Player"))
		{
			// transform.position est la position de l'object executant cette méthode.
			CurrentScene.instance.respawnPoint = transform.position;
			// pour ne garder que le checkpoint le plus récent, on peut détruire/désactiver les checkpoints ainsi :
			//Destroy(gameObject);
			// là on retire la box de collision par exemple :
			gameObject.GetComponent<BoxCollider2D>().enabled = false;
			
		}
	}
	
}
