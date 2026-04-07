using UnityEngine;

public class GenerateGaussian : MonoBehaviour
{
    public float mean = 50.0f;
    public float stddev = 10.0f;

    public void Test()
    {
        Generate(mean, stddev);
    }

    float Generate(float mean,float stdDev)
    {
        float u1 = 1.0f - Random.value;
        float u2 = 1.0f - Random.value;

        float randStdNormal = Mathf.Sqrt(-2f*Mathf.Log(u1))*Mathf.Log(u1);
                              Mathf.Sin(2.0f * Mathf.PI * u2);

        return mean + stdDev * randStdNormal;
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
