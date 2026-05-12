using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;  

public class BombLauncher : MonoBehaviour
{
    [Header("프리팹 설정")]
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private Transform targetTransform;

    [Header("베지어 랜덤 범위 설정")]
    [SerializeField] private float p1Radius = 3f;
    [SerializeField] private float p2Radius = 3f;
    [SerializeField] private float minHeight = 2f;
    [SerializeField] private float maxHeight = 6f;

    [Header("발사 설정")]
    [SerializeField] private int bombCount = 10;
    [SerializeField] private float flyDuration = 2f;
    public void OnAttack(InputValue value)
    {
        if (value.isPressed)
        {
            LaunchBombs();
        }
    }

    private void LaunchBombs()
    {
        if (bombPrefab == null || targetTransform == null)
        {
            Debug.LogWarning("폭탄 프리팹 혹은 타겟(적)이 지정되지 않았습니다.");
            return;
        }

        Vector3 p0 = transform.position;          
        Vector3 p3 = targetTransform.position;    

        for (int i = 0; i < bombCount; i++)
        {
            Vector2 randomCircle1 = Random.insideUnitCircle * p1Radius;
            float randomHeight1 = Random.Range(minHeight, maxHeight);

            Vector3 p1 = p0 + new Vector3(randomCircle1.x, randomHeight1, randomCircle1.y);
            p1 = p0 + new Vector3(randomCircle1.x, randomHeight1, randomCircle1.y);

            Vector2 randomCircle2 = Random.insideUnitCircle * p2Radius;
            float randomHeight2 = Random.Range(minHeight, maxHeight);
            Vector3 p2 = p3 + new Vector3(randomCircle2.x, randomHeight2, randomCircle2.y);

            GameObject bombObj = Instantiate(bombPrefab, p0, Quaternion.identity);

            if (bombObj.TryGetComponent<BezierBomb>(out var bombScript))
            {
                bombScript.Initialize(p0, p1, p2, p3, flyDuration);
            }
            else
            {
                bombScript = bombObj.AddComponent<BezierBomb>();
                bombScript.Initialize(p0, p1, p2, p3, flyDuration);
            }
        }
    }
}   