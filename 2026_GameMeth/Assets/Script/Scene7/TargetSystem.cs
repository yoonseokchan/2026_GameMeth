    using UnityEngine;
using UnityEngine.InputSystem;

public class TargetSystem : MonoBehaviour
{
    [Header("Targeting Settings")]
    public Transform currentTarget;
    public float rotationSpeed = 5f;

    [Header("Crosshair UI (LerpUnclamped용)")]
    public RectTransform crosshair; 
    public float animationSpeed = 10f;
    private float targetScale = 1.0f;
    public void OnRightClick(InputValue value)
    {
        if (!value.isPressed) return;

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                currentTarget = hit.collider.transform;
                targetScale = 1.5f;
            }
            else
            {
                ResetTarget();
            }
        }
        else
        {
            ResetTarget();
        }
    }

    private void ResetTarget()
    {
        currentTarget = null;
        targetScale = 1.0f; 
    }

    private void Update()
    {
        if (currentTarget != null)
        {
            Vector3 direction = currentTarget.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        if (crosshair != null)
        {
            float currentScale = crosshair.localScale.x;
            float nextScale = Mathf.LerpUnclamped(currentScale, targetScale, Time.deltaTime * animationSpeed);
            crosshair.localScale = new Vector3(nextScale, nextScale, 1f);
        }
    }
}
