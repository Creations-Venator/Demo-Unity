using UnityEngine;

public class PickUpCoins : MonoBehaviour
{
	public AudioClip sound;
    
    private void OnTriggerEnter2D(Collider2D collision)
	{
			if (collision.CompareTag("Player"))
			{	
				AudioScript.instance.PlayClipAt(sound, transform.position);
				// audioSource.PlayOneShot(sound);   marche pas car se fait désactiver
				Inventory.instance.AddCoins(1);
				CurrentScene.instance.coinsCollectedInThisSceneCount++;
				gameObject.SetActive(false);
				// Destroy(gameObject);
				
				// creer un empty temporaire à l'endroit voulu qui joue le sound
				//AudioSource.PlayClipAtPoint(sound, transform.position);
				// bien mais on ne peut pas gérer les volumes multipes avec ça
			}
	}
	
}