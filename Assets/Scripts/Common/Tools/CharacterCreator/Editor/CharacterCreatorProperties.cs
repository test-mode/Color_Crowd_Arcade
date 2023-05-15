using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterCreatorProps", menuName = "ToolsData/Character Creator Properties", order = 1)]
public class CharacterCreatorProperties : ScriptableObject
{
    //Settings

    // Connections
    public RuntimeAnimatorController defaultAnimatorController;
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
        
    }
}

