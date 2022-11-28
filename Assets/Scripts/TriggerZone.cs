using UnityEngine;

// =========================================
//
// Script polyvalent de Zone invisible pouvant déclancher une action quand un joueur la traverse
// 
// =========================================

public class TriggerZone : MonoBehaviour
{
    public GameObject targetToTrigger;
	public bool isTrigger = false;
	
    private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.CompareTag("Player") && isTrigger == false)
		{
			isTrigger = true;
		}
	}
}
