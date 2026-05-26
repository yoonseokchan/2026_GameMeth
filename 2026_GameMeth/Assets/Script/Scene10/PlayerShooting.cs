using UnityEngine;
using UnityEngine.InputSystem; 

public class PlayerFire : MonoBehaviour
{
    [Header("Q Skill (Bomb)")]
    public GameObject bombPrefab;
    public Transform firePoint;
    public float throwForce = 12f;
    public float upwardForce = 5f;

    [Header("E Skill (Mine)")]
    public GameObject minePrefab;   
    public float spawnDistance = 5f; 

    void Update()
    {

        if (Keyboard.current != null && Keyboard.current.qKey.wasPressedThisFrame)
        {
            FireBomb();
        }


        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            SpawnMine();
        }
    }

    void FireBomb()
    {
        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position + transform.forward;
        GameObject bomb = Instantiate(bombPrefab, spawnPos, Quaternion.identity);
        Vector3 initialVelocity = transform.forward * throwForce + Vector3.up * upwardForce;

        ReflectTest bombScript = bomb.GetComponent<ReflectTest>();
        if (bombScript != null)
        {
            bombScript.velocity = initialVelocity;
        }
    }

    void SpawnMine()
    {

        Vector3 spawnPos = transform.position + (transform.forward * spawnDistance);
        spawnPos.y = transform.position.y;
        Instantiate(minePrefab, spawnPos, Quaternion.identity);
    }
}