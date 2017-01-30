using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CameraScript : MonoBehaviour {

    public float travellingSpeed = 0.5f;
    public float InGameSpeed = 3;

    public Vector3 targetPosition;
    public bool AtPosition;
    public float precision = 0.2f;

    public Player player;

    Vector2 direction;
    Rigidbody2D rigid;

    void Start()
    {   
        rigid = GetComponent<Rigidbody2D>();
        targetPosition = player.transform.position + Vector3.back * 10;
        TPtoTarget();
    }

    void FixedUpdate()
    {

        if (GameController.current.gamestate == GameController.GameState.InScene)
        {
            MovetoTarget(travellingSpeed);
        }
        else
        {
            targetPosition = player.transform.position + Vector3.back * 10;
            //lerper la vitesse de la caméra plus tard
            MovetoTarget(InGameSpeed);
        }
    }

    void TPtoTarget()
    {
        transform.position = targetPosition;
        AtPosition = true;
    }

    //Move toward the target if away from it
    void MovetoTarget(float sp)
    {
        AtPosition = Vector2.Distance(transform.position, targetPosition) <= precision;
        if (!AtPosition)
        {
            direction = (targetPosition - transform.position).normalized * sp;
            rigid.velocity = direction;
        }
        else
        {
            rigid.velocity = Vector2.zero;
        }
    }
}
