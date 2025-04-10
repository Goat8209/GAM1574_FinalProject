using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class Boid : MonoBehaviour
{
    private Flock flock;

    private Vector3 velocity;

    private Vector3 alignment;
    private Vector3 seperation;
    private Vector3 cohesion;
    private int nearbyFlockmates;

    private const float colorChange = 0.03f;

    public Flock GetFlock
    {
        get { return flock; }
    }

    public Vector3 Alignment { get { return alignment; } set { alignment = value; } }
    public Vector3 Seperation { get { return seperation; } set { seperation = value; } }
    public Vector3 Cohesion { get { return cohesion; } set { cohesion = value; } }
    public int NearbyFlockmates { get { return nearbyFlockmates; } set { nearbyFlockmates = value; } }



    public void InitializeBoid(Flock flock)
    {
        this.flock = flock;
    }

    // Start is called before the first frame update
    void Start()
    {
        float startSpeed = (flock.settings.minSpeed + flock.settings.maxSpeed) * 0.5f;
        velocity = transform.forward * startSpeed;
    }



    // Update is called once per frame
    public void UpdateBoid()
    {
        Vector3 acceleration = Vector3.zero;
        Vector3 location = transform.position;

        // If there's a target set, steer towards it
        if (flock.target != null)
        {
            Vector3 targetDisplacement = (flock.target.transform.position - location);
            acceleration = SteerTowards(targetDisplacement) * flock.settings.targetWeight;
        }

        // Ensure the nearby flockmates value is not zero, to avoid a divide by zero
        if (nearbyFlockmates != 0)
        {
            // Divide the cohesion vector by the number of nearby flockmates
            cohesion /= nearbyFlockmates;

            // Calculate the offset value based on the relative position to the nearby flockmates position
            Vector3 offsetToFlockmatesCentre = (cohesion - transform.position);

            // Based on the values and the 
            Vector3 alignmentForce = SteerTowards(alignment) * flock.settings.alignmentWeight;
            Vector3 cohesionForce = SteerTowards(offsetToFlockmatesCentre) * flock.settings.cohesionWeight;
            Vector3 seperationForce = SteerTowards(seperation) * flock.settings.seperationWeight;

            // Add the alignment, cohesion and seperation forces to the acceleration vector
            acceleration += alignmentForce;
            acceleration += cohesionForce;
            acceleration += seperationForce;
        }

        // Check if the boid is moving towards an obstacle, if they are, caluclate a new direction for the boid
        // to avoid the obstacle(s) and steer the boid away
        if (CheckForCollision())
        {
            Vector3 avoidanceDirection = CalculateAvoidanceDirection();
            Vector3 collisionAvoidForce = SteerTowards(avoidanceDirection) * flock.settings.avoidCollisionWeight;
            acceleration += collisionAvoidForce;
        }

        // Add acceleration multiplied by delta time to the velocity
        velocity += acceleration * Time.deltaTime;

        // Calculate the speed, which is the magnitude of the velocity
        float speed = velocity.magnitude;

        // Calculate the forward vector
        transform.forward = velocity / speed;

        // Clmap the speed between the min and max speed settings and then recalculate the velocity
        speed = Mathf.Clamp(speed, flock.settings.minSpeed, flock.settings.maxSpeed);
        velocity = transform.forward * speed;

        // Calculate the displacement and translate the boid
        Vector3 displacement = velocity * Time.deltaTime;
        location += displacement;

        if(flock.settings.lockAxisY)
        {
            location.y = flock.settings.positionY;
        }

        // Apply the location to the transform
        transform.position = location;
    }

    private bool CheckForCollision()
    {
        RaycastHit hit;
        return Physics.SphereCast(transform.position, flock.settings.boundsRadius, transform.forward, out hit, flock.settings.avoidCollisionDistance, flock.settings.collisionMask);
    }

    private Vector3 CalculateAvoidanceDirection()
    {
        for (int i = 0; i < BoidHelper.directions.Length; i++)
        {
            Vector3 direction = transform.TransformDirection(BoidHelper.directions[i]);
            Ray ray = new Ray(transform.position, direction);
            if (Physics.SphereCast(ray, flock.settings.boundsRadius, flock.settings.avoidCollisionDistance, flock.settings.collisionMask) == false)
            {
                return direction;
            }
        }

        return transform.forward;
    }

    private Vector3 SteerTowards(Vector3 vector)
    {
        Vector3 newVelocity = vector.normalized * flock.settings.maxSpeed - velocity;
        return Vector3.ClampMagnitude(newVelocity, flock.settings.maxSteerForce);
    }
}
