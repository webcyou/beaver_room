using UnityEngine;
using System.Collections;

public class SpriteAnimator : MonoBehaviour {
	private Transform parent;
	private Animator animation;
	private GameObject camera;
	private string currentAnimation;
	private float rotation;

	// Use this for initialization
	void Start () {
		parent = transform.parent.FindChild("View").transform;
		animation = gameObject.GetComponent<Animator>();
		camera = GameObject.Find("Main Camera");
		currentAnimation = "WalkD";
		animation.Play(currentAnimation);
	}
	
	// Update is called once per frame
	void Update () {
		SwitchDirection();
		LookCamera();
	}

	private void SwitchDirection(){
		float tDegree = parent.localRotation.eulerAngles.y;
		string tNextAnimation;
		if (tDegree < 45f){
			tNextAnimation = "WalkR";
		} else if (tDegree < 135f){
			tNextAnimation = "WalkD";
		} else if (tDegree < 225f){
			tNextAnimation = "WalkL";
		} else if (tDegree < 315f){
			tNextAnimation = "WalkU";
		} else {
			tNextAnimation = "WalkR";
		}
		if (currentAnimation != tNextAnimation){
			currentAnimation = tNextAnimation;
			animation.Play(currentAnimation);
		}
	}

	private void LookCamera(){
		rotation = camera.transform.localRotation.eulerAngles.x;
		transform.localRotation = Quaternion.Euler(rotation, 0f, 0f);
	}
}
