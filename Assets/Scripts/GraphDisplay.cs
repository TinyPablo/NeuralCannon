using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
public class GraphDisplay : MonoBehaviour
{
    [SerializeField] Canon canon;
    [SerializeField] RectTransform graphContainer;
    [SerializeField] GameObject pointPrefab;
    [SerializeField] Color lineColor = Color.orangeRed;
    [SerializeField] Color pointColor = Color.limeGreen;

    List<GameObject> points = new();
    LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.02f;
        lineRenderer.endWidth = 0.02f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
        lineRenderer.positionCount = 2;
        lineRenderer.widthMultiplier = 3;
        lineRenderer.sortingOrder = 40;

        graphContainer.pivot = new Vector2(0.5f, 0.5f);
    }

    void Update()
    {
        DrawGraph();
    }

    void DrawGraph()
    {
        foreach (var p in points) Destroy(p);
        points.Clear();

        var data = canon.shootingData;
        if (data.Count == 0) return;

        float w = graphContainer.rect.width;
        float h = graphContainer.rect.height;

        float maxX = Mathf.Max(1f, data.Keys.Max());
        float maxY = Mathf.Max(1f, data.Values.Max());

        Vector2 center = Vector2.zero; 

        foreach (var kv in data)
        {
            if (kv.Value <= -1f) continue;
            float x = (kv.Key / maxX) * (w / 2f);
            float y = (kv.Value / maxY) * (h / 2f);

            GameObject point = Instantiate(pointPrefab, graphContainer);
            var rect = point.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(x, y);
            points.Add(point);
        }

        float weight = canon.neuronWeight;
        float halfWidth = w / 2f;
        float halfHeight = h / 2f;

        float scaledWeight = weight * (maxX / maxY);

        Vector3 start = new Vector3(-halfWidth, -scaledWeight * halfWidth, 0);
        Vector3 end = new Vector3(halfWidth, scaledWeight * halfWidth, 0);

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, graphContainer.TransformPoint(start));
        lineRenderer.SetPosition(1, graphContainer.TransformPoint(end));
    }
}