using UnityEngine;

// =========================================
//
// Ce script détermine si le mob l'utilisant doit s'auto-détruire
//
// =========================================

public class AutoDestruction : MonoBehaviour
{
	private float timeOntheTop = 0f;
	public bool selfDestructionActivate = false;
	public float timeBeforeTrigger = 1f;
	
    private void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.transform.CompareTag("Player"))
		{
			timeOntheTop = timeOntheTop + Time.deltaTime;
			if(timeOntheTop > timeBeforeTrigger && !selfDestructionActivate)
			{
				selfDestructionActivate = true;
			}
		}
	}
}
