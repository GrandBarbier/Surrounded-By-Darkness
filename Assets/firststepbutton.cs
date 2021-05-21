using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firststepbutton : MonoBehaviour
{
    public GameObject firstPartLights;
    public GameObject secondPartLights;
    public bool neverused;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.A) && neverused)
        {
            neverused = false;
            firstPartLights.SetActive(false);
            secondPartLights.SetActive(true);
            //trigger sound
            //trigger animation (ou particle system
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.A) && neverused)
        {
            neverused = false;
            firstPartLights.SetActive(false);
            secondPartLights.SetActive(true);
            //trigger sound
            //trigger animation (ou particle system
        }
    }

}
