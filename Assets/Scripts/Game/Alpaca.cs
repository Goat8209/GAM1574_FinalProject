using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Alpaca : Animal
{
    [SerializeField] private Blackboard blackboard;
    [SerializeField] private AnimalSettings settings;

    private void Awake()
    {
        animalType = AnimalType.Alpaca;
    }

    // Update is called once per frame
    void Update()
    {
    }

    protected override void OnStart()
    {
        Initialize(settings);
    }
}
