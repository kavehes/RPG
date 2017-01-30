using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "Theater/Actor", order = 1)]
[RequireComponent(typeof(Rigidbody2D))]
public class Actor : ScriptableObject {

    //The icone/face of the actor, to be displayed in the dialogue box and the editor
    public Texture2D icon;
    public Sprite iconSprite;
    //Differentes stats liées au personnage
    public string Name;

    //Tout ce qui est lié aux cinematiques
    public bool ready;

    public Color color;

    //To be called at each step of a cinematic, a charger dans les personnages
    public void NewStep()
    {
        ready = false;
    }

    public void DoNothing()
    {
        ready = true;
    }

    //Pour déplacer le personnage vers un point
    public void cinMoveTo (Vector2 v) {
		
	}
}
