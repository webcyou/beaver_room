using UnityEngine;
using System.Collections;

public class Character {
	private float maxVelocity = 1.2f;
	public GameObject gameObject;
	public Rigidbody rigidbody;

	public Vector3 position = new Vector3();
	public Vector3 speed = new Vector3();
	public float direction = 0f;
	public float velocity = 0f;

	public float groundFriction = 0.8f;

	private bool isGrounded;

	private GameObject view;

	public Character(GameObject pGameObject){
		gameObject = pGameObject;
		rigidbody = pGameObject.GetComponent<Rigidbody>();
		view = gameObject.transform.FindChild("View").gameObject;
	}
	
//	public void Init(Vector3 pPosition){
//		pPosition *= 100f;
//		position.x = pPosition.x;
//		position.y = pPosition.y;
//		position.z = pPosition.z;
//		isGrounded = false;
//		Draw();
//	}

	public void Init(Vector3 pPosition){
		position.x = gameObject.transform.localPosition.x;
		position.y = gameObject.transform.localPosition.y;
		position.z = gameObject.transform.localPosition.z;
	}
	
	public void OnUpdate(Vector2 pInputValue, Quaternion pRotation){
		position.x = gameObject.transform.localPosition.x;
		position.y = gameObject.transform.localPosition.y;
		position.z = gameObject.transform.localPosition.z;

		float tRadian = pRotation.eulerAngles.y * Mathf.Deg2Rad;
		float tDirection =  GetRadianFromVector2(pInputValue) -tRadian;
		velocity = GetVelocityFromVector2(pInputValue);

		speed.x += Mathf.Cos(tDirection) * velocity;
		speed.z += Mathf.Sin(tDirection) * velocity;
		speed.x *= groundFriction;
		speed.z *= groundFriction;
		direction = (Mathf.Atan2(speed.z, speed.x) -pRotation.eulerAngles.y) *-1f;
		
		rigidbody.velocity = speed;

		isGrounded = false;
	}

	private float GetRadianFromVector2(Vector2 pVector2){
		return Mathf.Atan2(pVector2.y, pVector2.x);
	}
	private float GetVelocityFromVector2(Vector2 pVector2){
		float tVelocity = Mathf.Sqrt(pVector2.x * pVector2.x + pVector2.y * pVector2.y) /100f;
		if (tVelocity > maxVelocity){
			tVelocity = maxVelocity;
		}
		return tVelocity;
	}

	public void Draw(){
		view.transform.localRotation = Quaternion.Euler(0f, direction * Mathf.Rad2Deg, 0f);
//		gameObject.transform.localPosition = position / 100f;
	}
}
