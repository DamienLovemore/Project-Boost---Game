using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortcutHandler : MonoBehaviour
{
    private CollisionHandler collisionHandler;

    //Cache all the necessary scripts references
    void Start()
    {
        collisionHandler = FindObjectOfType<CollisionHandler>();
    }
    
    //Listen for the user to press the shortkeys
    void Update()
    {
        //Toggle screen mode (Fullscreen//Windowed)
        if (Input.GetKeyDown(KeyCode.F11))
        {
            FullScreenMode currentScreenMode = Screen.fullScreenMode;

            if ((currentScreenMode != FullScreenMode.Windowed) || (currentScreenMode != FullScreenMode.MaximizedWindow))
            {
                Screen.fullScreenMode = FullScreenMode.Windowed;
            }
            else
            {
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
            }
        }
        //When it press the Esc key it should end the
        //game
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        //When the user press the key L, it should pass
        //to the next level
        else if (Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(collisionHandler.LevelFinish());
        }
        //When the user press the key C, it should disable
        //the rocket collision system
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionHandler.DisableRocketCollision();
        }
    }
}
