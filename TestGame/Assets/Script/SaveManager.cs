using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager
{
	double count = 0;
	DateTime start;
	string path;

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
