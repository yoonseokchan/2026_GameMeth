using UnityEngine;

public class Moon : MonoBehaviour
{
    public Transform earth;   
    public float distance;     
    public float speed;        
    private float currentAngle = 0f;

    void Update()
    {
        currentAngle += speed * Time.deltaTime;

        float x = earth.position.x + Mathf.Cos(currentAngle) * distance;
        float z = earth.position.z + Mathf.Sin(currentAngle) * distance;

        transform.position = new Vector3(x, transform.position.y, z);
    }
}
