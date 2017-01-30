using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ActorScript : MonoBehaviour {

    public Actor identity;

    Rigidbody2D rigid;
    public Vector3 targetPosition;
    public bool AtDestination;
    public float movePrecision = 0.1f;
    //Speed ingame
    public float speed = 5.0f;
    //SpeedInCinematic 
    //public float speedIC;

    public bool Mesh3D;
    public Animator anim;
    public Transform mesh;

    void Start () {
        rigid = GetComponent<Rigidbody2D>();
        targetPosition = transform.position;
	}

    void FixedUpdate () {
        AtDestination = Vector3.Distance(transform.position, targetPosition) <= movePrecision;
        if (!AtDestination)
        {
            rigid.velocity = new Vector2(targetPosition.x - transform.position.x,
                                         targetPosition.y - transform.position.y).normalized * speed;
        }
        else
        {
            rigid.velocity = Vector2.zero;
        }

        if (Mesh3D)
        {   
            anim.SetFloat("Speed",rigid.velocity.magnitude);
            Vector3 direction = new Vector3(rigid.velocity.x, rigid.velocity.y, 0);
            Quaternion hey = Quaternion.FromToRotation(Vector3.down, direction);
            mesh.localRotation = Quaternion.Euler(new Vector3(0, -hey.eulerAngles.z, 0));
        }
    }
}
