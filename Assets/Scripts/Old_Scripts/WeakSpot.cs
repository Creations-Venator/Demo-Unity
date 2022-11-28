using UnityEngine;

public class WeakSpot : MonoBehaviour
{
	public GameObject objectToDestroy;
	
	public AudioClip dieSound;
					
	
	// un des 2 objets en contact doit avoir isTrigger et un des 2 doit avoir un Rigidbody
    private void OnTriggerEnter2D(Collider2D collision)
	{
		// si collision par un objet ayant le Tag 'player'
		if(collision.CompareTag("Player"))
		{
			AudioScript.instance.PlayClipAt(dieSound, transform.position);
			
			// destroy le parent du weakspot, soit le mob
			// autre façon : Destroy(transform.parent.parent.gameObject);
			Destroy(objectToDestroy);
			
			Debug.Log("mob dead");
		}
	}
}
