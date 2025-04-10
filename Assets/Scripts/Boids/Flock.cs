using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Flock : MonoBehaviour
{
    private const int kThreadGroupSize = 1024;

    public ComputeShader computeShader;
    public BoidSettings settings;
    public GameObject boidPrefab;
    public Target target;
    public int boidsToSpawn = 100;
    public float spawnRadius = 10.0f;

    private bool isPaused = false;

    private List<Boid> boids = new List<Boid>();

    public List<Boid> Boids { get { return boids; } }

    public bool IsPaused
    {
        get { return isPaused; }
        set { isPaused = value; }
    }

    public struct BoidData
    {
        public Vector3 position;
        public Vector3 direction;

        public Vector3 alignment;
        public Vector3 seperation;
        public Vector3 cohesion;
        public int nearbyFlockmates;

        public static int Size
        {
            get
            {
                return sizeof(float) * 3 * 5 + sizeof(int);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Loop through spawn the requested boids
        for (int i = 0; i < boidsToSpawn; i++)
        {
            boids.Add(SpawnBoid());
        }
    }

    private Boid SpawnBoid()
    {
        // Instantiate the boid prefab
        GameObject gameObject = Instantiate(boidPrefab);

        // Get the boid component and invoke the InitializeBoid method, passing in the flock and the settings
        Boid boid = gameObject.GetComponent<Boid>();
        boid.InitializeBoid(this);

        // Randomize the Boid's position and direction
        boid.transform.position = transform.position + Random.insideUnitSphere * spawnRadius;
        boid.transform.forward = Random.insideUnitSphere;
        return boid;
    }

    public void SpawnFlock(ComputeShader computeShader, BoidSettings settings, Target target)
    {
        this.computeShader = computeShader;
        this.settings = settings;
        this.target = target;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused == false)
        {
            int boidsCount = boids.Count;
            if (boidsCount > 0)
            {
                // Create the array of boid data
                BoidData[] boidData = new BoidData[boidsCount];

                // Set the boidData's position and direction
                for (int i = 0; i < boidsCount; i++)
                {
                    boidData[i].position = boids[i].transform.position;
                    boidData[i].direction = boids[i].transform.forward;
                }

                // Create the ComputeBuffer, pass in the count and the stride and then set the buffer data
                ComputeBuffer boidBuffer = new ComputeBuffer(boidsCount, BoidData.Size);
                boidBuffer.SetData(boidData);

                // Set the computer shader's buffer, boidsCount, viewRadius and avoidRadius values
                computeShader.SetBuffer(0, "boids", boidBuffer);
                computeShader.SetInt("boidsCount", boidsCount);
                computeShader.SetFloat("viewRadius", settings.viewRadius);
                computeShader.SetFloat("avoidRadius", settings.avoidRadius);

                // Dispatch the compute shader - this runs the computer shader, one for each boid run in parallel
                int threadGroups = Mathf.CeilToInt(boidsCount / (float)kThreadGroupSize);
                computeShader.Dispatch(0, threadGroups, 1, 1);

                // Get the boidData from the compute buffer
                boidBuffer.GetData(boidData);

                // Loop through and copy the alignment,seperation, cohesion and nearbyFlockmates values
                // from the boidData to the list of boids
                for (int i = 0; i < boidsCount; i++)
                {
                    boids[i].Alignment = boidData[i].alignment;
                    boids[i].Seperation = boidData[i].seperation;
                    boids[i].Cohesion = boidData[i].cohesion;
                    boids[i].NearbyFlockmates = boidData[i].nearbyFlockmates;

                    boids[i].UpdateBoid();
                }

                // Release the computer buffer
                boidBuffer.Release();
            }
        }
    }

    void LateUpdate()
    {
        if (isPaused == false)
        {
            // Did the number of boids to spawn increase in the editor, if so spawn new boids
            if (boids.Count < boidsToSpawn)
            {
                int newBoids = boidsToSpawn - boids.Count;
                for (int i = 0; i < newBoids; i++)
                {
                    boids.Add(SpawnBoid());
                }
            }
        }
    }
}
