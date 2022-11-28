using UnityEngine;

public class JustAGizmo : MonoBehaviour
{
	public float rayon;
    private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position, rayon);
	}
}
