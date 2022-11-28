using UnityEngine;

// =========================================
//
// Script du Background, il applique un léger scrolling selon le déplacement du joueur
//
// =========================================

public class BackgroundScroll : MonoBehaviour
{
	[Range(-1f,1f)]
    public float scrollSpeed;
	private float offset;
	private Material mat;
	
	public GameObject laCamera;
	
	
    void Start()
    {
        mat = GetComponent<Renderer>().material;
		
    }

  
    void Update()
    {
		// + ou - 5.6
		scrollSpeed = PlayerMovement.instance.horizontalMovement /10f ;
		
		// si on ne divise pas pas 10, le scrolling sera trop rapide
        offset += (Time.deltaTime * scrollSpeed) / 10f ;
		mat.SetTextureOffset("_MainTex", new Vector2(offset, 0));		
		
		// suit le joueur :   
		transform.position = laCamera.transform.position + new Vector3(0,0,12);
    
    }
}
