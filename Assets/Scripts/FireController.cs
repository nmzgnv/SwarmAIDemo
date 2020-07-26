using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class FireController : MonoBehaviour
{
	void OnCollisionEnter2D(Collision2D other)
	{
		var body = other.gameObject.GetComponent<Rigidbody2D>();
		Vector2 force = other.transform.position - transform.position;
		force = new Vector2(force.x + Random.Range(-2, 2), force.y + Random.Range(-2, 2));
		if (body)
			body.AddForce(force, ForceMode2D.Impulse);
	}
}
