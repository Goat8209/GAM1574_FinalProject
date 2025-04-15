using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chick : Animal
{
    [SerializeField] private Blackboard blackboard;
    public Chicken targetChicken;

    private void Awake()
    {
        animalType = AnimalType.Chick;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetChicken.State == ChickenState.pecking && !blackboard.GetValue<bool>("CanPeck"))
        {
            blackboard.SetOrAddValue<bool>("CanPeck", true);
        }
        else if (targetChicken.State == ChickenState.running && blackboard.GetValue<bool>("CanPeck"))
        {
            blackboard.SetOrAddValue<bool>("CanPeck", false);
        }
    }

    protected override void OnStart()
    {

    }
}