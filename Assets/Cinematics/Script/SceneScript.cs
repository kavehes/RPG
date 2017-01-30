using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New_Scenario", menuName = "Theater/Scenario", order = 2)]
public class SceneScript : ScriptableObject {

    [SerializeField]
    public List<Actor> Actors = new List<Actor>();
    [SerializeField]
    public List<SceneEventClass> Events = new List<SceneEventClass>(1);

    public SceneScript nextScene;
    
    public SceneScript()
    {
        Actors.Add(null);
        Events.Add(new SceneEventClass(1));
    }
}