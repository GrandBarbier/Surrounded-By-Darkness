using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class SelectionUI : MonoBehaviour
{
    public MenuManager menuManager;

    public float scaleMultiplierX = 1.1f;
    public float scaleMultiplierY = 1.1f;

    public Vector2Int posOnMap;

    public Color idleColor;
    public Color pressedColor;

    private Image _image;
    private RectTransform _rectTransform;

    private Action<InputAction.CallbackContext> action1;
    private Action<InputAction.CallbackContext> action2;

    void Awake()
    {
        //Gears.gears.playerInput.actions["MoveMenu"].performed += context => MoveMapPosition(
            //new Vector2Int((int) context.ReadValue<Vector2>().x, (int) context.ReadValue<Vector2>().y));
        
        //Gears.gears.playerInput.actions["Enter"].performed += context => TriggerSelection();
        
        action1 = context => TriggerSelection();
        action2 = context => MoveMapPosition(new Vector2Int((int) context.ReadValue<Vector2>().x, (int) context.ReadValue<Vector2>().y));

        Gears.gears.playerInput.actions["Enter"].performed += action1;
        Gears.gears.playerInput.actions["MoveMenu"].performed += action2;

        //LevelManager.preLoadingScene += () => Gears.gears.playerInput.actions["Enter"].performed -= action1;
        //LevelManager.preLoadingScene += () => Gears.gears.playerInput.actions["MoveMenu"].performed -= action2;
        
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
    }
    
    void OnDestroy()
    {
        if (Gears.gears.playerInput != null)
        {
            Gears.gears.playerInput.actions["Enter"].performed -= action1;
            Gears.gears.playerInput.actions["MoveMenu"].performed -= action2;
        }
    }

    void Start()
    {
        
    }
    
    void Update()
    {
       
    }

    public void TriggerSelection()
    {
        if (menuManager.currentMap.map[posOnMap.x, posOnMap.y].TryGetComponent(out Button button) && button != null)
        {
            button.onClick?.Invoke();
        }

        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(SelectionColor());
        }
    }

    public IEnumerator SelectionColor()
    {
        _image.color = pressedColor;
        
        yield return new WaitForSecondsRealtime(0.1f);
        
        //Debug.Log("set color to idle");
        _image.color = idleColor;
    }

    public void MoveMapPosition(Vector2Int vector2I)
    {
        Vector2Int vector2Int = new Vector2Int(vector2I.x, -vector2I.y);
        
        if (menuManager.currentMap.map.GetLength(0) > posOnMap.x + vector2Int.x)
        {
            if (posOnMap.x + vector2Int.x >= 0)
            {
                posOnMap.x += vector2Int.x;
            }
            else
            {
                posOnMap.x = menuManager.currentMap.map.GetLength(0) - 1;
            }
        }
        else
        {
            posOnMap.x = 0;
        }

        if (menuManager.currentMap.map.GetLength(1) > posOnMap.y + vector2Int.y)
        {
            if (posOnMap.y + vector2Int.y >= 0)
            {
                posOnMap.y += vector2Int.y;
            }
            else
            {
                posOnMap.y = menuManager.currentMap.map.GetLength(1) - 1;
            }
        }
        else
        {
            posOnMap.y = 0;
        }
        
        if (menuManager.currentMap.map[posOnMap.x, posOnMap.y] != null && menuManager.currentMap.map[posOnMap.x, posOnMap.y].gameObject.activeSelf)
        {
           ScaleSelection();
        }
        else
        {
            MoveMapPosition(vector2I);
        }
    }
    
    public void ScaleSelection()
    {
        _rectTransform = GetComponent<RectTransform>();
        _rectTransform.position = menuManager.currentMap.map[posOnMap.x, posOnMap.y].position;
        _rectTransform.sizeDelta = menuManager.currentMap.map[posOnMap.x, posOnMap.y].sizeDelta;
        
        Vector3 v = AdaptScale(menuManager.currentMap.map[posOnMap.x, posOnMap.y].gameObject, menuManager.currentMap.map[posOnMap.x, posOnMap.y].localScale);
        _rectTransform.localScale = new Vector3(v.x * scaleMultiplierX, v.y * scaleMultiplierY);
        //Debug.Log(vector2Int + " -> " + posOnMap);
    }

    public Vector3 AdaptScale(GameObject go, Vector3 scale)
    {
        if (go.transform.parent != transform.parent)
        {
            Vector3 childScale = new Vector3(scale.x * go.transform.parent.localScale.x, scale.y * go.transform.parent.localScale.y);
            //Debug.Log($"ChildScale : {childScale}");
            return AdaptScale(go.transform.parent.gameObject, childScale);
        }
        
        //Debug.Log($"finale scale : {scale}");
        return scale;
    }
}
