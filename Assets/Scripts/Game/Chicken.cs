using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Chicken : Animal
{
    [SerializeField] private Blackboard blackboard;

    Target target;

    private void Awake()
    {
        animalType = AnimalType.Chicken;
    }

    private void Update()
    {

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
        blackboard.SetOrAddValue<bool>("CanPeck", false);
        target = GetComponent<Target>();
    }
}
