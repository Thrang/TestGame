using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
	CameraController cameraController;

	Map map;

	// Use this for initialization
	void Start()
	{
		cameraController = new CameraController();
		cameraController.Init(Camera.main);

		map = new Map();
		map.Create();
		map.Draw(-15f, -10f);
	}
	
	// Update is called once per frame
	void Update()
	{
		map.Update();
	}

	void LateUpdate()
	{
		cameraController.Update();
	}
}
