using UnityEngine;
using System.Collections;

public class TriggerEventDispatcher : MonoBehaviour {

	[System.NonSerialized]public System.Action<Collider> onEnter;
	[System.NonSerialized]public System.Action<Collider> onStay;
	[System.NonSerialized]public System.Action<Collider> onExit;
	
	void OnTriggerEnter(Collider pTarget){
		onEnter(pTarget);
	}
//	void OnTriggerStay(Collider pTarget){
//		onStay(pTarget);
//	}
	void OnTriggerExit(Collider pTarget){
		onExit(pTarget);
	}
}
