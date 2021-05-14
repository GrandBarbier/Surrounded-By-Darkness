using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public GameObject menuPanel;

    public KeyCode[] interactions = new KeyCode[2];

    public Button[] interactionButtons = new Button[2];

    private KeyCode currentKeyCode;

    private bool menu;
    
    void Start()
    {
        interactions[0] = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("InteractionKey", "E"));

        interactions[1] = KeyCode.None;
        

        for (int i = 0; i < interactionButtons.Length; i++)
        {
            int index = i; //fix : Access to Modified Closure
            GetButtonText(interactionButtons[i].gameObject).text = interactions[i].ToString();
            interactionButtons[i].onClick.AddListener(() => GetButtonText(interactionButtons[index].gameObject).SetText("..."));
            interactionButtons[i].onClick.AddListener(() => StartCoroutine(WaitForKey("InteractionKey_00", index)));
        }
    }

    void Update()
    {
        foreach (var interaction in interactions)
        {
            if (Input.GetKey(interaction))
            {
                Debug.Log("Interaction_01");
                break;
            }
        }

        if (Input.GetButtonDown("Cancel"))
        {
            Menu();
        }
    }

    void OnGUI()
    {
        if (Event.current != null && Event.current.isKey)
        {
            currentKeyCode = Event.current.keyCode;
        }
        else
        {
            currentKeyCode = KeyCode.None;
        }
    }
    
    public void Menu()
    {
        if (menu)
        {
            menuPanel.SetActive(false);
        }
        else
        {
            menuPanel.SetActive(true);
        }

        menu = !menu;
    }

    #region ButtonFunc

    public IEnumerator WaitForKey(string keyName, int id = 0)
    {
        //Debug.Log("Waiting for a key");
        yield return new WaitUntil(() => currentKeyCode != KeyCode.None);
        //Debug.Log("Key received");
        
        switch (keyName)
        {
            case "InteractionKey_00" :
                interactions[id] = currentKeyCode;
                GetButtonText(interactionButtons[id].gameObject).text = interactions[id].ToString();
                break;
        }
    }

    /*public void StartWaitForKey(string keyName)
    {
        StartCoroutine(WaitForKey(keyName));
    }*/

    public TextMeshProUGUI GetButtonText(GameObject button)
    {
        Transform child = button.transform.GetChild(0);

        if (child.TryGetComponent(out TextMeshProUGUI t))
        {
            return t;
        }
        else
        {
            Debug.Log(child.name + " doesn't have textMeshPro");
            return null;
        }
    }

    #endregion
}
