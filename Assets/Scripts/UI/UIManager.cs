using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : MonoBehaviour
{
    public TMP_InputField widthInputField;
    public Slider widthSlider;
    public TMP_InputField seedInput;

    public Slider seedSlider;
    public Button generateMapButton;
    private PerlinNoiseGenerator perlin;
    

    private void Start()
    {
        GameObject perlinObject = new GameObject("PerlinNoiseGenerator");
        perlin = perlinObject.AddComponent<PerlinNoiseGenerator>();
        perlin.toggleDistrict = true;

        widthInputField.text = "128";
        seedInput.text = "6";

        generateMapButton.onClick.AddListener(GenerateMap);
        widthSlider.onValueChanged.AddListener(OnWidthValueChanged);
        seedSlider.onValueChanged.AddListener(OnSeedValueChanged);

        perlin.setWidthHeight(int.Parse(widthInputField.text));
        perlin.setSeed(int.Parse(seedInput.text));
        GameObject targetObject = GameObject.Find("PerlinMinimap");
        Renderer renderer = targetObject.GetComponent<Renderer>();
        perlin.setRenderer(renderer);


    }
    private void Update(){
        perlin.setWidthHeight(int.Parse(widthInputField.text));
        perlin.setSeed(int.Parse(seedInput.text));
        perlin.createTexture();
    }
     private void OnSeedValueChanged(float value)
    {
        // Convert the slider value to an integer and update the width
        int seed = Mathf.RoundToInt(value);
        seedInput.text = seed.ToString();
        perlin.setSeed(seed);
        perlin.createTexture();
    }

    private void OnWidthValueChanged(float value)
    {
        // Convert the slider value to an integer and update the width
        int width = Mathf.RoundToInt(value);
        widthInputField.text = width.ToString();
        perlin.setWidthHeight(width);
        perlin.createTexture();
    }
    private void GenerateMap()
    {
        int width = int.Parse(widthInputField.text);
        int height = width;
        int seed = int.Parse(seedInput.text);

        // Assuming the Controller script is still present, you can access its methods
        Controller controller = FindObjectOfType<Controller>();

        if (controller != null)
        {
            controller.startMap(width, height, seed);
        }
        controller.changeCamera();

        // Handle other actions or switch to a GameManager if needed
    }
}
