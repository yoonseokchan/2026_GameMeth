using UnityEngine;

public class FinalRerrarian : MonoBehaviour
{
    [Header("Terrain Settings")]
    public int width = 30;
    public int depth = 30;
    public float scale = 0.1f;
    public float heightMultiplier = 8f;

    [Header("Water Settings")]
    public int waterLevel = 3; 

    [Header("Prefabs")]
    public GameObject grassPrefab;
    public GameObject dirtPrefab;
    public GameObject waterPrefab;

    private SimplePerlinNoise simpleNoise;

    void Start()
    {
        simpleNoise = GetComponent<SimplePerlinNoise>();

        if (simpleNoise != null)
        {
            simpleNoise.seed = Random.Range(0, 99999);
        }
        else
        {
            Debug.LogError("오류있지에 빨리 고치는지에");
        }

        Generate();
    }

    public void Generate()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                float xCoord = x * scale;
                float zCoord = z * scale;
                float noise = simpleNoise.Noise(xCoord, zCoord);

                int height = Mathf.RoundToInt(noise * heightMultiplier);

                CreateTerrainAndWaterColumn(x, z, height);
            }
        }
    }

    void CreateTerrainAndWaterColumn(int x, int z, int height)
    {
        for (int y = 0; y <= height; y++)
        {
            Vector3 position = new Vector3(x, y, z);
            GameObject prefabToSpawn = (y == height) ? grassPrefab : dirtPrefab;

            Instantiate(prefabToSpawn, position, Quaternion.identity, transform);
        }
        if (height < waterLevel)
        {
            for (int y = height + 1; y <= waterLevel; y++)
            {
                Vector3 waterPosition = new Vector3(x, y, z);
                Instantiate(waterPrefab, waterPosition, Quaternion.identity, transform);
            }
        }
    }
}