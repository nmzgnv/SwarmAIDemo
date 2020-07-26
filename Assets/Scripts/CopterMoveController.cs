using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopterMoveController : MonoBehaviour
{
	[SerializeField]
	private Rigidbody2D _copter;

	public float speedX = 0.001f;
	public float speedY = 0.001f;

	public Vector2 direction = new Vector2(1, 1);
	private Vector2 maxDirectionVector = new Vector2(1f, 1f);

	void Awake()
	{
		var posX = Random.Range(Constant.minPosX, Constant.maxPosX);
		var posY = Random.Range(Constant.minPosY, Constant.maxPosY);
		_copter.gameObject.transform.position = new Vector3(posX, posY, 0);
		/*var speed = Random.Range(0.001f, 0.003f);
		speedX = speed;
		speedY = speed;*/
	}

	void Update()
	{
		ControlMaxDirection();
		CopterAddForce(direction);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		var objectPosX = other.transform.position.x;
		var objectPosY = other.transform.position.y;
		switch (other.tag)
		{
			case "Fire":
				Debug.Log($"WARNING fire is detected! Coordinates: {objectPosX}; {objectPosY}");
				SwarmAI.SurroundFire((CircleCollider2D)other);
				break;
			case "Water":
				Debug.Log($"Water is detected! Coordinates: {objectPosX}; {objectPosY}");
				Vector2 dron2Water = new Vector2(other.transform.position.x - _copter.transform.position.x,
								 			     other.transform.position.y - _copter.transform.position.y);
				SwarmAI.AvoidWater(dron2Water);
				break;
		}
	}

	void ControlMaxDirection()
	{
		direction.x = Mathf.Min(maxDirectionVector.x, Mathf.Abs(direction.x));
		direction.y = Mathf.Min(maxDirectionVector.y, Mathf.Abs(direction.y));
	}

	public void CopterAddForce(Vector2 direction)
	{
		_copter.AddForce(new Vector2(direction.x * speedX, direction.y * speedY), ForceMode2D.Impulse);
	}

}
