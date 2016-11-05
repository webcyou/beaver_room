using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {
	private static float THRESHOLD = 12.0f;
	
	private Vector2 touchStartPosition;
	private Vector2 mouseStartPosition;
	private int mouseCount = 0;
	private bool isMouseDown = false;
	private bool isMouseSwiped = false;
	private bool isSwiped = false;

	[System.NonSerialized]public Vector2 distance = new Vector2();
	[System.NonSerialized]public System.Action onTap;

	// Use this for initialization
	void Start () {
		touchStartPosition = new Vector2(0f, 0f);
		mouseStartPosition = new Vector2(0f, 0f);
	}
	
	// Update is called once per frame
	void Update () {
		TouchUpdate();
		MouseUpdate();
	}
	
	void TouchUpdate () {
		Touch tTouch;
		Vector2 tTouchPosition;
		float distanceX;
		float distanceY;
		if (Input.touchCount > 0){
			tTouch = Input.GetTouch(0);
			tTouchPosition = tTouch.position;
			switch (tTouch.phase){
			case TouchPhase.Began:
				touchStartPosition.x = tTouchPosition.x;
				touchStartPosition.y = tTouchPosition.y;
				isSwiped = false;
				distance.x = 0f;
				distance.y = 0f;
				break;
			case TouchPhase.Moved:
				distanceX = tTouchPosition.x -touchStartPosition.x;
				distanceY = tTouchPosition.y -touchStartPosition.y;
				if (distanceX > THRESHOLD || distanceX < -THRESHOLD || distanceY > THRESHOLD || distanceY < -THRESHOLD){
					isSwiped = true;
					distance.x = distanceX;
					distance.y = distanceY;
				}
				break;
			case TouchPhase.Ended:
				distance.x = 0f;
				distance.y = 0f;
				if (isSwiped == false){
					onTap();
				}
				break;
			}
		}
	}

	void MouseUpdate(){
		Vector2 tMousePosition;
		float distanceX;
		float distanceY;
		if (Input.GetMouseButtonDown(0) == true){
			isMouseDown = true;
		}
		if (isMouseDown == true){
			mouseCount++;
		}
		if (Input.GetMouseButtonUp(0) == true){
			isMouseDown = false;
			mouseCount = 0;
			distance.x = 0f;
			distance.y = 0f;
			if (isMouseSwiped == false){
				onTap();
			}
			return;
		}
		tMousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

		if (mouseCount == 1){
			mouseStartPosition.x = tMousePosition.x;
			mouseStartPosition.y = tMousePosition.y;
			isMouseSwiped = false;
			distance.x = 0f;
			distance.y = 0f;
		} else if (mouseCount > 1){
			distanceX = tMousePosition.x -mouseStartPosition.x;
			distanceY = tMousePosition.y -mouseStartPosition.y;
			if (distanceX > THRESHOLD || distanceX < -THRESHOLD || distanceY > THRESHOLD || distanceY < -THRESHOLD){
				isMouseSwiped = true;
				distance.x = distanceX;
				distance.y = distanceY;
			}
		}
	}
}
