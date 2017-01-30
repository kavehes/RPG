using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoDrawer : MonoBehaviour {

    public bool display;
    public Vector3 cameraPos;


    public Vector3[] Positions;
    public Color[] Colors;

	void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (cameraPos != null && display)
            Gizmos.DrawWireCube(cameraPos,Vector3.up*Camera.main.orthographicSize*2 + Vector3.right*16);


        Gizmos.color = Color.white;
        for (int i = 0; i < Positions.Length; i++)
        {
            Gizmos.DrawWireCube(Positions[i], Vector3.one);
        }
    }

    public void CamInfo(Vector3 Ya, bool show)
    {
        cameraPos = Ya;
        display = show;
    }

    public void  CubeInfo(Vector3[] Yo)
    {
        Positions = Yo;
    }
    public void CubeInfo(bool a)
    {
        if (!a)
            Positions = new Vector3[0];
    }
}
