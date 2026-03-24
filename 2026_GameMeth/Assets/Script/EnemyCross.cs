using UnityEngine;

public class EnemyCross : MonoBehaviour
{
    public Transform target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 forward = transform.forward;
        Vector3 dirToTarget = ( target.position - transform.position).normalized;

        Vector3 crossProduct = Vector3.Cross(forward, dirToTarget);

        if (crossProduct.y > 0.1f)
        {
            Debug.Log("적이 오른쪽에 있습니다");
        }
        else if (crossProduct.y < -0.1f)
        {
            Debug.Log("적이 왼쪽에 있습니다");       
        }
       
    }
}
