using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AnimalSettings : ScriptableObject
{
    [Header("NavMesh")]
    public float speed = 5.0f;
    public float acceleration = 8.0f;
    public float angularSpeed = 8.0f;
    public float stoppingDistance = 8.0f;
}
