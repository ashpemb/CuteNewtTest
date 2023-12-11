using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;
    public Vector3 Offset;
    public float Damping;
    
    private Vector3 _velocity = Vector3.zero;

    void FixedUpdate()
    {
        Vector3 movePosition = Target.position + Offset;
        transform.position = Vector3.SmoothDamp(transform.position, movePosition, ref _velocity, Damping);
    }
}
