using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor.Rendering;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.AI;

public enum DonkeyState : byte
{
    Idle,
    Walking,
    Eating,
    Attacking
}

public class Donkey : Animal
{
    [SerializeField] private Blackboard blackboard;
    [SerializeField] private AnimalSettings settings;

    public bool debugText;

    //the donkey only stands in one place for so long
    private float idleTimer;
    private float minIdle = 3;
    private float maxIdle = 10;

    //while the donkey walks it gets hungry
    private int hunger;
    private int minHunger = 4;
    private int maxHunger = 6;
    private float eatingTimer;
    private float minEating = 3;
    private float maxEating = 6;

    //if there are foxes in the donkeys personal space its sanity goes down
    private float sanity;
    private float minSanity = 15;
    private float maxSanity = 20;
    private float personalSpace = 20;
    public List<Fox> foxList;
    private Fox targetFox;

    private float attackTimer;
    private float minAttackTime = 5;
    private float maxAttackTime = 10; 

    private float attackSpeed = 7;
    private float walkSpeed = 3.5f;

    private DonkeyState state;

    public DonkeyState State { get { return state; } }

    private Vector3[] haybales = { new Vector3(252.45f, 0, 396), new Vector3(239.21f, 0, 393.38f), new Vector3(228.73f, 0, 396.89f) };

    private void Awake()
    {
        animalType = AnimalType.Donkey;
        SetState(DonkeyState.Idle);
        blackboard.SetValue<bool>("Walking", false);
        blackboard.SetValue<bool>("Eating", false);
        blackboard.SetValue<bool>("Attacking", false);
        hunger = Random.Range(minHunger, maxHunger);
        sanity = Random.Range(minSanity, maxSanity);
        targetFox = foxList[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (foxList.Count > 0)//if a fox is inside the donkeys personal place the donkey loses sanity
        {
            foreach (Fox i in  foxList)
            {
                if (Vector3.Distance(transform.position, i.transform.position) < personalSpace && state != DonkeyState.Attacking)
                { 
                    sanity -= Time.deltaTime; 
                }
            }
        }

        if (sanity <= 0)
        {
            if (state != DonkeyState.Attacking)
            {
                SetState(DonkeyState.Attacking);
            }
        }
        
        switch (state)
        {
            case DonkeyState.Idle:
                idleTimer -= Time.deltaTime;
                if (idleTimer <= 0)
                {
                    SetState(DonkeyState.Walking);
                }
                break;

            case DonkeyState.Walking:
                if (hunger <= 0)
                {
                    GetComponent<NavMeshAgent>().destination = haybales[(int)Random.Range(0, 2)];//picks a random haybale to eat from and walks to it
                }

                if (Vector3.Distance(transform.position, GetComponent<NavMeshAgent>().destination) <= 5)//gets to its destination/going to haybale
                {
                    if (hunger <= 0)// if the cow is needs to eat set state to eating
                    {
                        if (state != DonkeyState.Eating)
                        {
                            SetState(DonkeyState.Eating);
                        }
                    }
                    else if (state != DonkeyState.Idle)
                    {
                        SetState(DonkeyState.Idle);
                    }
                }
                break;

            case DonkeyState.Eating:
                eatingTimer -= Time.deltaTime; 
                if(eatingTimer <= 0)
                {
                    GetComponent<NavMeshAgent>().isStopped = false;
                    SetState(DonkeyState.Walking);
                }
                break;

            case DonkeyState.Attacking:
                attackTimer -= Time.deltaTime;
                if (foxList.Count > 0)
                {
                    targetFox = foxList[0];
                    foreach (Fox i in foxList)
                    {
                        if (Vector3.Distance(transform.position, i.transform.position) <= Vector3.Distance(transform.position, i.transform.position))
                        {
                            targetFox = i;
                        }
                    }
                }
                GetComponent<NavMeshAgent>().destination = targetFox.transform.position;
                if (Vector3.Distance(transform.position, targetFox.transform.position) < 5 || attackTimer <= 0)
                {
                    SetState(DonkeyState.Walking);
                }
                break;
        }

        if (debugText == true)//outputs the states
        {
            Debug.Log(this.gameObject.name.ToString() + " Debug:" +
                "\nState:" + state + " Idle:" + blackboard.GetValue<bool>("Idle").ToString() + " Walking:" + blackboard.GetValue<bool>("Walking").ToString() + " Eating:" + blackboard.GetValue<bool>("Eating").ToString() + " Attacking:" + blackboard.GetValue<bool>("Attacking").ToString());
            Debug.Log("sanity:" + sanity.ToString() + " Attack Timer:" + attackTimer + " targetFox distance:" + Vector3.Distance(targetFox.transform.position,transform.position).ToString());
        }
    }

    protected override void OnStart()
    {
        Initialize(settings);
    }

    private void SetState(DonkeyState newState)
    {
        DonkeyState lastState = state;
        switch (lastState)
        {
            case DonkeyState.Idle:
                blackboard.SetValue<bool>("Idle", false);
                break;

            case DonkeyState.Walking:
                blackboard.SetValue<bool>("Walking", false);
                break;
            case DonkeyState.Eating:
                blackboard.SetValue<bool>("Eating", false);
                break;
            case DonkeyState.Attacking:
                blackboard.SetValue<bool>("Attacking", false);
                break;
        }

        switch (newState)
        {
            case DonkeyState.Idle:
                state = DonkeyState.Idle;
                blackboard.SetValue<bool>("Idle", true);
                idleTimer = Random.Range(minIdle, maxIdle);
                break;

            case DonkeyState.Walking:
                state = DonkeyState.Walking;
                blackboard.SetValue<bool>("Walking", true);
                GetComponent<NavMeshAgent>().destination = new Vector3(Random.Range(175, 300), 0, Random.Range(250, 400));
                GetComponent<NavMeshAgent>().speed = walkSpeed;
                hunger--;
                break;

            case DonkeyState.Eating:
                state = DonkeyState.Eating;
                blackboard.SetValue<bool>("Eating", true);
                eatingTimer = Random.Range(minEating, maxEating);
                hunger = Random.Range(minHunger, maxHunger);
                GetComponent<NavMeshAgent>().isStopped = true;
                break;

            case DonkeyState.Attacking:
                state = DonkeyState.Attacking;
                blackboard.SetValue<bool>("Attacking", true);
                GetComponent<NavMeshAgent>().speed = attackSpeed;
                sanity = Random.Range(-minSanity, maxSanity);
                attackTimer = Random.Range(minAttackTime, maxAttackTime);
                break;
        }
    }
}