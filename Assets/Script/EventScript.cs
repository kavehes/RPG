using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class EventScript : MonoBehaviour {

    public List<MethodInfo> myMethods = new List<MethodInfo>(0);

    //Will have a fuckton of methods that will do various things, One script, one object, one event
    /* List of Events :
     * Loading a new Scene
     * Add items to inventory
     * 
     * 
     */

    public void LoadAScene(string scene)
    {
        //Load a scene
    }

}
