using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class CharacterCreator : MonoBehaviour
{
    static string propertiesFilePath = " Assets/Scripts/Common/Tools/CharacterCreator/CharacterCreatorProps";

    //Settings
    public string characterPrefabName;
    // Connections
    public GameObject fbxPrefab;
    // State Variables
    
    // Start is called before the first frame update
    void Start()
    {
        //InitConnections();
        //InitState();
    }
    void InitConnections(){
    }
    void InitState(){
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PacketCharacter();
        }
    }

    private void OnGUI()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PacketCharacter();
        }
    }

   
    public void PacketCharacter()
    {
        if (!Directory.Exists("Assets/Prefabs/BaseGameElements"))
            AssetDatabase.CreateFolder("Assets", "Prefabs/Models");
        string localPath = "Assets/Prefabs/Models/" + characterPrefabName + ".prefab";

        Debug.Log("Packet Character");

        // Make sure the file name is unique, in case an existing Prefab has the same name.
        localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

        // Create the new Prefab and log whether Prefab was saved successfully.
        bool prefabSuccess;
        GameObject parentObject = new GameObject();
        parentObject.name = characterPrefabName;

        GameObject modelGO = GameObject.Instantiate(fbxPrefab);
        modelGO.transform.parent = parentObject.transform;
        modelGO.name = characterPrefabName + "Model";

        PrefabUtility.SaveAsPrefabAssetAndConnect(parentObject, localPath, InteractionMode.UserAction, out prefabSuccess);
        if (prefabSuccess == true)
            Debug.Log("Prefab was saved successfully");
        else
            Debug.Log("Prefab failed to save" + prefabSuccess);
    }

    public static void PacketCharacter(string characterPrefabName, GameObject fbxPrefab)
    {
        if (!Directory.Exists("Assets/Prefabs/BaseGameElements"))
            AssetDatabase.CreateFolder("Assets", "Prefabs/BaseGameElements");
        string localPath = "Assets/Prefabs/BaseGameElements/" + characterPrefabName + ".prefab";

        Debug.Log("Packet Character");

        // Make sure the file name is unique, in case an existing Prefab has the same name.
        localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

        // Create the new Prefab and log whether Prefab was saved successfully.
        bool prefabSuccess;
        GameObject parentObject = new GameObject();
        parentObject.name = characterPrefabName;

        string modelPath = "Assets/Prefabs/Models/" + characterPrefabName + "_model.prefab";
        GameObject modelGO = CreateModel(fbxPrefab);
 
        modelGO.transform.parent = parentObject.transform;
        PrefabUtility.SaveAsPrefabAssetAndConnect(modelGO, modelPath, InteractionMode.UserAction, out prefabSuccess);
        PrefabUtility.SaveAsPrefabAssetAndConnect(parentObject, localPath, InteractionMode.UserAction, out prefabSuccess);
        if (prefabSuccess == true)
            Debug.Log("Prefab was saved successfully");
        else
            Debug.Log("Prefab failed to save" + prefabSuccess);
    }


    private static GameObject CreateModel(GameObject fbxPrefab) {
        GameObject modelGO = GameObject.Instantiate(fbxPrefab);

        Animator modelAnimator = modelGO.GetComponent<Animator>();
        if(modelAnimator != null)
        {
            ConfigAnimator(modelAnimator);
        }

        modelGO.name = fbxPrefab.name + "Model";
      
        return modelGO;

    }

    private static void ConfigAnimator(Animator animator)
    {
        animator.applyRootMotion = false;
        //CharacterCreatorProperties properties = (CharacterCreatorProperties)AssetDatabase.LoadAssetAtPath(propertiesFilePath, typeof(CharacterCreatorProperties));
        //animator.runtimeAnimatorController = properties.defaultAnimatorController;
    }

   

   // [MenuItem("Assets/Create Character Prefab")]
    private static void CreateCharacterPrefab()
    {
        GameObject fbxPrefab =  (GameObject)Selection.activeObject;
        PacketCharacter(fbxPrefab.name, fbxPrefab);
        

    }

}

