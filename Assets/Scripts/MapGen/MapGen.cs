using UnityEngine;

public class MapGen : MonoBehaviour
{
    public PerlinNoiseGenerator perlinClass;
    public GameObject downtownPrefab;
    public GameObject urbanPrefab;
    public GameObject ruralPrefab;
    public GameObject villagePrefab;

    private void Update()
    {
        float[,] perlinMap = perlinNoiseGenerator.GeneratePerlinMap();
        GenerateTiles(perlinMap);
    }

    void GenerateTiles(float[,] perlinMap)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float sample = perlinMap[x, y];
                GameObject tilePrefab = ChooseTilePrefab(sample);
                Instantiate(tilePrefab, new Vector3(x, 0, y), Quaternion.identity);
            }
        }
    }
    GameObject ChooseTilePrefab(float sample)
    {
        if (sample < perlinClass.downtownThreshold)
        {
            return downtownPrefab;
        }
        else if (sample < perlinClass.urbanThreshold)
        {
            return urbanPrefab;
        }
        else if (sample < perlinClass.ruralThreshold)
        {
            return ruralPrefab;
        }
        else
        {
            return villagePrefab;
        }      
        
    }

}
