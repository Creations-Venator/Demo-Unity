using UnityEngine;

public class PickUpPowerJump : MonoBehaviour
{
	public AudioClip sound;
	public GameObject visualGetPowerJump;
    
    private void OnTriggerEnter2D(Collider2D collision)
	{
			if (collision.CompareTag("Player"))
			{	
				AudioScript.instance.PlayClipAt(sound, transform.position);
				PlayerMovement.instance.maxJump += 1;
				
				Instantiate(visualGetPowerJump, transform.position, Quaternion.identity);				
	
				//gameObject.SetActive(false);
				Destroy(gameObject);
			}
	}
	
}