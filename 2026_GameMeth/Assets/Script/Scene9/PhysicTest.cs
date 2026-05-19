using UnityEngine;
using UnityEngine.InputSystem;

public class PhysicsTest : MonoBehaviour
{
    public float forcePower = 10f;
    private Rigidbody rb;
    [SerializeField] private float speed;
    private bool isSprint;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnSprint(InputValue value)
    {
        isSprint = value.isPressed;
    }

    void FixedUpdate()
    {
        if (isSprint)
        {
            rb.AddForce(Vector3.forward * forcePower, ForceMode.Force);
        }
    }

    void Update()
    {
        speed = rb.linearVelocity.magnitude;
    }
}