using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Date created: 4/21/2020
 * Creator: Nate Smith
 * 
 * Description: Game Manager.
 * This a single instance static object - There should only be 1 GameManager.
 * Manages and controls game various functionalities.
 */
public class GameManager : MonoBehaviour
{
    // Static instance of the object.
    public static GameManager instance = null;

    public bool debug;

    private void Awake()
    {
        // Ensure that there is only one instance of the GameManager.
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    private void Update()
    {
        // DEBUG: Press escape to pause the editor or exit the game.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Break();
            Application.Quit();
        }
    }
}
