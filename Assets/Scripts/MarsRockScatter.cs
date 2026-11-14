
using UnityEngine;

public class MarsRockScatter : MonoBehaviour
{
    public Terrain terrain;
    public GameObject[] rockPrefabs;
    public int rockCount = 10000;
    public int clusterCount = 250;

    void Start()
    {
        Vector2[] clusterCenters = new Vector2[clusterCount];

        for (int i = 0; i < clusterCount; i++)
        {
            clusterCenters[i] = new Vector2(
                Random.Range(0f, terrain.terrainData.size.x),
                Random.Range(0f, terrain.terrainData.size.z)
            );
        }

        for (int i = 0; i < rockCount; i++)
        {
            int clusterIndex = i % clusterCount;
            Vector2 center = clusterCenters[clusterIndex];

            float angle = Random.Range(0f, 360f);
            float distance = Random.Range(100f, 800f);

            float x = Mathf.Clamp(
                center.x + Mathf.Cos(angle * Mathf.Deg2Rad) * distance,
                0f, terrain.terrainData.size.x
            );

            float z = Mathf.Clamp(
                center.y + Mathf.Sin(angle * Mathf.Deg2Rad) * distance,
                0f, terrain.terrainData.size.z
            );

            float y = terrain.SampleHeight(
                new Vector3(x, 0, z)
            );

            GameObject prefab = rockPrefabs[
                Random.Range(0, rockPrefabs.Length)
            ];

            float rockHeight = prefab.transform.localScale.y;

            Vector3 position = new Vector3(
                x, y + rockHeight * 0.45f, z
            );

            GameObject rock = Instantiate(
                prefab,
                position,
                Quaternion.Euler(
                    Random.Range(-12f, 12f),
                    Random.Range(0f, 360f),
                    Random.Range(-12f, 12f)
                ),
                transform
            );

            float variation = Random.Range(0.75f, 1.3f);

            rock.transform.localScale = Vector3.Scale(
                rock.transform.localScale,
                new Vector3(
                    variation * Random.Range(0.8f, 1.2f),
                    variation * Random.Range(0.75f, 1.15f),
                    variation * Random.Range(0.8f, 1.2f)
                )
            );

            MeshFilter filter = rock.GetComponentInChildren<MeshFilter>();

            if (filter != null)
            {
                Mesh mesh = filter.mesh;
                Vector3[] vertices = mesh.vertices;

                for (int v = 0; v < vertices.Length; v++)
                {
                    vertices[v] *= Random.Range(0.82f, 1.18f);
                }

                mesh.vertices = vertices;
                mesh.RecalculateNormals();
                mesh.RecalculateBounds();
            }
        }
    }
}
