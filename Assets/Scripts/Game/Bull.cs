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

    private BullState state;

    public BullState State { get { return state; } }

    private void Awake()
    {
        animalType = AnimalType.Bull;
        GetComponent<NavMeshAgent>().destination = new Vector3(Random.Range(175, 300), 0, Random.Range(250, 400));
    }

    // Update is called once per frame
    void Update()
    {
        //if (Vector3.Distance(transform.position, GetComponent<NavMeshAgent>().destination) < 1 )
        //{
        //    if (idleTimer < 0)
        //    {
        //        blackboard.SetValue<bool>("", true);
        //        GetComponent<NavMeshAgent>().destination = new Vector3(Random.Range(175, 300), 0, Random.Range(250, 400));
        //        idleTimer = Random.Range(minIdle, maxIdle);
        //    }
        //        idleTimer -= Time.deltaTime;
        //}

        if (Vector3.Distance(transform.position, GetComponent<NavMeshAgent>().destination) <= 1)
        {
            if (state != BullState.Idle)
            {
                stateToBlackboard(BullState.Idle);
            }
            state = BullState.Idle;
        }

        if (idleTimer <= 0)
        {
            state = BullState.Walking;
            idleTimer = Random.Range(minIdle, maxIdle);
        }

        if (Vector3.Distance(transform.position, partner.transform.position) <= 10)
        {
            state = BullState.Breeding;
        }

        if (hunger <= 0)
        {
            state = BullState.Eating;
        }

        switch (state)
        {
            case BullState.Idle:
                idleTimer -= Time.deltaTime;
                GetComponent<NavMeshAgent>().destination = new Vector3(Random.Range(175, 300), 0, Random.Range(250, 400));
                break;

            case BullState.Walking:
                stateToBlackboard(BullState.Walking);


                break;

            case BullState.Breeding:
                stateToBlackboard(BullState.Breeding);

                break;

            case BullState.Eating:
                stateToBlackboard(BullState.Eating);

                break;
        }

    }
    protected override void OnStart()
    {
        Initialize(settings);
    }

    private void stateToBlackboard(BullState state)
    {
        //switch (state)
        //{
        //    case BullState.Idle:
        //        blackboard.SetValue<bool>("Idle", true);
        //        blackboard.SetValue<bool>("Walking", false);
        //        blackboard.SetValue<bool>("Breeding", false);
        //        blackboard.SetValue<bool>("Eating", false);
        //        break;
        //    case BullState.Walking:
        //        blackboard.SetValue<bool>("Idle", false);
        //        blackboard.SetValue<bool>("Walking", true);
        //        blackboard.SetValue<bool>("Breeding", false);
        //        blackboard.SetValue<bool>("Eating", false);
        //        break;
        //    case BullState.Breeding:
        //        blackboard.SetValue<bool>("Idle", false);
        //        blackboard.SetValue<bool>("Walking", false);
        //        blackboard.SetValue<bool>("Breeding", true);
        //        blackboard.SetValue<bool>("Eating", false);
        //        break;
        //    case BullState.Eating:
        //        blackboard.SetValue<bool>("Idle", false);
        //        blackboard.SetValue<bool>("Walking", false);
        //        blackboard.SetValue<bool>("Breeding", false);
        //        blackboard.SetValue<bool>("Eating", true);
        //        break;
        //}
    }
}
