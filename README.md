# GameObject Selector Editor Script 
## Explanation and Usage Guide
---------------
<p align="center">
  <img src="https://github.com/potatojoyful/gameobjectselector/assets/30577083/33bc541e-d86e-43e7-973d-32eb4ad7e724" alt="Image" />
</p>


---------------
The **GameObjectSelector** script is a versatile and user-friendly editor window for Unity, offering a wide range of options to efficiently select GameObjects in the scene. Whether it's filtering by name, script, layer, or component, or using dedicated buttons, this script empowers developers to easily target specific elements. Additionally, it provides the convenience of hiding/unhiding GameObjects in both Hierarchy and Scene views, ensuring a clutter-free editing experience. The script also enables grouping selected objects under an empty parent GameObject or moving them to an existing GameObject for seamless organization. With a console log displaying the number of selected GameObjects, the "GameObject Selector" enhances productivity and simplifies Unity development, making it an enjoyable and efficient journey for all developers.

---------------

## Key Features:

1. **General Selection:** Select all GameObjects, Cameras, or Lights in the scene with just a single click.

2. **Audio Objects Selection:** Quickly select GameObjects with Audio Source or Audio Reverb Zone components for audio-related tasks.

3. **Effects Selection:** Easily select GameObjects with Particle Systems, Trail Renderers, or Line Renderers for effect-related operations.

4. **Search by Name:** Search for GameObjects by name using keywords, numbers, or letters, making it effortless to locate specific objects in complex scenes.

5. **Search by Script:** Select all GameObjects that contain a specific script by simply dragging and dropping the script into the tool.

6. **Search by Layer:** Choose a layer from a list of used layers in the scene to select all GameObjects assigned to that layer.

7. **Search by Component:** Select GameObjects that have a specific component from the list of used components in the scene.

8. **Create Empty Object and Move Selection:** Create a new empty GameObject and move the selected objects inside it, streamlining scene organization.

9. **Move Selected Objects to Existing GameObject:** Move the selected objects into an existing GameObject for hierarchical structuring.

10. **Hide/Show GameObjects in Hierarchy:** Hide or show unselected GameObjects in the Hierarchy window, focusing on the selected GameObjects.

11. **Hide Unselected in Scene:** Option to hide GameObjects in the Scene view that are also hidden in the Hierarchy view.

12. **Keep Selected GameObjects in Hierarchy:** Preserve selected GameObjects in the Hierarchy view while applying hide/unhide options.

---------------
## Usage

1. Open Unity and go to the `Window` menu.
2. Find and select `GameObject Selector` from the menu to open the window.
---------------
### Select General Objects
![select general objects](https://github.com/potatojoyful/gameobjectselector/assets/30577083/aab26da4-a3d4-42cd-88cd-44cae4ee24f8)

- Click on one of the buttons in this section to select GameObjects based on the specified criteria.
---------------
### Select Audio Objects
![audio objects](https://github.com/potatojoyful/gameobjectselector/assets/30577083/b03743f5-9923-44e0-a8a7-0c6b4c9ca911)

- Click on one of the buttons in this section to select GameObjects specific to audio components.
---------------
### Select GameObjects by Effects
![select gameobjects by effects](https://github.com/potatojoyful/gameobjectselector/assets/30577083/72364dd7-88d6-493a-8b6b-3d321112dbb3)

- Click on one of the buttons in this section to select GameObjects specific to visual effects components.
---------------
### Search GameObjects by Name
![search gameobject by name](https://github.com/potatojoyful/gameobjectselector/assets/30577083/13b3194c-9efe-4044-88c3-6f76a99d70e2)

- Enter a search query in the "Search Name" field.
- Click the "Search" button to find and select GameObjects that match the search query.

> ###### You can use letters, numbers, words, or combinations of them, including a few letters from the beginning of a word, to form your search query.
---------------
### Search GameObjects by Script
![search gameobject by script](https://github.com/potatojoyful/gameobjectselector/assets/30577083/a7c2fed8-feaa-4624-b8e8-dfad77517f34)

- Drag and drop a script file into the "Drag Script Here" field.
- Click the "Search" button to find and select GameObjects that contain the specified script.
---------------
### Search GameObjects by Layer
![search gameobject by layer](https://github.com/potatojoyful/gameobjectselector/assets/30577083/541b6892-4a44-473d-b3d8-abf191e2624d)

- Select a layer from the "Select Layer" dropdown.
- Click the "Select" button to find and select GameObjects on the chosen layer.

> ###### The layers listed in the layer menu are only those that are currently used in the scene.
---------------
### Search GameObjects by Component
![search gameobject by component](https://github.com/potatojoyful/gameobjectselector/assets/30577083/8a135b10-a049-4103-8982-8173d8001835)

- Select a component from the "Select Component" dropdown menu.
- Click the "Select" button to find and select GameObjects that contain the chosen component.

> ###### The Components listed in the component menu are only those that are currently used in the scene.
---------------
### Create Empty Object and Move Selection
![create empty and move selection](https://github.com/potatojoyful/gameobjectselector/assets/30577083/7722bc43-444d-44a7-b031-6883a759f7d8)

- Enter a name for the empty GameObject in the "Empty Object Name" field.
- Click the "Create Empty and Move" button to create the empty GameObject and move selected objects inside it.
---------------
### Move Selected Objects to Existing GameObject
![move selected into existing gameobject](https://github.com/potatojoyful/gameobjectselector/assets/30577083/b4b9ce9c-e01c-4309-8681-3e722050346e)

- Drag and drop an existing GameObject into the "Drag GameObject Here" field.
- Select the GameObjects you want to move.
- Click the "Move Selected GameObjects" button to move the selected GameObjects to the target GameObject.
---------------
### Options
![options](https://github.com/potatojoyful/gameobjectselector/assets/30577083/0e0dfa6c-e6ff-4c94-bea1-364656cd583c)

- Check/uncheck the options as needed:
  - **Hide GameObjects in Hierarchy:** Hides unselected GameObjects in the Hierarchy view.
  - **Hide Unselected in Scene:** Hides GameObjects in the Scene view that are also hidden in the Hierarchy view.
  >This feature works with Hide GameObject bool = true.
  - **Keep Selected GameObjects in Hierarchy:** Prevents the script from hiding selected GameObjects in the Hierarchy view when Hide GameObjects in Hierarchy is enabled.
  >This feature works with Hide GameObject bool = true.
---------------
### Finalize Selection and Hierarchy Management

- The script will apply the options and perform the necessary operations to hide/unhide GameObjects and manage the Hierarchy view.
---------------
## Notes

- The script uses `EditorGUILayout` and `EditorGUI` to create the editor window interface.
- It subscribes to Unity events such as `EditorApplication.hierarchyWindowItemOnGUI` and `SceneView.duringSceneGui` to implement features like hiding GameObjects in the Hierarchy and Scene views.
- Be cautious while using the "Move Selected Objects to Existing GameObject" option, as moving objects in the scene can have unintended consequences.
- This script is intended for use within Unity's UnityEditor, and it enhances productivity by simplifying GameObject selection and management tasks, saving valuable development time. It is a powerful tool for developers and designers working on Unity projects with complex scenes and large numbers of GameObjects.
---------------
## Final Word

>Remember to save your scene and project regularly, especially when using custom editor scripts, as they can cause changes that are not always undoable. Use this script responsibly and make sure to understand its implications before applying it to your project.

>Use at your own risk. Im not responsible if you crash your project. Try first at blank and always make backup before trying new assets/scripts in editor.
