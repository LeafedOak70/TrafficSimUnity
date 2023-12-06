using UnityEngine;

public class PerlinNoiseGenerator : MonoBehaviour
{
    public int width;
    public int height;
    public float scale = 0.9f;
    public int octaves = 6;
    public float persistence = 0.5f;
    public float lacunarity = 2;
    public int seed = 6;
    public bool toggleDistrict;

    public float downtownThreshold = 0.3f;
    public float urbanThreshold = 0.6f;
    public float ruralThreshold = 0.8f;

    private void createTexture()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = GenerateTexture();
    }
    public float[,] getPerlinMap(int w, int h){
        this.width = w;
        this.height = h;
        float[,] perlinMap = new float[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float xCoord = (float)x / width * scale;
                float yCoord = (float)y / height * scale;

                perlinMap[x, y] = GeneratePerlinNoise(xCoord, yCoord);
            }
        }

        return perlinMap;
    }
    Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(width, height);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float xCoord = (float)x / width * scale;
                float yCoord = (float)y / height * scale;

                float sample = GeneratePerlinNoise(xCoord, yCoord);

                // Assign colors based on the sample value and different thresholds
                Color pixelColor;
                if(toggleDistrict){
                    if (sample < downtownThreshold)
                    {
                        pixelColor = Color.black; // Downtown
                    }
                    else if (sample < urbanThreshold)
                    {
                        pixelColor = Color.gray; // Urban
                    }
                    else if (sample < ruralThreshold)
                    {
                        pixelColor = new Color(sample, sample, sample) * 0.8f;; // Rural
                    }
                    else
                    {
                        pixelColor = Color.white; // Villages
                    }
                }else{
                    pixelColor = new Color(sample, sample, sample);

                };
                

                texture.SetPixel(x, y, pixelColor);
            }
        }

        texture.Apply();
        return texture;
    }

    float GeneratePerlinNoise(float x, float y)
    {
        float total = 0;
        float frequency = 1;
        float amplitude = 1;
        float maxValue = 0;

        for (int i = 0; i < octaves; i++)
        {
            total += Mathf.PerlinNoise(x * frequency + seed, y * frequency + seed) * amplitude;
            maxValue += amplitude;
            amplitude *= persistence;
            frequency *= lacunarity;
        }

        return total / maxValue;
    }
}
