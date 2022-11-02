using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private float restartDelay= 2f;

    private Movement rocketController;

    void Start()
    {
        rocketController = GetComponent<Movement>();
    }

    void OnCollisionEnter(Collision collision)
    {
        //The tag of the object that we collided into
        string collisionObjectTag = collision.gameObject.tag;

        switch (collisionObjectTag)
        {
            case "Friendly":
                Debug.Log("This thing is friendly");
                break;
            case "Finish":
                Debug.Log("Congrats, yo, you finished!");
                break;
            case "Fuel":
                Debug.Log("You picked up fuel");
                break;
            default:
                StartCoroutine(LevelRestart());
                break;
        }
    }

    //Restart the level when the player loses
    private IEnumerator LevelRestart()
    {
        //Disable rocket thrust movement because it was destroyed
        rocketController.SetRocketDestroyed();
        //Awaits for an amount of seconds before restarting the level
        yield return new WaitForSeconds(this.restartDelay);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentSceneIndex);
    }
}
