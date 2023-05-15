using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(CharacterCreator))]
public class CharacterCreatorEditor : Editor
{
    //Settings

    // Connections

    // State Variables
    string characterPrefabName = "";
    // Start is called before the first frame update
    public override void OnInspectorGUI() 
    {
        base.DrawDefaultInspector();
        CharacterCreator charCreatorTarget = (CharacterCreator)target;
        if (GUILayout.Button("Create")) 
        {
            charCreatorTarget.PacketCharacter();
           
        }
    }
}

