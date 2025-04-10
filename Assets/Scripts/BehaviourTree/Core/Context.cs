using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Context
{
    public GameObject gameObject;
    public Transform transform;
    public Animal animal;
    public AnimalType animalType;
    public NavMeshAgent agent;

    // Add other game specific systems here


    public static Context CreateFromGameObject(GameObject gameObject)
    {
        // Fetch all commonly used components
        Context context = new Context();
        context.gameObject = gameObject;
        context.transform = gameObject.transform;
        context.animal = gameObject.GetComponent<Animal>();
        context.animalType = context.animal.GetAnimalType;
        context.agent = context.animal.GetComponentInChildren<NavMeshAgent>();

        // Add whatever else you need here...

        return context;
    }
}
