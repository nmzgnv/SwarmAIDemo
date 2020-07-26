using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq.Expressions;
using UnityEngine;

public class SwarmAI : MonoBehaviour
{

	[SerializeField]
	private GameObject dronePrefab;
	[SerializeField]
	private static bool needAroundFire = false;

	public static List<GameObject> Copters = new List<GameObject>();
	private static GameObject mainCopter;
	private static Dictionary<GameObject, Vector3> coptersSurroundPositions = new Dictionary<GameObject, Vector3>();

	void Start()
	{
		for (int i = 0; i < Constant.dronesNumber; i++)
		{
			GameObject drone = (GameObject)Instantiate(dronePrefab);
			Copters.Add(drone);
		}
		Copters.Sort((x1, x2) => x1.transform.position.x.CompareTo(x2.transform.position.x));
		mainCopter = Copters[0];
		var mainCopterController = mainCopter.GetComponent<CopterMoveController>();
		mainCopterController.speedX = 1;
		mainCopterController.speedY = 1;
	}


	void Update()
	{
		if (!needAroundFire)
			FixDroneOrder();
		else
		{
			DronesSurroundFire();
		}
	}


	public static Vector2 AveragePosition()
	{
		if (Copters.Count == 0)
			return Vector2.zero;
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

	private void CoptersAddForce(Vector2 direction)
	{
		foreach (var copter in SwarmAI.Copters)
		{
			var copterController = copter.GetComponent<CopterMoveController>();
			var oldDirection = copterController.direction;
			copter.GetComponent<Rigidbody2D>().AddForce(direction * 0.2f, ForceMode2D.Impulse);
			copterController.direction = oldDirection;
		}
	}

	private static void MainCopterAddForce(Vector2 direction)
	{
		mainCopter.GetComponent<CopterMoveController>().CopterAddForce(direction);
	}

	private void SetEachDronDirection()
	{
		Copters.Sort((x1, x2) => x1.transform.position.x.CompareTo(x2.transform.position.x));
		foreach (GameObject copter in Copters)
		{
			if (copter == mainCopter) continue;
			copter.GetComponent<CopterMoveController>().direction = AverageDirection();
		}
	}


	private void FixDroneOrder()
	{
		Copters.Sort((x1, x2) => x1.transform.position.x.CompareTo(x2.transform.position.x));
		mainCopter = Copters[0];
		bool isCorrectStructure = true;
		for (int i = 0; i < Copters.Count; i++)
		{
			//Мне стыдно за этот код, это было написано за 1 ночь :)
			CopterMoveController _copter = Copters[i].GetComponent<CopterMoveController>();
			if (Copters[i] == mainCopter)
			{
				continue;
				_copter.speedX = 0;
				_copter.speedY = 0;
			};
			if (Mathf.Abs(mainCopter.transform.position.x - Copters[i].transform.position.x) < Constant.droneCoordDifference * i)
			{
				_copter.speedX += Constant.droneCorrectionSpeed;
				isCorrectStructure = false;
			}
			else if (Mathf.Abs(mainCopter.transform.position.x - Copters[i].transform.position.x) > Constant.maxDroneCoordDifference * i)
			{
				_copter.speedX += mainCopter.transform.position.x - Copters[i].transform.position.x > 0 ? Constant.droneCorrectionSpeed : -Constant.droneCorrectionSpeed;
				isCorrectStructure = false;
			}
			else _copter.speedX = 0;

			if (mainCopter.transform.position.y - Copters[i].transform.position.y < Constant.droneCoordDifference * i)
			{
				_copter.speedY -= Constant.droneCorrectionSpeed;
				isCorrectStructure = false;
			}
			else if (Mathf.Abs(mainCopter.transform.position.y - Copters[i].transform.position.y) > Constant.maxDroneCoordDifference * i)
			{
				_copter.speedY += mainCopter.transform.position.y - Copters[i].transform.position.y > 0 ? Constant.droneCorrectionSpeed : -Constant.droneCorrectionSpeed;
				isCorrectStructure = false;
			}
			else _copter.speedY = 0;
		}

		if (isCorrectStructure)
		{
			SetDronesSpeed(mainCopter.GetComponent<CopterMoveController>().speedY);
		}
	}

	private static void SetDronesSpeed(float speed)
	{
		foreach (var copter in Copters)
		{
			CopterMoveController copterMoveController = copter.GetComponent<CopterMoveController>();
			copterMoveController.speedX = speed;
			copterMoveController.speedY = speed;
		}
	}


	public static void SurroundFire(CircleCollider2D fire)
	{
		needAroundFire = true;
		SetDronesSpeed(0.2f);
		for (int i = 0; i < Copters.Count; i++)
		{
			float radian = i * Mathf.PI / (Copters.Count / 2);
			Vector2 dronePosition = new Vector2((fire.radius + 2) * Mathf.Cos(radian) + fire.transform.position.x,
												(fire.radius + 2) * Mathf.Sin(radian) + fire.transform.position.y
												);
			if (!coptersSurroundPositions.ContainsKey(Copters[i]))
				coptersSurroundPositions.Add(Copters[i], dronePosition);
			else
			{
				coptersSurroundPositions[Copters[i]] = dronePosition;
			}
		}
	}

	public static void DronesSurroundFire()
	{
		foreach (GameObject copter in coptersSurroundPositions.Keys)
		{
			Vector3 direction = coptersSurroundPositions[copter] - copter.transform.position;
			var copterMoveController = copter.GetComponent<CopterMoveController>();
			copterMoveController.CopterAddForce(direction);
		}
		Debug.Log($"Fire area: {GetCoptersArea()} m^2");
	}

	public static void AvoidWater(Vector2 drone2Water)
	{
		var copterMoveController = mainCopter.GetComponent<CopterMoveController>();
		copterMoveController.speedX = 1;
		copterMoveController.speedY = 1;
		MainCopterAddForce(copterMoveController.direction - drone2Water);
	}


	public static float GetCoptersArea()
	{
		// Формула площади Гаусса,
		// https://ru.wikipedia.org/wiki/%D0%A4%D0%BE%D1%80%D0%BC%D1%83%D0%BB%D0%B0_%D0%BF%D0%BB%D0%BE%D1%89%D0%B0%D0%B4%D0%B8_%D0%93%D0%B0%D1%83%D1%81%D1%81%D0%B0
		float S = 0;

		for (int i = 0; i < Copters.Count; i++)
		{
			var copter = Copters[i];
			int nextInd = (i + 1 >= Copters.Count ? 0 : i + 1);
			S += copter.transform.position.x * Copters[nextInd].transform.position.y - copter.transform.position.y * Copters[nextInd].transform.position.x;
		}
		return Mathf.Abs(S / 2);
	}


	public static void SetMainCopterDirectionToArea(List<Vector2> points)
	{
		float positionX = 0;
		float positionY = 0;
		foreach (var point in points)
		{
			positionX += point.x;
			positionY += point.y;
		}
		positionX /= points.Count;
		positionY /= points.Count;
		var mainCopterController = mainCopter.GetComponent<CopterMoveController>();
		mainCopterController.speedX = positionX;
		mainCopterController.speedY = positionY;
	}

}

