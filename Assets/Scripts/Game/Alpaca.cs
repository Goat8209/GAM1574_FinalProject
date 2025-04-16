using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Alpaca : Animal
{
    [SerializeField] private Blackboard blackboard;
    [SerializeField] private AnimalSettings settings;

    public Spit spitPrefab;

    public Donkey donk;
    bool isSpittin = false;
    float spitTimer = 5;
    float targetAngle;
    float rotationSpeed = 20;
    private void Awake()
    {
        animalType = AnimalType.Alpaca;
    }

    // Update is called once per frame
    void Update()
    {
        if (spitTimer < 0 && !isSpittin)
        {
            Vector3 target = donk.transform.position;
            Vector3 relative = transform.InverseTransformPoint(target);
            targetAngle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
            isSpittin = true;
        }

        else if(isSpittin)
        {
            transform.Rotate( 0, targetAngle, 0);
            Spit spit = Instantiate(spitPrefab, transform.position, Quaternion.identity);
            spit.direction = targetAngle;
            
            spitTimer = 5;
            isSpittin = false;
        }

        else
        {
            spitTimer -= Time.deltaTime;
        }
    }

    protected override void OnStart()
    {
        Initialize(settings);
    }
}
