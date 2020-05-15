using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{

    public float speed = 10f;
    public bool spin;
    void Update()
    {
        if (spin)
            transform.Rotate(Vector3.up, speed);
    }
}
