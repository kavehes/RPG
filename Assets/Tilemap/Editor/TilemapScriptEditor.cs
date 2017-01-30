using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TilemapScript))]
public class TilemapScriptEditor : Editor
{

    Vector2 scrollPos;
    Vector2 tileZone;

    //Les coordonnées du tile selectionné
    int SelectedTile;
    int HighlightTile;
    int NeutralTile;
    public bool toggleNumbers;

    LayerMask layermask;


    public override void OnInspectorGUI()
    {
        int tlRs = 0, tlPR = 0, nR = 0;
        TilemapScript tm = (TilemapScript)target;
        if(tm.tileset != null){
            tlRs = tm.tileResolution;
            tlPR = tm.tileset.width / tlRs;
            nR = tm.tileset.height / tlRs;
        }

        SelectedTile = tm.SelectedTile;
        NeutralTile = tm.NeutralTile;

        DrawDefaultInspector();
        /*
        if (tm.tileIndex.GetLength(0) != 0 && tm.tileIndex.GetLength(1) != 0)
        {
            //Un visualisateur de tilemapping
            tileZone = EditorGUILayout.BeginScrollView(tileZone, true, true, GUILayout.MinHeight(tm.tileIndex.GetLength(1) * 16));
            EditorGUI.DrawRect(new Rect(3, 6, tm.tileIndex.GetLength(0) * 16, tm.tileIndex.GetLength(1) * 16), Color.gray);

            for (int y = 0; y < tm.tileIndex.GetLength(1); y++)
            {
                    for (int x = 0; x < tm.tileIndex.GetLength(0); x++)
                {
                    EditorGUI.LabelField(new Rect(6 + x * 16, 3 + (tm.tileIndex.GetLength(1) - y) * 16, 16, 16), "" + tm.tileIndex[x,y]);
                }
            }
            EditorGUILayout.EndScrollView();
        }*/
        
        
        GUILayout.Space(20);

        if (GUILayout.Button("Create Mesh"))
        {
            tm.BuildMesh();
        }


        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Fill with tile"))
        {
            /*for (int y = 0; y < tm.tileIndex.GetLength(1); y++)
            {
                for (int x = 0; x < tm.tileIndex.GetLength(0); x++)
                {
                    tm.tileIndex[x, y] = SelectedTile;
                }
            }*/
            for(int i = 0; i<tm.tilesIndex.Length;i++)
            {
                tm.tilesIndex[i] = SelectedTile;
            }
            tm.BuildMesh();
        }
        if (GUILayout.Button("Empty Mesh"))
        {
            /*
            for (int y = 0; y < tm.tileIndex.GetLength(1); y++)
            {
                for (int x = 0; x < tm.tileIndex.GetLength(0); x++)
                {
                    tm.tileIndex[x, y] = NeutralTile;
                }
            }            */
            for (int i = 0; i < tm.tilesIndex.Length; i++)
            {
                tm.tilesIndex[i] = NeutralTile;
            }
            tm.BuildMesh();
        }

        if (GUILayout.Button("ImportJson"))
            {
            }
        GUILayout.Space(20);
        GUILayout.EndHorizontal();

        //Buttons for offsetting the tiles
        GUILayout.Label("Offset");
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("+Y"))
            tm.MoveTiles("Up");
        if (GUILayout.Button("+X"))
            tm.MoveTiles("Right");
        if (GUILayout.Button("-Y"))
            tm.MoveTiles("Down");
        if (GUILayout.Button("-X"))
            tm.MoveTiles("Left");

        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);

        //The size of the tileset, for drawing purposes i guess
        Rect rect = new Rect(0, 0, tm.tileset.width, tm.tileset.height);

        if (tm.tileset != null)
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos,GUILayout.Height(256));

            //Draw the tileset in the editor
            GUILayout.Label(tm.tileset);

        
            //Pour gerer tous les evenements si la souris est sur le tileset
            if (rect.Contains(Event.current.mousePosition))
            {
                int tileMouseOn = (int)(Event.current.mousePosition.x / tlRs) + (int)(Event.current.mousePosition.y / tlRs) * tlPR;

                //Quand on appuie sur la souris
                if (Event.current.type == EventType.MouseDown)
                {

                    switch (Event.current.button)
                    {
                        case 0:
                            SelectedTile = tileMouseOn;
                            this.Repaint();
                            break;
                        case 1:
                            NeutralTile = tileMouseOn;
                            this.Repaint();
                            break;
                    }

                }
                else
                {
                    //Draw the highlighted one
                    HighlightTile = tileMouseOn;
                    int HTx = HighlightTile % tlPR;
                    int HTy = (int)HighlightTile / tlPR;
                    EditorGUI.DrawRect(new Rect(6 + HTx * tlRs, 3 + HTy * tlRs, tlRs, tlRs), new Color(1f, 1, 1, 0.5f));
                    this.Repaint();
                }

                
            }
            tm.SelectedTile = SelectedTile;
            tm.NeutralTile = NeutralTile;

            //From tile number to Coordonate
            int STx = SelectedTile % tlPR;
            int Sty = (int)SelectedTile/tlPR;
            int NTx = NeutralTile % tlPR;
            int NTy = (int)NeutralTile / tlPR;

            EditorGUI.DrawRect(new Rect(6 + STx * tlRs, 3 + Sty * tlRs, tlRs, tlRs), new Color(0.5f, 0.5f, 1, 0.5f));
            EditorGUI.DrawRect(new Rect(6 + NTx * tlRs, 3 + NTy * tlRs, tlRs, tlRs), new Color(1f, 0.25f, 0.25f, 0.5f));
            //EditorGUI.DrawRect(new Rect(6 + NeutralTile.x * 32, 3 + NeutralTile.y * 32, 32, 32), new Color(1f, 0.25f, 0.25f, 0.5f));

            //Put the numbers on the tiles
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.white;
            for (int i = 0; i < tlPR * nR; i++)
            {
                EditorGUI.LabelField(new Rect(6 + (i % tlPR) * tlRs, 3 + ((int)i / tlPR) * tlRs, tlRs, tlRs), "" + i, style);
            }


            EditorGUILayout.EndScrollView();
        }
    }

    public void OnSceneGUI()
    {
        int ControlID = GUIUtility.GetControlID(FocusType.Passive);
        //Cast a ray on the map and get the point
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, float.MaxValue))
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
            if (Event.current.type == EventType.mouseDown || Event.current.type == EventType.mouseDrag)
            {

                TilemapScript tm = (TilemapScript)target;

                Vector3 realPosition = hit.point - tm.transform.position;
                switch (Event.current.button)
                {
                    case 0:
                        tm.PaintTile(realPosition.x, realPosition.y, SelectedTile);
                        break;
                    case 1:
                        tm.PaintTile(realPosition.x, realPosition.y, NeutralTile);
                        break;
                }
            }
        }
    }
}
