using UnityEngine;

public class ReflectTest : MonoBehaviour
{
    public Vector3 velocity;
    public Vector3 gravity = new Vector3(0, -9.81f, 0);
    public float damping = 0.8f; 

    private int bounceCount = 0; 

    void Update()
    {
        velocity += gravity * Time.deltaTime;
        transform.position += velocity * Time.deltaTime;
    }

    void OnCollisionEnter(Collision col)
    {   
        if (col.gameObject.CompareTag("Enemy"))
        {
            TriggerExplosion();
            return;
        }

        bounceCount++;

        if (bounceCount >= 4    )
        {
            TriggerExplosion();
            return;
        }

        Vector3 normal = col.contacts[0].normal.normalized; 
        float dot = Vector3.Dot(velocity, normal);
        Vector3 reflect = velocity - 2f * dot * normal; 

        velocity = reflect * damping;
    }

    void TriggerExplosion()
    {
        ManualExplode exploder = GetComponent<ManualExplode>();
        if (exploder != null)
        {
            exploder.Explode(); 
        }       
        else
        {
            Destroy(gameObject);
        }
    }
}