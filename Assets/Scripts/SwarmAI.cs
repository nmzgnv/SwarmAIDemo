using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class SwarmAI : MonoBehaviour
{

	[SerializeField]
	private GameObject dronePrefab;

	public static List<GameObject> Copters = new List<GameObject>();

	void Start()
	{
		for (int i = 0; i < Constant.dronesNumber; i++)
		{
			GameObject drone = (GameObject)Instantiate(dronePrefab);
			Copters.Add(drone);
		}
	}


	void Update()
	{
		foreach (GameObject copter in Copters)
		{
			copter.GetComponent<CopterMoveController>().direction = AverageDirection();
		}
	}


	public static Vector2 AveragePosition()
	{
		float averageX = 0;
		float averageY = 0;

		foreach (GameObject copter in Copters)
		{
			averageX += copter.transform.position.x;
			averageY += copter.transform.position.y;
		}

		averageX /= Copters.Count;
		averageY /= Copters.Count;

		return new Vector2(averageX, averageY);
	}


	public Vector2 AverageDirection()
	{
		Vector2 directionSum = Vector2.zero;

		foreach (GameObject copter in Copters)
		{
			var copterDirection = copter.GetComponent<CopterMoveController>().direction;
			directionSum += copterDirection;
		}

		return directionSum / new Vector2(Copters.Count, Copters.Count);
	}


	public void SetDronesInDiagonal()
	{
		float x = Copters[0].transform.position.x + 1;
		float y = Copters[0].transform.position.y - 1;

		for (int i = 1; i < Constant.dronesNumber; i++)
		{
			foreach(GameObject copter in Copters)
			{
				copter.transform.position = new Vector2(x, y);
				x += 1;
				y -= 1;
			}
		}
	}

}

