using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private float loadDelay= 2f;
    [SerializeField] private AudioClip levelFinish;
    [SerializeField] private AudioClip crashSFX;

    [SerializeField] private ParticleSystem levelFinishParticles;
    [SerializeField] private ParticleSystem crashParticles;

    private AudioSource audioPlayer;

    private bool isCollisionDisabled = false;
    private bool isTransitioning = false;

    void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if ((isTransitioning) || (isCollisionDisabled))
            return;

        //The tag of the object that we collided into
        string collisionObjectTag = collision.gameObject.tag;
        switch (collisionObjectTag)
        {
            case "Friendly":                
                break;
            case "Finish":
                StartCoroutine(LevelFinish());
                break;            
            default:
                StartCoroutine(LevelRestart());
                break;
        }
    }

    //Things to do when finishing the level or losing it
    private void StartEndSequence()
    {
        this.isTransitioning = true;
        //Makes the rocket engine stop making sound
        audioPlayer.Stop();
        //Stops all the particles effects that are playing
        GetComponent<Movement>().StopAllEffects();
        //Disable rocket movement
        GetComponent<Movement>().enabled = false;
    }

    //Disables the rocket collision system
    public void DisableRocketCollision()
    {
        this.isCollisionDisabled = true;
    }

    public IEnumerator LevelFinish()
    {
        StartEndSequence();
        audioPlayer.PlayOneShot(levelFinish);
        levelFinishParticles.Play();
        //Awaits for an amount of seconds before starting the
        //next level
        yield return new WaitForSeconds(this.loadDelay);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        //If it is in the last level then it should restart
        if (currentSceneIndex == SceneManager.sceneCountInBuildSettings-1)
        {
            nextSceneIndex = 0;
        }
       
        SceneManager.LoadScene(nextSceneIndex);
    }

    //Restart the level when the player loses
    public IEnumerator LevelRestart()
    {
        StartEndSequence();
        audioPlayer.PlayOneShot(crashSFX);
        crashParticles.Play();
        //Awaits for an amount of seconds before restarting the level
        yield return new WaitForSeconds(this.loadDelay);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentSceneIndex);
    }
}
