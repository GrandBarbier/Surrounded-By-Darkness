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

    [Header("Menus")] 
    public Menu pauseMenu;
    private bool pause;

    public Menu mainMenu;
    public Menu parametersMenu;
    public Menu inputsMenu;
    public Menu saveMenu;
    public Menu languageMenu;

    [Header("MenuButtons")]
    public Button resumeButton;
    public Button parameterButton;
    public Button saveButton;
    public Button inputsButton;
    public Button languagesButton;
    public Button mainMenuButton;
    
    public Button backButton;

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

    [Header("MainMenuButtons")] 
    public Button playButton;
    public Button quitButton;

    private List<Menu> allPanels;

    //public UnityEvent[] Events;

    void Awake()
    {
        Gears.gears.playerInput.actions["Escape"].performed += context => Pause();
        Gears.gears.playerInput.actions["EscapeMenu"].performed += context => Pause();
        
        allPanels = new List<Menu>{parametersMenu, mainMenu, inputsMenu, saveMenu, languageMenu};

        PanelMaps = new[] { CreatePanelMap(mainMenu.panel, true),
            //new PanelMap("MainMenuMap", new [,] {{playButton?.GetComponent<RectTransform>(), parameterButton?.GetComponent<RectTransform>(), quitButton?.GetComponent<RectTransform>()}}, new Vector2Int(0, 0)), 
            new PanelMap("PauseMap", 
                new [, ] {{resumeButton?.GetComponent<RectTransform>(), parameterButton?.GetComponent<RectTransform>(), saveButton?.GetComponent<RectTransform>()}}, 
                new Vector2Int(0, 0)),
            new PanelMap("ParameterMap", new [,] {{backButton.GetComponent<RectTransform>(), languagesButton?.GetComponent<RectTransform>(), 
                inputsButton?.GetComponent<RectTransform>()}}, new Vector2Int(0, 0)), 
            new PanelMap("LanguageMap", new [,] {{englishButton?.GetComponent<RectTransform>(), frenchButton?.GetComponent<RectTransform>(), 
                    backButton.GetComponent<RectTransform>()}}, 
                new Vector2Int(0, 0)), 
            CreatePanelMap(saveMenu.panel),
           // new PanelMap("SaveMap", new [,] {{backButton.GetComponent<RectTransform>()}}, new Vector2Int(0, 0)), 
            new PanelMap("RebindingMap", new [,] {
                {backButton?.GetComponent<RectTransform>(), rebindingInteractKeyboard, rebindingMoveKeyboard, rebindingJumpKeyboard, rebindingPoseTorchKeyboard}, 
                {backButton?.GetComponent<RectTransform>(), resetInteractKeyboard, resetMoveKeyboard, resetJumpKeyboard, resetPoseTorchKeyboard}, 
                {backButton?.GetComponent<RectTransform>(), rebindingInteractController, rebindingMoveController, rebindingJumpController, rebindingPoseTorchController}, 
                {backButton?.GetComponent<RectTransform>(), resetInteractController, resetMoveController, resetJumpController, resetPoseTorchController}}, new Vector2Int(0, 0))
        };
        
        resumeButton?.onClick.AddListener(Pause);
        
        //Go to parameter panel when you click parameter button
        parameterButton?.onClick.AddListener(() => GoToPanel(parametersMenu, parameterButton.transform.parent.gameObject, () => GoToPanel(mainMenu)));
        
        //Go to save panel when you click save button
        saveButton?.onClick.AddListener(() => GoToPanel(saveMenu, saveButton.transform.parent.gameObject, () => GoToPanel(mainMenu)));
        
        //Go to Inputs panel when you click inputs button and setup the back button to go back to parameter panel
        inputsButton?.onClick.AddListener(() => GoToPanel(inputsMenu, inputsButton.transform.parent.gameObject, 
            () => GoToPanel(parametersMenu, parameterButton.transform.parent.gameObject, () => GoToPanel(mainMenu))));
        
        //Go to language panel when you click language button and setup the back button to go back to parameter panel
        languagesButton?.onClick.AddListener(() => GoToPanel(languageMenu, languagesButton.transform.parent.gameObject, 
            () => GoToPanel(parametersMenu, parameterButton.transform.parent.gameObject, () => GoToPanel(mainMenu))));
        
        mainMenuButton?.onClick.AddListener(() => LevelManager.LoadScene(0));
        
        //Change language when you click languages buttons
        englishButton?.onClick.AddListener(() => LanguageSystem.SetLanguage(LanguageSystem.Languages.English));
        englishButton?.onClick.AddListener(() => StartCoroutine(TriggerButtonColor(englishButton)));
        frenchButton?.onClick.AddListener(() => LanguageSystem.SetLanguage(LanguageSystem.Languages.French));
        frenchButton?.onClick.AddListener(() => StartCoroutine(TriggerButtonColor(frenchButton)));
        
        playButton?.onClick.AddListener(() => LevelManager.LoadScene(1));
        playButton?.onClick.AddListener(() => Gears.gears.playerInput.SwitchCurrentActionMap("Gameplay"));
        quitButton?.onClick.AddListener(() => Application.Quit());
    }

    void Start()
    {
        if (pauseMenu.panel == null)
        {
            GoToPanel(mainMenu);
            Gears.gears.playerInput.SwitchCurrentActionMap("Menu");
        }
    }

    void Update()
    {
        
    }

    #region MenuFunc

    public void Pause()
    {
        if (pause)
        {
            HideAllPanel();
            Gears.gears.playerInput.SwitchCurrentActionMap("Gameplay");
            Time.timeScale = 1f;
        }
        else
        {
            GoToPanel(pauseMenu);
            Gears.gears.playerInput.SwitchCurrentActionMap("Menu");
            Time.timeScale = 0f;
        }

        pause = !pause;
    }

    public void GoToPanel(Menu panelToGo, GameObject panelToGoBackButton = null, UnityAction backButtonAction = null, PanelMap panelMap = null)
    { 
        HideAllPanel();
        
        panelToGo.panel.SetActive(true);
        selection.transform.SetParent(panelToGo.panel.transform);
        selection.transform.SetAsFirstSibling();

        if (panelToGoBackButton)
        {
            SetBackButton(panelToGoBackButton, panelToGo.panel, () => backButton.onClick.AddListener(backButtonAction));
        }

        if (panelMap != null)
        {
            currentMap = panelMap;
        }
        else
        {
            currentMap = PanelMaps[panelToGo.panelMapIndex];
        }
        
        selection.GetComponent<RectTransform>().position = currentMap.map[currentMap.startPos.x, currentMap.startPos.y].position;
        selection.posOnMap = currentMap.startPos;
        selection.ScaleSelection();
        //Debug.Log(panelMap.mapName);
    }

    public void HideAllPanel()
    {
        foreach (var panel in allPanels)
        {
            panel.panel.SetActive(false);
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

    public PanelMap CreatePanelMap(GameObject panel, bool noBackButton = false)
    {
        List<GameObject> allChilds = GetAllChilds(panel, true);
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
        if (!noBackButton)
        {
            lists[0].Insert(0, null);
        }

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
        if (!noBackButton)
        {
            rectTransform[0, 0] = backButton.GetComponent<RectTransform>();
        }

        for (int i = 0; i < column; i++)
        {
            for (int j = 0; j < row; j++)
            {
                if (!(i == 0 && j == 0) || noBackButton)
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

    public static List<GameObject> GetAllChilds(GameObject go, bool activeChildsOnly = false)
    {
        List<GameObject> allChilds = new List<GameObject>();

        for (int i = 0; i < go.transform.childCount; i++)
        {
            if (!activeChildsOnly || go.transform.GetChild(i).gameObject.activeSelf)
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

[Serializable]
public class Menu
{
    public Menu()
    {
        
    }

    public int panelMapIndex;
    public GameObject panel;
}
