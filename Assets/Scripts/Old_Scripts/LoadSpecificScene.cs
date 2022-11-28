// il faut utiliser ce module pour utiliser SceneManager
using UnityEngine.SceneManagement;
using UnityEngine;

// pour les coroutines
using System.Collections;

public class LoadSpecificScene : MonoBehaviour
{
	public string sceneName;
	
	private Animator fadeSystem;
	
	private void Awake()
	{
		
		fadeSystem = GameObject.FindGameObjectWithTag("FadeTag").GetComponent<Animator>();
	}
	
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
		{
			StartCoroutine(loadNextScene());							
			
		}
    }
	
	// coroutine :
	public IEnumerator loadNextScene()
	{
		LoadAndSaveData.instance.SaveData();
		
		// active le Trigger du nom de FadeIn (il sert de condition à la boucle d'animation pour le fade
		fadeSystem.SetTrigger("FadeIn");		
		yield return new WaitForSeconds(0.85f);
		
		// charge la scène du nom correspondant		
		SceneManager.LoadScene(sceneName);
		
	}

}
