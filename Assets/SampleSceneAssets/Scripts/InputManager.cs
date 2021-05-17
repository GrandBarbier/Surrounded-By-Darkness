using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public static KeyCode[] interactions = new KeyCode[2];

    public Button[] interactionButtons = new Button[2];

    private KeyCode currentKeyCode;

    public void Start()
    {
        interactions[0] = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("InteractionKey_00", "E"));

        interactions[1] = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("InteractionKey_01", "None"));
        

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
        /*foreach (var interaction in interactions)
        {
            if (Input.GetKey(interaction))
            {
                Debug.Log("Interaction_01");
                break;
            }
        }*/
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
                PlayerPrefs.SetString("InteractionKey_0" + id, interactions[id].ToString());
                break;
        }
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

    #endregion
}
