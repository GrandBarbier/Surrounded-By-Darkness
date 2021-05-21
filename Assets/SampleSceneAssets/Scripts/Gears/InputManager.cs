using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public enum Keys {Interaction, Forward, Left, BackWard, Right }
    
    public static KeyCode[] interactionKeys = new KeyCode[2];

    public GameObject interaction;
    private Button[] interactionButtons;
    
    public static KeyCode[] forwardKeys = new KeyCode[2];

    public GameObject forward;
    private Button[] forwardButtons;
    
    public static KeyCode[] LeftKeys = new KeyCode[2];

    public GameObject left;
    private Button[] leftButtons;

    public static KeyCode[] backWardKeys = new KeyCode[2];

    public GameObject backWard;
    private Button[] backWardButtons;

    public static KeyCode[] rightKeys = new KeyCode[2];

    public GameObject right;
    private Button[] rightButtons;

    private KeyCode currentKeyCode;

    public void Start()
    {
        interactionButtons = interaction.GetComponentsInChildren<Button>();
        forwardButtons = forward.GetComponentsInChildren<Button>();
        leftButtons = left.GetComponentsInChildren<Button>();
        backWardButtons = backWard.GetComponentsInChildren<Button>();
        rightButtons = right.GetComponentsInChildren<Button>();

        SetUpKeys(interactionKeys, "InteractionKey_0", new List<string>{"E", "None"});
        SetUpKeys(forwardKeys, "Forward_0", new List<string>{"Z", "None"});
        SetUpKeys(LeftKeys, "Left_0", new List<string>{"Q", "None"});
        SetUpKeys(backWardKeys, "BackWard_0", new List<string>{"S", "None"});
        SetUpKeys(rightKeys, "Right_0", new List<string>{"D", "None"});

        SetUpButtonArray(interactionButtons, interactionKeys, Keys.Interaction);
        SetUpButtonArray(forwardButtons, forwardKeys, Keys.Interaction);
        SetUpButtonArray(leftButtons, LeftKeys, Keys.Left);
        SetUpButtonArray(backWardButtons, backWardKeys, Keys.BackWard);
        SetUpButtonArray(rightButtons, rightKeys, Keys.Right);
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

    public void SetUpKeys(KeyCode[] keys, string keyName, List<string> defaultValue)
    {
        for (int i = 0; i < keys.Length; i++)
        {
            keys[i] = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(keyName + i, defaultValue[i]));
        }
    }

    public void SetUpButtonArray(Button[] buttons, KeyCode[] keys, Keys keyName)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i; //fix : Access to Modified Closure
            GetButtonText(buttons[i].gameObject).text = keys[i].ToString();
            buttons[i].onClick.AddListener(() => GetButtonText(buttons[index].gameObject).SetText("..."));
            buttons[i].onClick.AddListener(() => StartCoroutine(WaitForKey(keyName, index)));
        }
    }

    public IEnumerator WaitForKey(Keys keyName, int id = 0)
    {
        //Debug.Log("Waiting for a key");
        yield return new WaitUntil(() => currentKeyCode != KeyCode.None);
        //Debug.Log("Key received");
        
        switch (keyName)
        {
            case Keys.Interaction :
                interactionKeys[id] = currentKeyCode;
                GetButtonText(interactionButtons[id].gameObject).text = interactionKeys[id].ToString();
                PlayerPrefs.SetString("InteractionKey_0" + id, interactionKeys[id].ToString());
                break;
            case Keys.Forward :
                forwardKeys[id] = currentKeyCode;
                GetButtonText(forwardButtons[id].gameObject).text = forwardKeys[id].ToString();
                PlayerPrefs.SetString("Forward_0" + id, forwardKeys[id].ToString());
                break;
            case Keys.Left :
                LeftKeys[id] = currentKeyCode;
                GetButtonText(leftButtons[id].gameObject).text = LeftKeys[id].ToString();
                PlayerPrefs.SetString("Left_0" + id, LeftKeys[id].ToString());
                break;
            case Keys.BackWard :
                backWardKeys[id] = currentKeyCode;
                GetButtonText(backWardButtons[id].gameObject).text = backWardKeys[id].ToString();
                PlayerPrefs.SetString("Backward_0" + id, backWardKeys[id].ToString());
                break;
            case Keys.Right :
                rightKeys[id] = currentKeyCode;
                GetButtonText(rightButtons[id].gameObject).text = rightKeys[id].ToString();
                PlayerPrefs.SetString("Right_0" + id, rightKeys[id].ToString());
                break;
        }
    }

    public static TextMeshProUGUI GetButtonText(GameObject button)
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
