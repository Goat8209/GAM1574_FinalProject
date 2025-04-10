using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chick : Animal
{
    private void Awake()
    {
        animalType = AnimalType.Chick;
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override void OnStart()
    {

    }
}
