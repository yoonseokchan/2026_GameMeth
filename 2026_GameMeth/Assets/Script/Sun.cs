using UnityEngine;

public class Sun : MonoBehaviour
{
    [System.Serializable]
    public class Planet
    {
        public string name;           
        public Transform transform;    
        public float distance;        
        public float speed;           
        [HideInInspector] public float currentAngle = 0f; 
    }

    public Planet[] planets; 

    void Update()
    {
        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].currentAngle += planets[i].speed * Time.deltaTime;

            float x = transform.position.x + Mathf.Cos(planets[i].currentAngle) * planets[i].distance;
            float z = transform.position.z + Mathf.Sin(planets[i].currentAngle) * planets[i].distance;

            planets[i].transform.position = new Vector3(x, planets[i].transform.position.y, z);
        }
    }
}