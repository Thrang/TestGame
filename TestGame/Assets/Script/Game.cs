using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
	public Text text;
	public Text text2;

	double count = 0;
	DateTime start;
	string path;

	Map map;
	// Use this for initialization
	void Start()
	{
		start = DateTime.Now;
		path = Application.persistentDataPath + @"/save.txt";
		Debug.Log("path = " + path);

		map = new Map();
		map.Create();
		map.Draw(-15, -10);
	}
	
	// Update is called once per frame
	void Update()
	{
		count += Time.deltaTime;
		text.text = count.ToString("0.00");

		double second = (DateTime.Now - start).TotalSeconds;
		text2.text = second.ToString("0.00");

		if (second > 5)
		{
			start = DateTime.Now;
		}
	}


}
