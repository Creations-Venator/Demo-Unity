using System.Collections;
using UnityEngine;

// =========================================
//
//  Scrip expérimental
// 
// =========================================


// Singelton , ce script est dans le game Manager
// il permet de grouper des Coroutines

// ne marche pas pour le moment // le transformer en un générateur de boucle ?

public class MyDelay : MonoBehaviour
{
    public void WaitDelay(float delay)
	{
		StartCoroutine(Waiting(delay));
	}
	
	public IEnumerator Waiting(float _delay)
	{	
		yield return new WaitForSeconds(_delay);
	}
	
}
