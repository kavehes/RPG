using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshRenderer))]
public class TilemapScript : MonoBehaviour {

    [Header("Setup of the scene")]
    public int sizeX = 10;
    int prevX = 0;
    public int sizeY = 10;
    int prevY = 0;
    public int tileSize = 1;
    public int tileResolution = 16;
    public Texture2D tileset;

    [Header("Differents tiles used by the editor")]
    public int SelectedTile = 0;
    public int NeutralTile = 0;
    
    public int[] tilesIndex;

    public TextAsset json;
    
	void Start () {
        if(tilesIndex == null)
        {
            tilesIndex = new int[0];
        }
        BuildMesh();
	}
	
    public void BuildMesh()
    {

        if (tilesIndex.Length == 0)
        {
            tilesIndex = new int[sizeX*sizeY];
        }
        else if(tilesIndex.Length != sizeX*sizeY){
            //Store all the data
            int[] previous = tilesIndex;
            int Xdiff = sizeX - prevX;

            tilesIndex = new int[sizeX * sizeY];
            for(int i = 0; i < tilesIndex.Length; i++)
            {
                tilesIndex[i] = NeutralTile;
            }

            //Keep the previous tiles
            for (int y = 0; y < Mathf.Min(sizeY, prevY); y++)
            {
                for (int x = 0; x < Mathf.Min(sizeX, prevX); x++)
                {
                    tilesIndex[x + y * sizeX] = previous[x + y * sizeX - y * Xdiff];
                }
            }
            Debug.Log("New Size");
        }
        prevX = sizeX;
        prevY = sizeY;

        //Differents Meshes variable
        int tileNumber = sizeX * sizeY;
        int triNumber = tileNumber * 2;
       
        Vector3[] vertices = new Vector3[tileNumber * 4];
        Vector3[] normals = new Vector3[tileNumber * 4];
        Vector2[] uv = new Vector2[tileNumber * 4];
        int[] triangles = new int[triNumber * 3];

        //We build each tiles
        for(int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                int vertOri = y * sizeX * 4 + x * 4;
                //The vertice
                vertices[vertOri] = new Vector3(x * tileSize, (y+1)*tileSize, 0);
                vertices[vertOri + 1] = new Vector3((x + 1) * tileSize, (y + 1) * tileSize, 0);
                vertices[vertOri + 2] = new Vector3((x + 1)*tileSize, y * tileSize, 0);
                vertices[vertOri + 3] = new Vector3(x * tileSize, y * tileSize, 0);

                //The normals
                for (int i = 0; i < 4; i++)
                {
                    normals[vertOri + i] = Vector3.back;
                }

                //The UVs
                for (int i = 0; i < 4; i++)
                {
                    //uv[vertOri + i] = uvFromIndex(tileIndex[x, y],i);
                    uv[vertOri + i] = uvFromIndex(tilesIndex[x+y*sizeX], i);
                }
            }
        }

        //We make the triangles
        for (int i = 0; i < tileNumber; i++)
        {
            triangles[i * 6 + 0] = i * 4 + 0;
            triangles[i * 6 + 1] = i * 4 + 1;
            triangles[i * 6 + 2] = i * 4 + 2;

            triangles[i * 6 + 3] = i * 4 + 0;
            triangles[i * 6 + 4] = i * 4 + 2;
            triangles[i * 6 + 5] = i * 4 + 3;
        }


        //Create a Mesh and populate with the data
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
        mesh.uv = uv;

        //Assign Mesh to the component
        MeshFilter mesh_filter = GetComponent<MeshFilter>();
        MeshCollider mesh_collider = GetComponent<MeshCollider>();
        MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();

        if (mesh_renderer.sharedMaterial != null)
        {
            mesh_renderer.sharedMaterial.mainTexture = tileset;
        }
        mesh_collider.sharedMesh = mesh;
        mesh_filter.mesh = mesh;

    }

    Vector2 uvFromIndex(int tileIdex, int VerticeNumber)
    {
        /* Enter the index of the tile to map
         * Vertice number gives which corner to make
         * Peut être mettre une orientation après */

        Vector2 v = new Vector2(0,0);
        int tileX = tileIdex % (tileset.width/tileResolution);
        int tileY = Mathf.FloorToInt(tileIdex/ (tileset.width / tileResolution));
        float tileSizeX = (float)tileResolution / tileset.width;
        float tileSizeY = (float)tileResolution / tileset.height;

        switch (VerticeNumber)
        {
            case 0:
                v = new Vector2(tileSizeX*(tileX) ,1-tileSizeY * (tileY));
                break;
            case 1:
                v = new Vector2(tileSizeX * (tileX + 1),1-tileSizeY * (tileY));
                break;
            case 2:
                v = new Vector2(tileSizeX * (tileX + 1),1- tileSizeY * (tileY + 1));
                break;
            case 3:
                v = new Vector2(tileSizeX * (tileX), 1-tileSizeY * (tileY + 1));
                break;
        }
        return v;
    }

    public void PaintTile(float x, float y, int newTile)
    {
        int X = (int)x / tileSize;
        int Y = (int)y / tileSize;
        //tileIndex[X, Y] = newTile;
        tilesIndex[X + Y*sizeX] = newTile;
        BuildMesh();
    }

    public void MoveTiles(string direction)
    {
        int[] previous = new int[tilesIndex.Length];
        for (int i = 0; i < previous.Length; i++)
        {
            previous[i] = tilesIndex[i];
        }
        switch (direction)
        {
            case "Down":
                for (int y = 0; y < sizeY; y++)
                {
                    for (int x = 0; x < sizeX; x++)
                    {
                        if (y == sizeY-1)
                            tilesIndex[x + y * sizeX] = previous[x];
                        else
                            tilesIndex[x + y * sizeX] = previous[x + y * sizeX + sizeX];
                    }
                }
                break;
            case "Right":
                for (int y = 0; y < sizeY; y++)
                {
                    for (int x = 0; x < sizeX; x++)
                    {
                        if (x == 0)
                            tilesIndex[x + y * sizeX] = previous[sizeX - 1 + y * sizeX];
                        else
                            tilesIndex[x + y * sizeX] = previous[x + y * sizeX - 1];
                    }
                }
                break;
            case "Up":
                for (int y = 0; y < sizeY; y++)
                {
                    for (int x = 0; x < sizeX; x++)
                    {
                        if (y == 0)
                            tilesIndex[x + y * sizeX] = previous[x + sizeX * (sizeY-1)];
                        else
                            tilesIndex[x + y * sizeX] = previous[x + y * sizeX - sizeX];
                    }
                }
                break;
            case "Left":
                for (int y = 0; y < sizeY; y++)
                {
                    for (int x = 0; x < sizeX; x++)
                    {
                        if (x == sizeX - 1)
                            tilesIndex[x + y * sizeX] = previous[y * sizeX];
                        else
                            tilesIndex[x + y * sizeX] = previous[x + y * sizeX + 1];
                    }
                }
                break;
        }
        BuildMesh();
    }

}