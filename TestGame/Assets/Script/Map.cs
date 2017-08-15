using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Terrain
{
	MOUNTAIN,
	HILL,
	GRASS,
	PLAIN,
	SEA,
	FOREST,
	JUNGLE,
	DESERT
}

public class Tile
{
	public GameObject sprite;
	public SpriteRenderer spriteRenderer;
	public int height;
	public int x;
	public int y;
	public int flag;

	public void Init()
	{
		spriteRenderer = sprite.GetComponent<SpriteRenderer>();
	}

	public void SetColor(int r, int g, int b)
	{
		spriteRenderer.color = new Color(r / 255.0f, g / 255.0f, b / 255.0f);
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

	private GameObject squarePrefab;
	private Queue<Tile> queue;

	public Tile[][] map;

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

		queue = new Queue<Tile>();

		GenerateHeight();
	}

	private void GenerateHeight()
	{
		int minMount = 1;
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

			if (currentTile.x > 0)
			{
				Tile nextTile = map[currentTile.x - 1][currentTile.y];

				if (nextTile.flag == 0)
				{
					int heightDecrease = Random.Range(minHeightDecrease, maxHeightDecrease);
					nextTile.height = currentTile.height - heightDecrease;
					nextTile.flag = 1;
					queue.Enqueue(nextTile);
				}
			}

			if (currentTile.y > 0)
			{
				Tile nextTile = map[currentTile.x][currentTile.y - 1];

				if (nextTile.flag == 0)
				{
					int heightDecrease = Random.Range(minHeightDecrease, maxHeightDecrease);
					nextTile.height = currentTile.height - heightDecrease;
					nextTile.flag = 1;
					queue.Enqueue(nextTile);
				}
			}

			if (currentTile.x < mapWidth - 1)
			{
				Tile nextTile = map[currentTile.x + 1][currentTile.y];

				if (nextTile.flag == 0)
				{
					int heightDecrease = Random.Range(minHeightDecrease, maxHeightDecrease);
					nextTile.height = currentTile.height - heightDecrease;
					nextTile.flag = 1;
					queue.Enqueue(nextTile);
				}
			}

			if (currentTile.y < mapHeight - 1)
			{
				Tile nextTile = map[currentTile.x][currentTile.y + 1];

				if (nextTile.flag == 0)
				{
					int heightDecrease = Random.Range(minHeightDecrease, maxHeightDecrease);
					nextTile.height = currentTile.height - heightDecrease;
					nextTile.flag = 1;
					queue.Enqueue(nextTile);
				}
			}
		}
	}

	public void Draw(float x, float y)
	{
		for (int i = 0; i < mapWidth; i++)
		{
			for (int j = 0; j < mapHeight; j++)
			{
				map[i][j].sprite.gameObject.transform.position = new Vector2(x + i * 1, y + j * 1);
				map[i][j].sprite.SetActive(true);

				if (map[i][j].height > minMountHeight)
				{
					map[i][j].SetColor(200, 0, 0);
				}
				else if (map[i][j].height > 0)
				{
					map[i][j].SetColor(0, 200, 0);
				}
				else
				{
					map[i][j].SetColor(0, 0, 200);
				}
			}
		}
	}
}
