using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Gears : MonoBehaviour
{
    public static Gears gears;

    public PlayerInput playerInput;

    public MenuManager menuManager;

    //public InputManager InputManager;

    public LayerMask interactionLayer;

    [Header("References")] 
    public GameObject somkeTrail;

    void Awake()
    {
        if (gears == null)
        {
            gears = this;
            DontDestroyOnLoad(gameObject);
            LanguageSystem.Init();
            //InputManager.Start();
        }
        else
        {
            //gears.menuManager = menuManager;
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        /*if (menuManager?.blackPanel)
        {
            StartCoroutine(LevelManager.FadeDuration(menuManager.blackPanel, start: new Color(r: 0f, g: 0f, b: 0f, a: 1f), end: new Color(r: 0f, g: 0f, b: 0f, a: 0f), duration: 2f));
        }*/
    }
    
    void Update()
    {
        
    }
}
