using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour {

	public enum DrawMode {NoiseMap, ColorMap, Mesh};
	public DrawMode drawMode;

	const int mapChunkSize = 121;
	[Range(0,6)]
	public int levelOfDetail;
	public float noiseScale;

	public int seed;
	public int octaves;
	[Range(0,1)]
	public float persistence;
	public float lacunarity;
	public Vector2 offset;

	public float meshHeightMultiplier;

	public bool autoUpdate;

	public TerrainType[] regions;

	Grid mapGrid;

	public void Start() {
		GenerateMap ();
	}

	public void GenerateMap() {

		float[,] noiseMap = Noise.GenerateNoiseMap (mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistence, lacunarity, offset);
		Color[] colorMap = new Color[mapChunkSize * mapChunkSize];

//		mapGrid = new Grid(transform.position, (mapChunkSize - 1) * 10, (mapChunkSize - 1) * 10, 5f, true);
		mapGrid = Grid.GridInstance;
		if (mapGrid != null)
			mapGrid.CreateGrid ();

		// Assign Regions and build Color Map
		for (int y = 0; y < mapChunkSize; y++) {
			for (int x = 0; x < mapChunkSize; x++) {

				TerrainType currentTerrain = new TerrainType ();

				float noise = noiseMap [x, y];

				for (int i = 0; i < regions.Length; i++) {
					if (noise <= regions [i].height) {
						currentTerrain = regions[i];
						colorMap [y * mapChunkSize + x] = currentTerrain.color;
						break;
					}
				}

				if (mapGrid != null)
				if (x < mapChunkSize - 1 && y < mapChunkSize - 1) {
					Node currentNode = mapGrid.NodeFromGridPoint (x, y);
					currentNode.Terrain = currentTerrain;
					currentNode.Elevation = meshHeightMultiplier * noise * noise * noise;
				}

			}
		}

		// Draw the map
		MapDisplay display = FindObjectOfType<MapDisplay> ();
		if (drawMode == DrawMode.NoiseMap) {
			display.DrawTexture (TextureGenerator.TextureFromHeightMap (noiseMap));
		} else if (drawMode == DrawMode.ColorMap) {
			display.DrawTexture (TextureGenerator.TextureFromColorMap (colorMap, mapChunkSize, mapChunkSize));
		} else if (drawMode == DrawMode.Mesh) {
			display.DrawMesh (
				MeshGenerator.GenerateTerrainMesh (noiseMap, meshHeightMultiplier, levelOfDetail),
				TextureGenerator.TextureFromColorMap (colorMap, mapChunkSize, mapChunkSize)
			);
		}
			
	}

	void OnValidate() {
		if (lacunarity < 1) {
			lacunarity = 1;
		}
		if (octaves < 1) {
			octaves = 1;
		} else if (octaves > 10) {
			octaves = 10;
		}
	}

//	void OnDrawGizmos() {
	void OnDrawGizmosSelected() {
		try {
			mapGrid.OnDrawGizmos (); 
		}
		catch {
		}
	}

	public Grid MapGrid { 
		get { 
			return mapGrid; 
		} 
	}

}

[System.Serializable]
public struct TerrainType {
	public string name;
	public float height;
	public Color color;
	public int moveCost;
}
