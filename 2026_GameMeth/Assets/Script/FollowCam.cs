using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 4f, -5f);

    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        Vector3 latePosition = target.position + Quaternion.Euler(0f, target.eulerAngles.y, 0f) * offset;
        transform.position = latePosition;

        transform.LookAt(target);
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
