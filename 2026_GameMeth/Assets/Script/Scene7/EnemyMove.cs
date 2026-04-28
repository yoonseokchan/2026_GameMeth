using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public float moveDistance = 3f;     
    public float moveSpeed = 2f;    

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float pingPongX = Mathf.PingPong(Time.time * moveSpeed, moveDistance) - (moveDistance / 2f);
        transform.position = startPosition + new Vector3(pingPongX, 0, 0);
    }
}
