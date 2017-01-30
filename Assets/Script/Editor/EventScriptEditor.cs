using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

[CustomEditor(typeof(EventScript))]
public class EventScriptEditor : Editor {

    List<MethodInfo> myMethods = new List<MethodInfo>();

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }

}
