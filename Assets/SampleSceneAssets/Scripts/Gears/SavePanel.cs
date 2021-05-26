using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SavePanel : MonoBehaviour
{
    public TextMeshProUGUI pathText;
    public string path;

    public RectTransform scrollable;
    public float sensibility = 1f;
    private Vector2 startPos;
    public Vector2 maxPos;
    private float progress;
    
    void Start()
    {
        UpdatePathText(Application.persistentDataPath);

        startPos = scrollable.position;
        maxPos = startPos + maxPos;
    }
    
    void Update()
    {
        progress += -Mouse.current.scroll.ReadValue().y * 0.001f * sensibility;

        progress = Mathf.Clamp(progress, 0f, 1f);

        scrollable.position = Vector2.Lerp(startPos, maxPos, progress);
    }

    public void UpdatePathText(string s)
    {
        path = s;
        pathText.text = $"path : {s}";
    }
}
