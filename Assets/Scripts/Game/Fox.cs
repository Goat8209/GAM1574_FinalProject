using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum FoxState : byte
{
    Idle,
    Walking,
    Hunting,
    Eating,
    Running,
    Dead
}

public class Fox : Animal
{
    [SerializeField] private Blackboard blackboard;
    [SerializeField] private AnimalSettings settings;

    public bool debugText;

    //the fox only stands in one place for so long
    private float idleTimer;
    private float minIdle = 3;
    private float maxIdle = 10;

    //the fox loses hunger when it walks somwhere
    private int hunger;
    private int minHunger = 3;
    private int maxHunger = 5;
    private float eatingTimer;
    private float minEating = 3;
    private float maxEating = 6;

    public List<Donkey> donkeyList;

    Chick[] chickList;
    private Chick prey;

    private float personalSpace = 15;

    private float runSpeed = 18;
    private float walkSpeed = 6;

    private FoxState state;

    public FoxState State { get { return state; } }

    private void Awake()
    {
        animalType = AnimalType.Fox;
        SetState(FoxState.Idle);
        blackboard.SetValue<bool>("Walking", false);
        blackboard.SetValue<bool>("Hunting", false);
        blackboard.SetValue<bool>("Running", false);
        idleTimer = Random.Range(minIdle, maxIdle);
        hunger = Random.Range(minHunger, maxHunger);

    }

    // Update is called once per frame
    void Update()
    {
        if (donkeyList.Count > 0)//starts running if a donkey is close enough to attack it
        {
            foreach (Donkey i in donkeyList)
            {
                if (Vector3.Distance(i.transform.position, transform.position) <= personalSpace)
                {
                    if (i.State == DonkeyState.Attacking)
                    {
                        SetState(FoxState.Running);
                        if(Vector3.Distance(i.transform.position,transform.position) < 2) //fox dies
                        {
                            SetState(FoxState.Dead);
                        }
                    }
                    if (debugText == true)
                    {
                        Debug.Log(i.State.ToString());
                    }
                }
            }
        }

        switch (state)
        {
            case FoxState.Idle:
                idleTimer -= Time.deltaTime;
                if (idleTimer <= 0)
                {
                    SetState(FoxState.Walking);
                }
                break;

            case FoxState.Walking:
                if (hunger <= 0)
                {
                    chickList = FindObjectsByType<Chick>(FindObjectsSortMode.None);

                    Vector3 tempVec3 = new Vector3(0, 0, 0);
                    
                    if (chickList.Length > 0)//if there are no chickens the fox eats dirt. if there are the fox will hunt down the closest chicken
                    {
                        for (int i = 0; i < chickList.Length; i++)
                        {
                            Vector3 tempChickPos = chickList[i].transform.position;
                            if (Vector3.Distance(transform.position, tempVec3) > Vector3.Distance(transform.position, tempChickPos))
                            {
                                tempVec3 = tempChickPos;
                                prey = chickList[i];
                            }
                        }
                    }
                    else
                    {
                        SetState(FoxState.Eating);
                    }
                }

                if (Vector3.Distance(transform.position, GetComponent<NavMeshAgent>().destination) <= 5)//gets to its destination/going to haybale
                {
                    if (state != FoxState.Running)//ignore these if the fox is running from a donkey
                    {
                        if (hunger <= 0)
                        {
                            if (state != FoxState.Hunting)
                            {
                                SetState(FoxState.Hunting);
                            }
                        }
                        else if (state != FoxState.Idle)
                        {
                            SetState(FoxState.Idle);
                        }
                    }
                }
                break;

            case FoxState.Hunting:
                if (prey != null)
                {
                    GetComponent<NavMeshAgent>().destination = prey.transform.position;
                    if (Vector3.Distance(transform.position, prey.transform.position) < 5)
                    {
                        Destroy(prey);
                        SetState(FoxState.Eating);
                    }
                }
                break;

            case FoxState.Eating:
                eatingTimer -= Time.deltaTime;
                if (eatingTimer <= 0)
                {
                    GetComponent<NavMeshAgent>().isStopped = false;
                    hunger = Random.Range(minHunger, maxHunger);
                    SetState(FoxState.Walking);
                }
                break;

            case FoxState.Running:
                if (donkeyList.Count > 0)//if there is no attacking donkey close enough the fox stops running
                {
                    foreach (Donkey i in donkeyList)
                    {
                        if (Vector3.Distance(i.transform.position, transform.position) <= personalSpace)
                        {
                            if (Vector3.Distance(GetComponent<NavMeshAgent>().destination, transform.position) <= 1)//if the donkey is chasing the fox and the fox reaches it's destination it finds an other one
                            {
                                GetComponent<NavMeshAgent>().destination = new Vector3(Random.Range(175, 300), 0, Random.Range(250, 400));
                            }
                        }
                        if (i.State != DonkeyState.Attacking)//donkey stops attacking, fox stops running
                        {
                            SetState(FoxState.Walking);
                        }
                    }
                }
                break;
        }

        if (debugText == true)//outputs the states
        {
            Debug.Log(this.gameObject.name.ToString() + " Debug:" +
                "\nState:" + state + " Hunger:" + hunger);
        }
    }

    protected override void OnStart()
    {
        Initialize(settings);
    }

    private void SetState(FoxState newState)
    {
        FoxState lastState = state;
        switch (lastState)
        {
            case FoxState.Idle:
                blackboard.SetValue<bool>("Idle", false);
                break;

            case FoxState.Walking:
                blackboard.SetValue<bool>("Walking", false);
                break;
            case FoxState.Hunting:
                blackboard.SetValue<bool>("Hunting", false);
                break;
            case FoxState.Eating:
                blackboard.SetValue<bool>("Eating", false);
                break;
            case FoxState.Running:
                blackboard.SetValue<bool>("Running", false);
                break;
            case FoxState.Dead:
                blackboard.SetValue<bool>("Dead", false);//i don't know how it would end up getting revived but this is just in case
                break;
        }

        switch (newState)
        {
            case FoxState.Idle:
                state = FoxState.Idle;
                blackboard.SetValue<bool>("Idle", true);
                idleTimer = Random.Range(minIdle, maxIdle);
                break;

            case FoxState.Walking:
                state = FoxState.Walking;
                blackboard.SetValue<bool>("Walking", true);
                GetComponent<NavMeshAgent>().destination = new Vector3(Random.Range(175, 300), 0, Random.Range(250, 400));
                GetComponent<NavMeshAgent>().speed = walkSpeed;
                hunger--;
                break;

            case FoxState.Hunting:
                state = FoxState.Hunting;
                blackboard.SetValue<bool>("Hunting", true);
                GetComponent<NavMeshAgent>().speed = runSpeed;
                break;

            case FoxState.Eating:
                state = FoxState.Eating;
                eatingTimer = Random.Range(minEating, maxEating);
                hunger = Random.Range(minHunger, maxHunger);
                GetComponent<NavMeshAgent>().isStopped = true;
                blackboard.SetValue<bool>("Eating", true);
                break;

            case FoxState.Running:
                state = FoxState.Running;
                blackboard.SetValue<bool>("Running", true);
                GetComponent<NavMeshAgent>().speed = runSpeed;
                break;

            case FoxState.Dead:
                state = FoxState.Dead;
                blackboard.SetValue<bool>("Dead", true);
                GetComponent<NavMeshAgent>().isStopped = true;
                break;
        }
    }
}