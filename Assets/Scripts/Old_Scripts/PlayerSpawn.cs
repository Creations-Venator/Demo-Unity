using UnityEngine;

// avec MonoBehaviour, il faut garder le même nom entre le script et la class
public class PlayerSpawn : MonoBehaviour
{
    
    private void Awake()
    {
		// FindGameObjectsWithTag  avec un s à objects fonctionne pour plusieurs objets
        GameObject.FindGameObjectWithTag("Player").transform.position = transform.position;
		// transform.position revient aux positions de l'object ayant ce script

    }
    
}
