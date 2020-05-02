using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailerTitleScreen : MonoBehaviour
{
    public GameObject portal;

    private void Start()
    {
        GameObject blue = Instantiate(portal);
        blue.transform.position = transform.position;
    }
}
