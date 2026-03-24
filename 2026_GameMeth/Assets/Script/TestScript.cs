using UnityEngine;
using UnityEngine.InputSystem;

public class TestScript : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 moveInput;
    public float rotationSpeed = 30f;
    public bool isLeftParrying = false;
    public bool isRightParrying = false;

    public void OnLeftParry(InputValue Value)
    {
        isLeftParrying=Value.isPressed;
    }

    public void OnRightParry(InputValue Value)
    {
        isRightParrying=Value.isPressed;
    }

    Vector3 normalizedVector;

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();       
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = new Vector3(moveInput.x, moveInput.y, 0);
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        Quaternion rotation = Quaternion.Euler(0f, moveInput.x * rotationSpeed * Time.deltaTime, 0f);
        transform.rotation = rotation * transform.rotation; 
    }
}
