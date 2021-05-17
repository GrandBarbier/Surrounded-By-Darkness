using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject menuPanel;
    private bool menu;
    
    public GameObject parametersPanel;
    public GameObject inputsPanel;
    public GameObject savePanel;

    public Button resumeButton;
    public Button parameterButton;
    public Button inputsButton;
    public Button saveButton;
    
    public Button backButton;

    private List<GameObject> allPanels;

    void Start()
    {
        allPanels = new List<GameObject>{parametersPanel, menuPanel, inputsPanel, savePanel};

        resumeButton.onClick.AddListener(Menu);
        
        //Go to parameter panel when you click parameter button
        parameterButton.onClick.AddListener(() => GoToPanel1(parametersPanel, parameterButton.transform.parent.gameObject));
        
        //Go to save panel when you click save button
        saveButton.onClick.AddListener(() => GoToPanel1(savePanel, saveButton.transform.parent.gameObject));
        
        //inputsButton.onClick.AddListener(GoToInputsPanel);
        inputsButton.onClick.AddListener(() => GoToPanel1(inputsPanel, inputsButton.transform.parent.gameObject, true, 
            () => GoToPanel1(parametersPanel, parameterButton.transform.parent.gameObject)));
    }
    
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            Menu();
        }
        
        foreach (var interaction in InputManager.interactions)
        {
            if (Input.GetKeyDown(interaction))
            {
                Debug.Log("Interaction_01");
                break;
            }
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

    public void GoToPanel1(GameObject panelToGo, GameObject panelToGoBackButton, bool enableBackButton = false, UnityAction backButtonAction = null)
    { 
        HideAllPanel();
        
        panelToGo.SetActive(true);
        
        if (enableBackButton)
        {
            SetBackButton(panelToGoBackButton, panelToGo, () => backButton.onClick.AddListener(backButtonAction));
        }
        else
        {
            SetBackButton(panelToGoBackButton, panelToGo, () =>  backButton.onClick.AddListener(() => backButton.gameObject.SetActive(false)));
        }
    }

    public void HideAllPanel()
    {
        foreach (var panel in allPanels)
        {
            panel.SetActive(false);
        }
    }

    public void SetBackButton(GameObject menuToGo, GameObject currentPanel, Action action = null)
    {
        backButton.gameObject.SetActive(true);
        
        backButton.transform.SetParent(currentPanel.transform);
        
        backButton.onClick.RemoveAllListeners();
        
        backButton.onClick.AddListener(() => menuToGo.SetActive(true));
        backButton.onClick.AddListener(() => currentPanel.SetActive(false));
        
        action?.Invoke();
    }
}
