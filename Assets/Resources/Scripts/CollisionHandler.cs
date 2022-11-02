using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private float loadDelay= 2f;
    [SerializeField] private AudioClip levelFinish;
    [SerializeField] private AudioClip crashSFX;

    private AudioSource audioPlayer;

    void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
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
        //Makes the rocket engine stop making sound
        GetComponent<Movement>().StopThrustSound();
        //Disable rocket movement
        GetComponent<Movement>().enabled = false;
    }

    private IEnumerator LevelFinish()
    {
        StartEndSequence();
        audioPlayer.PlayOneShot(levelFinish);
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
    private IEnumerator LevelRestart()
    {
        StartEndSequence();
        audioPlayer.PlayOneShot(crashSFX);
        //Awaits for an amount of seconds before restarting the level
        yield return new WaitForSeconds(this.loadDelay);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentSceneIndex);
    }
}
