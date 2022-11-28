using UnityEngine;

// =========================================
//
// Ce script gère les plateformes mobiles comme des ascenseurs. Elles se déplacent suivant un patern cyclique d'au moins 2 waypoints
// Elles doivent pouvoirs êtres désactivés par des Disrupteurs
// =========================================


public class Plateform : MonoBehaviour
{
    public bool isActive = false;
	public int disturbanceRadius = 10;
	public Animator animator;
	public float speedElevator;
	
	// l'objet pouvant trigger la plateforme :
	public GameObject triggerObject = null;
	// 2ième object pouvant trigger :
	public GameObject triggerObject2 = null;
	
	private int destPoints = 0;
	// target : prochaine destination
	private Transform target;
	// array : les zones où se déplacer ( liste de points )
	public Transform[] waypoints;
	
	void Start()
    {
        target = waypoints[0];		
    }

    // Update is called once per frame
    void Update()
    {
        if( (!isActive && triggerObject == null) || (!isActive && Vector3.Distance(transform.position + new Vector3(0,4,0), triggerObject.transform.position) > 13f)   )	   
		{
			animator.SetTrigger("Activate");
			isActive = true;	
		}
		if(isActive && Vector3.Distance(transform.position, PlayerMovement.instance.playerCirclePosition.position) < 2.5f)
		{			
			Vector3 dir = target.position - transform.position;
			transform.Translate(dir.normalized * speedElevator * Time.deltaTime, Space.World );
			if(Vector3.Distance(transform.position, target.position) < 0.1f)
			{							   
				destPoints = (destPoints + 1 ) % waypoints.Length;
				target = waypoints[destPoints];
				isActive = false;
				triggerObject = triggerObject2;
				if(triggerObject != null)
				{
					animator.SetTrigger("Deactivate");
				}
				
			}
		}
		
    }

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position + new Vector3(0,4,0), disturbanceRadius);
	}
}
