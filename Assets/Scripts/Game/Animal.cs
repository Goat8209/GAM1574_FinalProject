using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AnimalType
{
    Chicken,
    Chick,
    Alpaca,
    Bull,
    Donkey,
    Fox
}

public class Animal : MonoBehaviour
{
    protected AnimalType animalType;

    public AnimalType GetAnimalType
    {
        get { return animalType; }
    }

    protected Animator animator;
    protected NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        OnStart();

        animator = GetComponentInChildren<Animator>();
    }

    public void PlayAnimation(string animationState)
    {
        animator.Play(animationState);
    }

    protected void Initialize(AnimalSettings settings)
    {
        agent = GetComponentInChildren<NavMeshAgent>();

        if (settings != null && agent != null)
        {
            agent.speed = settings.speed;
            agent.acceleration = settings.acceleration;
            agent.angularSpeed = settings.angularSpeed;
            agent.stoppingDistance = settings.stoppingDistance;
        }
    }

    protected virtual void OnStart()
    {
        // Override this in a derived class to be notified when start is called
    }
}
