using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Gears : MonoBehaviour
{
    public static Gears gears;

    public PlayerInput playerInput;

    //public InputManager InputManager;

    public Image blackPanel;

    public LayerMask interactionLayer;

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
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        StartCoroutine(LevelManager.FadeDuration(start: new Color(r: 0f, g: 0f, b: 0f, a: 1f), end: new Color(r: 0f, g: 0f, b: 0f, a: 0f), duration: 2f));
    }
    
    void Update()
    {
        
    }
}
