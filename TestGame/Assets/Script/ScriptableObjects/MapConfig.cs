using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapConfig", menuName = "Scriptable Objects/MapConfig")]
public class MapConfig : ScriptableObject
{
	public int MAP_NUMBER_OF_ROWS;
	public int MAP_NUMBER_OF_COLUMNS;

	public float MAP_TILE_MARGIN;
}
