using UnityEngine;
using UnityEngine.InputSystem;

public class TestScript1 : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 mouseScreenposition;
    private Vector3 targetPosition;
    private bool isSprinting;
    private bool isMoving = false;

    public void OnSprint(InputValue value)
    {
        isSprinting = value.isPressed;
    }


    public void OnPoint(InputValue value)
    {
        mouseScreenposition = value.Get<Vector2>();
    }

    public void OnClick(InputValue value)
    {
        if (value.isPressed)
        {
            Ray ray = Camera.main.ScreenPointToRay(mouseScreenposition);
            RaycastHit[] hits = Physics.RaycastAll(ray);

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject != gameObject)
                {
                    targetPosition = hit.point;
                    targetPosition.y = transform.position.y;
                    isMoving = true;

                    break;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            
            Vector3 direction = targetPosition - transform.position;

           
            float sqrMagnitude = (direction.x * direction.x) + (direction.y * direction.y) + (direction.z * direction.z);
            float magnitude = Mathf.Sqrt(sqrMagnitude);

            
            Vector3 normalizedVector = Vector3.zero;
                                            
           
            if (magnitude > 0)
            {
                normalizedVector = direction / magnitude;
            }


            float currentSpeed = moveSpeed;

           
            if (isSprinting == true)
            {
                
                currentSpeed = moveSpeed * 2f;
            }

           
            transform.position += normalizedVector * currentSpeed * Time.deltaTime;


            if (magnitude <= 0.05f)
            {
                isMoving = false;
            }
        }
    }
}