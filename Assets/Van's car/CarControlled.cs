using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(NeurolN))]

public class CarControlled : MonoBehaviour
{
    //Reverse the position at the beginning (Respawn point)
    private Vector3 startPos;
    private Vector3 startRotation;
    private NeurolN network;

    [Range(-1f, 1f)]
    public float a, t; //accerlation, turning speed

    public float timing = 0f;

    [Header("Fitness")]
    public float overallFitness;
    public float distanceM = 1.4f;
    public float avgspeedM = 0.2f;
    public float sensorM = 0.1f;

    [Header("Network Option: ")]
    public int Layers = 1;
    public int neurons = 10;


    private Vector3 lastPos;
    private float totalDistance;
    private float avgSpeed;

    private float aSensor, bSensor, cSensor;

    private void Awake()
    {
        startPos = transform.position;
        startRotation = transform.eulerAngles;

        network = GetComponent<NeurolN>();

        //test code
        //network.Initialise(Layers, neurons);

    }
    public void ResetNetwork(NeurolN net)
    {
        network = net;
        Reset();
    }
    public void Reset()
    {
        //test code (Remove later on)
       // network.Initialise(Layers, neurons);
        //--------------------
		//-----------------------
        timing = 0f;
        totalDistance = 0f;
        avgSpeed = 0f;
        lastPos = startPos;
        overallFitness = 0f;
        transform.position = startPos;
        transform.eulerAngles = startRotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "CollideWall")
        {
           // print("YES! Successfully detect the collide wall!!");
            CarDeath();
            //Reset();
        }
    }

    private void CarDeath()
    {
        GameObject.FindObjectOfType<GeneticManager>().Death(overallFitness,network);
    }
    private void FixedUpdate()
    {
        InputSensors();
        lastPos = transform.position;


        //Neurol network code
        (a, t) = network.RunNetwork(aSensor, bSensor, cSensor);
        //-----------------------
        MoveCar(a, t);
        timing += Time.deltaTime;
        Calculatefitness();
    }

    private void Calculatefitness()
    {
        totalDistance += Vector3.Distance(transform.position,lastPos);
        avgSpeed = totalDistance / timing;
        overallFitness = (totalDistance*distanceM) + (avgSpeed*avgspeedM) + (((aSensor+bSensor+cSensor) /3) * sensorM);
        
        //if the car is stuck then reset
        if(timing >20 && overallFitness < 40)
        {
            CarDeath();
        }
        
        if(overallFitness >= 1000)
        {
            CarDeath();
        }

    }

    private void InputSensors()
    {
        Vector3 a = (transform.forward + transform.right); //raycast forward to the right side
        Vector3 b = (transform.forward); //raycast forward
        Vector3 c = (transform.forward - transform.right); //raycast forward to the left side

        //Ray tracker
        Ray r = new Ray(transform.position, a);
        RaycastHit hit;

        //calculate distance between wall and car
        if(Physics.Raycast(r,out hit,80))
        {
            aSensor = hit.distance / 130;
            Debug.DrawLine(r.origin,hit.point,Color.red);
           // print("A: " + hit.distance);
        }
        r.direction = b;
        if (Physics.Raycast(r, out hit, 80))
        {
            bSensor = hit.distance / 130;
            Debug.DrawLine(r.origin, hit.point, Color.red);
           // print("B: " + hit.distance);
        }
        r.direction = c;
        if (Physics.Raycast(r, out hit, 80))
        {
            cSensor = hit.distance / 130;
            Debug.DrawLine(r.origin, hit.point, Color.red);
           // print("C: " + hit.distance); //bring out the distance between the object and car
        }

    }

    private Vector3 input;
    public void MoveCar(float vertical, float horizontal)
    {
        input = Vector3.Lerp(Vector3.zero, new Vector3(0,0,vertical * 11.4f),0.02f);
        input = transform.TransformDirection(input);
        transform.position += input;

        transform.eulerAngles += new Vector3(0, (horizontal * 90) * 0.02f, 0);
    }
}
