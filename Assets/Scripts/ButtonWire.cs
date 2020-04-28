using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonWire : MonoBehaviour
{
    public Material off;
    public Material on;

    public Renderer r;

    private void Start()
    {
        r = transform.GetChild(0).GetComponent<Renderer>();
        TurnOff();
    }

    public void TurnOn()
    {
        r.material = on;
    }

    public void TurnOff()
    {
        r.material = off;
    }
}
