using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DelayFunc {
	public delegate void DelayFuncDelegate();
	
	private List<DelayFuncDelegate> functions;
	private List<int> frameCounts;
	
	public DelayFunc(){
		functions = new List<DelayFuncDelegate> ();
		frameCounts = new List<int> ();
	}
	
	// This function should to call from MonoBehaviour's Update Method.
	public void Update(){
		UpdateDelay();
	}
	
	public void Add(DelayFuncDelegate pFunc, int pFrameCount){
		if (pFunc == null) return;
		functions.Add (pFunc);
		frameCounts.Add (pFrameCount);
	}
	
	void UpdateDelay(){
		for (int i = 0; i < functions.Count; i++) {
			if (frameCounts[i] > 0){
				frameCounts[i]--;
				continue;
			}
			functions[i]();
			functions.RemoveAt(i);
			frameCounts.RemoveAt(i);
			i--;
		}
	}
}
