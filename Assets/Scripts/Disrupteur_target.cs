using UnityEngine;
using System.Collections;

// =========================================
//
// Script gérant la façon de cibler le joueur par les disrupteurs
//
// =========================================

public class Disrupteur_target : MonoBehaviour
{
    
	public GameObject waypoint1;
	public GameObject waypoint2;
	public GameObject waypoint3;
	public GameObject waypoint4;
	
	public GameObject disrupteur;
	public Animator animator;
	public AudioClip detectSound;
	
	private float timeToTarget = 0.8f;
	private float timeToNewFocus = 8f;
	
   	// Le joueur traversant la zone de détection du disrupteur se fera alors ciblé //
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.CompareTag("Player"))
		{				
			transform.position = disrupteur.transform.position + new Vector3(0,1,0);
			StartCoroutine(DelayToTarget());
			StartCoroutine(NewFocus());
		}
	}
	
	// Les waypoints dictant le déplacement de ce mob sont déplacés de manière à ce que le joueur risque de se faire toucher //
	private IEnumerator DelayToTarget()
	{
		yield return new WaitForSeconds(timeToTarget);
		waypoint4.transform.position = PlayerMovement.instance.playerCirclePosition.position + new Vector3(-7,5,0);		
		waypoint3.transform.position = PlayerMovement.instance.playerCirclePosition.position + new Vector3(-5f,0.8f,0f);
		waypoint2.transform.position = PlayerMovement.instance.playerCirclePosition.position + new Vector3(4f,0.8f,0f);
		waypoint1.transform.position = PlayerMovement.instance.playerCirclePosition.position + new Vector3(6,5,0);
		yield return new WaitForSeconds(timeToTarget);
		transform.position = PlayerMovement.instance.playerCirclePosition.position + new Vector3(0,4,0);
	}
	
	// permet de désactiver temporairement le ciblage pour suivre le patern d'attaque pendant plusieurs secondes. //
	private IEnumerator NewFocus()
	{
		AudioScript.instance.PlayClipAt(detectSound, transform.position);
		animator.SetBool("Bool",true);
		GetComponent<BoxCollider2D>().enabled = false;
		yield return new WaitForSeconds(timeToNewFocus);
		GetComponent<BoxCollider2D>().enabled = true;
		animator.SetBool("Bool",false);
	}
}
