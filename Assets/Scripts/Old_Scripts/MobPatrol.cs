using UnityEngine;

public class MobPatrol : MonoBehaviour
{
	public float speed;
	// array : les zones où se déplacer ( liste de points )
	public Transform[] waypoints;	
	// zone cible actuelle du mob
	private Transform target;
	// index du array
	private int destPoints = 0;
	
	public SpriteRenderer graphics;
	
	public int damageOnCollision = 20;
	
	
    void Start()
    {
        target = waypoints[0];
		
    }

    void Update()
    {
       Vector3 dir = target.position - transform.position;
	   // Simple Translation sans rigid body ni force
	   // normalized => vecteur de valeur 1
	   // Time.deltaTime => au fil du temps // Space.World : relatif au monde // Space.Self : relatif au parent
	   transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World );
	   
	   // distance à sa zone ciblée < 0.3 pour une meuilleur flexilibité
	   // il change alors de zone target
	   if(Vector3.Distance(transform.position, target.position) < 0.3f)
	   {
		   //   % permet d'avoir le reste d'une division // % waypoints.Length permet de revenir au 1er point du array
		   destPoints = (destPoints + 1 ) % waypoints.Length;
		   target = waypoints[destPoints];
		   // inverse le flip
		   graphics.flipX = !graphics.flipX;
	   }
	   
	   
    }
	
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.transform.CompareTag("Player"))
		{
			// reférence temporaire au script PlayerHealth
			PlayerHealth playerHealth = collision.transform.GetComponent<PlayerHealth>();
			
			playerHealth.TakeDamage(damageOnCollision);
		}
	}
}
