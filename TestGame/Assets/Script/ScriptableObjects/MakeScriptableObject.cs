using UnityEngine;
using System.Collections;
using UnityEditor;

public class MakeScriptableObject 
{
	[MenuItem("Scriptable Objects/MapConfig")]
	public static void CreateMyAsset()
	{
		MapConfig asset = ScriptableObject.CreateInstance<MapConfig>();

		AssetDatabase.CreateAsset(asset, "Assets/MapConfig.asset");
		AssetDatabase.SaveAssets();

		EditorUtility.FocusProjectWindow();

		Selection.activeObject = asset;
	}
}