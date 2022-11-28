
using UnityEngine;

// =========================================
//
// Ce sript permet de faire perdre des HP au joueur qui entre en contact avec la scie circulaire
//
// =========================================

public class CircularSaw : MonoBehaviour
{
	public int sawDamageAmount = 20;
	public Transform spriteCircularSaw;
	public float rotateSpeed;
		
    void Update()
    {
        spriteCircularSaw.Rotate(new Vector3(0f,0f,-rotateSpeed), Space.Self);
    }
	
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if( collision.CompareTag("Player"))
		{
			PlayerHealth.instance.TakeDamage(sawDamageAmount);
		}
	}	
	
}
