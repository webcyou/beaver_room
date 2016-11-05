using UnityEngine;
using System.Collections;

public class CameraController {
	public Transform target;
	public bool isFollowTarget;
	private Vector3 position = new Vector3();
	public Vector3 offset = new Vector3();
	public Vector3 coefficient = new Vector3(1.0f, 1.0f, 1.0f);
	public bool isActiveMinX = false;
	public bool isActiveMinY = false;
	public bool isActiveMinZ = false;
	public bool isActiveMaxX = false;
	public bool isActiveMaxY = false;
	public bool isActiveMaxZ = false;
	public Vector3 min = new Vector3();
	public Vector3 max = new Vector3();

	private float zoom;

	private GameObject gameObject;
	public Transform transform;

	public CameraController(GameObject pGameObject){
		gameObject = pGameObject;
		transform = pGameObject.transform;
		offset.x = transform.localPosition.x;
		offset.y = transform.localPosition.y;
		offset.z = transform.localPosition.z;
	}

	public void Init() {
		if (target == null) {
			isFollowTarget = false;
		} else {
			isFollowTarget = true;
		}
		position.x = transform.localPosition.x +offset.x;
		position.y = transform.localPosition.y +offset.y;
		position.z = transform.localPosition.z +offset.z;
		zoom = 1f;
	}

	public void OnUpdate() {
		if (isFollowTarget == true){
			Follow();
		}
		Refresh();
	}

	private void Follow(){
		float tX = target.position.x *coefficient.x +offset.x;
		float tY = target.position.y *coefficient.y +offset.y /zoom;
		float tZ = target.position.z *coefficient.z +offset.z /zoom;
		//x
		if (isActiveMinX == true){
			if (tX < min.x) tX = min.x;
		}
		if (isActiveMaxX == true){
			if (tX > max.x) tX = max.x;
		}
		position.x += (tX -position.x) *0.2f;
		//y
		if (isActiveMinY == true){
			if (tY < min.y) tY = min.y;
		}
		if (isActiveMaxY == true){
			if (tY > max.y) tY = max.y;
		}
		position.y += (tY -position.y) *0.2f;
		//z
		if (isActiveMinZ == true){
			if (tZ < min.z) tZ = min.z;
		}
		if (isActiveMaxZ == true){
			if (tZ > max.z) tZ = max.z;
		}
		position.z += (tZ -position.z) *0.2f;		
	}

	private void Refresh(){
		transform.position = position;
	}

	public void Zoom(float pTargetZoom, float pTime){
//		float tZoom = zoom;
//		iTween.ValueTo(
//			gameObject,
//			iTween.Hash(
//				"from", tZoom,
//				"to", pTargetZoom,
//				"time", pTime,
//				"onupdate", "TweenUpdateHandler",
//				"easetype", "easeInOutCubic"
//			)
//       );
	}
//	void TweenUpdateHandler(float pValue){
//		zoom = pValue;
//	}
}
