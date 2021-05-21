using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpTorch : MonoBehaviour
{
    public GameObject player;
    public GameObject torchHandPos;
    public float maxDistToPickUp;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("quelqu'un stay ici");
        if (Input.GetKeyDown(KeyCode.F) && player.GetComponent<PlaceTorch>().torchOnGround)
        {
            Debug.Log("pickup");
            gameObject.transform.parent = player.transform;
            gameObject.transform.position = torchHandPos.transform.position;
            gameObject.transform.rotation = torchHandPos.transform.rotation;
            player.GetComponent<PlaceTorch>().torchOnGround = false;
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("quelqu'un stay ici");
        if (Input.GetKeyDown(KeyCode.F) && player.GetComponent<PlaceTorch>().torchOnGround)
        {
            Debug.Log("pickup");
            gameObject.transform.parent = player.transform;
            gameObject.transform.position = torchHandPos.transform.position;
            gameObject.transform.rotation = torchHandPos.transform.rotation;
            player.GetComponent<PlaceTorch>().torchOnGround = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
