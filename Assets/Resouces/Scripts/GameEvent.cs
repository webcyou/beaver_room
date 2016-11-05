using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

public class GameEvent : MonoBehaviour {
	public string filePath;
	public Dictionary<string, object> nodes;
	[System.NonSerialized]public int nodesIndex;

	[System.NonSerialized]public int id;
	private Main main;

	void Start(){
		main = GameObject.Find("Main").GetComponent<Main>();
		id = main.eventManager.gameEventQyt;
		main.eventManager.gameEventQyt += 1;

		nodesIndex = 0;
		LoadJSON();
	}

	private void LoadJSON(){
		if (filePath == null || filePath == "") return;
		TextAsset tText = (TextAsset)Resources.Load (filePath);
		nodes = (Dictionary<string, object>)Json.Deserialize(tText.text);
	}
}
