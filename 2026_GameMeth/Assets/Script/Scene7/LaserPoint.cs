using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class EnemyTargetingLaser : MonoBehaviour
{
    [Header("레이저 설정")]
    public Transform firePoint;
    public float laserDrawSpeed = 5f;

    private LineRenderer lineRenderer;
    private Transform currentTarget;
    private float lerpTime = 0f;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        lineRenderer.positionCount = 2;
    }

    public void OnRightClick(InputValue value)
    {
        if (!value.isPressed) return;


        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                currentTarget = hit.transform;

                lineRenderer.enabled = true;
                lerpTime = 0f;
            }
            else
            {
                ReleaseTarget();
            }
        }
        else
        {
            ReleaseTarget();
        }
    }

    void Update()
    {
        if (currentTarget != null && lineRenderer.enabled)
        {
            Vector3 startPos = firePoint != null ? firePoint.position : transform.position;

            lineRenderer.SetPosition(0, startPos);

            lerpTime += Time.deltaTime * laserDrawSpeed;
            Vector3 endPos = Vector3.LerpUnclamped(startPos, currentTarget.position, lerpTime);

            lineRenderer.SetPosition(1, endPos);
        }
    }

    private void ReleaseTarget()
    {
        currentTarget = null;
        lineRenderer.enabled = false;
        lerpTime = 0f;
    }
}