using UnityEngine;
using System.Collections;

public class CreateTerrain : MonoBehaviour {

  public int Height;
  public int Length;

  public void BuildingTerrain (float [,] Map2D) {
    var terrainData = new TerrainData();

    var heightmap = Map2D;
    terrainData.SetHeights(0, 0, heightmap);
    // terrainData.size = new Vector3(Length, Height, Length);
 
    // var newTerrainGameObject = Terrain.CreateTerrainGameObject(terrainData);
    // newTerrainGameObject.transform.position = new Vector3(X * Length, 0, Z * Length);
    // Terrain = newTerrainGameObject.GetComponent<Terrain>();
    // Terrain.Flush();
  }
}
