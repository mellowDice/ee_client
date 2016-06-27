using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreateTerrain : MonoBehaviour {
  public Terrain Terrain;
  // private TerrainData tData;

  void Awake() {
    // tData = Terrain.terrainData;
  }

  public void BuildingTerrain (JSONObject Map2D) {
    var length = Map2D.list.Count;

    var terrainData = new TerrainData();
    terrainData.heightmapResolution = 100; //tData.heightmapResolution;
    terrainData.alphamapResolution = 100; //tData.alphamapResolution;
    Debug.Log("Building map!");
    var heightmap = GetHeightmap(Map2D, length);
    terrainData.SetHeights(0, 0, heightmap);
    terrainData.size = new Vector3(length, 10, length);
    
    Terrain.activeTerrain.terrainData = terrainData;
    Terrain.Flush();
    Debug.Log("Done building!");
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
