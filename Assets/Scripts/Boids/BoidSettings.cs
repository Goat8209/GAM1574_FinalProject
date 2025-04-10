using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BoidSettings : ScriptableObject
{
    [Header("Movement")]
    public float minSpeed = 5.0f;
    public float maxSpeed = 8.0f;
    public float viewRadius = 8.0f;
    public float avoidRadius = 1.0f;
    public float maxSteerForce = 10.0f;

    [Header("Motivation")]
    public float seperationWeight = 1.0f;
    public float alignmentWeight = 1.0f;
    public float cohesionWeight = 1.0f;
    public float targetWeight = 1.0f;

    [Header("Collision")]
    public LayerMask collisionMask;
    public float boundsRadius = 1.5f;
    public float avoidCollisionWeight = 15.0f;
    public float avoidCollisionDistance = 5.0f;

    [Header("Lock Y-axis")]
    public bool lockAxisY = false;
    public float positionY = 0.0f;
}
