using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class BezierBomb : MonoBehaviour
{
    private Vector3 p0, p1, p2, p3;
    private float timeValue = 0f;
    private float speed = 0.5f; // 이동 속도 (0.5f면 2초 동안 날아감)
    private bool isInitialized = false;

    // 생성 직후 제어점 정보를 세팅해주는 함수
    public void Initialize(Vector3 start, Vector3 control1, Vector3 control2, Vector3 end, float duration)
    {
        p0 = start;
        p1 = control1;
        p2 = control2;
        p3 = end;
        speed = 1f / duration; // duration초 동안 이동하도록 속도 계산

        transform.position = start;
        isInitialized = true;

        // 트레일 렌더러가 있다면 초기화해서 꼬리가 안 튀게 방지
        if (TryGetComponent<TrailRenderer>(out var trail))
        {
            trail.Clear();
        }
    }

    void Update()
    {
        if (!isInitialized) return;

        // 시간 누적
        timeValue += Time.deltaTime * speed;

        if (timeValue <= 1f)
        {
            // 3차 베지어 커브 공식 적용
            transform.position = GetPointOnBezierCurve(p0, p1, p2, p3, timeValue);
        }
        else
        {
            // 목적지 도착 시 처리 (예: 폭발 이펙트 생성 후 삭제)
            Destroy(gameObject);
        }
    }

    // 제공해주신 3차 베지어 곡선 수식 적용
    private Vector3 GetPointOnBezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        Vector3 a = Vector3.Lerp(p0, p1, t);
        Vector3 b = Vector3.Lerp(p1, p2, t);
        Vector3 c = Vector3.Lerp(p2, p3, t);

        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);

        Vector3 abc = Vector3.Lerp(ab, bc, t);

        return abc;
    }
}