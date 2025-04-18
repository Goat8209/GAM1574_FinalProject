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
        if (targetBull.State == BullState.Idle && !blackboard.GetValue<bool>("Idle"))
        {
            blackboard.SetOrAddValue<bool>("Idle", true);
            blackboard.SetOrAddValue<bool>("Eating", false);
            blackboard.SetOrAddValue<bool>("Walking", false);
        }

        else if (targetBull.State == BullState.Eating && !blackboard.GetValue<bool>("Eating"))
        {
            blackboard.SetOrAddValue<bool>("Idle", false);
            blackboard.SetOrAddValue<bool>("Eating", true);
            blackboard.SetOrAddValue<bool>("Walking", false);
        }

        else if (targetBull.State == BullState.Walking && !blackboard.GetValue<bool>("Walking"))
        {
            blackboard.SetOrAddValue<bool>("Idle", false);
            blackboard.SetOrAddValue<bool>("Eating", false);
            blackboard.SetOrAddValue<bool>("Walking", true);
        }
    }

    protected override void OnStart()
    {

    }
}