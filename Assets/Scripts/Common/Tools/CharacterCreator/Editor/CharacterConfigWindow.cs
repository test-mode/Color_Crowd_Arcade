using UnityEngine;
using UnityEditor;

public class CharacterConfigWindow : EditorWindow
{
    public static GameObject modelFile;
    RuntimeAnimatorController animController;
    public static CharacterCreatorProperties properties;
    string myString = "Hello World";
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;

    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/Character Config")]
    static void LauchWindowFromMenu()
    {
        modelFile = null;
        // Get existing open window or if none, make a new one:
        CharacterConfigWindow window = (CharacterConfigWindow)EditorWindow.GetWindow(typeof(CharacterConfigWindow));
        window.Show();
    }

    [MenuItem("Assets/Create Character Prefab")]
    static void LaunchWindowFromAssets()
    {
        modelFile = Selection.activeGameObject;
         properties = (CharacterCreatorProperties)AssetDatabase.LoadAssetAtPath("./CharacterCreatorProps.asset", typeof(CharacterCreatorProperties));
        CharacterConfigWindow window = (CharacterConfigWindow)EditorWindow.GetWindow(typeof(CharacterConfigWindow));
        
        window.Show();
    }

    void OnGUI()
    {

        float labelWidth = 150f;

        GUILayout.BeginHorizontal(); //4
        GUILayout.Label("FBX File", GUILayout.Width(labelWidth)); //5
        modelFile = (GameObject)EditorGUILayout.ObjectField(modelFile, typeof(GameObject), true);
        GUILayout.EndHorizontal(); //

        GUILayout.BeginHorizontal(); //4
        GUILayout.Label("Animator Controller", GUILayout.Width(labelWidth)); //5
        animController = (RuntimeAnimatorController)EditorGUILayout.ObjectField(animController, typeof(RuntimeAnimatorController), true);
        GUILayout.EndHorizontal(); //


        if (GUILayout.Button("Create")) //10
        {
            CharacterCreator.PacketCharacter(modelFile.name, modelFile);
            PlayerPrefs.DeleteAll();
            Debug.Log("PlayerPrefs Reset");
        }
    }


}