using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))] 
public class LaserTargetingSystem : MonoBehaviour
{
    [Header("Targeting Settings")]
    public Transform currentTarget;
    public float rotationSpeed = 5f;

    [Header("Laser Settings (LerpUnclamped 활용)")]
    public LineRenderer lineRenderer;
    public float laserSpeed = 15f; 
    private float laserProgress = 0f; 
    private bool isLaserActive = false;

    void Start()
    {
        if (lineRenderer == null) lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.positionCount = 2; 
        lineRenderer.enabled = false;   


        Material laserMat = new Material(Shader.Find("Sprites/Default")); 
        lineRenderer.material = laserMat;

        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.red, 0.0f), new GradientColorKey(Color.magenta, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) }
        );
        lineRenderer.colorGradient = gradient;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
    }

    public void OnRightClick(InputValue value)
    {
        if (!value.isPressed) return;

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                currentTarget = hit.collider.transform;
                isLaserActive = true;
                laserProgress = 0f; // 레이저 발사 시작 (0부터)
                lineRenderer.enabled = true;
                Debug.Log("적 타겟팅! 레이저 발사.");
            }
            else { ResetTarget(); }
        }
        else { ResetTarget(); }
    }

    private void ResetTarget()
    {
        currentTarget = null;
        isLaserActive = false;
        laserProgress = 0f;
        lineRenderer.enabled = false;
    }

    private void Update()
    {
        {
            Vector3 direction = currentTarget.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        if (isLaserActive && currentTarget != null)
        {
            laserProgress += Time.deltaTime * laserSpeed;

            float t = Mathf.LerpUnclamped(0f, 1f, laserProgress);

            Vector3 startPos = transform.position; 
            Vector3 targetPos = currentTarget.position; 

            Vector3 currentLaserEndPos = Vector3.LerpUnclamped(startPos, targetPos, t);

            lineRenderer.SetPosition(0, startPos);
            lineRenderer.SetPosition(1, currentLaserEndPos);
        }
    }
}
