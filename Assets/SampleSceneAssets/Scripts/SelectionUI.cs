using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionUI : MonoBehaviour
{
    public MenuManager menuManager;

    public float scaleMultiplierX = 1.1f;
    public float scaleMultiplierY = 1.1f;

    public Vector2Int posOnMap;

    private RectTransform _rectTransform;

    void Awake()
    {
        Gears.gears.playerInput.actions["MoveMenu"].performed += context => MoveMapPosition(
            new Vector2Int((int) context.ReadValue<Vector2>().x, (int) context.ReadValue<Vector2>().y));

        Gears.gears.playerInput.actions["Enter"].performed += context => TriggerSelection();
        
        _rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        
    }
    
    void Update()
    {
        /*foreach (var interaction in InputManager.forwardKeys)
        {
            if (Input.GetKeyDown(interaction))
            {
                MoveMapPosition(new Vector2Int(0,-1));
                break;
            }
        }
        foreach (var interaction in InputManager.LeftKeys)
        {
            if (Input.GetKeyDown(interaction))
            {
                MoveMapPosition(new Vector2Int(-1,0));
                break;
            }
        }
        foreach (var interaction in InputManager.backWardKeys)
        {
            if (Input.GetKeyDown(interaction))
            {
                MoveMapPosition(new Vector2Int(0,1));
                break;
            }
        }
        foreach (var interaction in InputManager.rightKeys)
        {
            if (Input.GetKeyDown(interaction))
            {
                MoveMapPosition(new Vector2Int(1,0));
                break;
            }
        }*/
    }

    public void TriggerSelection()
    {
        if (menuManager.currentMap.map[posOnMap.x, posOnMap.y].TryGetComponent(out Button button))
        {
            button.onClick?.Invoke();
        }
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

        if (menuManager.currentMap.map[posOnMap.x, posOnMap.y] != null)
        {
           ScaleSelection();
        }
        else
        {
            MoveMapPosition(vector2I);
        }
    }

    public Vector3 AdaptScale(GameObject go, Vector3 scale)
    {
        if (go.transform.parent != transform.parent)
        {
            Vector3 childScale = new Vector3(scale.x * go.transform.parent.localScale.x, scale.y * go.transform.parent.localScale.y);
            //Debug.Log($"ChildScale : {childScale}");
            return AdaptScale(go.transform.parent.gameObject, childScale);
        }
        else
        {
            //Debug.Log($"finale scale : {scale}");
            return scale;
        }
    }

    public void ScaleSelection()
    {
        _rectTransform.position = menuManager.currentMap.map[posOnMap.x, posOnMap.y].position;
        _rectTransform.sizeDelta = menuManager.currentMap.map[posOnMap.x, posOnMap.y].sizeDelta;

        Vector3 v = AdaptScale(menuManager.currentMap.map[posOnMap.x, posOnMap.y].gameObject, menuManager.currentMap.map[posOnMap.x, posOnMap.y].localScale);
        _rectTransform.localScale = new Vector3(v.x * scaleMultiplierX, v.y * scaleMultiplierY);
        //Debug.Log(vector2Int + " -> " + posOnMap);
    }
}
