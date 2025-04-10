using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox : Animal
{
    [SerializeField] private Blackboard blackboard;
    [SerializeField] private AnimalSettings settings;

    private void Awake()
    {
        animalType = AnimalType.Fox;
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
