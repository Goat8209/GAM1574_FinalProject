using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChickenState : byte
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

    Animal[] animalList;
    bool danger = false;

    ChickenState state;
    public ChickenState State {  get { return state; } }

    [SerializeField] private Blackboard blackboard;

    Target target;

    private void Awake()
    {
        animalType = AnimalType.Chicken;
        state = ChickenState.running;

        animalList = FindObjectsByType<Animal>(FindObjectsSortMode.None);
    }

    private void Update()
    {
        for(int i = 0; i < animalList.Length; i++)
        {
            if (animalList[i].GetAnimalType != AnimalType.Chicken)
            {
                if (Vector3.Distance(transform.position, animalList[i].transform.position) < 50)
                {
                    danger = true;
                    break;
                }

                else
                {
                    danger = false;
                }
            }
        }

        if (peckTimer <= 0 && !blackboard.GetValue<bool>("CanPeck") && !danger)//when the time is out the chicken pecks
        {
            blackboard.SetOrAddValue<bool>("CanPeck", true);
            target.IsPaused = true;
            state = ChickenState.pecking;
        }

        else if (peckTimer <= -peckingTime && blackboard.GetValue<bool>("CanPeck"))//after one second the chicken stops pecking
        {
            blackboard.SetOrAddValue<bool>("CanPeck", false);
            target.IsPaused = false;
            state = ChickenState.running;
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
