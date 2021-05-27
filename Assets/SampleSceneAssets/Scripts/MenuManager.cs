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

    [Header("MenuButtons")]
    public Button resumeButton;
    public Button parameterButton;
    public Button saveButton;
    public Button inputsButton;
    public Button languagesButton;

    [Header("RebindingButtons")] 
    public RectTransform rebindingInteractKeyboard;
    public RectTransform resetInteractKeyboard;
    public RectTransform rebindingInteractController;
    public RectTransform resetInteractController;

    public RectTransform rebindingMoveKeyboard;
    public RectTransform resetMoveKeyboard;
    public RectTransform rebindingMoveController;
    public RectTransform resetMoveController;
    
    public RectTransform rebindingJumpKeyboard;
    public RectTransform resetJumpKeyboard;
    public RectTransform rebindingJumpController;
    public RectTransform resetJumpController;
    
    public RectTransform rebindingPoseTorchKeyboard;
    public RectTransform resetPoseTorchKeyboard;
    public RectTransform rebindingPoseTorchController;
    public RectTransform resetPoseTorchController;

    [Header("Language Buttons")]
    public Button englishButton;
    public Button frenchButton;
    
    public Button backButton;

    [Header("MainMenuButtons")] 
    public Button playButton;
    public Button quitButton;

    private List<GameObject> allPanels;

    void Awake()
    {
        //Gears.gears.playerInput.actions["Interact"].performed += context => Debug.Log("Interaction");
        Gears.gears.playerInput.actions["Escape"].performed += context => Menu();
        Gears.gears.playerInput.actions["EscapeMenu"].performed += context => Menu();
    }

    void Start()
    {
        allPanels = new List<GameObject>{parametersPanel, menuPanel, inputsPanel, savePanel, languagePanel};

        PanelMaps = new[] {new PanelMap("PauseMap", 
            new [, ] {{resumeButton.GetComponent<RectTransform>(), parameterButton.GetComponent<RectTransform>(), saveButton.GetComponent<RectTransform>()}}, 
            new Vector2Int(0, 0)),  
            new PanelMap("ParameterMap", new [,] {{backButton.GetComponent<RectTransform>(), languagesButton.GetComponent<RectTransform>(), 
                    inputsButton.GetComponent<RectTransform>()}}, new Vector2Int(0, 0)), 
            new PanelMap("LanguageMap", new [,] {{englishButton.GetComponent<RectTransform>(), frenchButton.GetComponent<RectTransform>(), 
                    backButton.GetComponent<RectTransform>()}}, 
                new Vector2Int(0, 0)), CreatePanelMap(savePanel),
            //new PanelMap("SaveMap", new [,] {{backButton.GetComponent<RectTransform>()}}, new Vector2Int(0, 0)), 
            new PanelMap("RebindingMap", new [,] {
                {backButton?.GetComponent<RectTransform>(), rebindingInteractKeyboard, rebindingMoveKeyboard, rebindingJumpKeyboard, rebindingPoseTorchKeyboard}, 
                {backButton?.GetComponent<RectTransform>(), resetInteractKeyboard, resetMoveKeyboard, resetJumpKeyboard, resetPoseTorchKeyboard}, 
                {backButton?.GetComponent<RectTransform>(), rebindingInteractController, rebindingMoveController, rebindingJumpController, rebindingPoseTorchController}, 
                {backButton?.GetComponent<RectTransform>(), resetInteractController, resetMoveController, resetJumpController, resetPoseTorchController}}, new Vector2Int(0, 0))
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
                () => GoToPanel(menuPanel, null, null, PanelMaps[0], false), PanelMaps[1]), PanelMaps[4]));
        
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

    #region MenFunc

    public void Menu()
    {
        if (menu)
        {
            HideAllPanel();
            Gears.gears.playerInput.SwitchCurrentActionMap("Gameplay");
        }
        else
        {
            GoToPanel(menuPanel, null, null, PanelMaps[0], false);
            Gears.gears.playerInput.SwitchCurrentActionMap("Menu");
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
            //Debug.Log(panelMap.mapName);
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
    
    #endregion

    public PanelMap CreatePanelMap(GameObject panel)
    {
        List<GameObject> allChilds = GetAllChilds(panel);
        List<RectTransform> recter = new List<RectTransform>();

        foreach(var child in allChilds)
        {
            if (child.TryGetComponent(out Button button))
            {
                recter.Add(button.GetComponent<RectTransform>());
            }
        }

        List<List<RectTransform>> lists = new List<List<RectTransform>>();
        int currentListIndex = -1;
        
        int column = 0;
        int row = 0;
        float posX = Mathf.Infinity;

        for (int i = 0; i < recter.Count; i++)
        {
            if (recter[i].position.x != posX)
            {
                posX = recter[i].position.x;
                lists.Add(new List<RectTransform>());
                currentListIndex++;
                
                //Debug.Log($"add end list1 : {recter[i].gameObject.name}");
                lists[currentListIndex].Add(recter[i]);
            }
            else
            {
                //order the row lists
                /*if (lists[currentListIndex].Count > 0)
                {
                    for (int j = 0; j < lists[currentListIndex].Count; j++)
                    {
                        if (lists[currentListIndex][j].position.y > recter[i].position.y)
                        {
                            Debug.Log($"{j} {lists[currentListIndex][j].position.y} > {recter[i].position.y}");
                            lists[currentListIndex].Insert(j, recter[i]);
                            break;
                        }
                        
                        if (j == lists[currentListIndex].Count - 1)
                        {
                            Debug.Log($"end");
                            lists[currentListIndex].Add(recter[i]);
                            break;
                        }
                    }
                }
                else
                {
                    Debug.Log($"count 0");
                    lists[currentListIndex].Add(recter[i]);
                }*/
                
                //Debug.Log($"add end list : {recter[i].gameObject.name}");
                lists[currentListIndex].Add(recter[i]);
            }
        }
        
        //backButton setup
        lists[0].Insert(0, null);

        column = lists.Count;
        int maxLength = 0;

        foreach (var list in lists)
        {
            if (list.Count > maxLength)
            {
                maxLength = list.Count;
            }
        }

        row = maxLength;
        
        RectTransform [,] rectTransform = new RectTransform[column, row];
        rectTransform[0, 0] = backButton.GetComponent<RectTransform>();

        for (int i = 0; i < column; i++)
        {
            for (int j = 0; j < row; j++)
            {
                if (!(i == 0 && j == 0))
                {
                    if (j < lists[i].Count)
                    {
                        rectTransform[i, j] = lists[i][j];
                    }
                    else
                    {
                        rectTransform[i, j] = null;
                    }
                }
            }
        }

        PanelMap panelMap = new PanelMap(panel.name, rectTransform, new Vector2Int(0, 0));
        return panelMap;
    }

    public static List<GameObject> GetAllChilds(GameObject go)
    {
        List<GameObject> allChilds = new List<GameObject>();

        for (int i = 0; i < go.transform.childCount; i++)
        {
            allChilds.Add(go.transform.GetChild(i).gameObject);

            if (go.transform.GetChild(i).childCount > 0)
            {
                List<GameObject> g = GetAllChilds(go.transform.GetChild(i).gameObject);

                foreach (var gj in g)
                {
                    if (!allChilds.Contains(gj))
                    {
                        allChilds.Add(gj);
                    }
                }
            }
        }
        
        return allChilds;
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
