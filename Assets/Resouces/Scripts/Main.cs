using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using MiniJSON;

public class Main : MonoBehaviour {
	[System.NonSerialized]public bool isGameOver;
	[System.NonSerialized]public Character player;
	[System.NonSerialized]public EventManager eventManager;

	[System.NonSerialized]public CameraController camera;
	[System.NonSerialized]public GroundGenerator ground;

	private string filePathJSON = "Data/level_Village";

//	public UI ui;
	public MessageWindow messageWindow;

	public static DelayFunc delayFunc;

	private InputManager input;

	private string mode;

	// Use this for initialization
	void Awake () {
		delayFunc = new DelayFunc();
		input = GetComponent<InputManager>();
		input.onTap += () => {
			CallEvent();
		};
		eventManager = new EventManager(GameObject.Find("EventChecker"));


		BuildScene();

		SwitchMode("Search");
		isGameOver = false;

		GameStart();
	}

	private void BuildScene(){
		player = BuildPlayer();

		ground = BuildGround();
//		ui = new UI(GameObject.Find ("UI"));
//		messageWindow = new MessageWindow(GameObject.Find("MessageWindow"));

		camera = new CameraController(GameObject.Find("Main Camera"));
		camera.target = player.gameObject.transform;
		camera.Init();
	}

	private Character BuildPlayer(){
		GameObject tGameObjet = transform.FindChild("Player").gameObject;
		Character tPlayer = new Character(tGameObjet);

		return tPlayer;
	}

	private GroundGenerator BuildGround(){
		GameObject tGameObjet = GameObject.Find("Ground");

		//Dictionary<string, object> tNode = LoadJSON(filePathJSON);

		GroundGenerator tGround = new GroundGenerator(tGameObjet, ParseGroundData());
		return tGround;
	}

	//
	private void GameStart(){
		isGameOver = false;
		player.Init(player.gameObject.transform.localPosition);

		camera.isFollowTarget = true;
	}


	private void GameOver(){
		if (isGameOver == true) return;
		isGameOver = true;
		camera.isFollowTarget = false;

	}
	
	// Update is called once per frame
	void Update () {
//		delayFunc.Update();
		switch (mode){
		case "Search":
			SearchUpdate();
			break;
		}
	}
	
	private void SearchUpdate(){
		player.OnUpdate(input.distance, camera.transform.localRotation);
		Draw();
	}

	private void Draw(){
		player.Draw();
//		ui.Refresh();
		camera.OnUpdate();
	}

	public void CallEvent(){
		Dictionary<string, object> tD = eventManager.GetGameEventNode();
		if (tD.Count == 0) return;
		if (tD.ContainsKey("message") == true){
			eventManager.textMessage = (string)tD["message"];
			SwitchMode("Message");
			return;
		}
		if (tD.ContainsKey("animation") == true){
			Dictionary<string, object> tD2 = (Dictionary<string, object>)tD["animation"];
			string tTarget = (string)tD2["target"];
			Animator tAnime;
			if (tTarget == "parent"){
				GameObject tGameObject = eventManager.GetGameEventHost();
				tAnime = tGameObject.transform.parent.parent.gameObject.GetComponent<Animator>();
			} else {
				tAnime = GameObject.Find(tTarget).GetComponent<Animator>();
			}
			tAnime.Play((string)tD2["frameLabel"]);
		}
	}

	public void SwitchMode(string pString){
		string lastMode = mode;
		mode = pString;
		switch(lastMode){
		case "Search":
			input.enabled = false;
			break;
		case "Message":
			messageWindow.gameObject.SetActive(false);
			break;
		}
		switch(mode){
		case "Search":
			input.enabled = true;
			break;
		case "Message":
			messageWindow.text.text = eventManager.textMessage;
			if (messageWindow.text.text == ""){
				SwitchMode(lastMode);
				return;
			}
			messageWindow.gameObject.SetActive(true);
			break;
		}
	}

	private List<int[]> ParseGroundData(){
		List<int[]> tList = new List<int[]>{
			//          0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
			new int[32]{ 0, 0, 1, 4, 3, 5, 0,-1,-1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			new int[32]{ 0, 0, 0, 0, 2, 1, 0,-1,-1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			new int[32]{ 0, 0, 0, 0, 0, 1, 0,-1,-1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			new int[32]{ 0, 0, 0, 0, 0, 0,-1,-1,-1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			new int[32]{ 0, 0, 0, 0, 0, 0,-1,-1,-1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			new int[32]{ 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			new int[32]{ 0, 0, 0, 0, 0,-1,-1,-1,-1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			new int[32]{ 0, 0, 0, 0, 0,-1,-1,-1,-1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			new int[32]{ 0, 0, 0, 0, 0,-1,-1,-1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			new int[32]{ 0, 0, 0, 1, 0,-1,-1,-1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			new int[32]{ 0, 0, 0, 1, 0,-1,-1,-1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			new int[32]{ 0, 0, 0, 0, 0,-1,-1,-1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			new int[32]{ 0, 0, 0, 0, 0,-1,-1,-1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			new int[32]{ 0, 0, 0, 1, 0,-1,-1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			new int[32]{ 0, 0, 0, 1, 0,-1,-1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			new int[32]{ 0, 0, 0, 1, 0,-1,-1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			new int[32]{ 0, 0, 0, 2, 0,-1,-1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
		};
		return tList;
	}
//	private Dictionary<string, object> LoadJSON(string pFilePath){
//		TextAsset tText = (TextAsset)Resources.Load(pFilePath);
//		return (Dictionary<string, object>)Json.Deserialize(tText.text);
//	}
}


public static class Util {

}
