using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class SceneScriptEditor : EditorWindow{

    public SceneScript sceneScript;
    int sceneIndex = 0;
    int newIndex = 0;
    public Text bulleText;
    public GizmoDrawer Giz;

    Vector2 scrollPos;
    
    SerializedObject serializedObject;

    GUIStyle speechbuble;

    [MenuItem("Window/Scene Script Editor O_O")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(SceneScriptEditor)); //Get's us the Window
    }

    void OnEnable()
    {
        Giz = Camera.main.GetComponent<GizmoDrawer>();
        if (EditorPrefs.HasKey("ObjectPath"))
        {
            string objectPath = EditorPrefs.GetString("ObjectPath");
            sceneScript = AssetDatabase.LoadAssetAtPath(objectPath, typeof(SceneScript)) as SceneScript;
        }

        SceneScript obj = ScriptableObject.CreateInstance<SceneScript>();
        serializedObject = new UnityEditor.SerializedObject(obj);
    }

    void OnGUI()
    {
        EditorGUI.BeginChangeCheck();
        
        speechbuble = GUI.skin.textArea;
        speechbuble.wordWrap = true;

        GUILayout.BeginHorizontal();
        GUILayout.Label("Scene Script Editor", EditorStyles.boldLabel);
        if (sceneScript != null)
        {
            //Basically show us the assets in the inspector ?
            if (GUILayout.Button("Show Item List"))
            {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = sceneScript;
            }
        }
        if (GUILayout.Button("Open Item List"))
        {
            OpenItemList();
        }/*
        if (GUILayout.Button("New Item List"))
        {
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = sceneScript;
        }*/
        GUILayout.EndHorizontal();

        if (sceneScript != null)
        {

            GUILayout.Space(20);
            GUILayout.Label("Setup", EditorStyles.boldLabel);
            bulleText = (Text)EditorGUILayout.ObjectField("Bulle", bulleText, typeof(Text), true);
            Giz = (GizmoDrawer)EditorGUILayout.ObjectField("MainCamera", Giz, typeof(GizmoDrawer), true);
            GUILayout.Space(5);
            if (sceneScript.Actors != null)
            {
                for (int a = 0; a < sceneScript.Actors.Count; a++)
                {
                    EditorGUILayout.BeginHorizontal();
                    sceneScript.Actors[a] = (Actor)EditorGUILayout.ObjectField("Actor : " + a, sceneScript.Actors[a], typeof(Actor), true); ;
                    if (GUILayout.Button("Remove Actor"))
                    {
                        sceneScript.Actors.RemoveAt(a);
                        for (int j = 0; j < sceneScript.Events.Count; j++)
                        {
                            sceneScript.Events[j].RemoveActor(a);
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            if (GUILayout.Button("Add Actor"))
            {
                sceneScript.Actors.Add(null);
                for (int i = 0; i < sceneScript.Events.Count; i++)
                {
                    sceneScript.Events[i].AddActor();
                }
                this.Repaint();
            }

            GUILayout.Space(10);

            if (sceneScript != null)
            {
                GUILayout.Label("Navigation", EditorStyles.boldLabel);

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Previous"))
                    sceneIndex = sceneIndex == 0 ? 0 : sceneIndex - 1;

                newIndex = EditorGUILayout.IntField(newIndex, GUILayout.Width(60));
                if (GUILayout.Button("Go To"))
                    sceneIndex = newIndex >= sceneScript.Events.Count - 1 ? sceneScript.Events.Count - 1 : newIndex;

                if (GUILayout.Button("Next"))
                    sceneIndex = sceneIndex >= sceneScript.Events.Count - 1 ? sceneScript.Events.Count - 1 : sceneIndex + 1;

                GUILayout.EndHorizontal();

                GUILayout.Space(10);

                //Les actions pour chaque events, un par un
                if (sceneScript.Events != null)
                    GUILayout.Label("Event : " + (sceneIndex+1) + "/" + (sceneScript.Events.Count), EditorStyles.boldLabel);
                GUILayout.BeginHorizontal();

                GUILayout.Space(10);
                //Managing the events
                if (GUILayout.Button("Add Event"))
                {
                    AddEvent(sceneIndex);
                    sceneIndex++;
                }
                if (GUILayout.Button("Remove Event") && sceneScript.Events.Count>1)
                    RemoveEvent(sceneIndex);

                GUILayout.EndHorizontal();

                if (sceneScript.Events != null)
                {
                    //What does the director do ?
                    GUILayout.Label("Director", EditorStyles.boldLabel);
                    Giz.CamInfo(Vector3.zero, false);
                    sceneScript.Events[sceneIndex].Director = (SceneEventClass.directorAction)EditorGUILayout.EnumPopup(sceneScript.Events[sceneIndex].Director);
                    switch (sceneScript.Events[sceneIndex].Director)
                    {
                        case SceneEventClass.directorAction.DoNothing:
                            //Huh
                            break;
                        case SceneEventClass.directorAction.MoveCamera:
                            sceneScript.Events[sceneIndex].newCameraPosition = EditorGUILayout.Vector2Field("New Position",sceneScript.Events[sceneIndex].newCameraPosition);
                            Giz.CamInfo(sceneScript.Events[sceneIndex].newCameraPosition, true);
                                break;
                        case SceneEventClass.directorAction.DisplayImage:
                            sceneScript.Events[sceneIndex].imageToDisplay = (Sprite)EditorGUILayout.ObjectField("Image : ",sceneScript.Events[sceneIndex].imageToDisplay,typeof(Sprite),false);
                            break;
                        case SceneEventClass.directorAction.RemoveImage:
                            //Will be taken care of in the reader
                            break;
                        case SceneEventClass.directorAction.ZoomIn:
                            //Zoom Level
                            break;
                    }

                    GUILayout.Space(10);

                    //What does the actors do ?
                    scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.MaxWidth(512));
                    for (int i = 0; i < sceneScript.Actors.Count; i++)
                    {
                        if (sceneScript.Actors[i] != null)
                        {
                            GUILayout.Space(10);



                            GUILayout.BeginHorizontal();
                            GUILayout.Label(sceneScript.Actors[i].icon, GUILayout.Width(100), GUILayout.Height(100));
                            GUILayout.BeginVertical();
                            GUILayout.Label(sceneScript.Actors[i].name, EditorStyles.boldLabel);
                            sceneScript.Events[sceneIndex].act[i] = (SceneEventClass.actorAction)EditorGUILayout.EnumPopup("Action ", sceneScript.Events[sceneIndex].act[i]);

                            switch (sceneScript.Events[sceneIndex].act[i])
                            {
                                case SceneEventClass.actorAction.DoNothing:
                                    break;
                                case SceneEventClass.actorAction.Speak:
                                    sceneScript.Events[sceneIndex].txtAction[i] = EditorGUILayout.TextArea(sceneScript.Events[sceneIndex].txtAction[i], speechbuble, GUILayout.Height(60), GUILayout.ExpandWidth(true));
                                    if (GUILayout.Button("ShowInText"))
                                    {
                                        if(bulleText != null)
                                        bulleText.text = sceneScript.Events[sceneIndex].txtAction[i];
                                        else
                                        Debug.LogWarning("Need a Text for displaying purpose");
                                    }
                                    break;
                                case SceneEventClass.actorAction.WalkTo:
                                    sceneScript.Events[sceneIndex].new_positions[i] = EditorGUILayout.Vector2Field("Move Toward", sceneScript.Events[sceneIndex].new_positions[i]);

                                    break;
                            }

                            int numberOfVector = 0;
                            Vector3[] vectors = new Vector3[0];

                            for (int a = 0; a < sceneScript.Actors.Count; a++)
                            {
                                if (sceneScript.Events[sceneIndex].act[a] == SceneEventClass.actorAction.WalkTo)
                                {
                                    numberOfVector++;
                                    Vector3[] _v = vectors;
                                    vectors = new Vector3[numberOfVector];
                                    vectors[vectors.Length - 1] = sceneScript.Events[sceneIndex].new_positions[a];
                                    for (int j = 0; j < _v.Length; j++)
                                    {
                                        vectors[j] = _v[j];
                                    }
                                    if (Giz != null)
                                    {
                                        if (vectors != null)
                                            Giz.CubeInfo(vectors);
                                        else
                                            Giz.CubeInfo(false);
                                    }
                                }
                                SceneView.RepaintAll();
                            }

                            GUILayout.EndVertical();
                            GUILayout.EndHorizontal();
                        }
                    }
                }

                GUILayout.Space(10);

                EditorGUILayout.EndScrollView();
            }
        }
        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(sceneScript);
        }
    }

    void OnSceneGUI()
    {
        HandleUtility.Repaint();
    }

    void OpenItemList()
    {
        string absPath = EditorUtility.OpenFilePanel("Select Scenario", "/Assets/Data/Scenario", "");
        if (absPath.StartsWith(Application.dataPath))
        {
            string relPath = absPath.Substring(Application.dataPath.Length - "Assets".Length);
            sceneScript = AssetDatabase.LoadAssetAtPath(relPath, typeof(SceneScript)) as SceneScript;
            if (sceneScript.Actors == null)
            {
               //sceneScript.Initiate();
            }
            if (sceneScript)
            {
                EditorPrefs.SetString("ObjectPath", relPath);
            }
        }
    }

    void AddEvent(int index)
    {
        SceneEventClass newEvent = new SceneEventClass(sceneScript.Actors.Count);
        sceneScript.Events.Insert(index+1,newEvent);
    }
    void RemoveEvent(int index)
    {
        sceneScript.Events.RemoveAt(index);
        //Decremente le compteur si on est au bout des events
        sceneIndex = sceneIndex >= sceneScript.Events.Count ? sceneScript.Events.Count - 1 : sceneIndex;
    }

}
