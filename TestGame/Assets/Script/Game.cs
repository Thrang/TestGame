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
	// Use this for initialization
	void Start()
	{
		start = DateTime.Now;
		path = Application.persistentDataPath + @"/save.txt";
		Debug.Log("path = " + path);

		Load();
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

			Save();
		}
	}

	void Save()
	{
		StreamWriter writer = new StreamWriter(path);
		writer.WriteLine(count.ToString("0.00"));
		writer.WriteLine(start.ToString());
		writer.Close();
	}

	void Load()
	{
		if (File.Exists(path))
		{
			StreamReader reader = new StreamReader(path);
			count = Convert.ToDouble(reader.ReadLine());
			DateTime time = Convert.ToDateTime(reader.ReadLine());
			Debug.Log("time = " + time.ToString());

			double seconds = (DateTime.Now - time).TotalSeconds;
			count += seconds;
		}
	}
}
