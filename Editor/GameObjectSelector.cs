using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameObjectSelector : EditorWindow
{
    #region Variables
    // Selection variables
    private string[] layerNames; // Array to store the names of all used layers in the scene
    private int selectedLayerIndex = -1; // Index of the selected layer in the layerNames array
    private MonoScript searchScript; // The script to search for when filtering GameObjects
    private string[] componentNames; // Array to store the names of all used components in the scene
    private int selectedComponentIndex = -1; // Index of the selected component in the componentNames array
    private string emptyObjectName = "NewEmptyObject"; // Default name for the new empty GameObject
    private string searchName = ""; // Search query for searching GameObjects by name
    private bool hideUnselected = false; // Flag to hide unselected GameObjects in the Hierarchy view
    private bool hideUnselectedInScene = false; // Flag to hide unselected GameObjects in the Scene view
    private bool keepSelected = false; // Flag to keep selected GameObjects in the Hierarchy view
    private GameObject targetMoveTo; // Target GameObject to move the selected GameObjects to
    private bool confirmMove = false; // Flag to confirm the move action
    private bool confirmCreateEmpty = false; // Flag to confirm creating an empty GameObject and moving selected GameObjects inside it
    private Dictionary<GameObject, bool> originalActiveStates = new Dictionary<GameObject, bool>(); // Dictionary to store the original active states of GameObjects

    // Constants
    private const HideFlags NonSelectedHideFlags = HideFlags.HideInHierarchy;

    // Selection and hierarchy management variables
    private HashSet<GameObject> selectedObjects = new HashSet<GameObject>();
    private HashSet<GameObject> hiddenObjectsInScene = new HashSet<GameObject>();
    private Vector2 scrollPosition;
    #endregion

    #region Menu Item
    [MenuItem("Window/GameObject Selector")]
    public static void ShowWindow()
    {
        GetWindow<GameObjectSelector>("GameObject Selector");
    }
    #endregion

    #region Unity Event Callbacks
    private void OnEnable()
    {
        // Initialize layer names and component names
        layerNames = GetUsedLayerNames();
        componentNames = GetUsedComponentNames();

        // Subscribe to necessary Unity events
        EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        // Unsubscribe from Unity events when disabling the window
        EditorApplication.hierarchyWindowItemOnGUI -= HandleHierarchyWindowItemOnGUI;
        SceneView.duringSceneGui -= OnSceneGUI;
    }
    #endregion

    #region Hierarchy Management
    private void HandleHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        // Highlight unselected GameObjects if hideUnselected option is enabled
        GameObject go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        if (go != null && hideUnselected && !selectedObjects.Contains(go))
        {
            EditorGUI.DrawRect(selectionRect, Color.gray);
        }
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        // Hide/unhide GameObjects in the Scene view based on hideUnselectedInScene option
        if (hideUnselectedInScene)
        {
            foreach (var go in GameObject.FindObjectsOfType<GameObject>())
            {
                bool isHiddenInHierarchy = (go.hideFlags & NonSelectedHideFlags) == NonSelectedHideFlags;
                if (isHiddenInHierarchy)
                {
                    go.SetActive(false);
                    hiddenObjectsInScene.Add(go);
                }
                else
                {
                    if (hiddenObjectsInScene.Contains(go))
                    {
                        go.SetActive(true);
                        hiddenObjectsInScene.Remove(go);
                    }
                }
            }
        }
        else
        {
            // Show all hidden objects that were previously hidden
            foreach (var go in hiddenObjectsInScene)
            {
                go.SetActive(true);
            }
            hiddenObjectsInScene.Clear();
        }
    }
    #endregion

    #region OnGUI
    private void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Width(position.width));

        // General Selection
        GUILayout.Space(5);
        DrawHeader("Select General Objects", "Select general GameObjects. Camera gets all objects containing the Camera component.");
        GUILayout.Space(5);
        DrawHorizontalButtons(new string[] { "Select All GameObjects", "Select All Cameras", "Select All Lights" },
            new System.Action[] { SelectAllGameObjects, SelectAllCameras, SelectAllLights });
        DrawHorizontalLine();

        // Audio Objects Selection
        DrawHeader("Select Audio Objects", "Select GameObjects that are specific to Audio.");
        GUILayout.Space(5);
        DrawHorizontalButtons(new string[] { "Select Audio Source", "Select Audio Reverb Zone" },
            new System.Action[] { SelectAllAudio, SelectAudioReverbZone });
        DrawHorizontalLine();

        // Effects Selection
        DrawHeader("Select GameObjects by Effects", "Select GameObjects that are specific to Effects.");
        GUILayout.Space(5);
        DrawHorizontalButtons(new string[] { "Select Particles", "Select Trail", "Select Line" },
            new System.Action[] { SelectAllParticles, SelectTrail, SelectLine });
        DrawHorizontalLine();

        // Search by Name
        DrawHeader("Search GameObjects by Name", "Search for GameObjects that contain letters, numbers, or words as the search query.");
        GUILayout.Space(5);
        EditorGUILayout.BeginHorizontal();
        searchName = EditorGUILayout.TextField("Search Name", searchName);
        if (GUILayout.Button("Search"))
        {
            SearchGameObjectsByName(searchName);
        }
        EditorGUILayout.EndHorizontal();
        DrawHorizontalLine();

        // Search by Script
        DrawHeader("Search GameObjects by Script", "Search for GameObjects with a specific script. It will select all GameObjects that contain the specified script.");
        GUILayout.Space(8);
        EditorGUILayout.BeginHorizontal();
        searchScript = EditorGUILayout.ObjectField("Drag Script Here", searchScript, typeof(MonoScript), false) as MonoScript;
        if (GUILayout.Button("Search"))
        {
            SearchGameObjectsByScript();
        }
        EditorGUILayout.EndHorizontal();
        DrawHorizontalLine();

        // Search by Layer
        DrawHeader("Search GameObjects by Layer", "Search for GameObjects with a specific layer. Only used Layers in the scene are shown on the list. Layer 31 is reserved.");
        GUILayout.Space(5);
        EditorGUILayout.BeginHorizontal();
        selectedLayerIndex = EditorGUILayout.Popup("Select Layer", selectedLayerIndex, layerNames);
        if (GUILayout.Button("Select") && selectedLayerIndex >= 0)
        {
            SelectGameObjectsOnLayer(selectedLayerIndex);
        }
        EditorGUILayout.EndHorizontal();
        DrawHorizontalLine();

        // Search by Component
        DrawHeader("Search GameObjects by Component", "Search for GameObjects with a specific component. Only used Components in the scene are shown on the list.");
        GUILayout.Space(8);
        EditorGUILayout.BeginHorizontal();
        selectedComponentIndex = EditorGUILayout.Popup("Select Component", selectedComponentIndex, componentNames);
        if (GUILayout.Button("Select") && selectedComponentIndex >= 0)
        {
            SelectGameObjectsWithComponent(selectedComponentIndex);
        }
        EditorGUILayout.EndHorizontal();
        DrawHorizontalLine();

        // Create Empty Object and Move Selection
        DrawHeader("Create Empty Object and Move Selection", "Create a new empty GameObject, define the name of the empty object, and move all selected objects inside it.");
        GUILayout.Space(5);
        EditorGUILayout.BeginHorizontal();
        emptyObjectName = EditorGUILayout.TextField("Empty Object Name", emptyObjectName);
        if (GUILayout.Button("Create Empty and Move"))
        {
            confirmCreateEmpty = true;
        }
        EditorGUILayout.EndHorizontal();
        DrawHorizontalLine();

        if (confirmCreateEmpty)
        {
            EditorGUILayout.BeginVertical();
            GUILayout.Label("Are you sure you want to create an empty object and move the selected objects inside it?", EditorStyles.wordWrappedLabel);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Yes"))
            {
                CreateEmptyObjectAndMoveSelection(emptyObjectName);
                confirmCreateEmpty = false;
            }
            if (GUILayout.Button("No"))
            {
                confirmCreateEmpty = false;
            }
            EditorGUILayout.EndHorizontal();
            DrawHorizontalLine();
            EditorGUILayout.EndVertical();
        }

        // Move Selected Objects to Existing GameObject
        DrawHeader("Move Selected Objects to Existing GameObject", "Move selected objects into an existing GameObject.");
        GUILayout.Space(5);
        EditorGUILayout.BeginHorizontal();
        targetMoveTo = EditorGUILayout.ObjectField("Drag GameObject Here", targetMoveTo, typeof(GameObject), true) as GameObject;

        EditorGUI.BeginDisabledGroup(targetMoveTo == null || Selection.gameObjects.Length == 0);
        if (GUILayout.Button("Move Selected GameObjects"))
        {
            confirmMove = true;
        }
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.EndHorizontal();
        DrawHorizontalLine();

        if (confirmMove)
        {
            EditorGUILayout.BeginVertical();
            GUILayout.Label("Are you sure you want to move the selected objects to the target GameObject?", EditorStyles.wordWrappedLabel);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Yes"))
            {
                MoveSelectedToTarget(targetMoveTo);
                confirmMove = false;
            }
            if (GUILayout.Button("No"))
            {
                confirmMove = false;
            }
            EditorGUILayout.EndHorizontal();
            DrawHorizontalLine();
            EditorGUILayout.EndVertical();
        }

        // Options
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label(new GUIContent("Hide GameObjects in Hierarchy:                  ", "Hides all Non-Selected GameObjects in the Hierarchy window, displaying only Selected GameObjects in the scene."));
        hideUnselected = EditorGUILayout.Toggle(hideUnselected);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label(new GUIContent("Hide Unselected in Scene:                             ", "Hides the GameObjects in the Scene view that are also hidden in the Hierarchy view. This feature works with Hide GameObject Bool."));
        hideUnselectedInScene = EditorGUILayout.Toggle(hideUnselectedInScene);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label(new GUIContent("Keep Selected GameObjects in Hierarchy:", "Keep Selected GameObjects in the Hierarchy view, making selecting individual GameObjects from the Selection. Works With Hide GameObject Bool."));
        keepSelected = EditorGUILayout.Toggle(keepSelected);
        EditorGUILayout.EndHorizontal();
        DrawHorizontalLine();

        // Finalize selection and hierarchy management
        SetHideFlagsForAll();
        SelectParentObjects();

        EditorGUILayout.EndScrollView();
    }
    #endregion

    #region Helper Methods
    // Draw a header with a tooltip in the GUI
    private void DrawHeader(string title, string tooltip)
    {
        GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel);
        headerStyle.fontSize = 15;

        EditorGUILayout.LabelField(new GUIContent(title, tooltip), headerStyle);
    }

    // Draw a horizontal line in the GUI
    private void DrawHorizontalLine()
    {
        Color previousColor = GUI.color;
        GUI.color = Color.black;
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUI.color = previousColor;
    }

    // Draw horizontal buttons with corresponding actions
    private void DrawHorizontalButtons(string[] buttonLabels, System.Action[] buttonActions)
    {
        EditorGUILayout.BeginHorizontal();
        for (int i = 0; i < buttonLabels.Length; i++)
        {
            if (GUILayout.Button(buttonLabels[i]))
            {
                buttonActions[i].Invoke();
            }
        }
        EditorGUILayout.EndHorizontal();
    }
    #endregion

    #region Selection Methods
    // Select all GameObjects in the scene
    private void SelectAllGameObjects()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        Selection.objects = allObjects;
        Debug.Log("Total GameObjects selected: " + allObjects.Length);
    }

    // Select all Camera GameObjects in the scene
    private void SelectAllCameras()
    {
        GameObject[] cameras = GameObject.FindObjectsOfType<Camera>()
            .Select(cam => cam.gameObject)
            .ToArray();

        Selection.objects = cameras;
        Debug.Log("Total Cameras selected: " + cameras.Length);
    }

    // Select all Light GameObjects in the scene
    private void SelectAllLights()
    {
        GameObject[] lights = GameObject.FindObjectsOfType<Light>()
            .Select(light => light.gameObject)
            .ToArray();

        Selection.objects = lights;
        Debug.Log("Total Lights selected: " + lights.Length);
    }

    // Select all Particle System GameObjects in the scene
    private void SelectAllParticles()
    {
        GameObject[] particles = GameObject.FindObjectsOfType<ParticleSystem>()
            .Select(particle => particle.gameObject)
            .ToArray();

        Selection.objects = particles;
        Debug.Log("Total Particles selected: " + particles.Length);
    }

    // Select all AudioSource GameObjects in the scene
    private void SelectAllAudio()
    {
        GameObject[] audioObjects = GameObject.FindObjectsOfType<AudioSource>()
            .Select(audio => audio.gameObject)
            .ToArray();

        Selection.objects = audioObjects;
        Debug.Log("Total Audio objects selected: " + audioObjects.Length);
    }

    // Select all AudioReverbZone GameObjects in the scene
    private void SelectAudioReverbZone()
    {
        GameObject[] audioReverbZones = GameObject.FindObjectsOfType<AudioReverbZone>()
            .Select(zone => zone.gameObject)
            .ToArray();

        Selection.objects = audioReverbZones;
        Debug.Log("Total Audio Reverb Zones selected: " + audioReverbZones.Length);
    }

    // Select all Trail Renderer GameObjects in the scene
    private void SelectTrail()
    {
        GameObject[] trails = GameObject.FindObjectsOfType<TrailRenderer>()
            .Select(trail => trail.gameObject)
            .ToArray();

        Selection.objects = trails;
        Debug.Log("Total Trail renderers selected: " + trails.Length);
    }

    // Select all Line Renderer GameObjects in the scene
    private void SelectLine()
    {
        GameObject[] lines = GameObject.FindObjectsOfType<LineRenderer>()
            .Select(line => line.gameObject)
            .ToArray();

        Selection.objects = lines;
        Debug.Log("Total Line renderers selected: " + lines.Length);
    }

    // Select all GameObjects with a specific layer
    private void SelectGameObjectsOnLayer(int layerIndex)
    {
        string[] usedLayerNames = GetUsedLayerNames();
        GameObject[] objectsOnLayer = GameObject.FindObjectsOfType<GameObject>()
            .Where(go => go.layer == layerIndex)
            .ToArray();

        Selection.objects = objectsOnLayer;
        Debug.Log("Total GameObjects selected on layer " + usedLayerNames[layerIndex] + ": " + objectsOnLayer.Length);
    }

    // Search for GameObjects with a specific script
    private void SearchGameObjectsByScript()
    {
        if (searchScript == null)
        {
            Debug.LogError("Please provide a script to search for.");
            return;
        }

        string scriptName = searchScript.GetClass().Name;
        GameObject[] objectsWithScript = GameObject.FindObjectsOfType<GameObject>()
            .Where(go => go.GetComponent(scriptName) != null)
            .ToArray();

        Selection.objects = objectsWithScript;
        Debug.Log("Total GameObjects with script " + scriptName + " selected: " + objectsWithScript.Length);
    }

    // Select all GameObjects with a specific component
    private void SelectGameObjectsWithComponent(int componentIndex)
    {
        if (componentIndex < 0 || componentIndex >= componentNames.Length)
        {
            Debug.LogError("Invalid component selection.");
            return;
        }

        GameObject[] objectsWithComponent = GameObject.FindObjectsOfType<GameObject>()
            .Where(go => go.GetComponent(componentNames[componentIndex]) != null)
            .ToArray();

        Selection.objects = objectsWithComponent;
        Debug.Log("Total GameObjects with component " + componentNames[componentIndex] + " selected: " + objectsWithComponent.Length);
    }

    // Create an empty GameObject and move selected objects into it
    private void CreateEmptyObjectAndMoveSelection(string name)
    {
        GameObject emptyObject = new GameObject(name);
        emptyObject.transform.position = Vector3.zero;

        GameObject[] selectedObjects = Selection.gameObjects;
        foreach (GameObject obj in selectedObjects)
        {
            obj.transform.parent = emptyObject.transform;
        }

        Selection.activeGameObject = emptyObject;
    }

    // Move selected objects to the target GameObject
    private void MoveSelectedToTarget(GameObject target)
    {
        if (target == null)
        {
            Debug.LogError("Target GameObject is not assigned.");
            return;
        }

        GameObject[] selectedObjects = Selection.gameObjects;
        foreach (GameObject obj in selectedObjects)
        {
            obj.transform.parent = target.transform;
        }
    }

    // Search for GameObjects by name and select them
    private void SearchGameObjectsByName(string searchQuery)
    {
        List<GameObject> foundObjects = new List<GameObject>();

        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            string objName = obj.name.ToLower();
            if (objName.Contains(searchQuery.ToLower()))
            {
                foundObjects.Add(obj);
            }
        }

        Selection.objects = foundObjects.ToArray();
        Debug.Log("Total GameObjects found: " + foundObjects.Count);
    }

    // Set HideFlags for GameObjects based on the hideUnselected and keepSelected options
    private void SetHideFlagsForAll()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        if (!keepSelected)
        {
            selectedObjects.Clear();
        }

        foreach (GameObject obj in allObjects)
        {
            bool isSelected = Selection.Contains(obj);
            bool isUnselected = hideUnselected && !isSelected;

            if (isSelected && !keepSelected)
            {
                selectedObjects.Add(obj);
            }

            SetNonSelectedFlag(obj, isUnselected);

            if (keepSelected && selectedObjects.Contains(obj))
            {
                SetNonSelectedFlag(obj, false);
            }
        }
    }

    // Set HideFlags for GameObjects based on the hideUnselected option
    private void SetNonSelectedFlag(GameObject go, bool setFlag)
    {
        if (setFlag)
        {
            go.hideFlags |= NonSelectedHideFlags;
        }
        else
        {
            go.hideFlags &= ~NonSelectedHideFlags;
        }
    }

    // Get used layer names in the scene
    private string[] GetUsedLayerNames()
    {
        List<string> usedLayerList = new List<string>();
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject go in allObjects)
        {
            int layer = go.layer;
            string layerName = LayerMask.LayerToName(layer);
            if (!string.IsNullOrEmpty(layerName) && !usedLayerList.Contains(layerName))
            {
                usedLayerList.Add(layerName);
            }
        }
        return usedLayerList.ToArray();
    }

    // Get used component names in the scene
    private string[] GetUsedComponentNames()
    {
        HashSet<string> componentNamesSet = new HashSet<string>();
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject go in allObjects)
        {
            var components = go.GetComponents<Component>();
            foreach (var component in components)
            {
                if (component == null)
                    continue;
                componentNamesSet.Add(component.GetType().Name);
            }
        }
        return componentNamesSet.ToArray();
    }

    // Select parent objects of the selected GameObjects
    private void SelectParentObjects()
    {
        List<GameObject> parentObjects = new List<GameObject>();
        foreach (GameObject go in Selection.gameObjects)
        {
            Transform parent = go.transform.parent;
            while (parent != null)
            {
                if (!parentObjects.Contains(parent.gameObject))
                {
                    parentObjects.Add(parent.gameObject);
                }
                parent = parent.parent;
            }
        }

        foreach (GameObject parentObj in parentObjects)
        {
            if (!Selection.Contains(parentObj))
            {
                Selection.objects = Selection.objects.Append(parentObj).ToArray();
            }
        }
    }
    #endregion
}