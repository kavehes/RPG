using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Player : CharacterClass {

    Rigidbody2D rigid;
    Animator anim;

    //The closest npc, to talk to
    public NPC npc;
    public bool facinRight;

	void Start () {
        Player = true;
        AggroPlayer = false;
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
	}
	
    void Update()
    {
        anim.SetFloat("SpeedH", Mathf.Abs(Input.GetAxis("Horizontal")));
        anim.SetFloat("RatioVH", Mathf.Abs(Input.GetAxis("Vertical") == 0? 2 : (float)Input.GetAxis("Horizontal"))/Mathf.Abs(Input.GetAxis("Vertical")));
        anim.SetFloat("SpeedV", Input.GetAxis("Vertical"));
        //On va chopper les inputs ici, puis renvoyer celles qui faut dans le FixedUpdate
        switch (GameController.current.gamestate)
        {
            case GameController.GameState.Ingame:
                if(Input.GetButton("Fire1") && npc != null)
                {
                    if(npc.GetComponent<SceneReader>()!= null && npc.GetComponent<SceneReader>().CanRead)
                    {
                            npc.GetComponent<SceneReader>().SceneRead();
                        GameController.current.gamestate = GameController.GameState.InScene;
                        //npc = null;
                        rigid.velocity = Vector2.zero;
                    }
                }
                break;

        }

        if((Input.GetAxis("Horizontal")>0 && !facinRight )|| (Input.GetAxis("Horizontal") < 0 && facinRight))
        {
            Flip();
        }
    }

    void FixedUpdate() {
        switch (GameController.current.gamestate)
        {
            case GameController.GameState.Ingame:
                //In game we get to  move, attack and talk to characters
                rigid.velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized * Speed;
                break;
            case GameController.GameState.InScene:
                //Basically nothing ?
                break;
            case GameController.GameState.Menu:
                break;
        }
	}

    void OnTriggerEnter2D( Collider2D other)
    {
        if (other.GetComponent<NPC>() != null)
        {
            npc = other.GetComponent<NPC>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(npc != null && other.gameObject == npc.gameObject)
        {
            npc = null;
        }
    }

    void Flip()
    {
        facinRight = !facinRight;
        gameObject.GetComponent<SpriteRenderer>().flipX = !gameObject.GetComponent<SpriteRenderer>().flipX;
    }
}
