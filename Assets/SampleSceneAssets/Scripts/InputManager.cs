using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public KeyCode interaction;
    
    public GameObject menuPanel;

    public Button interactionButton;

    public KeyCode currentKeyCode;
    
    void Start()
    {
        interaction = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("InteractionKey", "E"));

        GetButtonText(interactionButton.gameObject).text = interaction.ToString();
    }


    void Update()
    {
        if (Input.GetKey(interaction))
        {
            Debug.Log("Interaction");
        }
    }

    void OnGUI()
    {
        if (Event.current != null && Event.current.isKey)
        {
            currentKeyCode = Event.current.keyCode;
        }
    }

    public IEnumerator WaitForKey(string keyName)
    {
        Debug.Log("Waiting for a key");
        //yield return Event.current;
        yield return new WaitUntil(() => currentKeyCode != KeyCode.None);
        Debug.Log("Event happened");
        
        switch (keyName)
        {
            case "InteractionKey" :
                interaction = currentKeyCode;
                GetButtonText(interactionButton.gameObject).text = interaction.ToString();
                break;
        }
    }

    public void StartWaitForKey(string keyName)
    {
        StartCoroutine(WaitForKey(keyName));
    }

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
}
