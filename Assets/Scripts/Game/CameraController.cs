using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public CatmullRomPath path;
    public Transform cameraTarget;
    public int startingIndex = 0;

    private new Camera camera;
    private const float kSpeed = 20.0f;
    private float movement = 1.0f;
    private int currentIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();

        if (startingIndex >= 0 && startingIndex < path.Points.Count)
            currentIndex = startingIndex;

        Vector3 location = path.Points[currentIndex];
        camera.transform.position = location;
        camera.transform.rotation = Quaternion.LookRotation(cameraTarget.position - location);
    }

    // Update is called once per frame
    void Update()
    {
        if (movement > 0.0f)
        {
            HandleForwardMovement();
        }
        else if (movement < 0.0f)
        {
            HandleBackwardMovement();
        }
    }

    public void HandleForwardMovement()
    {
        Vector3 location = transform.position;
        Vector3 targetLocation = GetNextPoint();
        Vector3 direction = (targetLocation - location).normalized;
        float distance = Vector3.Distance(location, targetLocation);
        float distanceToTravel = kSpeed * Time.deltaTime;

        Vector3 displacement;

        if (distanceToTravel > distance)
        {
            while (distanceToTravel >= 0.0f)
            {
                if (distance > distanceToTravel)
                {
                    displacement = direction * movement * distanceToTravel;
                }
                else
                {
                    displacement = direction * movement * distance;
                }

                location += displacement;

                distanceToTravel -= distance;

                currentIndex = CalculateIndexIncrement(currentIndex);
                targetLocation = GetNextPoint();
                direction = (targetLocation - location).normalized;
                distance = Vector3.Distance(location, targetLocation);
            }
        }
        else
        {
            displacement = direction * movement * distanceToTravel;
            location += displacement;
        }

        camera.transform.position = location;
        camera.transform.rotation = Quaternion.LookRotation(cameraTarget.position - location);
    }

    public void HandleBackwardMovement()
    {
        Vector3 location = transform.position;
        Vector3 targetLocation = GetNextPoint();
        Vector3 direction = (location - targetLocation).normalized;
        float distance = Vector3.Distance(location, targetLocation);
        float distanceToTravel = kSpeed * Time.deltaTime;

        Vector3 displacement;

        if (distanceToTravel > distance)
        {
            while (distanceToTravel >= 0.0f)
            {
                if (distance > distanceToTravel)
                {
                    displacement = direction * movement * distanceToTravel;
                }
                else
                {
                    displacement = direction * movement * distance;
                }

                location += displacement;

                distanceToTravel -= distance;

                currentIndex = CalculateIndexDecrement(currentIndex);
                targetLocation = GetNextPoint();
                direction = (targetLocation - location).normalized;
                distance = Vector3.Distance(location, targetLocation);
            }
        }
        else
        {
            displacement = direction * movement * distanceToTravel;
            location += displacement;
        }

        camera.transform.position = location;
        camera.transform.rotation = Quaternion.LookRotation(cameraTarget.position - location);
    }

    Vector3 GetNextPoint()
    {
        int nextIndex = 0;

        if (movement > 0.0f)
        {
            nextIndex = CalculateIndexIncrement(currentIndex);
        }
        else if (movement < 0.0f)
        {
            nextIndex = CalculateIndexDecrement(currentIndex);
        }

        return path.Points[nextIndex];
    }

    private int CalculateIndexIncrement(int currentIndex)
    {
        int index = currentIndex + 1;
        if (index >= path.Points.Count)
        {
            index = 0;
        }
        return index;
    }

    private int CalculateIndexDecrement(int currentIndex)
    {
        int index = currentIndex - 1;
        if (index < 0)
        {
            index = path.Points.Count - 1;
        }
        return index;
    }
}
