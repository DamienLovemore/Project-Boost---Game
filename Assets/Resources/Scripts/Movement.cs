using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float thrustSpeed = 1000f;
    [SerializeField] private float rotateSpeed;

    private Rigidbody rocketRb;
       
    void Start()
    {
        rocketRb = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    //Makes the rocket fly up
    private void ProcessThrust()
    {
        // -- Going up --
        //While the player is holding the space button the
        //rocket should fly up
        if(Input.GetKey(KeyCode.Space))
        {
            //Determines how fast it should go
            Vector3 acceleration = Vector3.up * thrustSpeed;
            //Make it frame rate independent
            acceleration *= Time.deltaTime;

            //Uses AddRelativeForce instead of the normal AddForce because,
            //AddForce considers the global coordinate system, not the
            //local rotation of the rocket.
            rocketRb.AddRelativeForce(acceleration, ForceMode.Force);
        }
    }

    //Handles the rocket rotation
    private void ProcessRotation()
    {
        // -- Rotating the rocket --
        //While the player is holding the left arrow button
        //the rocket should rotate to the left
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            ApplyRotation(Vector3.forward);
        }
        //While the player is holding the left arrow button
        //the rocket should rotate to the right
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            ApplyRotation(-Vector3.forward);
        }
    }

    //Makes the rocket rotate
    private void ApplyRotation(Vector3 rotationDirection)
    {
        //Makes the rocket rotation to not get buggy when it
        //collides with another object
        //(By disablying it)
        rocketRb.freezeRotation = true;

        //Calculates the rotation speed and make it frame independent
        float rotateAcceleration = rotateSpeed * Time.deltaTime;

        transform.Rotate(rotationDirection * rotateAcceleration);

        //Puts the rotation back to normal
        rocketRb.freezeRotation = false;
    }
}
