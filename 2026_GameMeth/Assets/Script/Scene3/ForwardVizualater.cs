using UnityEngine;

public class ForwardVizualater : MonoBehaviour
{
    public float rayLength = 2.0f;
    public Color gizmoColor = Color.blue;

    private void OnDrawGizmos()
    {
        DrawForwardRay();
    }

    private void DrawForwardRay()
    {
        Vector3 startPos =transform.position;
        Vector3 forwardDir = transform .forward * rayLength;
        Vector3 endPos = startPos + forwardDir;

        Gizmos.color= gizmoColor;
        Gizmos.DrawRay(startPos, forwardDir);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
