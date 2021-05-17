using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public PanelMap[] PanelMaps;
    public GameObject selection;

    [NonSerialized] public PanelMap currentMap;
    
    [Header("Panels")]
    public GameObject menuPanel;
    private bool menu;

    public GameObject parametersPanel;
    public GameObject inputsPanel;
    public GameObject savePanel;
    public GameObject languagePanel;

    [Header("Buttons")]
    public Button resumeButton;
    public Button parameterButton;
    public Button saveButton;
    public Button inputsButton;
    public Button languagesButton;

    [Header("Language Buttons")]
    public Button englishButton;
    public Button frenchButton;
    
    public Button backButton;

    private List<GameObject> allPanels;

    void Start()
    {
        allPanels = new List<GameObject>{parametersPanel, menuPanel, inputsPanel, savePanel, languagePanel};

        PanelMaps = new[] {new PanelMap("PauseMap", 
            new RectTransform[, ] {{resumeButton.GetComponent<RectTransform>(), parameterButton.GetComponent<RectTransform>(), saveButton.GetComponent<RectTransform>()}}, 
            new Vector2Int(0, 0)),  };

        resumeButton?.onClick.AddListener(Menu);
        
        //Go to parameter panel when you click parameter button
        parameterButton?.onClick.AddListener(() => GoToPanel(parametersPanel, parameterButton.transform.parent.gameObject, 
            () => backButton.gameObject.SetActive(false)));
        
        //Go to save panel when you click save button
        saveButton?.onClick.AddListener(() => GoToPanel(savePanel, saveButton.transform.parent.gameObject, 
            () => backButton.gameObject.SetActive(false)));
        
        //Go to Inputs panel when you click inputs button and setup the back button to go back to parameter panel
        inputsButton?.onClick.AddListener(() => GoToPanel(inputsPanel, inputsButton.transform.parent.gameObject, 
            () => GoToPanel(parametersPanel, parameterButton.transform.parent.gameObject, 
                () => backButton.gameObject.SetActive(false))));
        
        //Go to language panel when you click language button and setup the back button to go back to parameter panel
        languagesButton?.onClick.AddListener(() => GoToPanel(languagePanel, languagePanel.transform.parent.gameObject, 
            () => GoToPanel(parametersPanel, parameterButton.transform.parent.gameObject, 
                () => backButton.gameObject.SetActive(false))));
        
        //Change language when you click languages buttons
        englishButton?.onClick.AddListener(() => LanguageSystem.SetLanguage(LanguageSystem.Languages.English));
        frenchButton?.onClick.AddListener(() => LanguageSystem.SetLanguage(LanguageSystem.Languages.French));
    }
    
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            Menu();
        }
        
        foreach (var interaction in InputManager.interactionKeys)
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
            GoToPanel(menuPanel, null, null, PanelMaps[0], false);
        }

        menu = !menu;
    }

    public void GoToPanel(GameObject panelToGo, GameObject panelToGoBackButton, UnityAction backButtonAction, PanelMap panelMap = null, bool enableBackButton = true)
    { 
        HideAllPanel();
        
        panelToGo.SetActive(true);
        selection.transform.SetParent(panelToGo.transform);

        if (panelMap != null)
        {
            currentMap = panelMap;
            selection.GetComponent<RectTransform>().position = panelMap.map[panelMap.startPos.x, panelMap.startPos.y].position;
        }

        if (enableBackButton)
        {
            SetBackButton(panelToGoBackButton, panelToGo, () => backButton.onClick.AddListener(backButtonAction));
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

public class PanelMap
{
    public PanelMap(string mapName, RectTransform[,] map, Vector2Int startPos)
    {
        this.mapName = mapName;
        this.map = map;
        this.startPos = startPos;
    }
    
    public string mapName;
    
    public RectTransform[,] map;

    public Vector2Int startPos;
}
