using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopterMoveController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _copter;

    private float speed = 0;

    public Vector2 direction = new Vector2(0, 0);
    private Vector2 maxVector = new Vector2(0.3f, 0.3f);

    void Awake()
    {
        var posX = Random.Range(Constant.minPosX, Constant.maxPosX);
        var posY = Random.Range(Constant.minPosY, Constant.maxPosY);
        _copter.gameObject.transform.position = new Vector3(posX, 0, 0);
        speed = Random.Range(0.001f, 0.003f);
    }

    void Update()
    {
        if (direction.magnitude > maxVector.magnitude)
        {
            direction /= 2;
        }
        _copter.AddForce(direction * speed, ForceMode2D.Impulse);
        ChangeDirection();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Vector2 currentDirection = direction;
        switch (other.tag)
        {
            case "Fire":
                
                break;
            case "Water":
                Debug.Log($"Water is detected! Coordinates: ({other.transform.position.x}; {other.transform.position.y})");
                Vector2 dron2Water = new Vector2(other.transform.position.x - _copter.transform.position.x,
                                            other.transform.position.y - _copter.transform.position.y);
                Vector2 moveFromWater = (direction - dron2Water);
                CoptersAddForce(moveFromWater);
                break;
        }
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


    private void ChangeDirection()
    {
        GameObject currnetCopter = _copter.gameObject;
        foreach (var copter in SwarmAI.Copters)
        {
            if (copter.gameObject == currnetCopter) continue;
            if (Mathf.Abs(copter.gameObject.transform.position.x - currnetCopter.transform.position.x) < 0.1)
                direction.x += 0.5f;
            if (Mathf.Abs(copter.gameObject.transform.position.y - currnetCopter.transform.position.y) < 0.1)
                direction.y += 0.5f;
        }
    }

}
