using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/*
 * Date created: 4/30/2020
 * Creator: Nate Smith
 * 
 * Description: Transitions to the next level.
 * Place onto a trigger collider at the end of the level.
 * When the player enters the trigger, the next scene is loaded.
 */
public class LevelTransition : MonoBehaviour
{

    public string nextLevel;

    private void Start()
    {
        if (nextLevel == "")
        {
            Debug.Log("ERROR: Next Level in " + name + " has not been set.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            SceneManager.LoadScene(nextLevel, LoadSceneMode.Single);
    }
}
