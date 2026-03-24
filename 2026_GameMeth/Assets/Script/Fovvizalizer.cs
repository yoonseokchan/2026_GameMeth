using UnityEngine;

public class Fovvizalizer : MonoBehaviour
{
    public float viewAngle = 60f;

    public float viewDistance = 5f;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Vector3 forward = transform.forward * viewDistance;

        Vector3 LeftBoundary =Quaternion.Euler(0, -viewAngle/2,0) * forward;

        Vector3 rightBoundary = Quaternion.Euler(0, -viewAngle / 2, 0) * forward;

        Gizmos.DrawRay(transform.position, LeftBoundary);
        Gizmos.DrawRay(transform.position, rightBoundary);

        Gizmos.color += Color.red;
        Gizmos.DrawRay(transform.position, forward);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
