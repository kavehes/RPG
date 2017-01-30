using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public static GameController current;

    public enum GameState
    {
        Ingame,
        //Dans un menu, pour figer tout le reste
        Menu,
        //Dans une cinematique, pour figer les npc je suppose
        InScene
    }
    
    public GameState gamestate;
    
    void Awake()
    {   
        /*
        if (GameObject.FindGameObjectWithTag("GameController")!=null)
        {
            Destroy(gameObject);
        }*/
        current = this;
        
        DontDestroyOnLoad(gameObject);    }

    void Update()
    {

    }
}
