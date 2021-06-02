using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pushableBlock : MonoBehaviour
{
    public Vector3 forceOfPush;
    public float speed;
    public GameObject block;

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKey(KeyCode.A))
        {
            block.transform.position += forceOfPush * (Time.deltaTime * speed);
            /*lancer anim pose des mains
            while (Input.GetKey(KeyCode.A))
            {
                boucler anim effort
                block.transform.position += forceOfPush * (Time.deltaTime * speed);
            }
            lancer anim enlevage des mains
            */
        }
    }
}
