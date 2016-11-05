using UnityEngine;
using System.Collections;

public class CollisionEventDispatcher : MonoBehaviour {
	[System.NonSerialized]
	public System.Action onEnter;

	void OnCollisionEnter(){
		onEnter();
	}
}
