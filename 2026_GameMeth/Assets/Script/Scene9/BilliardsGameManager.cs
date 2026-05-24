using UnityEngine;
using TMPro; 
using System.Collections.Generic;

public class BilliardsGameManager : MonoBehaviour
{
    public static BilliardsGameManager Instance;

    public enum Turn { Player1, Player2 }
    [Header("Game State")]
    public Turn currentTurn = Turn.Player1;
    public int player1Score = 0;
    public int player2Score = 0;

    [Header("References")]
    public Ball p1Ball;
    public Ball p2Ball;
    public List<Ball> targetBalls = new List<Ball>();
    public CameraOrbit cameraOrbit;
    public TextMeshProUGUI statusText; 

    [Header("Physics Settings")]
    public float stopThreshold = 0.05f; 

    private List<Ball> allBalls = new List<Ball>();
    private bool isMoving = false;
    private bool hasFired = false;
    private float fireTime = 0f;

    // ���� ���� �浹 ��� ������
    private bool hitOpponent = false;
    private HashSet<Ball> hitTargetsThisTurn = new HashSet<Ball>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (p1Ball != null) allBalls.Add(p1Ball);
        if (p2Ball != null) allBalls.Add(p2Ball);
        allBalls.AddRange(targetBalls);

        UpdateUI();
        UpdateCameraTarget();
    }

    void Update()
    {
        if (isMoving)
        {
            if (Time.time - fireTime < 0.2f) return;

            if (AreAllBallsStopped())
            {
                isMoving = false;
                EvaluateTurnResult();
            }
        }
    }

    public bool CanPlay()
    {
        return !isMoving && player1Score < 5 && player2Score < 5;
    }

    public bool IsCorrectTurnBall(BallType type)
    {
        if (currentTurn == Turn.Player1 && type == BallType.Player1) return true;
        if (currentTurn == Turn.Player2 && type == BallType.Player2) return true;
        return false;
    }

    public void OnBallFired()
    {
        isMoving = true;
        hasFired = true;
        fireTime = Time.time;

        hitOpponent = false;
        hitTargetsThisTurn.Clear();
    }

    public void HandleBallCollision(Ball strikingBall, Collision collision)
    {
        if (!isMoving) return;

        if (currentTurn == Turn.Player1 && strikingBall.ballType != BallType.Player1) return;
        if (currentTurn == Turn.Player2 && strikingBall.ballType != BallType.Player2) return;

        Ball hitBall = collision.gameObject.GetComponent<Ball>();
        if (hitBall != null)
        {
            if ((currentTurn == Turn.Player1 && hitBall.ballType == BallType.Player2) ||
                (currentTurn == Turn.Player2 && hitBall.ballType == BallType.Player1))
            {
                hitOpponent = true;
            }
            else if (hitBall.ballType == BallType.Target)
            {
                hitTargetsThisTurn.Add(hitBall);
            }
        }
    }

    private bool AreAllBallsStopped()
    {
        foreach (var ball in allBalls)
        {
            Rigidbody rb = ball.GetRigidbody();
            if (rb != null)
            {
                if (rb.linearVelocity.magnitude > stopThreshold || rb.angularVelocity.magnitude > stopThreshold)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void EvaluateTurnResult()
    {
        if (!hasFired) return;
        hasFired = false;

        if (hitOpponent)
        {
            if (currentTurn == Turn.Player1) player1Score = Mathf.Max(0, player1Score - 1);
            else player2Score = Mathf.Max(0, player2Score - 1);
        }
        else if (hitTargetsThisTurn.Count == targetBalls.Count && targetBalls.Count > 0)
        {
            if (currentTurn == Turn.Player1) player1Score++;
            else player2Score++;
        }

        UpdateUI();

        if (player1Score >= 5 || player2Score >= 5)
        {
            statusText.text = $"<b>���� ����!</b>\n����: {(player1Score >= 5 ? "1P" : "2P")}";
            return;
        }

        currentTurn = (currentTurn == Turn.Player1) ? Turn.Player2 : Turn.Player1;
        UpdateCameraTarget();
        UpdateUI();
    }

    private void UpdateCameraTarget()
    {
        if (cameraOrbit != null)
        {
            cameraOrbit.target = (currentTurn == Turn.Player1) ? p1Ball.transform : p2Ball.transform;
        }
    }

    private void UpdateUI()
    {
        if (statusText != null)
        {
            string turnStr = (currentTurn == Turn.Player1) ? "<color=#FF5555>1P</color>" : "<color=#5555FF>2P</color>";
            statusText.text = $"���� ��: {turnStr}\n<b>1P ����:</b> {player1Score} / 5\n<b>2P ����:</b> {player2Score} / 5";
        }
    }
}