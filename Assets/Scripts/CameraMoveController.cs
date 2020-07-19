using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveController : MonoBehaviour
{
    Vector2 offset = new Vector2(0, 0);
    float smooth = 1.0f;

    void FixedUpdate()
    {
        //Vector2 averagePos = Vector2.Lerp(transform.position, SwarmAI.AveragePosition() + offset, Time.fixedDeltaTime * smooth);
        Vector2 averagePos = SwarmAI.AveragePosition();
        transform.position = new Vector3(averagePos.x, averagePos.y, -10);
    }




}
