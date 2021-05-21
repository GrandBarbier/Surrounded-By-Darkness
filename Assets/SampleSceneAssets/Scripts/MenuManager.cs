using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public PanelMap[] PanelMaps;
    public SelectionUI selection;

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

    public PlayerInput playerInput;

    void Awake()
    {
        playerInput.actions["Interact"].performed += context => Debug.Log("Interaction");
        playerInput.actions["Escape"].performed += context => Menu();
        playerInput.actions["EscapeMenu"].performed += context => Menu();
    }

    void Start()
    {
        allPanels = new List<GameObject>{parametersPanel, menuPanel, inputsPanel, savePanel, languagePanel};

        PanelMaps = new[] {new PanelMap("PauseMap", 
            new [, ] {{resumeButton.GetComponent<RectTransform>(), parameterButton.GetComponent<RectTransform>(), saveButton.GetComponent<RectTransform>()}}, 
            new Vector2Int(0, 0)),  
            new PanelMap("ParameterMap", new [,] {{languagesButton.GetComponent<RectTransform>(), backButton.GetComponent<RectTransform>()}}, 
                new Vector2Int(0, 0)), 
            new PanelMap("LanguageMap", new [,] {{englishButton.GetComponent<RectTransform>(), frenchButton.GetComponent<RectTransform>(), 
                    backButton.GetComponent<RectTransform>()}}, 
                new Vector2Int(0, 0)), 
            new PanelMap("Save Panel", new [,] {{backButton.GetComponent<RectTransform>()}}, new Vector2Int(0, 0)), 
        };

        resumeButton?.onClick.AddListener(Menu);
        
        //Go to parameter panel when you click parameter button
        parameterButton?.onClick.AddListener(() => GoToPanel(parametersPanel, parameterButton.transform.parent.gameObject, 
            () => GoToPanel(menuPanel, null, null, PanelMaps[0], false), PanelMaps[1]));
        
        //Go to save panel when you click save button
        saveButton?.onClick.AddListener(() => GoToPanel(savePanel, saveButton.transform.parent.gameObject, 
            () => GoToPanel(menuPanel, null, null, PanelMaps[0], false), PanelMaps[3]));
        
        //Go to Inputs panel when you click inputs button and setup the back button to go back to parameter panel
        inputsButton?.onClick.AddListener(() => GoToPanel(inputsPanel, inputsButton.transform.parent.gameObject, 
            () => GoToPanel(parametersPanel, parameterButton.transform.parent.gameObject, 
                () => GoToPanel(menuPanel, null, null, PanelMaps[0], false), PanelMaps[1])));
        
        //Go to language panel when you click language button and setup the back button to go back to parameter panel
        languagesButton?.onClick.AddListener(() => GoToPanel(languagePanel, languagePanel.transform.parent.gameObject, 
            () => GoToPanel(parametersPanel, parameterButton.transform.parent.gameObject, 
                () => GoToPanel(menuPanel, null, null, PanelMaps[0], false), PanelMaps[1]), PanelMaps[2]));
        
        //Change language when you click languages buttons
        englishButton?.onClick.AddListener(() => LanguageSystem.SetLanguage(LanguageSystem.Languages.English));
        englishButton?.onClick.AddListener(() => StartCoroutine(TriggerButtonColor(englishButton)));
        frenchButton?.onClick.AddListener(() => LanguageSystem.SetLanguage(LanguageSystem.Languages.French));
        frenchButton?.onClick.AddListener(() => StartCoroutine(TriggerButtonColor(frenchButton)));
    }

    void Update()
    {
        
    }
    
    public void Menu()
    {
        if (menu)
        {
            HideAllPanel();
            playerInput.SwitchCurrentActionMap("Gameplay");
        }
        else
        {
            GoToPanel(menuPanel, null, null, PanelMaps[0], false);
            playerInput.SwitchCurrentActionMap("Menu");
        }

        menu = !menu;
    }

    public void GoToPanel(GameObject panelToGo, GameObject panelToGoBackButton, UnityAction backButtonAction, PanelMap panelMap = null, bool enableBackButton = true)
    { 
        HideAllPanel();
        
        panelToGo.SetActive(true);
        selection.transform.SetParent(panelToGo.transform);
        selection.transform.SetAsFirstSibling();

        if (enableBackButton)
        {
            SetBackButton(panelToGoBackButton, panelToGo, () => backButton.onClick.AddListener(backButtonAction));
        }

        if (panelMap != null)
        {
            currentMap = panelMap;
            selection.GetComponent<RectTransform>().position = panelMap.map[panelMap.startPos.x, panelMap.startPos.y].position;
            selection.posOnMap = currentMap.startPos;
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
    
    public static IEnumerator TriggerButtonColor(Button button)
    {
        button.GetComponent<Image>().color = button.colors.pressedColor;
        
        yield return new WaitForSeconds(0.1f);
        
        button.GetComponent<Image>().color = button.colors.normalColor;
    }

    public IInteractable Interact(float interactionRadius)
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, interactionRadius, gameObject.transform.forward, 
            Mathf.Infinity, Gears.gears.interactionLayer);

        Dictionary<IInteractable, GameObject> interactable = new Dictionary<IInteractable, GameObject>();
        
        List<IInteractable> interact = new List<IInteractable>();
        
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.TryGetComponent(out IInteractable interaction))
            {
                interactable.Add(interaction, hits[i].collider.gameObject);
                interact.Add(interaction);
            }
        }

        IInteractable interactionF = null;
        float distance = Mathf.Infinity;
        
        for (int i = 0; i < interactable.Count; i++)
        {
            if (Vector3.Distance(transform.position, interactable[interact[i]].transform.position) < distance)
            {
                distance = Vector3.Distance(transform.position, interactable[interact[i]].transform.position);
                interactionF = interact[i];
            }
        }
        
        interactionF?.Interact();

        return interactionF;
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
