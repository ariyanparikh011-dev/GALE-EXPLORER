
using UnityEngine;

public class MarsRockFormations : MonoBehaviour
{
    public Terrain terrain;
    public GameObject largeRockPrefab;

    public int formationCount = 25;

    void Start()
    {
        for (int i = 0; i < formationCount; i++)
        {
            float centerX = Random.Range(0f, terrain.terrainData.size.x);
            float centerZ = Random.Range(0f, terrain.terrainData.size.z);

            int rocksInGroup = Random.Range(3, 7);

            for (int j = 0; j < rocksInGroup; j++)
            {
                float x = centerX + Random.Range(-400f, 400f);
                float z = centerZ + Random.Range(-400f, 400f);

                x = Mathf.Clamp(x, 0f, terrain.terrainData.size.x);
                z = Mathf.Clamp(z, 0f, terrain.terrainData.size.z);

                float y = terrain.SampleHeight(new Vector3(x, 0, z));

                Vector3 position = new Vector3(
                    x,
                    y + 20f,
                    z
                );

                GameObject rock = Instantiate(
                    largeRockPrefab,
                    position,
                    Random.rotation,
                    transform
                );

                float size = Random.Range(1.5f, 3f);
                rock.transform.localScale *= size;
            }
        }
    }
}
