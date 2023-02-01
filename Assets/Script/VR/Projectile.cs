using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Rigidbody rb = null;
    [SerializeField] private float force = 5.0f;
    void Start()
    {
        rb.AddForce(transform.TransformDirection(Vector3.forward)*force,ForceMode.Impulse);
    }
}
