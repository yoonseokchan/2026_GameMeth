using UnityEngine;

public class EnemyDetect : MonoBehaviour
{
    public Transform player;        
    public float maxSightDistance = 10f; 

    [Range(0, 360)]
    public float viewAngle = 90f;    

    private Vector3 originalScale;   

    void Start()
    {
      
        originalScale = transform.localScale;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= maxSightDistance)
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;

            float dotProduct = Vector3.Dot(transform.forward, directionToPlayer);
            float viewCos = Mathf.Cos(viewAngle * 0.5f * Mathf.Deg2Rad);

            if (dotProduct >= viewCos)
            {
                transform.localScale = Vector3.one * 2;
            }
            else
            {
                transform.localScale = originalScale;
            }
        }
        else
        {
            transform.localScale = originalScale;
        }
    }
}