using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreateTerrainMesh : MonoBehaviour {

  public Material mats;
  public GameObject player;
  
  public void BuildMesh (JSONObject Map2D) {
    var length = Map2D.list.Count;
    var hMap = GetHeightmap(Map2D, length);
    
    List<Vector3> verts = new List<Vector3>();
    List<int> tris = new List<int>();

    //Bottom left section of the map, other sections are similar
    for(int i = 0; i < length; i++)
    {
      for(int j = 0; j < length; j++)
      {
        //Add each new vertex in the plane
        verts.Add(new Vector3(i, hMap[i,j] * 100, j));
        //Skip if a new square on the plane hasn't been formed
        if (i == 0 || j == 0) continue;
        //Adds the index of the three vertices in order to make up each of the two tris
        tris.Add(length * i + j); //Top right
        tris.Add(length * i + j - 1); //Bottom right
        tris.Add(length * (i - 1) + j - 1); //Bottom left - First triangle
        tris.Add(length * (i - 1) + j - 1); //Bottom left 
        tris.Add(length * (i - 1) + j); //Top left
        tris.Add(length * i + j); //Top right - Second triangle
      }
    }

    Vector2[] uvs = new Vector2[verts.Count];
    for (var i = 0; i < uvs.Length; i++) {//Give UV coords X,Z world coords
      uvs[i] = new Vector2(verts[i].x, verts[i].z);
    }
    GameObject plane = new GameObject("ProcPlane"); //Create GO and add necessary components
    plane.AddComponent<MeshFilter>();
    plane.AddComponent<MeshRenderer>();
    plane.GetComponent<MeshRenderer>().material = mats;
    Mesh procMesh = new Mesh();
    procMesh.vertices = verts.ToArray(); //Assign verts, uvs, and tris to the mesh
    procMesh.uv = uvs;
    procMesh.triangles = tris.ToArray();
    procMesh.RecalculateNormals(); //Determines which way the triangles are facing
    plane.GetComponent<MeshFilter>().mesh = procMesh; //Assign Mesh object to MeshFilter
    plane.AddComponent<MeshCollider>();
    plane.GetComponent<MeshCollider>().sharedMesh = null;
    plane.GetComponent<MeshCollider>().sharedMesh = procMesh;
    plane.AddComponent<ClickMove>();
    plane.GetComponent<ClickMove>().player = player;
  }

  private float[,] GetHeightmap(JSONObject arr, int length) {//JSONObject arr, int length) {
    var heightmap = new float[length, length];

    for (var i = 0; i < length; i++) {
      for (var j = 0; j < length; j++) { 
        heightmap[i, j] = Mathf.Abs(arr[i][j].n) * .5f;
        // heightmap[i, j] = Random.Range(0f,1f);
      }
    }
    return heightmap;
  }
}
