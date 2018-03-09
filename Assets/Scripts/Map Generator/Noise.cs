using UnityEngine;
using System.Collections;

public static class Noise {

	public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistence, float lacunarity, Vector2 offset) {
		float[,] noiseMap = new float[mapWidth, mapHeight];

		System.Random pseudoRandom = new System.Random (seed);
		Vector2[] octaveOffsets = new Vector2[octaves];

		for (int i = 0; i < octaves; i++) {
			float offsetX = pseudoRandom.Next (-100000, 100000) + offset.x;
			float offsetY = pseudoRandom.Next (-100000, 100000) + offset.y;
			octaveOffsets [i] = new Vector2 (offsetX, offsetY);
		}

		if (scale <= 0) {
			scale = 0.0001f;
		}

		float maxNoise = float.MinValue;
		float minNoise = float.MaxValue;

		float halfWidth = mapWidth / 2f;
		float halfHeight = mapHeight / 2f;

		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {

				float amplitude = 1;
				float frequency = 1;
				float noiseHeight = 0;

				for (int i = 0; i < octaves; i++) {

					float sampleX = (x-halfWidth) / scale * frequency + octaveOffsets[i].x;
					float sampleY = (y-halfHeight) / scale * frequency + octaveOffsets[i].y;

					float perlinValue = Mathf.PerlinNoise (sampleX, sampleY) * 2 - 1;
					//noiseMap [x, y] = perlinValue;
					noiseHeight += perlinValue * amplitude;

					amplitude *= persistence;
					frequency *= lacunarity;
				}

				if (noiseHeight > maxNoise) {
					maxNoise = noiseHeight;
				} else if (noiseHeight < minNoise) {
					minNoise = noiseHeight;
				}

				noiseMap [x, y] = noiseHeight;
				
			}
		}

		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {
				noiseMap [x, y] = Mathf.InverseLerp (minNoise, maxNoise, noiseMap [x, y]);
			}
		}

		return noiseMap;
	}

}
