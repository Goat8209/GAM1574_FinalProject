using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public CatmullRomPath path;
    public float speed = 8.0f;

    private Vector3 previousPosition;
    private int currentIndex = 0;
    private bool isPaused = false;

    public Vector3 PreviousPosition
    {
        get { return previousPosition; }
    }

    public bool IsPaused
    {
        get { return isPaused; }
        set { isPaused = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        Vector3 location = path.Points[currentIndex];
        transform.position = location;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused == false)
        {
            Vector3 location = transform.position;
            Vector3 targetLocation = GetNextPoint();
            Vector3 direction = (targetLocation - location).normalized;
            float distance = Vector3.Distance(location, targetLocation);
            float distanceToTravel = speed * Time.deltaTime;

            Vector3 displacement;

            if (distanceToTravel > distance)
            {
                while (distanceToTravel >= 0.0f)
                {
                    if (distance > distanceToTravel)
                    {
                        displacement = direction * distanceToTravel;
                    }
                    else
                    {
                        displacement = direction * distance;
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
                displacement = direction * distanceToTravel;
                location += displacement;
            }

            previousPosition = transform.position;
            transform.position = location;
        }
    }

    Vector3 GetNextPoint()
    {
        int nextIndex = CalculateIndexIncrement(currentIndex);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.25f);
    }
}
