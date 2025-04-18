using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

public enum BullState : byte
{
    Idle,
    Walking,
    Eating,
    Breeding
}

public class Bull : Animal
{
    [SerializeField] private Blackboard blackboard;
    [SerializeField] private AnimalSettings settings;

    public bool debugText;

    public bool male;
    public Bull partner;

    private float idleTimer;
    private float minIdle = 3;
    private float maxIdle = 10;

    private int hunger;
    private int minHunger = 3;
    private int maxHunger = 5;
    private float eatingTimer;
    private float minEating = 3;
    private float maxEating = 6;

    private float breedingDistance = 30;
    private float breedingRefreshTimer;
    private float breedingRefreshTime = 25;
    private float breedingTimer;
    private float breedingTime = 10;
    public Flock caffs;

    private Vector3[] haybales = { new Vector3(252.45f, 0, 395), new Vector3(239.21f, 0, 392.38f), new Vector3(228.73f, 0, 395.89f) };

    private BullState state;

    public BullState State { get { return state; } }

    private void Awake()
    {
        animalType = AnimalType.Bull;
        SetState(BullState.Idle);
        blackboard.SetValue<bool>("Walking", false);
        blackboard.SetValue<bool>("Eating", false);
        blackboard.SetValue<bool>("Breeding", false);
        hunger = Random.Range(minHunger, maxHunger);
        breedingRefreshTimer = breedingRefreshTime;
        breedingTimer = breedingRefreshTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (state != BullState.Breeding)
        {
            breedingRefreshTimer -= Time.deltaTime;

            if (breedingRefreshTimer <= 0)//if the cows are close enough and they can breed they do
            {
                if (Vector3.Distance(partner.transform.position, transform.position) <= breedingDistance)
                {
                    if (caffs)
                        caffs.IsPaused = true;
                    SetState(BullState.Breeding);
                }
            }
        }

        switch (state)
        {
            case BullState.Idle:
                idleTimer -= Time.deltaTime;
                if (idleTimer <= 0)
                {
                    if(caffs)
                        caffs.IsPaused = false;
                    SetState(BullState.Walking);
                }
                break;

            case BullState.Walking:
                if (hunger <= 0)
                {
                    GetComponent<NavMeshAgent>().destination = haybales[(int)Random.Range(0, 2)];//picks a random haybale to eat from and walks to it
                }

                if (Vector3.Distance(transform.position, GetComponent<NavMeshAgent>().destination) <= 5)//gets to its destination/going to haybale
                {
                    if (hunger <= 0)// if the cow is needs to eat set state to eating
                    {
                        if (state != BullState.Eating)
                        {
                            if (caffs)
                                caffs.IsPaused = true;
                            SetState(BullState.Eating);
                        }
                    }
                    else if (state != BullState.Idle)
                    {
                        if (caffs)
                            caffs.IsPaused = true;
                        SetState(BullState.Idle);
                    }
                }
                break;

            case BullState.Eating:
                eatingTimer -= Time.deltaTime;
                if (eatingTimer <= 0)
                {
                    GetComponent<NavMeshAgent>().isStopped = false;
                    if (caffs)
                        caffs.IsPaused = false;
                    SetState(BullState.Walking);
                }
                break;

            case BullState.Breeding:
                breedingTimer -= Time.deltaTime;
                if (breedingTimer <= 0)
                {
                    if (partner.caffs != null)//add a caff
                    {
                        partner.caffs.boidsToSpawn++;
                    }
                    if (caffs)
                        caffs.IsPaused = false;
                    SetState(BullState.Walking);
                }
                break;
        }

        if (debugText == true)//outputs the states
        {
            Debug.Log(this.gameObject.name.ToString() + "\n State:" + state + " BreedingRefreshTimer:" + breedingRefreshTimer.ToString() + " Hunger:" + hunger.ToString() + " IdleTimer:" + idleTimer.ToString() + " EatingTimer:" + eatingTimer.ToString());
        }
    }
    protected override void OnStart()
    {
        Initialize(settings);
    }

    private void SetState(BullState newState)
    {
        BullState lastState = state;
        switch (lastState)
        {
            case BullState.Idle:
                blackboard.SetValue<bool>("Idle", false);
                break;

            case BullState.Walking:
                blackboard.SetValue<bool>("Walking", false);
                break;
            case BullState.Eating:
                blackboard.SetValue<bool>("Eating", false);
                break;
            case BullState.Breeding:
                blackboard.SetValue<bool>("Breeding", false);
                break;
        }

        switch (newState)
        {
            case BullState.Idle:
                state = BullState.Idle;
                blackboard.SetValue<bool>("Idle", true);
                idleTimer = Random.Range(minIdle, maxIdle);
                break;

            case BullState.Walking:
                state = BullState.Walking;
                blackboard.SetValue<bool>("Walking", true);
                GetComponent<NavMeshAgent>().destination = new Vector3(Random.Range(175, 300), 0, Random.Range(250, 400));
                GetComponent<NavMeshAgent>().isStopped = false;
                hunger--;
                break;

            case BullState.Eating:
                state = BullState.Eating;
                blackboard.SetValue<bool>("Eating", true);
                eatingTimer = Random.Range(minEating, maxEating);
                hunger = Random.Range(minHunger, maxHunger);
                GetComponent<NavMeshAgent>().isStopped = true;
                break;

            case BullState.Breeding:
                state = BullState.Breeding;
                blackboard.SetValue<bool>("Breeding", true);
                GetComponent<NavMeshAgent>().isStopped = true;
                breedingTimer = breedingTime;
                breedingRefreshTimer = breedingRefreshTime;
                //point at other bull
                break;
        }
    }
}
