using UnityEngine;

public enum BallType { Player1, Player2, Target }

public class Ball : MonoBehaviour
{
    public BallType ballType;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public Rigidbody GetRigidbody() => rb;

    // 공끼리의 충돌을 감지하여 매니저에게 전달
    private void OnCollisionEnter(Collision collision)
    {
        if (BilliardsGameManager.Instance != null)
        {
            BilliardsGameManager.Instance.HandleBallCollision(this, collision);
        }
    }
}