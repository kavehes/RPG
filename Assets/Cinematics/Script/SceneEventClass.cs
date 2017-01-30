using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SceneEventClass {

    [SerializeField]
    public enum actorAction
    {
        DoNothing,
        Speak,
        WalkTo,
        RunTo,
        Emote
    }


    //The actions the director will take
    [SerializeField]
    public enum directorAction
    {
        DoNothing,
        MoveCamera,
        DisplayImage,
        RemoveImage,
        ZoomIn,
        AddObject,
        EventScript
    }
    [SerializeField]
    public directorAction Director;
    [SerializeField]
    public bool DirectorReady;
    //If we display an image to the audience
    [SerializeField]
    public Sprite imageToDisplay;
    [SerializeField]
    public Vector2 newCameraPosition;
    [SerializeField]
    public EventScript eventToPlay;
    public float Speed;

    //Changer les arrays en list !!!
    [SerializeField]
    public List<actorAction> act = new List<actorAction>();
    //If talking, is used for Speech, if Emote, is used for the animation
    [SerializeField]
    public List<string> txtAction = new List<string>();
    [SerializeField]
    public List<Vector2> new_positions = new List<Vector2>();

    public void  SaveAssets()
    {
    }
    public SceneEventClass(int actorNumbers)
    {
        for (int i = 0; i < actorNumbers; i++)
        {
            txtAction.Add("Insert your text here");
            act.Add(actorAction.DoNothing);
            new_positions.Add(Vector2.zero);
        }
    }

    public void AddActor()
    {
        txtAction.Add("Insert your text here");
        act.Add(actorAction.DoNothing);
        new_positions.Add(Vector2.zero);
    }

    public void RemoveActor(int a)
    {
        txtAction.RemoveAt(a);
        act.RemoveAt(a);
        new_positions.RemoveAt(a);
    }

}
