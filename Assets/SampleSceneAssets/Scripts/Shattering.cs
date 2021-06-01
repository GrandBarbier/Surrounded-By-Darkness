using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shattering : MonoBehaviour
{
    //public GameObject destroyedVersion;

    private GameObject[] childs;
    private bool shatered;

    void Start()
    {
        childs = new GameObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            childs[i] = transform.GetChild(i).gameObject;
        }
    }
    void Shatter()
    {
        /*if (destroyedVersion)
        {
            Debug.Log("Shatter");
            GameObject g = Instantiate(destroyedVersion, transform.position, transform.rotation);
            g.transform.localScale = transform.localScale;
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("No Destroyed version");
        }*/

        if (!shatered)
        {
            foreach (var child in childs)
            {
                Rigidbody r = child.AddComponent<Rigidbody>();
                r.AddForce(new Vector3(0, -500, 0));
            }
            
            shatered = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log($"Collision : {collision.gameObject.name}");
        if (collision.gameObject.GetComponent<SphereCollider>())
        {
            Shatter();
        }
    }
}
