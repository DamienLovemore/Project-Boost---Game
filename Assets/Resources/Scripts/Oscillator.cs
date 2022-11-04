using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    [SerializeField] private Vector3 movementVector;    
    //How much seconds is a period
    [SerializeField] private float period = 2f;

    private Vector3 startingPosition;
    private float movementFactor;

    void Start()
    {
        this.startingPosition = transform.position;
    }
    
    void Update()
    {
        //Prevent divided by zero error in line 27
        //(The lower the value of period the faster the object move)
        //For float values instead of zero is best to use Epsilon, because
        //floats can have really small values and never be zero)
        if (this.period == Mathf.Epsilon)
            return;

        // Time.time store how much time the game has been played
        // in seconds
        float cycles = Time.time / this.period;

        //A complete lap around a circle
        const float tau = Mathf.PI * 2;
        //Float value that flutuates between -1 and -1
        //(Not just -1 and 1, but values between too)
        float rawSinWave = Mathf.Sin(cycles * tau);

        //Makes the values sits between 0 and 1
        //(And values between, not just 0 and 1)
        movementFactor = (rawSinWave + 1f) / 2f;

        Vector3 offset = movementVector * movementFactor;
        //Place the object in the new location.
        //Based on the movementFactor, and the max position
        //values of the movementVector
        transform.position = startingPosition + offset;
    }
}
