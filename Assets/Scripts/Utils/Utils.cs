using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static Vector2 GetMouseWorldPosition(Camera camera, Vector2 mousePosition)
    {
        return camera.ScreenToWorldPoint(mousePosition);
    }

    public static Vector2 GetMouseCameraPosition(Camera camera, Vector2 mousePosition)
    {
        return camera.WorldToScreenPoint(mousePosition);
    }
}
