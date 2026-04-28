using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class OnRightClick : MonoBehaviour
{
    [Header("Targeting Settings")]
    public Transform currentTarget;      
    public float cameraSpeed = 5f;       

    [Header("Line Renderer Settings")]
    public Transform lineStartPos;       
    private LineRenderer lr;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.widthMultiplier = 0.05f;
        lr.material = new Material(Shader.Find("Unlit/Color")) { color = Color.red };
        lr.enabled = false;
    }

    private void Update()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            HandleTargetingInput();
        }
    }

    private void HandleTargetingInput()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                currentTarget = hit.collider.transform;
                Debug.Log("적 타겟팅 완료: " + currentTarget.name);
            }
            else
            {
                currentTarget = null;
            }
        }
        else
        {
            currentTarget = null;
        }
    }
}


