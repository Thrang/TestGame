using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController
{	
	private Camera cameraComponent;
	private bool isMouseDown;
	private bool isMouseUp;
	private Vector3 startPostion;
	// Use this for initialization
	public void Init(Camera mainCamera)
	{
		cameraComponent = mainCamera;
		isMouseUp = true;
		isMouseDown = false;
	}
	
	// Update is called once per frame
	public void Update()
	{
		if (isMouseUp)
		{
			isMouseDown = Input.GetMouseButtonDown(0);

			if (isMouseDown)
			{
				startPostion = Input.mousePosition;	
				//Vector3 pos = cameraComponent.ScreenToWorldPoint(startPostion);
				//Debug.Log("pos.x = " + pos.x + " pos.y = " + pos.y);
				isMouseUp = false;
			}
		}

		if (isMouseDown)
		{			
			isMouseUp = Input.GetMouseButtonUp(0);

			if (isMouseUp)
			{
				isMouseDown = false;
			}
			else
			{
				Vector3 newPosition = Input.mousePosition;
				Vector3 delta = cameraComponent.ScreenToViewportPoint(startPostion - newPosition);
				//Debug.Log("delta.x = " + delta.x + " delta.y = " + delta.y);
				//Debug.Log("newPosition.x = " + newPosition.x + " newPosition.y = " + newPosition.y);

				//if (delta.x >= 0.01f || delta.y >= 0.01f || delta.x <= -0.01f || delta.y <= -0.01f)
				{
					startPostion = newPosition;
					MoveCamera(delta);
				}
			}
		}
	}

	public void MoveCamera(Vector3 delta)
	{
		Vector3 move = new Vector3(delta.x * 18, delta.y * 10, 0);
		//gameObject.transform.position = position;
		cameraComponent.gameObject.transform.Translate(move, Space.World);
		//gameObject.transform.Translate(move, Space.World);
	}
}

/*
using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	public float dragSpeed = 1;
	private Vector3 dragOrigin;

	public bool cameraDragging = true;

	public float outerLeft = -10f;
	public float outerRight = 10f;


	void LateUpdate()
	{



		Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

		float left = Screen.width * 0.2f;
		float right = Screen.width - (Screen.width * 0.2f);

		if(mousePosition.x < left)
		{
			cameraDragging = true;
		}
		else if(mousePosition.x > right)
		{
			cameraDragging = true;
		}






		if (cameraDragging) {

			if (Input.GetMouseButtonDown(0))
			{
				dragOrigin = Input.mousePosition;
				return;
			}

			if (!Input.GetMouseButton(0)) return;

			Vector3 pos = Camera.main.ScreenToViewportPoint(dragOrigin - Input.mousePosition);
			Vector3 move = new Vector3(pos.x, pos.y, 0);
			Debug.Log("move.x = " + move.x + " move.y = " + move.y);

					transform.Translate(move, Space.World);

		}
	}


}
*/
