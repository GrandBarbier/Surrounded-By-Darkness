using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gears : MonoBehaviour
{
    public static Gears gears;

    void Awake()
    {
        if (gears == null)
        {
            gears = this;
            DontDestroyOnLoad(gameObject);
            LanguageSystem.Init();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }
}
