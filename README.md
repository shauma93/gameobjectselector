# GameObject Selector Editor Script 
## Explanation and Usage Guide
---------------
<p align="center">
  <img src="https://github.com/shauma93/gameobjectselector/assets/141507721/6e0d5c2f-b596-44e2-b4a4-592e16689a07" alt="Image" />
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
![select general objects](https://github.com/shauma93/gameobjectselector/assets/141507721/b53b5c26-1922-4280-ab13-1eea32c91d3e)

- Click on one of the buttons in this section to select GameObjects based on the specified criteria.
---------------
### Select Audio Objects
![audio objects](https://github.com/shauma93/gameobjectselector/assets/141507721/c5f5084a-f5a4-4ad6-8905-d7c8920a4af1)

- Click on one of the buttons in this section to select GameObjects specific to audio components.
---------------
### Select GameObjects by Effects
![select gameobjects by effects](https://github.com/shauma93/gameobjectselector/assets/141507721/3595d811-719b-4188-8c49-0ed3af58edb2)

- Click on one of the buttons in this section to select GameObjects specific to visual effects components.
---------------
### Search GameObjects by Name
![search gameobject by name](https://github.com/shauma93/gameobjectselector/assets/141507721/25de1e8d-8731-4514-8f36-ba51896911e3)

- Enter a search query in the "Search Name" field.
- Click the "Search" button to find and select GameObjects that match the search query.

> ###### You can use letters, numbers, words, or combinations of them, including a few letters from the beginning of a word, to form your search query.
---------------
### Search GameObjects by Script
![search gameobject by script](https://github.com/shauma93/gameobjectselector/assets/141507721/c9cccff6-4534-4221-b8fd-c74797b81603)

- Drag and drop a script file into the "Drag Script Here" field.
- Click the "Search" button to find and select GameObjects that contain the specified script.
---------------
### Search GameObjects by Layer
![search gameobject by layer](https://github.com/shauma93/gameobjectselector/assets/141507721/7faaff0e-42c0-4531-b9c8-b56c3d751172)

- Select a layer from the "Select Layer" dropdown.
- Click the "Select" button to find and select GameObjects on the chosen layer.

> ###### The layers listed in the layer menu are only those that are currently used in the scene.
---------------
### Search GameObjects by Component
![search gameobject by component](https://github.com/shauma93/gameobjectselector/assets/141507721/b7539bbb-1144-4417-b412-20e866fa80d6)

- Select a component from the "Select Component" dropdown menu.
- Click the "Select" button to find and select GameObjects that contain the chosen component.

> ###### The Components listed in the component menu are only those that are currently used in the scene.
---------------
### Create Empty Object and Move Selection
![create empty and move selection](https://github.com/shauma93/gameobjectselector/assets/141507721/419deef6-3d7a-4628-a4d7-ef7f4cc71c4b)

- Enter a name for the empty GameObject in the "Empty Object Name" field.
- Click the "Create Empty and Move" button to create the empty GameObject and move selected objects inside it.
---------------
### Move Selected Objects to Existing GameObject
![move selected into existing gameobject](https://github.com/shauma93/gameobjectselector/assets/141507721/f11b95dc-293c-40cd-8f63-2c885ae3ff57)

- Drag and drop an existing GameObject into the "Drag GameObject Here" field.
- Select the GameObjects you want to move.
- Click the "Move Selected GameObjects" button to move the selected GameObjects to the target GameObject.
---------------
### Options
![options](https://github.com/shauma93/gameobjectselector/assets/141507721/8064b1e0-2fce-4b1a-807e-a6a72b4df6cb)

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
