using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : MonoBehaviour
{
    public TMP_InputField widthInputField;
    public Slider widthSlider;
    public TMP_InputField seedInput;
    public TMP_InputField runTimeInput;
    public Slider seedSlider;
    public Button generateMapButton;
    public Button districtToggle;
    private PerlinNoiseGenerator perlin;
    private bool districtBool = true;
    

    private void Start()
    {

        widthInputField.text = "75";
        seedInput.text = "60";
        runTimeInput.text = "1";
 
        generateMapButton.onClick.AddListener(GenerateMap);
        widthSlider.onValueChanged.AddListener(OnWidthValueChanged);
        seedSlider.onValueChanged.AddListener(OnSeedValueChanged);
        districtToggle.onClick.AddListener(ToggleDistrict);

        GameObject perlinObject = new GameObject("PerlinNoiseGenerator");
        perlin = perlinObject.AddComponent<PerlinNoiseGenerator>();
        perlin.toggleDistrict = districtBool;
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
    private void ToggleDistrict(){
        if(districtBool == true){
            districtBool = false;
        }else{
            districtBool = true;
        }
        perlin.toggleDistrict = districtBool;
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
        int time = int.Parse(runTimeInput.text);

        // Assuming the Controller script is still present, you can access its methods
        Controller controller = FindObjectOfType<Controller>();

        if (controller != null)
        {
            controller.startMap(width, height, seed, time);
        }
        controller.changeCamera();

        // Handle other actions or switch to a GameManager if needed
    }
}
