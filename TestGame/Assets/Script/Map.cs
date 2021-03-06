﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Terrain : int
{
	MOUNTAIN = 0,
	HILL,
	OCEAN,
	SEA,
	GRASS,
	PLAIN,
	FOREST,
	JUNGLE,
	DESERT,
	TOTAL
}

public struct TerrainGenerateInfo
{
	public Terrain type;
	public int expectedChance;
	public float chance;
	public int totalTiles;
}

public class Tile
{
	public GameObject sprite;
	public SpriteRenderer spriteRenderer;
	public int height;
	public int x;
	public int y;
	public int flag;
	public Terrain terrain;
	public Color color;

	public void Init()
	{
		spriteRenderer = sprite.GetComponent<SpriteRenderer>();
		color = new Color(0f, 0f, 0f);
	}

	public void SetColor(int r, int g, int b)
	{		
		color = new Color(r / 255.0f, g / 255.0f, b / 255.0f);
		spriteRenderer.color = color;
	}	

	public void ResetColor()
	{
		spriteRenderer.color = color;
	}

	public void HighLight()
	{
		spriteRenderer.color = new Color(0.8f, 0.8f, 0f);
	}
}

public class Map
{
	public int mapWidth = 30;
	public int mapHeight = 20;

	public int maxMountHeight = 200;
	public int minMountHeight = 140;
	public int minHeightDecrease = 5;
	public int maxHeightDecrease = 20;
	public int numberOfTilesDone = 0;

	private GameObject squarePrefab;
	private Queue<Tile> queue;

	public Tile[][] map;
	public TerrainGenerateInfo[] terrainGenerateInfoArray;

	public void Create()
	{
		squarePrefab = Resources.Load<GameObject>("Square");

		map = new Tile[mapWidth][];
		for (int i = 0; i < mapWidth; i++)
		{
			map[i] = new Tile[mapHeight];

			for (int j = 0; j < mapHeight; j++)
			{
				map[i][j] = new Tile();
				map[i][j].x = i;
				map[i][j].y = j;
				map[i][j].flag = 0;
				map[i][j].sprite = GameObject.Instantiate(squarePrefab) as GameObject;
				map[i][j].Init();
			}
		}

		Debug.Log("map[0][0].spriteRenderer.bounds.size.x = " + map[0][0].spriteRenderer.bounds.size.x);
		Debug.Log("map[0][0].spriteRenderer.bounds.size.y = " + map[0][0].spriteRenderer.bounds.size.y);

		queue = new Queue<Tile>();
		terrainGenerateInfoArray = new TerrainGenerateInfo[(int)Terrain.TOTAL];

		for (int i = 0; i < terrainGenerateInfoArray.Length; i++)
		{
			TerrainGenerateInfo info = new TerrainGenerateInfo();
			info.type = (Terrain)i;

			if (i > (int)Terrain.SEA)
				info.expectedChance = 20;
			
			info.totalTiles = 0;
			terrainGenerateInfoArray[i] = info;
		}

		terrainGenerateInfoArray[(int)Terrain.GRASS].expectedChance = 50;
		terrainGenerateInfoArray[(int)Terrain.PLAIN].expectedChance = 15;
		terrainGenerateInfoArray[(int)Terrain.FOREST].expectedChance = 20;
		terrainGenerateInfoArray[(int)Terrain.JUNGLE].expectedChance = 10;
		terrainGenerateInfoArray[(int)Terrain.DESERT].expectedChance = 5;

		GenerateHeight();
	}

	private void AddNextTile(Tile currentTile, Tile nextTile, List<Tile> adjacentTiles)
	{
		if (nextTile.flag == 0)
		{
			int heightDecrease = Random.Range(minHeightDecrease, maxHeightDecrease);
			nextTile.height = currentTile.height - heightDecrease;
			nextTile.flag = 1;
			queue.Enqueue(nextTile);
		}
		else if (nextTile.flag == 2)
		{
			adjacentTiles.Add(nextTile);
		}
	}

	private void GenerateHeight()
	{
		int minMount = 2;
		int maxMount = 5;
		int mount = Random.Range(minMount, maxMount + 1);

		for (int i = 0; i < mount; i++)
		{
			int row = Random.Range(0, mapWidth);
			int column = Random.Range(0, mapHeight);

			map[row][column].height = Random.Range(minMountHeight, maxMountHeight);
			map[row][column].flag = 1;

			queue.Enqueue(map[row][column]);
		}

		while (queue.Count > 0)
		{
			Tile currentTile = queue.Dequeue();
			List<Tile> adjacentTiles = new List<Tile>();

			if (currentTile.x > 0)
			{
				Tile nextTile = map[currentTile.x - 1][currentTile.y];

				AddNextTile(currentTile, nextTile, adjacentTiles);			
			}

			if (currentTile.y > 0)
			{
				Tile nextTile = map[currentTile.x][currentTile.y - 1];

				AddNextTile(currentTile, nextTile, adjacentTiles);	
			}

			if (currentTile.x < mapWidth - 1)
			{
				Tile nextTile = map[currentTile.x + 1][currentTile.y];

				AddNextTile(currentTile, nextTile, adjacentTiles);	
			}

			if (currentTile.y < mapHeight - 1)
			{
				Tile nextTile = map[currentTile.x][currentTile.y + 1];

				AddNextTile(currentTile, nextTile, adjacentTiles);	
			}

			GenerateTerrain(currentTile, adjacentTiles);
		}
	}

	public void GenerateTerrain(Tile currentTile, List<Tile> adjacentTiles)
	{
		if (currentTile.height >= minMountHeight)
		{
			currentTile.terrain = Terrain.MOUNTAIN;
			currentTile.flag = 2;
			numberOfTilesDone++;
			return;
		}

		if (currentTile.height >= minMountHeight - 30)
		{
			currentTile.terrain = Terrain.HILL;
			currentTile.flag = 2;
			numberOfTilesDone++;
			return;
		}

		if (currentTile.height <= -50)
		{
			currentTile.terrain = Terrain.OCEAN;
			currentTile.flag = 2;
			numberOfTilesDone++;
			return;
		}

		if (currentTile.height <= 0)
		{
			currentTile.terrain = Terrain.SEA;
			currentTile.flag = 2;
			numberOfTilesDone++;
			return;
		}

		int numberOfLowLandTerrain = (int)Terrain.TOTAL - (int)Terrain.SEA;
		for (int i = (int)Terrain.SEA + 1; i < terrainGenerateInfoArray.Length; i++)
		{
			terrainGenerateInfoArray[i].chance = terrainGenerateInfoArray[i].expectedChance;
			float chance = (terrainGenerateInfoArray[i].totalTiles + 10.0f) / (numberOfTilesDone + numberOfLowLandTerrain * 10) * numberOfLowLandTerrain;
			terrainGenerateInfoArray[i].chance /= chance;
		}

		for (int i = 0; i < adjacentTiles.Count; i++)
		{
			Tile nextTile = adjacentTiles[i];

			int nextTileTerrain = (int)nextTile.terrain;
			switch (nextTile.terrain)
			{
			case Terrain.GRASS:
				terrainGenerateInfoArray[nextTileTerrain].chance *= 1.5f;
				break;

			case Terrain.PLAIN:
				terrainGenerateInfoArray[nextTileTerrain].chance *= 1.5f;
				terrainGenerateInfoArray[(int)Terrain.JUNGLE].chance *= 0.5f;
				terrainGenerateInfoArray[(int)Terrain.DESERT].chance *= 0.1f;
				break;

			case Terrain.FOREST:
				terrainGenerateInfoArray[nextTileTerrain].chance *= 1.5f;
				terrainGenerateInfoArray[(int)Terrain.JUNGLE].chance *= 0.5f;
				terrainGenerateInfoArray[(int)Terrain.DESERT].chance *= 0.1f;
				break;

			case Terrain.JUNGLE:
				terrainGenerateInfoArray[nextTileTerrain].chance *= 2.5f;
				terrainGenerateInfoArray[(int)Terrain.FOREST].chance *= 0.5f;
				terrainGenerateInfoArray[(int)Terrain.PLAIN].chance *= 0.2f;
				terrainGenerateInfoArray[(int)Terrain.DESERT].chance *= 0.1f;
				break;

			case Terrain.DESERT:
				terrainGenerateInfoArray[nextTileTerrain].chance *= 5.0f;
				terrainGenerateInfoArray[(int)Terrain.PLAIN].chance *= 0.1f;
				terrainGenerateInfoArray[(int)Terrain.FOREST].chance *= 0.1f;
				terrainGenerateInfoArray[(int)Terrain.JUNGLE].chance *= 0.1f;
				break;
			}
		}

		float totalChance = 0;
		for (int i = (int)Terrain.SEA + 1; i < terrainGenerateInfoArray.Length; i++)
		{
			totalChance += terrainGenerateInfoArray[i].chance;
		}

		float randomChance = Random.Range(0, totalChance);
		int terrainIndex = (int)Terrain.SEA + 1;
		while (randomChance > 0 && terrainIndex < (int)Terrain.TOTAL)
		{
			randomChance -= terrainGenerateInfoArray[terrainIndex].chance;
			if (randomChance <= 0)
			{
				currentTile.terrain = (Terrain)terrainIndex;
				currentTile.flag = 2;
				terrainGenerateInfoArray[terrainIndex].totalTiles++;
			}
			terrainIndex++;
		}

		if (randomChance > 0)
		{
			currentTile.terrain = Terrain.GRASS;
			currentTile.flag = 2;
			terrainGenerateInfoArray[(int)Terrain.GRASS].totalTiles++;
			Debug.Log("randomChance = " + randomChance);
		}

		numberOfTilesDone++;
	}

	public void Draw(float x, float y)
	{
		for (int i = 0; i < mapWidth; i++)
		{
			for (int j = 0; j < mapHeight; j++)
			{
				map[i][j].sprite.gameObject.transform.position = new Vector2(x + i * 2, y + j * 2);
				map[i][j].sprite.SetActive(true);

				switch (map[i][j].terrain)
				{
				case Terrain.MOUNTAIN:
					map[i][j].SetColor(100, 0, 0);
					break;
				case Terrain.HILL:
					map[i][j].SetColor(200, 0, 0);
					break;
				case Terrain.OCEAN:
					map[i][j].SetColor(0, 0, 100);
					break;
				case Terrain.SEA:
					map[i][j].SetColor(0, 0, 200);
					break;
				case Terrain.GRASS:
					map[i][j].SetColor(0, 200, 0);
					break;
				case Terrain.PLAIN:
					map[i][j].SetColor(200, 200, 200);
					break;
				case Terrain.FOREST:
					map[i][j].SetColor(0, 100, 0);
					break;
				case Terrain.JUNGLE:
					map[i][j].SetColor(200, 0, 200);
					break;
				case Terrain.DESERT:
					map[i][j].SetColor(200, 200, 0);
					break;
				default:
					map[i][j].SetColor(0, 0, 0);
					break;
				}
			}
		}
	}

	Vector3 mouseStart;
	Tile selectedTile;

	public void Update()
	{
		bool isMouseDown = Input.GetMouseButtonDown(0);
		if (isMouseDown)
		{
			mouseStart = Input.mousePosition;
		}

		bool isMouseUp = Input.GetMouseButtonUp(0);
		if (isMouseUp)
		{			
			Vector3 mouseDelta = Input.mousePosition - mouseStart;
			Debug.Log("mouseDelta.x = " + mouseDelta.x + " mouseDelta.y = " + mouseDelta.y);

			if (mouseDelta.x < 10 && mouseDelta.x > -10 && mouseDelta.y < 10 && mouseDelta.y > -10)
			{
				Vector3 mouseEnd = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				Debug.Log("mouseEnd.x = " + mouseEnd.x + " mouseEnd.y = " + mouseEnd.y);

				for (int i = 0; i < mapWidth; i++)
				{
					for (int j = 0; j < mapHeight; j++)
					{
						if (mouseEnd.x < map[i][j].sprite.transform.position.x + map[i][j].spriteRenderer.bounds.size.x / 2
							&& mouseEnd.x > map[i][j].sprite.transform.position.x - map[i][j].spriteRenderer.bounds.size.x / 2
							&& mouseEnd.y < map[i][j].sprite.transform.position.y + map[i][j].spriteRenderer.bounds.size.y / 2
							&& mouseEnd.y > map[i][j].sprite.transform.position.y - map[i][j].spriteRenderer.bounds.size.y / 2)
						{
							if (selectedTile != null)
							{
								selectedTile.ResetColor();
							}

							map[i][j].HighLight();
							selectedTile = map[i][j];
						}
					}
				}
			}
		}
	}
}
