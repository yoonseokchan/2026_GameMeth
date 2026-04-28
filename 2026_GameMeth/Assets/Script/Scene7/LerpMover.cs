using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpMover : MonoBehaviour
{
    public Transform startPos;
    public Transform endPos;

    [SerializeField] private float duration = 2f;
    [SerializeField] private float t = 0f;

    void Update()
    {
        t += Time.deltaTime / duration;
        transform.position = Vector3.Lerp(startPos.position, endPos.position, t);
    }
}