using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float force = 5.0f;
    void Update()
    {
        transform.Translate(Vector3.forward*force*Time.deltaTime);
    }
}
