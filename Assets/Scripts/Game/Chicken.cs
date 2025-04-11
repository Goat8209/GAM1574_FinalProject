using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State : byte
{
    pecking,
    running
}

public class Chicken : Animal
{
    private float peckTimer = 0;
    public float minPeckBreak = 5;
    public float maxPeckBreak = 10;
    public float peckingTime;

    State state;
    public State State {  get { return state; } }

    [SerializeField] private Blackboard blackboard;

    Target target;

    private void Awake()
    {
        animalType = AnimalType.Chicken;
        state = State.running;
    }

    private void Update()
    {
        if (peckTimer <= 0 && !blackboard.GetValue<bool>("CanPeck"))//when the time is out the chicken pecks
        {
            blackboard.SetOrAddValue<bool>("CanPeck", true);
            target.IsPaused = true;
            state = State.pecking;
        }

        else if (peckTimer <= -peckingTime && blackboard.GetValue<bool>("CanPeck"))//after one second the chicken stops pecking
        {
            blackboard.SetOrAddValue<bool>("CanPeck", false);
            target.IsPaused = false;
            state = State.running;
            peckTimer = UnityEngine.Random.Range(minPeckBreak, maxPeckBreak);

        }
        //Debug.Log(peckTimer);
        peckTimer -= Time.deltaTime;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 current = target.transform.position;
        Vector3 previous = target.PreviousPosition;
        Vector3 direction = (current - previous).normalized;
        transform.forward = direction;
    }

    protected override void OnStart()
    {
        peckTimer = UnityEngine.Random.Range(minPeckBreak, maxPeckBreak);
        blackboard.SetOrAddValue<bool>("CanPeck", false);
        target = GetComponent<Target>();
    }
}
