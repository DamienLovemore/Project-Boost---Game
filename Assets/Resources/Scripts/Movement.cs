using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float thrustSpeed = 1000f;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private AudioClip mainEngine;

    [SerializeField] private ParticleSystem RocketBoosterParticles;
    [SerializeField] private ParticleSystem LeftThrusterParticles;
    [SerializeField] private ParticleSystem RightThrusterParticles;

    private Rigidbody rocketRb;
    private AudioSource thrustSound;

    void Start()
    {
        rocketRb = GetComponent<Rigidbody>();
        thrustSound = GetComponent<AudioSource>();
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

            PlayThrustSound();
            PlayRocketBoosterEffect();
        }
        //If the rocket is not thrusting then the sound
        //should stop
        else
        {
            thrustSound.Stop();
            RocketBoosterParticles.Stop();
        }
    }

    //If the sound is not playing right now, play it
    //(Waits for the sound to end to play it again,
    //instead of playing it every thrust)
    private void PlayThrustSound()
    {
        if (!thrustSound.isPlaying)
        {
            thrustSound.PlayOneShot(mainEngine);
        }
    }

    //Stops all particles effects when the user finish
    //the level, or crashes
    public void StopAllEffects()
    {
        RocketBoosterParticles.Stop();
        LeftThrusterParticles.Stop();
        RightThrusterParticles.Stop();
    }

    //If the particle is not currently playing, then play it
    //If it is then do nothing
    private void PlayRocketBoosterEffect()
    {
        if (!RocketBoosterParticles.isPlaying)
        {
            RocketBoosterParticles.Play();
        }
    }
   
    private void PlayLeftThrustEffect()
    {
        if (!LeftThrusterParticles.isPlaying)
        {
            LeftThrusterParticles.Play();
        }
    }

    private void PlayRightThrustEffect()
    {
        if (!RightThrusterParticles.isPlaying)
        {
            RightThrusterParticles.Play();
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
            LeftThrusterParticles.Stop();
            PlayRightThrustEffect();
            ApplyRotation(Vector3.forward);
        }
        //While the player is holding the left arrow button
        //the rocket should rotate to the right
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            RightThrusterParticles.Stop();
            PlayLeftThrustEffect();
            ApplyRotation(-Vector3.forward);
        }
        //If it is not steering then it should stop the side
        //thrusters
        else
        {
            LeftThrusterParticles.Stop();
            RightThrusterParticles.Stop();
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
