using UnityEngine;
using UnityEngine.UIElements;

public class FOVVizulizer : MonoBehaviour
{
    public Transform player;
    public float viewangle = 60f; // 시야각

 

    // Update is called once per frame
    void Update()
    {
        Vector3 toPlayer = (player.position - transform.position).normalized;
        Vector3 forward = transform.forward;

        float dot = Vector3.Dot(forward, toPlayer);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg; //내적을 각도로 변환

        if (angle < viewangle / 2)
        {
            Debug.Log("플레이어가 시야 안에 있음!");
        }
    }
}
