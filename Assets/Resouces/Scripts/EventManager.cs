using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventManager {
	public GameObject gameObject;
	public TriggerEventDispatcher dispatcher;
	public string textMessage;
	public List<GameEvent> gameEvents = new List<GameEvent>();
	public int gameEventQty = 0;


	public EventManager(GameObject pGameObject){
		gameObject = pGameObject;
		dispatcher = gameObject.GetComponent<TriggerEventDispatcher>();
		dispatcher.onEnter += delegate(Collider pTarget){
			GameEvent tGameEvent = pTarget.gameObject.GetComponent<GameEvent>();
			if (tGameEvent == null) return;
			for (int i = 0; i < gameEvents.Count; i++){
				if (gameEvents[i].id == tGameEvent.id){
					return;
				}
			}
			gameEvents.Add(tGameEvent);
		};
		dispatcher.onExit += delegate(Collider pTarget){
			GameEvent tGameEvent = pTarget.gameObject.GetComponent<GameEvent>();
			if (tGameEvent == null) return;
			for (int i = 0; i < gameEvents.Count; i++){
				if (gameEvents[i].id == tGameEvent.id){
					textMessage = "";
					gameEvents.RemoveAt(i);
					return;
				}
			}
		};
	}
	public Dictionary<string, object> GetGameEventNode(){
		if (gameEvents.Count == 0){
			return new Dictionary<string, object>();
		}
		int tIndex = gameEvents[0].nodesIndex +1;
		Dictionary<string, object> tD = (Dictionary<string, object>)gameEvents[0].nodes[tIndex.ToString()];
		gameEvents[0].nodesIndex++;
		gameEvents[0].nodesIndex %= gameEvents[0].nodes.Count;
		return tD;
	}

	public GameObject GetGameEventHost(){
		if (gameEvents.Count == 0){
			return gameObject;
		}
		return (GameObject)gameEvents[0].gameObject;
	}
}
