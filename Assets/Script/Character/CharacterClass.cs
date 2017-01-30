using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterClass : MonoBehaviour {
    /*Cold is basically anti-Human, and machines develloped a resistance to it, the more
     * Mecanical you are the more you are immune to Cold*/
    [Header("'RPG' stats")]
    public float health = 10;
    int stamina;
    float secStamina;
    float coldProgression = 0;
    public int maxStam = 7;

    [Header("'Avatar' Stats")]
    public float Speed = 1;
    public bool Player;
    public bool AggroPlayer;

    [Header("Movement")]
    public Vector2 GoTo;

	void Start () {
        stamina = maxStam;
        secStamina = stamina;
	}
	
	public void GetDamaged (int damage)
    {
        health -= damage;
		if(health <= 0)
        {
            Death();
        }
	}

    public void Death()
    {
        //whatever it's gonna get overrided anyway
    }
}
