using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CanvasRenderer))]
public class GraphDisplay : MonoBehaviour
{
    [SerializeField] private Cannon cannon;
    [SerializeField] private RectTransform graphArea;
    [SerializeField] private GameObject pointPrefab;
    [SerializeField] private Color lineColor = Color.red;
    [SerializeField] private Color pointColor = Color.green;

    private readonly List<GameObject> points = new();
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startWidth = 0.02f;
        lineRenderer.endWidth = 0.02f;
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
        lineRenderer.sortingOrder = 40;
    }

    private void Update() => DrawGraph();

    private void DrawGraph()
    {
        foreach (var point in points) Destroy(point);
        points.Clear();

        var data = cannon.shotResults;
        if (data.Count == 0) return;

        float width = graphArea.rect.width;
        float height = graphArea.rect.height;

        float maxX = Mathf.Max(1f, data.Keys.Max());
        float maxY = Mathf.Max(1f, data.Values.Max());

        foreach (var (xVal, yVal) in data)
        {
            if (yVal <= -1f) continue;
            float x = (xVal / maxX) * (width / 2f);
            float y = (yVal / maxY) * (height / 2f);

            GameObject point = Instantiate(pointPrefab, graphArea);
            point.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
            points.Add(point);
        }

        float weight = cannon.neuron.Weight;
        float slopeScale = weight * (maxX / maxY);

        Vector3 start = new(-width / 2f, -slopeScale * width / 2f, 0);
        Vector3 end = new(width / 2f, slopeScale * width / 2f, 0);

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, graphArea.TransformPoint(start));
        lineRenderer.SetPosition(1, graphArea.TransformPoint(end));
    }
}