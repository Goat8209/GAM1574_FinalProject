using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caff : Animal
{
    [SerializeField] private Blackboard blackboard;
    public Bull targetBull;

    private void Awake()
    {
        animalType = AnimalType.Caff;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetBull.State == BullState.Eating && !blackboard.GetValue<bool>("Eating"))
        {
            blackboard.SetOrAddValue<bool>("Eating", true);
        }
        else if (targetBull.State == BullState.Walking && blackboard.GetValue<bool>("Eating"))
        {
            blackboard.SetOrAddValue<bool>("Eating", false);
        }
    }

    protected override void OnStart()
    {

    }
}