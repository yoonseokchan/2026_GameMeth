using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ChaserEnemy : MonoBehaviour
{
    public Transform player;
    public float rotationSpeed = 50f;
    public float detectionRange = 8f;
    public float dashSpeed = 15f;
    public float stopDistance = 1.2f;
    public bool isDashing = false;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!isDashing) // 회전 모드
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

            // [과제] 내적을 사용하여 '전방 시야 60도 이내' 판정 추가 (수식 직접 계산)
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            Vector3 forward = transform.forward;

            // Vector3.Dot() 대신 내적 공식 직접 적용: (Ax * Bx) + (Ay * By) + (Az * Bz)
            float dotProduct = (forward.x * dirToPlayer.x) + (forward.y * dirToPlayer.y) + (forward.z * dirToPlayer.z);

            // 거리 판정과 60도 각도 판정
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance <= detectionRange && dotProduct >= Mathf.Cos(60f * Mathf.Deg2Rad))
            {
                isDashing = true;
            }
        }
        else // Dash 모드 일 때 플레이어 쪽으로 가기.
        {
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            transform.position += dirToPlayer * dashSpeed * Time.deltaTime;

            if (Vector3.Distance(transform.position, player.position) <= stopDistance)
            {
                CheckParry();
            }
        }
    }

    void CheckParry()
    {
        TestScript pc = player.GetComponent<TestScript>();

        // [과제] 외적을 사용하여 플레이어 기준 왼쪽/오른쪽 패링 판정 추가 (수식 직접 계산)
        Vector3 dirToEnemy = (transform.position - player.position).normalized;
        Vector3 pForward = player.forward;

        // Vector3.Cross() 대신 외적 공식 직접 적용
        // Cx = AyBz - AzBy
        // Cy = AzBx - AxBz
        // Cz = AxBy - AyBx
        float crossX = (pForward.y * dirToEnemy.z) - (pForward.z * dirToEnemy.y);
        float crossY = (pForward.z * dirToEnemy.x) - (pForward.x * dirToEnemy.z);
        float crossZ = (pForward.x * dirToEnemy.y) - (pForward.y * dirToEnemy.x);

        Vector3 crossProduct = new Vector3(crossX, crossY, crossZ);

        if (crossProduct.y > 0) // 적이 오른쪽에서 접근
        {
            if (Keyboard.current.qKey.isPressed)
            {
                Destroy(gameObject);
            }
            else // 못 누르고 있거나 틀린 키면 패리 실패 -> 씬 재시작
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        else if (crossProduct.y < 0) // 적이 왼쪽에서 접근
        {
            if (Keyboard.current.eKey.isPressed)
            {
                Destroy(gameObject);
            }
            else // 못 누르고 있거나 틀린 키면 패리 실패 -> 씬 재시작
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}