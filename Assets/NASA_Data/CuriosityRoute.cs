using UnityEngine;
using System.Collections.Generic;
using System.Globalization;

public class CuriosityRoute : MonoBehaviour
{
    public Terrain galeTerrain;
    public TextAsset routeCSV;

    public float lineWidth = 25f;
    public float heightOffset = 30f;

    void Start()
    {
        if (galeTerrain == null || routeCSV == null)
        {
            Debug.LogError("Please assign the terrain and CSV file.");
            return;
        }

        string[] lines = routeCSV.text.Split('\n');

        Dictionary<string, List<Vector3>> segments =
    new Dictionary<string, List<Vector3>>();

        float minX = 8085862.3981f;
        float maxX = 8260252.3981f;
        float minY = -403248.3071f;
        float maxY = -236988.3071f;

        foreach (string line in lines)
        {
            string[] values = line.Trim().Split(',');

            if (values.Length < 5 || values[0] == "easting")
                continue;

            if (!float.TryParse(values[0], NumberStyles.Float,
                CultureInfo.InvariantCulture, out float east))
                continue;

            float north = float.Parse(values[1],
                CultureInfo.InvariantCulture);

            string segment = values[3].Trim();
            
            float x = (east - minX) / (maxX - minX);
            float z = (north - minY) / (maxY - minY);

            Vector3 point = new Vector3(
                x * galeTerrain.terrainData.size.x,
                0,
                z * galeTerrain.terrainData.size.z
            );

            if (!segments.ContainsKey(segment))
                segments[segment] = new List<Vector3>();

            segments[segment].Add(point);
        }
        Debug.Log("Number of route segments loaded: " + segments.Count);
        foreach (var segment in segments)
        {
            GameObject routePart = new GameObject(
                "Curiosity Route Segment " + segment.Key
            );

            routePart.transform.SetParent(galeTerrain.transform, false);

            LineRenderer line = routePart.AddComponent<LineRenderer>();
            line.useWorldSpace = false;
            line.positionCount = segment.Value.Count;
            line.startWidth = lineWidth;
            line.endWidth = lineWidth;
            line.material = new Material(Shader.Find("Sprites/Default"));
            line.startColor = Color.yellow;
            line.endColor = Color.yellow;

            for (int i = 0; i < segment.Value.Count; i++)
            {
                Vector3 point = segment.Value[i];

                float terrainHeight =
                    galeTerrain.terrainData.GetInterpolatedHeight(
                        point.x / galeTerrain.terrainData.size.x,
                        point.z / galeTerrain.terrainData.size.z
                    );

                point.y = terrainHeight + heightOffset;
                line.SetPosition(i, point);
            }
        }

        Debug.Log("Curiosity route created!");
    }
}