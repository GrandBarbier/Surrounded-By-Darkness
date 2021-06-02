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
    private bool pause;
    public Menu mainMenu = new Menu(0, false);
    public Menu parametersMenu = new Menu(2, menuBackButtonIndex: 1);
    public Menu inputsMenu = new Menu(5, menuBackButtonIndex: 2);
    public Menu saveMenu = new Menu(4, menuBackButtonIndex: 2);
    public Menu languageMenu = new Menu(3, menuBackButtonIndex: 2);

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

    private List<Menu> allMenus;
    
    [Header("References")]
    public Image blackPanel;
    public GameObject loadScreen;
    public GameObject loadBarScaler;

    //public UnityEvent[] Events;
    public UnityEvent startEvent;

    void Awake()
    {
        Gears.gears.menuManager = this;

        allMenus = new List<Menu>{parametersMenu, mainMenu, inputsMenu, saveMenu, languageMenu};

        PanelMaps = new[] { CreatePanelMap(mainMenu.panel, true),
            //new PanelMap("MainMenuMap", new [,] {{playButton?.GetComponent<RectTransform>(), parameterButton?.GetComponent<RectTransform>(), quitButton?.GetComponent<RectTransform>()}}, new Vector2Int(0, 0)), 
            new PanelMap(new [, ] {{resumeButton?.GetComponent<RectTransform>(), parameterButton?.GetComponent<RectTransform>(), saveButton?.GetComponent<RectTransform>()}}, 
                new Vector2Int(0, 0), "PauseMap"),
            new PanelMap(new [,] {{backButton.GetComponent<RectTransform>(), languagesButton?.GetComponent<RectTransform>(), 
                inputsButton?.GetComponent<RectTransform>()}}, new Vector2Int(0, 0), "ParameterMap"), 
            new PanelMap(new [,] {{englishButton?.GetComponent<RectTransform>(), frenchButton?.GetComponent<RectTransform>(), 
                    backButton.GetComponent<RectTransform>()}}, new Vector2Int(0, 0), "LanguageMap"), 
            CreatePanelMap(saveMenu.panel),
           //new PanelMap("SaveMap", new [,] {{backButton.GetComponent<RectTransform>()}}, new Vector2Int(0, 0)), 
            new PanelMap(new [,] {{backButton?.GetComponent<RectTransform>(), rebindingInteractKeyboard, rebindingMoveKeyboard, rebindingJumpKeyboard, rebindingPoseTorchKeyboard}, 
                {backButton?.GetComponent<RectTransform>(), resetInteractKeyboard, resetMoveKeyboard, resetJumpKeyboard, resetPoseTorchKeyboard}, 
                {backButton?.GetComponent<RectTransform>(), rebindingInteractController, rebindingMoveController, rebindingJumpController, rebindingPoseTorchController}, 
                {backButton?.GetComponent<RectTransform>(), resetInteractController, resetMoveController, resetJumpController, resetPoseTorchController}}, new Vector2Int(0, 0)
                , "RebindingMap")
        };
        
        resumeButton?.onClick.AddListener(Pause);
        
        //Go to parameter panel when you click parameter button
        parameterButton?.onClick.AddListener(() => GoToPanel(parametersMenu));//, pauseMenu//, () => GoToPanel(mainMenu)));
        
        //Go to save panel when you click save button
        saveButton?.onClick.AddListener(() => GoToPanel(saveMenu));//, saveButton.transform.parent.gameObject, () => GoToPanel(mainMenu)));
        
        //Go to Inputs panel when you click inputs button and setup the back button to go back to parameter panel
        inputsButton?.onClick.AddListener(() => GoToPanel(inputsMenu));//, parametersMenu, () => GoToPanel(parametersMenu)));//, parametersMenu, () => GoToPanel(mainMenu))));
        
        //Go to language panel when you click language button and setup the back button to go back to parameter panel
        languagesButton?.onClick.AddListener(() => GoToPanel(languageMenu));//, parametersMenu, () => GoToPanel(parametersMenu)));//, pauseMenu, () => GoToPanel(mainMenu))));
        
        mainMenuButton?.onClick.AddListener(() => LevelManager.LoadScene(0));
        
        //Change language when you click languages buttons
        englishButton?.onClick.AddListener(() => LanguageSystem.SetLanguage(LanguageSystem.Languages.English));
        frenchButton?.onClick.AddListener(() => LanguageSystem.SetLanguage(LanguageSystem.Languages.French));

        playButton?.onClick.AddListener(() => StartCoroutine(LevelManager.LoadAsyncScene(1)));//() => LevelManager.LoadScene(1));
        playButton?.onClick.AddListener(() => Gears.gears.playerInput.SwitchCurrentActionMap("Gameplay"));
        playButton?.onClick.AddListener(() => Time.timeScale = 1f);
        quitButton?.onClick.AddListener(() => Application.Quit());
    }

    void Start()
    {
        startEvent?.Invoke();
        
        SetCurrentMenuMap(ConvertListToPanelMap(mainMenu.menuMap, mainMenu.startPos));
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
            blackPanel.gameObject.SetActive(false);
            GoToPanel(mainMenu);
            Gears.gears.playerInput.SwitchCurrentActionMap("Menu");
            Time.timeScale = 0f;
        }

        pause = !pause;
    }

    public void GoToPanel(Menu menuToGo, bool goTo00 = false, Menu menuToGoBackButton = null, UnityAction backButtonAction = null, PanelMap panelMap = null)
    { 
        HideAllPanel();
        
        menuToGo.panel.SetActive(true);
        selection.transform.SetParent(menuToGo.panel.transform);
        selection.transform.SetAsFirstSibling();

        if (menuToGoBackButton != null)
        {
            SetBackButton(menuToGoBackButton, menuToGo.panel, () => backButton.onClick.AddListener(backButtonAction));
        }else if (menuToGo.useBackButton)
        {
            SetBackButton(GetMenu(menuToGo.menuBackButtonIndex), menuToGo.panel, () => backButton.onClick.AddListener(backButtonAction));
        }

        if (panelMap != null)
        {
            SetCurrentMenuMap(panelMap, goTo00);
        }
        else
        {
            //currentMap = PanelMaps[menuToGo.panelMapIndex];
            SetCurrentMenuMap(ConvertListToPanelMap(menuToGo.menuMap, menuToGo.startPos), goTo00);
        }
    }

    public void SetCurrentMenuMap(PanelMap map, bool goTo00 = false)
    {
        currentMap = map;
        if (goTo00)
        {
            selection.posOnMap = Vector2Int.zero;
        }
        else
        {
            selection.posOnMap = currentMap.startPos;
        }
        selection.ScaleSelection();
        //Debug.Log(panelMap.mapName);
    }

    public Menu GetMenu(int index)
    {
        Menu finalMenu = null;

        foreach (var menu in allMenus)
        {
            if (menu != null && menu.panelMapIndex == index)
            {
                finalMenu = menu;
                break;
            }
        }

        return finalMenu;
    }

    public void HideAllPanel()
    {
        foreach (var menu in allMenus)
        {
            //Debug.Log(menu.panel);
            if (menu.panel != null)
            {
                menu.panel.SetActive(false);
            }
        }
    }

    public void SetBackButton(Menu menuToGo, GameObject currentPanel, Action action = null)
    {
        backButton.gameObject.SetActive(true);
        
        backButton.transform.SetParent(currentPanel.transform);

        backButton.onClick.RemoveAllListeners();
        
        //backButton.onClick.AddListener(() => currentPanel.SetActive(false));
        backButton.onClick.AddListener(() => GoToPanel(menuToGo, true));

        //action?.Invoke();
    }

    public void DefaultFadeIn()
    {
        //Debug.Log("DefaultFadeIn");
        StartCoroutine(LevelManager.FadeDuration(blackPanel, new Color(0f,0f,0f,1f), new Color(0f,0f,0f,0f), 1f));
    }

    public void EnablePause()
    {
        Action<InputAction.CallbackContext> action = context => Pause();
        
        Gears.gears.playerInput.actions["Escape"].performed += action;
        Gears.gears.playerInput.actions["EscapeMenu"].performed += action;

        LevelManager.preLoadingScene += () => Gears.gears.playerInput.actions["Escape"].performed -= action;
        LevelManager.preLoadingScene += () =>  Gears.gears.playerInput.actions["EscapeMenu"].performed -= action;
    }

    public static IEnumerator TriggerButtonColor(Button button)
    {
        button.GetComponent<Image>().color = button.colors.pressedColor;
        
        yield return new WaitForSecondsRealtime(0.1f);
        
        button.GetComponent<Image>().color = button.colors.normalColor;
    }
    
    #endregion

    #region PanelMapFunc
    
    public PanelMap ConvertListToPanelMap(List<Column<RectTransform>> lists, Vector2Int startPos)
    {
        int maxLength = 0;

        foreach (var column in lists)
        {
            if (column.list.Count > maxLength)
            {
                maxLength = column.list.Count;
            }
        }
        
        RectTransform[,] rectTransforms = new RectTransform[lists.Count, maxLength];
        
        for (int i = 0; i < lists.Count; i++)
        {
            for (int j = 0; j < lists[i].list.Count; j++)
            {
                if (j < lists[i].list.Count)
                {
                    rectTransforms[i, j] = lists[i].list[j];
                }
            }
        }
        
        PanelMap panelMap = new PanelMap(rectTransforms, startPos);
        return panelMap;
    }

    public PanelMap CreatePanelMap(GameObject panel, bool noBackButton = false)
    {
        if (panel == null)
        {
            //Debug.Log("Panel was null");
            return null;
        }
        
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

        PanelMap panelMap = new PanelMap(rectTransform, new Vector2Int(0, 0), panel.name);
        return panelMap;
    }
    
    #endregion

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

    public void SwitchActionMap(string s)
    {
        Gears.gears?.playerInput?.SwitchCurrentActionMap(s);
    }
}

public class PanelMap
{
    public PanelMap(RectTransform[,] map, Vector2Int startPos, string mapName = null)
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
    public Menu(int panelMapIndex, bool useBackButton = true, int menuBackButtonIndex = 0, GameObject panel = null)
    {
        this.panel = panel;
        this.panelMapIndex = panelMapIndex;
        this.useBackButton = useBackButton;
        this.menuBackButtonIndex = menuBackButtonIndex;
    }
    
    public GameObject panel;
    public Vector2Int startPos;
    public int panelMapIndex;
    public bool useBackButton;
    public int menuBackButtonIndex;
    public List<Column<RectTransform>> menuMap = new List<Column<RectTransform>>();
}

[Serializable]
public class Column<T>
{
    public List<T> list;
}
