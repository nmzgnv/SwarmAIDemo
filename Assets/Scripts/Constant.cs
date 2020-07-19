using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constant
{

    public static int dronesNumber = 5;

    public enum ObjectsTypes
    {
        Water,
        Fire,
        Road
    }

    public static float maxPosX = 8;
    public static float minPosX = -8;
    public static float maxPosY = 0;
    public static float minPosY = -4;
    public static float droneRadius = 1;

}
