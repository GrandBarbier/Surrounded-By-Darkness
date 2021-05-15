using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject menuPanel;
    private bool menu;
    
    public GameObject parametersPanel;
    public GameObject inputsPanel;

    public Button resumeButton;
    public Button parameterButton;
    public Button inputsButton;
    
    public Button backButton;

    private List<GameObject> allPanels = new List<GameObject>();

    void Start()
    {
        allPanels.Add(parametersPanel);
        allPanels.Add(menuPanel);
        allPanels.Add(inputsPanel);
            
        resumeButton.onClick.AddListener(Menu);

        parameterButton.onClick.AddListener(GoToParameterPanel);
        
        inputsButton.onClick.AddListener(GoToInputsPanel);
    }
    
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            Menu();
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

    public void GoToParameterPanel()
    { 
        HideAllPanel();
        
        parametersPanel.SetActive(true);
        
        SetBackButton(parameterButton.transform.parent.gameObject, parametersPanel, 
            () =>  backButton.onClick.AddListener(() => backButton.gameObject.SetActive(false)));
    }

    public void GoToInputsPanel()
    {
        HideAllPanel();
        
        inputsPanel.SetActive(true);
        
        SetBackButton(inputsButton.transform.parent.gameObject, inputsPanel, () => backButton.onClick.AddListener(GoToParameterPanel));
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
