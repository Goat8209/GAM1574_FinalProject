using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

    public bool male;
    public Bull partner;

    private float idleTimer = 10;
    public float minIdle;
    public float maxIdle;

    private float hunger = 10;
    private float minHunger = 20;
    private float maxHunger = 30;

    private Vector3[] haybales = { new Vector3(252.45f, 0, 396), new Vector3(239.21f, 0, 393.38f), new Vector3(228.73f, 0, 396.89f) };

    private BullState state;

    public BullState State { get { return state; } }

    private void Awake()
    {
        animalType = AnimalType.Bull;
        GetComponent<NavMeshAgent>().destination = new Vector3(Random.Range(175, 300), 0, Random.Range(250, 400));
        hunger = Random.Range(minHunger, maxHunger);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, GetComponent<NavMeshAgent>().destination) <= 1)
        {
            if (state == BullState.Eating)
            {
                hunger = hunger = Random.Range(minHunger, maxHunger);
            }
            if (state != BullState.Idle)
            {
                SetState(BullState.Idle);
            }
            state = BullState.Idle;
        }

        if (idleTimer <= 0)
        {
            if (state != BullState.Walking)
            {
                GetComponent<NavMeshAgent>().destination = new Vector3(Random.Range(175, 300), 0, Random.Range(250, 400));
                SetState(BullState.Walking);
            }
            idleTimer = Random.Range(minIdle, maxIdle);
        }

        //if (Vector3.Distance(transform.position, partner.transform.position) <= 10)
        //{
        //    if (state != BullState.Breeding)
        //    {
        //        SetState(BullState.Breeding);
        //    }
        //}

        if (hunger <= 0)
        {
            if (state != BullState.Eating)
            {
                SetState(BullState.Eating);
                GetComponent<NavMeshAgent>().destination = haybales[(int)Random.Range(0, 2)];

            }

        }

        switch (state)
        {
            case BullState.Idle:
                idleTimer -= Time.deltaTime;
                break;

            case BullState.Walking:

                hunger -= Time.deltaTime;
                break;

            case BullState.Breeding:

                break;

            case BullState.Eating:
                

                break;
        }
    }
    protected override void OnStart()
    {
        Initialize(settings);
    }

    private void SetState(BullState newState)
    {
        switch (newState)
        {
            case BullState.Idle:
                state = BullState.Idle;
                blackboard.SetValue("State", "Idle");
                //blackboard.SetValue<bool>("Idle", true);
                //blackboard.SetValue<bool>("Walking", false);
                //blackboard.SetValue<bool>("Breeding", false);
                //blackboard.SetValue<bool>("Eating", false);
                break;
            case BullState.Walking:
                state = BullState.Walking;
                //blackboard.SetValue<bool>("Idle", false);
                //blackboard.SetValue<bool>("Walking", true);
                //blackboard.SetValue<bool>("Breeding", false);
                //blackboard.SetValue<bool>("Eating", false);
                break;
            case BullState.Breeding:
                state = BullState.Breeding;
                //blackboard.SetValue<bool>("Idle", false);
                //blackboard.SetValue<bool>("Walking", false);
                //blackboard.SetValue<bool>("Breeding", true);
                //blackboard.SetValue<bool>("Eating", false);
                break;
            case BullState.Eating:
                state = BullState.Eating;
                //blackboard.SetValue<bool>("Idle", false);
                //blackboard.SetValue<bool>("Walking", false);
                //blackboard.SetValue<bool>("Breeding", false);
                //blackboard.SetValue<bool>("Eating", true);
                break;
        }
    }
}
