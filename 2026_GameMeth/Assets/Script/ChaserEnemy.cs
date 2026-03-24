using UnityEngine;

public class ChaserEnemy : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 8f;
    public float rotationSpeed = 50f;
    public float DashSpeed = 15f;
    public float stopDistance = 1.2f;
    public bool IsDashing =false;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody> ();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up*rotationSpeed*Time.deltaTime);
    }

    void checkParry()
    {
        TestScript pc = player.GetComponent<TestScript> ();

        //외적을 사용해서  플레이어 기준 왼쪽오른쪽 패링 판정 추가할것    
    }
}
