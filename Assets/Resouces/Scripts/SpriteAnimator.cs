using UnityEngine;
using System.Collections;

public class SpriteAnimator : MonoBehaviour {
	public GameObject target;
	private Transform parent;
	private string currentAnimation;
	private Animator animation;

	// Use this for initialization
	void Start () {
		parent = target.transform;
		animation = gameObject.GetComponent<Animator>();
		currentAnimation = "WalkD";
		animation.Play(currentAnimation);
	}
	
	// Update is called once per frame
	void Update () {
		SwitchDirection();
	}

	private void SwitchDirection(){
		float degree = parent.localRotation.eulerAngles.y;
		string nextAnimation;
		if (degree < 45f){
			nextAnimation = "WalkR";
		} else if (degree < 135f){
			nextAnimation = "WalkD";
		} else if (degree < 225f){
			nextAnimation = "WalkL";
		} else if (degree < 315f){
			nextAnimation = "WalkU";
		} else {
			nextAnimation = "WalkR";
		}
		if (currentAnimation != nextAnimation){
			currentAnimation = nextAnimation;
			animation.Play(currentAnimation);
		}
	}
}
