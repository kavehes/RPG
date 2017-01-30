using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneReader : MonoBehaviour {

    public ActorScript[] Actors;

    CameraScript cam;
    public bool CanRead = true;
    public bool Reading = false;

    int theyWhomTalks;
    public Image avat;
    public Text Name;
    public Text texte;
    public Image CG;
    string txtToDisplay;
    public GameObject DialogueBox;

    public bool[] ready;
    bool EveryoneReady;
    int TdL = 0;

    public enum State {Inactive,Waiting,InDialogue,InDialogueWait};

    public State state = State.Inactive;

    public SceneScript sceneToRead;

	void Start () {
        //Remplacer tout ça par une référence à un CanvasScript plus tard
        DialogueBox = GameObject.FindGameObjectWithTag("DialogObject");
        CG = GameObject.FindGameObjectWithTag("CGHolder").GetComponent<Image>();
        avat = GameObject.FindGameObjectWithTag("AvatHolder").GetComponent<Image>();
        Name = GameObject.FindGameObjectWithTag("NameHolder").GetComponent<Text>();
        texte = GameObject.FindGameObjectWithTag("BulleHolder").GetComponent<Text>();
        DialogueBox.SetActive(false);
        CG.gameObject.SetActive(false);

        cam = Camera.main.GetComponent<CameraScript>();

        if (Reading)
        {
            SceneRead();
        }
	}
	
	void Update () {
        //On check si tout le monde est ready (Way too complicated)
        EveryoneReady = true;
        if (Reading)
        {
            switch (sceneToRead.Events[TdL].Director)
            {
                case SceneEventClass.directorAction.MoveCamera:
                    sceneToRead.Events[TdL].DirectorReady = cam.AtPosition;
                    break;
            }

            for (int i = 0; i < ready.Length; i++)
            {
                if (sceneToRead.Events[TdL].act[i] == SceneEventClass.actorAction.WalkTo && Actors[i] != null)
                    ready[i] = Actors[i].AtDestination;

                if (!ready[i])
                {
                    EveryoneReady = false;
                }
                if (ready[i] && !sceneToRead.Events[TdL].DirectorReady)
                    EveryoneReady = false;
            }

            //Les différents states en si on peux passer au suivant ou non
            if (state == State.InDialogue && Input.GetButtonDown("Fire1"))
            {
                //Interromps le dialogue;
                StopAllCoroutines();
                texte.text = txtToDisplay;
                state = State.InDialogueWait;
                ready[theyWhomTalks] = true;
            }

            if (EveryoneReady)
            {
                if (state == State.InDialogueWait && Input.GetButtonDown("Fire1"))
                {
                    DialogueBox.SetActive(false);
                    if (TdL >= sceneToRead.Events.Count - 1)
                    {
                        DialogEnd();
                    }
                    else
                    {
                        TdL++;
                        SceneRead();
                    }
                }
                else if (state == State.Waiting && EveryoneReady)
                {
                    DialogueBox.SetActive(false);
                    if (TdL >= sceneToRead.Events.Count - 1)
                    {
                        DialogEnd();
                    }
                    else
                    {
                        TdL++;
                        SceneRead();
                    }
                }
            }
        }
	}

    public void SceneRead()
    {
        Reading = true;
        GameController.current.gamestate = GameController.GameState.InScene;
        Debug.Log(sceneToRead.Actors.Count);
        ready = new bool[sceneToRead.Actors.Count];
        state = State.Waiting;

        //Check who does what, do and do it
        //The Director
        switch (sceneToRead.Events[TdL].Director)
        {
            case SceneEventClass.directorAction.DoNothing:
                sceneToRead.Events[TdL].DirectorReady = true;
                break;
            case SceneEventClass.directorAction.DisplayImage:
                //Enable an Image in the Canvas, behind the philactere
                //Put a Sprite in It
                sceneToRead.Events[TdL].DirectorReady = true;
                CG.sprite = sceneToRead.Events[TdL].imageToDisplay;
                CG.gameObject.SetActive(true);
                break;
            case SceneEventClass.directorAction.RemoveImage:
                //Disable that Image
                CG.gameObject.SetActive(false);
                sceneToRead.Events[TdL].DirectorReady = true;
                break;
            case SceneEventClass.directorAction.MoveCamera:
                //Move the Camera to a specific place
                //Is set to ready once the camera reached it's destination
                cam.targetPosition = (Vector3)sceneToRead.Events[TdL].newCameraPosition + Vector3.forward*cam.transform.position.z;
                cam.AtPosition = false;
                break;
            case SceneEventClass.directorAction.AddObject:
                sceneToRead.Events[TdL].DirectorReady = true;
                break;
            case SceneEventClass.directorAction.ZoomIn:
                break;
        }

        for (int i = 0; i < sceneToRead.Actors.Count; i++)
        {
            switch (sceneToRead.Events[TdL].act[i])
            {   
                //On setup les différentes actions pour les Acteurs
                case SceneEventClass.actorAction.DoNothing:
                    ready[i] = true;
                    break;
                case SceneEventClass.actorAction.WalkTo:
                    Actors[i].targetPosition = sceneToRead.Events[TdL].new_positions[i];
                    Actors[i].AtDestination = false;
                    break;
                case SceneEventClass.actorAction.Speak:
                    theyWhomTalks = i;
                    state = State.InDialogue;
                    DialogueBox.SetActive(true);
                    Name.color = sceneToRead.Actors[i].color;
                    Name.text = sceneToRead.Actors[i].Name;
                    txtToDisplay = sceneToRead.Events[TdL].txtAction[i];
                    avat.sprite = sceneToRead.Actors[i].iconSprite;
                    StartCoroutine(WriteText(txtToDisplay, theyWhomTalks));
                    break;
            }
        }
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(1);
        CanRead = true;
    }

    public void DialogEnd()
    {
        if(sceneToRead.nextScene != null)
        {
            //Mettre un cooldown surement
            sceneToRead = sceneToRead.nextScene;
            TdL = 0;
        }
        StartCoroutine(Cooldown());
        DialogueBox.SetActive(false);
        CG.gameObject.SetActive(false);
        GameController.current.gamestate = GameController.GameState.Ingame;
        Reading = false;
        CanRead = false;
    }

    IEnumerator WriteText(string txt, int talker)
    {
        for (int i = 0; i < txtToDisplay.Length+1; i++)
        {
            texte.text = txtToDisplay.Substring(0, i);
            if (i > 0)
            {
                switch(txtToDisplay.Substring(i - 1, 1))
                {
                    case ",":
                        yield return new WaitForSeconds(0.25f);
                        break;
                    case ".":
                        yield return new WaitForSeconds(0.5f);
                        break;
                    case ":":
                        yield return new WaitForSeconds(0.4f);
                        break;
                }
            }
            yield return new WaitForSeconds(0.05f);
        }
        state = State.InDialogueWait;
        ready[talker] = true;
    }
}
