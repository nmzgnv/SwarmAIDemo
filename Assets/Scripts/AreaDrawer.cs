using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDrawer : MonoBehaviour
{
    [SerializeField]
    private GameObject _point;
    [SerializeField]
    private GameObject swarmAI;
    private bool isDrawingRegime = false;
    private List<Vector2> _points = new List<Vector2>();
    private int pointCount = 0;
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        AddAreaPoint();
        if (Input.GetMouseButtonDown(1))
        {
            isDrawingRegime = !isDrawingRegime;
        }
        if (swarmAI.active)
        {
            Vector3[] positions = new Vector3[lineRenderer.positionCount];
            SwarmAI.SetMainCopterDirectionToArea(_points);
        }

    }


    private void AddAreaPoint()
    {
        if (Input.GetMouseButtonUp(0) & isDrawingRegime)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lineRenderer.SetPosition(pointCount, mousePosition);
            _points.Add(mousePosition);
            pointCount += 1;
            pointCount = pointCount % lineRenderer.positionCount;
        }
    }

    private void DrawFigure()
    {
        lineRenderer.positionCount += 1;
    }


}
