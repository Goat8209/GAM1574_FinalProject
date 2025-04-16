using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Spit : MonoBehaviour
{
    // Start is called before the first frame update

    public float direction;
    void Start()
    {
        //direction *= Mathf.Deg2Rad;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 velocity = new Vector3(Mathf.Cos(direction), 0, Mathf.Sin(direction)) * 50;
        Vector3 displacement = velocity * Time.deltaTime;
        transform.transform.position += displacement;
    }
}
