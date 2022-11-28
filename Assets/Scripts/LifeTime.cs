using UnityEngine;

// =========================================
//
// Permet de supprimer un objet après un temps donné
// 
// =========================================


public class LifeTime : MonoBehaviour
{
	public float timeToLive;
	
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject,timeToLive);
    }

}
