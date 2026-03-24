using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AutoRotation : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0,45* Time.deltaTime,0);
    }
}
