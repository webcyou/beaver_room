using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MessageWindow {
	public GameObject gameObject;
	public Text text;
	
	public MessageWindow(GameObject pGameObject){
		gameObject = pGameObject;
		gameObject.SetActive(false);
		text = gameObject.transform.FindChild("Text").gameObject.GetComponent<Text>();
	}
}
