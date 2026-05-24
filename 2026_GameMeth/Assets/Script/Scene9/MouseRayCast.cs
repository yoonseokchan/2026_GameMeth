using UnityEngine;
using UnityEngine.InputSystem;

public class MouseRaycastTest : MonoBehaviour
{
    public float rayDistance = 100f;
    float moveInput;
    public CameraOrbit cam;

    public void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        moveInput = input.x;
        cam.moveInput = moveInput;
    }

    public void OnClick(InputValue value)
    {
        if (!value.isPressed) return;

        // [추가] 공이 움직이는 중이거나 게임이 끝났다면 입력 차단 (규칙 3번)
        if (BilliardsGameManager.Instance != null && !BilliardsGameManager.Instance.CanPlay()) return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            Rigidbody rb = hit.collider.attachedRigidbody;
            if (rb != null)
            {
                // [추가] 클릭한 오브젝트의 Ball 컴포넌트를 확인하여 내 턴의 공인지 검사 (규칙 2번)
                Ball ball = rb.GetComponent<Ball>();
                if (ball == null || !BilliardsGameManager.Instance.IsCorrectTurnBall(ball.ballType)) return;

                // 기존 마우스 위치 기반 힘 계산 및 발사 로직
                Vector3 hitPoint = hit.point;
                Vector3 center = rb.gameObject.transform.position;
                Vector3 forceDirection = center - hitPoint;
                forceDirection.y = 0f;
                forceDirection.Normalize();

                rb.AddForce(forceDirection * 10f, ForceMode.Impulse);

                // [추가] 매니저에게 공이 발사되었음을 알림
                BilliardsGameManager.Instance.OnBallFired();
            }
        }
    }
}
